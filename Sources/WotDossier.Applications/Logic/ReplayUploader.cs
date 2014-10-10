using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Windows;
using Common.Logging;
using Newtonsoft.Json;
using WotDossier.Common;
using WotDossier.Common.Extensions;

namespace WotDossier.Applications.Logic
{
    public class ReplayUploader
    {
        #region Constants

        private const string REQ_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
        
        private const string REQ_CONTENT_TYPE = "multipart/form-data; boundary=";
        private const string FIND_REPLAY_URL = "http://wotreplays.{0}/api/find/md5/{1}";
        private const string UPLOAD_REPLAY_URL = "http://wotreplays.{0}/api/upload/bwId/{1}/username/{2}";

        #endregion

        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Uploads the specified info.
        /// </summary>
        /// <param name="file">The info.</param>
        /// <param name="replayName">Name of the replay.</param>
        /// <param name="replayDescription">The replay description.</param>
        /// <param name="uploadUrl">The upload URL.</param>
        public string Upload(FileInfo file, string replayName, string replayDescription, string uploadUrl)
        {
            string url;

            string str = file.MD5();
            string result = new Uri(string.Format(FIND_REPLAY_URL, SettingsReader.Get().Server, str)).Get();

            WotReplaysSiteResponse response = null;

            try
            {
                response = JsonConvert.DeserializeObject<WotReplaysSiteResponse>(result);
            }
            catch (Exception) {/*ignore*/}

            //if replay not found
            if (response == null || (response.Result.HasValue && response.Result == false))
            {
                //http://wotreplays.{0}/api/upload/bwId/{1}/username/{2}        
                var cookieContainer = LoadCookies(uploadUrl);

                if (!IsAuthentificated(cookieContainer, uploadUrl))
                {
                    throw new AuthenticationException(string.Format("User not authentificated on site {0}", uploadUrl));
                }

                RequestComposer composer = new RequestComposer();
                byte[] requestBytes = composer.File(file)
                    .Title(replayName)
                    .Description(replayDescription)
                    .Secret("0")
                    .End()
                    .GetRequestBytes();

                HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(uploadUrl);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = REQ_USER_AGENT;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.CookieContainer = cookieContainer;
                request.ContentLength = file.Length;
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

                url = webResponse.ResponseUri.ToString();

                using (Stream stream = webResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string readToEnd = streamReader.ReadToEnd();
                    }
                }
            }
            else
            {
                url = response.Url;
            }

            Clipboard.SetText(url);

            return url;
        }

        private string Send(FileInfo file, string url, string hidden)
        {
            string result = null;
            try
            {
                RequestComposer composer = new RequestComposer();
                byte[] requestBytes = composer.File(file)
                    .Secret(hidden)
                    .End()
                    .GetRequestBytes();
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = REQ_CONTENT_TYPE + composer.Boundary;
                request.ServicePoint.Expect100Continue = false;
                request.Timeout = 10000;
                request.ReadWriteTimeout = 10000;
                request.ContentLength = requestBytes.Length;
                
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                try
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            StreamReader streamReader = new StreamReader(stream);
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    result = null;
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
                result = null;
            }
            return result;
        }

        public WotReplaysSiteResponse Upload(FileInfo file, int userId, string username, string hidden = "0")
        {
            WotReplaysSiteResponse response = null;
            if (userId != 0 && !string.IsNullOrEmpty(username))
            {
                try
                {
                    string str = file.MD5();

                    try
                    {
                        response = JsonConvert.DeserializeObject<WotReplaysSiteResponse>(new Uri(string.Format(FIND_REPLAY_URL, SettingsReader.Get().Server, str)).Get());
                    }
                    catch (Exception) {/*ignore*/}

                    if (response == null || response.Result.HasValue && (response.Result == false))
                    {
                        var result = Send(file, string.Format(UPLOAD_REPLAY_URL, SettingsReader.Get().Server, userId, username), hidden);
                        if (result != null)
                        {
                            response = JsonConvert.DeserializeObject<WotReplaysSiteResponse>(result);
                        }
                    }
                }
                catch (Exception exception)
                {
                    _log.Error(exception);
                }
            }
            return response;
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

        public static CookieContainer LoadCookies(String url)
        {
            CookieContainer cookies = new CookieContainer();
            Uri uri = new Uri(url);

            StringBuilder cookieBuilder = new StringBuilder(new string(' ', 256), 256);

            int cookieSize = cookieBuilder.Length;

            if (!NativeMethods.InternetGetCookie(url, null, cookieBuilder, out cookieSize))
            {
                if (cookieSize == 0)
                {
                    return cookies;
                }
                cookieBuilder = new StringBuilder(cookieSize);
                NativeMethods.InternetGetCookie(url, null, cookieBuilder, out cookieSize);
            }

            cookies.SetCookies(uri, cookieBuilder.ToString().Replace(";", ","));
            return cookies;
        }
    }
}
