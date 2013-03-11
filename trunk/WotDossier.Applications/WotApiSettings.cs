using System.Configuration;

namespace WotDossier.Applications
{
    public class WotDossierSettings
    {
        public static string ApiVersion
        {
            get { return ConfigurationManager.AppSettings["api"]; }
        }

        public static string SearchApiVersion
        {
            get { return ConfigurationManager.AppSettings["search_api"]; }
        }

        public static string SourceToken
        {
            get { return ConfigurationManager.AppSettings["source_token"]; }
        }

        public static string SettingsPath
        {
            get { return ConfigurationManager.AppSettings["settings-path"]; }
        }
    }
}
