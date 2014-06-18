using System;
using System.IO;
using System.Net;

namespace WotDossier.Common.Extensions
{
    public static class UriExtensions
    {
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
    }
}