using System;
using System.IO;
using System.Net;

namespace WotDossier.Common
{
    public static class HttpUtils
    {
        public static string Get(this Uri uri)
        {
            string str = null;
            WebClient client = new WebClient();
            WebClient client2 = client;
            try
            {
                str = new StreamReader(client.OpenRead(uri)).ReadToEnd();
            }
            catch (WebException)
            {
            }
            finally
            {
                if (client2 != null)
                {
                    client2.Dispose();
                }
            }
            return str;
        }
    }
}
