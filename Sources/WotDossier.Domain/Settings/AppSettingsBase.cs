namespace WotDossier.Domain.Settings
{
    public class AppSettingsBase
    {
        private string _language = "ru-RU";
        private string _server = "ru";
        private DossierTheme _theme = DossierTheme.Black;

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public DossierTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }
    }
}