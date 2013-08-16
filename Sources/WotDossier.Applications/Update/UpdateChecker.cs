using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using WotDossier.Domain;
using WotDossier.Framework.Presentation.Services;

namespace WotDossier.Applications.Update
{
    public class UpdateChecker
    {
        public static void CheckForUpdates()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                {
                    AppSettings appSettings = SettingsReader.Get();
                    if (appSettings.CheckForUpdates)
                    {
                        if (IsNewVersionAvailable())
                        {
                            string targetURL = @"http://code.google.com/p/wotdossier/downloads/list";
                            Process.Start(targetURL);
                        }
                    }
                }, null);
        }

        private static bool IsNewVersionAvailable()
        {
            WebRequest request = HttpWebRequest.Create("http://wotdossier.googlecode.com/files/Version.txt");
            WebResponse webResponse = request.GetResponse();
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string content = reader.ReadToEnd();

                string[] data = content.Split('\n');

                Version currentVersion = new Version(ApplicationInfo.Version);

                Version newVersion = new Version(data[0].Split(':')[1].Trim());
                return newVersion > currentVersion;
            }
        }
    }
}
