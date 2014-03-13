using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework.Presentation.Services;

namespace WotDossier.Applications.Update
{
    public class UpdateChecker
    {
        private static readonly ILog Logger = LogManager.GetLogger("UpdateChecker");

        public static void CheckForUpdates()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                {
                    AppSettings appSettings = SettingsReader.Get();
                    //one day from last check
                    if (appSettings.CheckForUpdates && (DateTime.Now - appSettings.NewVersionCheckLastDate).Days >= 1 )
                    {
                        appSettings.NewVersionCheckLastDate = DateTime.Now;
                        SettingsReader.Save(appSettings);

                        CheckNewVersionAvailable();
                    }
                }, null);
        }

        public static void CheckNewVersionAvailable()
        {
            Version currentVersion = new Version(ApplicationInfo.Version);
            Version newVersion = GetServerVersion();

            var isNewVersionAvailable = newVersion > currentVersion;

            if (isNewVersionAvailable &&
                MessageBox.Show(string.Format(Resources.Resources.Msg_NewVersion, newVersion), ApplicationInfo.ProductName,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start(AppConfigSettings.DownloadUrl);
            }
        }

        private static Version GetServerVersion()
        {
            Version newVersion = new Version(ApplicationInfo.Version);
            try
            {
                WebRequest request = HttpWebRequest.Create(AppConfigSettings.VersionUrl);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                WebResponse webResponse = request.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string content = reader.ReadToEnd();

                    string[] data = content.Split('\n');

                    newVersion = new Version(data[0].Split(':')[1].Trim());
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error on version check", e);
            }
            return newVersion;
        }
    }
}
