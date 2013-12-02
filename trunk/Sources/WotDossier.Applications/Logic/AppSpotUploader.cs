using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WotDossier.Common;

namespace WotDossier.Applications.Logic
{
    public class AppSpotUploader
    {
        #region Constants

        private const string REQ_BOUNDARY = "---------------------------{0}";
        private const string REQ_CONTENT_PART1_FORMAT = @"--{1}
Content-Disposition: form-data; name=""dossier""; filename=""{0}""
Content-Type: application/octet-stream

";
        private const string REQ_CONTENT_PART2_FORMAT = @"--{0}--";

        private const string REQ_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
        private const string REQ_CONTENT_TYPE = "multipart/form-data; boundary=";
        private const string URL_PREVIEW = "http://wot-dossier.appspot.com/preview";
        private const string URL_PERSIST = "http://wot-dossier.appspot.com/persist-dossier";
        private const string URL_SECTION_UPDATE = "http://wot-dossier.appspot.com/dossier-section/{0}/update";
        private const string URL_UPLOAD = "http://wot-dossier.appspot.com/reupload/{0}/{1}/{2}";

        #endregion

        /// <summary>
        /// Uploads the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        public int Upload(FileInfo info)
        {
            string boundary = string.Format(REQ_BOUNDARY, DateTime.Now.Ticks.ToString("x"));
            string firstPart = string.Format(REQ_CONTENT_PART1_FORMAT, info.Name, boundary);
            string secondPart = string.Format(REQ_CONTENT_PART2_FORMAT, boundary);

            byte[] fileBytes = File.ReadAllBytes(info.FullName);
            byte[] contentPart1Bytes = Encoding.UTF8.GetBytes(firstPart);
            byte[] contentPart2Bytes = Encoding.UTF8.GetBytes(secondPart);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL_PREVIEW);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = REQ_USER_AGENT;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.ContentLength = info.Length;
            request.ContentType = REQ_CONTENT_TYPE + boundary;
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

            string fileName = null;
            using (Stream stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string readToEnd = streamReader.ReadToEnd();

                    Match match = Regex.Match(readToEnd, @"g_dossier_file\s=\s'(?<Value>.*)'");
                    fileName = match.Groups["Value"].Value;
                }
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                request = (HttpWebRequest) HttpWebRequest.Create(URL_PERSIST);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = REQ_USER_AGENT;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentLength = fileName.Length;
                request.Method = WebRequestMethods.Http.Post;

                byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);
                // Открываем поток для записи
                uploadStream = request.GetRequestStream();
                // Записываем в поток (это и есть POST запрос(заполнение форм))
                uploadStream.Write(fileNameBytes, 0, fileNameBytes.Length);
                // Закрываем поток
                uploadStream.Flush();
                uploadStream.Close();

                webResponse = request.GetResponse();

                using (Stream stream = webResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string readToEnd = streamReader.ReadToEnd();
                        AppSpotResponse appSpotResponse = JsonConvert.DeserializeObject<AppSpotResponse>(readToEnd);
                        return appSpotResponse.id;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Uploads the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        public void Upload(FileInfo info, int id)
        {
            string uploadData = new Uri(string.Format(URL_SECTION_UPDATE, id)).Get();

            AppSpotResponse appSpotResponse = JsonConvert.DeserializeObject<AppSpotResponse>(uploadData);

            string uploadUrl = string.Format(URL_UPLOAD, id, appSpotResponse.expires, appSpotResponse.secret);

            string boundary = string.Format(REQ_BOUNDARY, DateTime.Now.Ticks.ToString("x"));
            string firstPart = string.Format(REQ_CONTENT_PART1_FORMAT, info.Name, boundary);
            string secondPart = string.Format(REQ_CONTENT_PART2_FORMAT, boundary);

            byte[] fileBytes = File.ReadAllBytes(info.FullName);
            byte[] contentPart1Bytes = Encoding.UTF8.GetBytes(firstPart);
            byte[] contentPart2Bytes = Encoding.UTF8.GetBytes(secondPart);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uploadUrl);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = REQ_USER_AGENT;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.ContentLength = info.Length;
            request.ContentType = REQ_CONTENT_TYPE + boundary;
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

            using (Stream stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string readToEnd = streamReader.ReadToEnd();
                }
            }
        }
    }

    public class AppSpotResponse
    {
        public string secret { get; set; }
        public int expires { get; set; }
        public int id { get; set; }
    }
}
