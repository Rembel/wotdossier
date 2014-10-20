using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Common.Logging;
using Ionic.Zip;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;
using WotDossier.Framework.Presentation.Services;

namespace WotDossier.Applications.Update
{
    public class UpdateChecker
    {
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
        private const string ACCEPT_HEADER = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Checks for updates.
        /// </summary>
        public static void CheckForUpdates()
        {
            AppSettings appSettings = SettingsReader.Get();
            //one day from last check
            if (appSettings.CheckForUpdates && (DateTime.Now.Date != appSettings.NewVersionCheckLastDate.Date))
            {
                appSettings.NewVersionCheckLastDate = DateTime.Now.Date;
                SettingsReader.Save(appSettings);

                CheckNewVersionAvailable();
            }
        }

        /// <summary>
        ///     Checks the new version available.
        /// </summary>
        public static void CheckNewVersionAvailable()
        {
            var currentVersion = new Version(ApplicationInfo.Version);
            DownloadedVersionInfo info = GetServerVersion();

            bool isNewVersionAvailable = info.InstallerVersion > currentVersion;

            if (isNewVersionAvailable &&
                MessageBox.Show(string.Format(Resources.Resources.Msg_NewVersion, info.InstallerVersion),
                    ApplicationInfo.ProductName,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                {
                    string filepath = Download(info.InstallerUrl, "installer.exe");
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        Process.Start(filepath);
                    }
                }
            }

            AppSettings appSettings = SettingsReader.Get();
            var dataVersion = new Version(appSettings.ExternalDataVersion ?? ApplicationInfo.Version);
            appSettings.ExternalDataVersion = info.DataVersion.ToString();
            SettingsReader.Save(appSettings);

            if (info.DataVersion > dataVersion)
            {
                using (new WaitCursor())
                {
                    string filepath = Download(info.DataUrl, "data.zip");
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        string currentDirectory = Folder.AssemblyDirectory();
                        var targetFolder = Path.Combine(currentDirectory, "External");
                        Unzip(filepath, targetFolder);
                    }
                }
            }
        }

        private static void Unzip(string filepath, string targetFolder)
        {
            using (var zip = new ZipFile(filepath, Encoding.GetEncoding((int) CodePage.CyrillicDOS)))
            {
                zip.ExtractAll(targetFolder, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        private static string Download(string url, string saveAs)
        {
            string filepath = string.Empty;
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = USER_AGENT;
                request.Accept = ACCEPT_HEADER;
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

                if (filename.Length == 0)
                {
                    filename = saveAs;
                }
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
                        rname += Path.GetExtension(saveAs);
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

                return filepath;
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
            return null;
        }

        private static DownloadedVersionInfo GetServerVersion()
        {
            AppSettings appSettings = SettingsReader.Get();
            Version newVersion = new Version(ApplicationInfo.Version);
            string installerUrl = null;
            Version dataVersion = null;
            string dataUrl = null;
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(AppConfigSettings.VersionUrl);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = USER_AGENT;
                request.Accept = ACCEPT_HEADER;
                //for analytics
                request.Referer = string.Format("http://wotdossier_{0}.{1}/", ApplicationInfo.Version,
                    appSettings.Server ?? "com");
                WebResponse webResponse = request.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    string content = reader.ReadToEnd();

                    string[] data = content.Split('\n');
                    
                    newVersion = new Version(data[0].Split(':')[1].Trim());

                    installerUrl = data[1].Substring(data[1].IndexOf("http")).Trim();

                    dataVersion = new Version(data[2].Split(':')[1].Trim());

                    dataUrl = data[3].Substring(data[3].IndexOf("http")).Trim();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error on version check", e);
            }
            return new DownloadedVersionInfo {InstallerUrl = installerUrl, InstallerVersion = newVersion, DataUrl = dataUrl, DataVersion = dataVersion};
        }
    }
}