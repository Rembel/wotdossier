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

        /// <summary>
        /// Checks for updates.
        /// </summary>
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

        /// <summary>
        /// Checks the new version available.
        /// </summary>
        public static void CheckNewVersionAvailable()
        {
            Version currentVersion = new Version(ApplicationInfo.Version);
            DownloadedVersionInfo info = GetServerVersion();

            var isNewVersionAvailable = info.LatestVersion > currentVersion;

            if (isNewVersionAvailable &&
                MessageBox.Show(string.Format(Resources.Resources.Msg_NewVersion, info.LatestVersion), ApplicationInfo.ProductName,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start(AppConfigSettings.DownloadUrl);
                //Download(info);
            }
        }

        private static void Download(DownloadedVersionInfo info)
        {
            string filepath = "";
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(info.InstallerUrl);
                request.MaximumAutomaticRedirections = 1;
                request.AllowAutoRedirect = true;
                WebResponse response = request.GetResponse();
                string filename = "";
                int contentLength = 0;
                for (int a = 0; a < response.Headers.Count; a++)
                {
                    try
                    {
                        string val = response.Headers.Get(a);

                        switch (response.Headers.GetKey(a).ToLower())
                        {
                            case "content-length":
                                contentLength = Convert.ToInt32(val);
                                break;
                            case "content-disposition":
                                string[] v2 = val.Split(';');
                                foreach (string s2 in v2)
                                {
                                    string[] v3 = s2.Split('=');
                                    if (v3.Length == 2)
                                    {
                                        if (v3[0].Trim().ToLower() == "filename")
                                        {
                                            char[] sss = {' ', '"'};
                                            filename = v3[1].Trim(sss);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (filename.Length == 0) filename = "installer.exe";
                filepath = Path.Combine(Path.GetTempPath(), filename);

                if (File.Exists(filepath))
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                    if (File.Exists(filepath))
                    {
                        string rname = Path.GetRandomFileName();
                        rname.Replace('.', '_');
                        rname += ".exe";
                        filepath = Path.Combine(Path.GetTempPath(), rname);
                    }
                }
                Stream stream = response.GetResponseStream();
                int pos = 0;
                var buf2 = new byte[8192];
                var fs = new FileStream(filepath, FileMode.CreateNew);
                while ((0 == contentLength) || (pos < contentLength))
                {
                    int maxBytes = 8192;
                    if ((0 != contentLength) && ((pos + maxBytes) > contentLength)) maxBytes = contentLength - pos;
                    int bytesRead = stream.Read(buf2, 0, maxBytes);
                    if (bytesRead <= 0) break;
                    fs.Write(buf2, 0, bytesRead);
                    pos += bytesRead;
                }
                fs.Close();
                stream.Close();
            }

            catch
            {
                // when something goes wrong - at least do the cleanup :)
                if (filepath.Length > 0)
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static DownloadedVersionInfo GetServerVersion()
        {
            Version newVersion = new Version(ApplicationInfo.Version);
            string installerUrl = "http://res-mods.ru/download/mod/4137";
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
                    var urlData = data[1];
                    installerUrl = urlData.Substring(urlData.IndexOf("http")).Trim();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error on version check", e);
            }
            return new DownloadedVersionInfo { InstallerUrl = installerUrl, LatestVersion = newVersion };
        }
    }
}
