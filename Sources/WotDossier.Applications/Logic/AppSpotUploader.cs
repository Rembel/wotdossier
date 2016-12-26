using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WotDossier.Common.Extensions;

namespace WotDossier.Applications.Logic
{
    public class AppSpotUploader
    {
        #region Constants

        private const string REQ_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
        private const string REQ_CONTENT_TYPE = "multipart/form-data; boundary=";
        private const string URL_PREVIEW = "http://wot-dossier.appspot.com/preview";
        private const string URL_PERSIST = "http://wot-dossier.appspot.com/persist-dossier";
        private const string URL_SECTION_UPDATE = "http://wot-dossier.appspot.com/dossier-section/{0}/update";
        private const string URL_UPLOAD = "http://wot-dossier.appspot.com/reupload/{0}/{1}/{2}";

        #endregion

        /// <summary>
        /// Uploads the specified dossier cache file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public long Upload(FileInfo file)
        {
            RequestComposer composer = new RequestComposer();
            byte[] requestBytes = composer.File(file, "dossier")
                .End()
                .GetRequestBytes();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL_PREVIEW);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = REQ_USER_AGENT;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.ContentType = REQ_CONTENT_TYPE + composer.Boundary;
            request.Method = WebRequestMethods.Http.Post;

            // Длинна запроса (обязательный параметр)
            request.ContentLength = requestBytes.Length;
            // Открываем поток для записи
            Stream uploadStream = request.GetRequestStream();
            // Записываем в поток (это и есть POST запрос(заполнение форм))
            uploadStream.Write(requestBytes, 0, requestBytes.Length);
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
                request = (HttpWebRequest)HttpWebRequest.Create(URL_PERSIST);
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
        /// <param name="file">The info.</param>
        public void Update(FileInfo file, long id)
        {
            RequestComposer composer = new RequestComposer();
            byte[] requestBytes = composer.File(file, "dossier")
                .End()
                .GetRequestBytes();

            AppSpotResponse appSpotResponse = new Uri(string.Format(URL_SECTION_UPDATE, id)).Get<AppSpotResponse>();

            string uploadUrl = string.Format(URL_UPLOAD, id, appSpotResponse.expires, appSpotResponse.secret);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uploadUrl);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = REQ_USER_AGENT;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.ContentType = REQ_CONTENT_TYPE + composer.Boundary;
            request.Method = WebRequestMethods.Http.Post;

            // Длинна запроса (обязательный параметр)
            request.ContentLength = requestBytes.Length;
            // Открываем поток для записи
            Stream uploadStream = request.GetRequestStream();
            // Записываем в поток (это и есть POST запрос(заполнение форм))
            uploadStream.Write(requestBytes, 0, requestBytes.Length);
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
        public long expires { get; set; }
        public long id { get; set; }
    }
}
