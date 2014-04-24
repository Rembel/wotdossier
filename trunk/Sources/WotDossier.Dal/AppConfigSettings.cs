using System;
using System.Configuration;
using System.Globalization;

namespace WotDossier.Dal
{
    /*<add key="api" value="1.9"/>
    <add key="search_api" value="1.1"/>
    <add key="source_token" value="WG-WoT_Assistant-1.3.2"/>
    <add key="settings-path" value="\app.settings"/>*/

    /// <summary>
    /// 
    /// </summary>
    public class AppConfigSettings
    {
        public const string FILE_NAME_FORMAT = "wotdossier_screenshot_{0:D2}.png";

        /// <summary>
        /// Gets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        public static string ApiVersion
        {
            get { return ConfigurationManager.AppSettings["api"] ?? "1.9"; }
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <param name="cluster">The cluster.</param>
        /// <returns></returns>
        public static string GetAppId(string cluster)
        {
            return ConfigurationManager.AppSettings["app_id." + cluster] ?? "171745d21f7f98fd8878771da1000a31";
        }

        /// <summary>
        /// Gets the path to settings file.
        /// </summary>
        /// <value>
        /// The settings path.
        /// </value>
        public static string SettingsPath
        {
            get { return ConfigurationManager.AppSettings["settings-path"] ?? "\app.settings"; }
        }

        /// <summary>
        /// Gets the slice time.
        /// </summary>
        /// <value>
        /// The slice time.
        /// </value>
        public static double SliceTime
        {
            get
            {
                string time = ConfigurationManager.AppSettings["slice-time"];
                TimeSpan result;
                if (!string.IsNullOrEmpty(time))
                {
                    if (TimeSpan.TryParse(time, CultureInfo.InvariantCulture, out result))
                    {
                        return result.Hours;
                    }
                }
                // at 4 hours every day
                return 4;
            }
        }

        /// <summary>
        /// Gets the download URL.
        /// </summary>
        /// <value>
        /// The download URL.
        /// </value>
        public static string DownloadUrl
        {
            get { return ConfigurationManager.AppSettings["update.download-url"] ?? "http://goo.gl/QpdbIUt"; }
        }

        /// <summary>
        /// Gets the version URL.
        /// </summary>
        /// <value>
        /// The version URL.
        /// </value>
        public static string VersionUrl
        {
            get { return ConfigurationManager.AppSettings["update.check-version-url"] ?? "https://docs.google.com/document/d/1dyaFXMEECT6o374sR-2ME-7zdqaSJItV-QNt7xfZ1n0/export?format=txt"; }
        }

        /// <summary>
        /// Gets the forum URL.
        /// </summary>
        /// <value>
        /// The forum URL.
        /// </value>
        public static string ForumUrl
        {
            get { return ConfigurationManager.AppSettings["update.forum-url"] ?? "http://forum.worldoftanks.ru/index.php?/topic/890389-wotdossier/"; }
        }

        /// <summary>
        /// Gets the download URI.
        /// </summary>
        /// <value>
        /// The download URI.
        /// </value>
        public static Uri DownloadUri
        {
            get { return new Uri(DownloadUrl); }
        }

        /// <summary>
        /// Gets the forum URI.
        /// </summary>
        /// <value>
        /// The forum URI.
        /// </value>
        public static Uri ForumUri
        {
            get { return new Uri(ForumUrl); }
        }
    }
}
