using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Windows;

namespace WotDossier.Applications
{
    public class ReplayUploader
    {
        private const string REQ_BOUNDARY = "---------------------------4391253124807";
        private const string REQ_CONTENT_PART1_FORMAT = @"--{1}
Content-Disposition: form-data; name=""Replay[file_name]""


--{1}
Content-Disposition: form-data; name=""Replay[file_name]""; filename=""{0}""
Content-Type: application/octet-stream

";
        private const string REQ_CONTENT_PART2_FORMAT = @"
--{4}
Content-Disposition: form-data; name=""Replay[title]""

{0}
--{4}
Content-Disposition: form-data; name=""Replay[description]""

{1}
--{4}
Content-Disposition: form-data; name=""Replay[isSecret]""

{2}
--{4}
Content-Disposition: form-data; name=""Replay[isSecret]""

{3}
--{4}
Content-Disposition: form-data; name=""yt0""

Загрузить реплей
--{4}--";

        /// <summary>
        /// Uploads the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="replayName">Name of the replay.</param>
        /// <param name="replayDescription">The replay description.</param>
        /// <param name="uploadUrl">The upload URL. "http://wotreplays.ru/site/upload"</param>
        public void Upload(FileInfo info, string replayName, string replayDescription, string uploadUrl)
        {
            var cookieContainer = LoadCookies(uploadUrl);

            if (!IsAuthentificated(cookieContainer, uploadUrl))
            {
                throw new AuthenticationException(string.Format("User not authentificated on site {0}", uploadUrl));
            }

            string firstPart = string.Format(REQ_CONTENT_PART1_FORMAT, info.Name, REQ_BOUNDARY);
            string secondPart = string.Format(REQ_CONTENT_PART2_FORMAT, replayName, replayDescription, 0, 0, REQ_BOUNDARY);

            byte[] fileBytes = File.ReadAllBytes(info.FullName);
            byte[] contentPart1Bytes = Encoding.UTF8.GetBytes(firstPart);
            byte[] contentPart2Bytes = Encoding.UTF8.GetBytes(secondPart);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uploadUrl);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.CookieContainer = cookieContainer;
            request.ContentLength = info.Length;
            request.ContentType = "multipart/form-data; boundary=" + REQ_BOUNDARY;
            request.Method = WebRequestMethods.Http.Post;

            // Длинна запроса (обязательный параметр)
            request.ContentLength = fileBytes.Length + contentPart1Bytes.Length + contentPart2Bytes.Length;
            // Открываем поток для записи
            Stream uploadStream = request.GetRequestStream();
            // Записываем в поток (это и есть POST запрос(заполнение форм))
            uploadStream.Write(contentPart1Bytes, 0, contentPart1Bytes.Length);
            uploadStream.Write(fileBytes, 0, fileBytes.Length);
            uploadStream.Write(contentPart2Bytes, 0, contentPart2Bytes.Length);
            // Закрываем поток
            uploadStream.Flush();
            uploadStream.Close();

            WebResponse webResponse = request.GetResponse();

            Clipboard.SetText(webResponse.ResponseUri.ToString());

            using (Stream stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string readToEnd = streamReader.ReadToEnd();
                }
            }
        }

        private bool IsAuthentificated(CookieContainer cookieContainer, string uploadUrl)
        {
            bool auth = false;

            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri(uploadUrl)))
            {
                if (cookie.Name.Length == 32)
                {
                    auth = true;
                }
            }
            return auth;
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]

        static extern bool InternetGetCookie(string lpszUrlName, string lpszCookieName,
        [Out] StringBuilder lpszCookieData, [MarshalAs(UnmanagedType.U4)] out int lpdwSize);

        public static CookieContainer LoadCookies(String url)
        {
            CookieContainer cookies = new CookieContainer();
            Uri uri = new Uri(url);

            StringBuilder cookieBuilder = new StringBuilder(new string(' ', 256), 256);

            int cookieSize = cookieBuilder.Length;

            if (!InternetGetCookie(url, null, cookieBuilder, out cookieSize))
            {
                if (cookieSize == 0)
                {
                    return cookies;
                }
                cookieBuilder = new StringBuilder(cookieSize);
                InternetGetCookie(url, null, cookieBuilder, out cookieSize);
            }

            cookies.SetCookies(uri, cookieBuilder.ToString().Replace(";", ","));
            return cookies;
        }
    }
}
