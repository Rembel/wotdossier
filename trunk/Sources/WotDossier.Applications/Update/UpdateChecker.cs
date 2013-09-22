using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WotDossier.Domain;
using WotDossier.Framework.Presentation.Services;

namespace WotDossier.Applications.Update
{
    public class UpdateChecker
    {
        private const string DownloadsList = @"http://code.google.com/p/wotdossier/downloads/list";
        private const string VersionUrl = "http://wotdossier.googlecode.com/files/Version.txt";

        public static void CheckForUpdates()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                {
                    AppSettings appSettings = SettingsReader.Get();
                    if (appSettings.CheckForUpdates)
                    {
                        Version currentVersion = new Version(ApplicationInfo.Version);
                        Version newVersion = GetServerVersion();
                        
                        var isNewVersionAvailable = newVersion > currentVersion;

                        if (isNewVersionAvailable && MessageBox.Show(string.Format(Resources.Resources.Msg_NewVersion, newVersion), ApplicationInfo.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            Process.Start(DownloadsList);
                        }
                    }
                }, null);
        }

        private static Version GetServerVersion()
        {
            Version newVersion;
            WebRequest request = HttpWebRequest.Create(VersionUrl);
            WebResponse webResponse = request.GetResponse();
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string content = reader.ReadToEnd();

                string[] data = content.Split('\n');

                newVersion = new Version(data[0].Split(':')[1].Trim());
            }
            return newVersion;
        }
    }
}
