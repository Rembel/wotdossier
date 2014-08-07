using System;

namespace WotDossier.Domain
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class AppSettings
    {
        private string _language = "ru-RU";
        private string _replaysUploadServerPath = "http://wotreplays.ru/site/upload";
        private bool _checkForUpdates = true;

        private TankFilterSettings _tankFilterSettings = new TankFilterSettings();
        private PeriodSettings _periodSettings = new PeriodSettings();
        private string _server = "ru";
        
        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        public int PlayerId { get; set; }

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
        /// Gets or sets the path to wot exe.
        /// </summary>
        /// <value>
        /// The path to wot exe.
        /// </value>
        public string PathToWotExe { get; set; }

        /// <summary>
        /// Gets or sets the replays folder path.
        /// </summary>
        /// <value>
        /// The replays folder path.
        /// </value>
        public string ReplaysFolderPath { get; set; }

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
        /// Gets or sets the replays upload server path.
        /// </summary>
        /// <value>
        /// The replays upload server path.
        /// </value>
        public string ReplaysUploadServerPath
        {
            get { return _replaysUploadServerPath; }
            set { _replaysUploadServerPath = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check for updates].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check for updates]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckForUpdates
        {
            get { return _checkForUpdates; }
            set { _checkForUpdates = value; }
        }

        /// <summary>
        /// Gets or sets the new version check last date.
        /// </summary>
        /// <value>
        /// The new version check last date.
        /// </value>
        public DateTime NewVersionCheckLastDate { get; set; }

        /// <summary>
        /// Gets or sets the tank filter settings.
        /// </summary>
        /// <value>
        /// The tank filter settings.
        /// </value>
        public TankFilterSettings TankFilterSettings
        {
            get { return _tankFilterSettings; }
            set { _tankFilterSettings = value; }
        }

        /// <summary>
        /// Gets or sets the period settings.
        /// </summary>
        /// <value>
        /// The period settings.
        /// </value>
        public PeriodSettings PeriodSettings
        {
            get { return _periodSettings; }
            set { _periodSettings = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic load statistic].
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic load statistic]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadStatistic { get; set; }

        private DossierTheme _theme = DossierTheme.Black;
        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public DossierTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        private bool _showExtendedReplaysData = true;
        /// <summary>
        /// Gets or sets a value indicating whether [show extended replays data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show extended replays data]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExtendedReplaysData
        {
            get { return _showExtendedReplaysData; }
            set { _showExtendedReplaysData = value; }
        }

        private bool _useIncompleteReplaysResultsForCharts = false;
        public bool UseIncompleteReplaysResultsForCharts
        {
            get { return _useIncompleteReplaysResultsForCharts; }
            set { _useIncompleteReplaysResultsForCharts = value; }
        }
    }
}
