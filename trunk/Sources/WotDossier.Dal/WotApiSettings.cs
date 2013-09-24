using System;
using System.Configuration;
using System.Globalization;

namespace WotDossier.Dal
{
    /*<add key="api" value="1.9"/>
    <add key="search_api" value="1.1"/>
    <add key="source_token" value="WG-WoT_Assistant-1.3.2"/>
    <add key="settings-path" value="\app.settings"/>*/

    public class WotDossierSettings
    {
        public static string ApiVersion
        {
            get { return ConfigurationManager.AppSettings["api"] ?? "1.9"; }
        }

        public static string SearchApiVersion
        {
            get { return ConfigurationManager.AppSettings["search_api"] ?? "1.1"; }
        }

        public static string SourceToken
        {
            get { return ConfigurationManager.AppSettings["source_token"] ?? "WG-WoT_Assistant-1.3.2"; }
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
    }
}
