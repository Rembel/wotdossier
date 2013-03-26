using System.Configuration;

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
    }
}
