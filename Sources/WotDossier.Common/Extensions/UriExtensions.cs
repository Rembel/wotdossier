using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace WotDossier.Common.Extensions
{
    public static class UriExtensions
    {
        public static void Delete(this Uri uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "DELETE";
                request.GetResponse();
            }
            catch (Exception e)
            {
                
            }
        }

        public static string Get(this Uri uri)
        {
            string str = null;
            var client = new WebClient();
            client.Proxy.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                Stream stream = client.OpenRead(uri);
                if (stream != null)
                {
                    str = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (WebException)
            {
            }
            finally
            {
                client.Dispose();
            }
            return str;
        }

        public static void Post(this Uri uri, string data)
        {
            string str = null;
            var client = new WebClient();
            client.Proxy.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                //client.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString(CultureInfo.InvariantCulture);
                client.UploadStringAsync(uri, data);
            }
            catch (WebException)
            {
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}