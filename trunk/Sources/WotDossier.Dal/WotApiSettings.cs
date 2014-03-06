using System;
using System.Configuration;
using System.Globalization;

namespace WotDossier.Dal
{
    /*<add key="api" value="1.9"/>
    <add key="search_api" value="1.1"/>
    <add key="source_token" value="WG-WoT_Assistant-1.3.2"/>
    <add key="settings-path" value="\app.settings"/>*/

    public class AppConfigSettings
    {
        public const string FILE_NAME_FORMAT = "wotdossier_screenshot_{0:D2}.png";

        public static string ApiVersion
        {
            get { return ConfigurationManager.AppSettings["api"] ?? "1.9"; }
        }

        public static string GetAppId(string cluster)
        {
            return ConfigurationManager.AppSettings["app_id." + cluster] ?? "171745d21f7f98fd8878771da1000a31";
        }

        public static string SettingsPath
        {
            get { return ConfigurationManager.AppSettings["settings-path"] ?? "\app.settings"; }
        }

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

        public static string DownloadUrl
        {
            get { return ConfigurationManager.AppSettings["update.download-url"] ?? "http://code.google.com/p/wotdossier/downloads/list"; }
        }

        public static string VersionUrl
        {
            get { return ConfigurationManager.AppSettings["update.check-version-url"] ?? "http://wotdossier.googlecode.com/files/Version.txt"; }
        }
    }
}
