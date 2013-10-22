using System;

namespace WotDossier.Domain
{
    public class AppSettings
    {
        private string _language = "ru-RU";
        private string _replaysUploadServerPath = "http://wotreplays.ru/site/upload";
        private bool _checkForUpdates = true;

        private TankFilterSettings _tankFilterSettings = new TankFilterSettings();
        private PeriodSettings _periodSettings = new PeriodSettings();

        public string PlayerName { get; set; }

        public int PlayerId { get; set; }

        public string Server { get; set; }

        public string PathToWotExe { get; set; }

        public string ReplaysFolderPath { get; set; }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string ReplaysUploadServerPath
        {
            get { return _replaysUploadServerPath; }
            set { _replaysUploadServerPath = value; }
        }

        public bool CheckForUpdates
        {
            get { return _checkForUpdates; }
            set { _checkForUpdates = value; }
        }

        public DateTime NewVersionCheckLastDate { get; set; }

        public TankFilterSettings TankFilterSettings
        {
            get { return _tankFilterSettings; }
            set { _tankFilterSettings = value; }
        }

        public PeriodSettings PeriodSettings
        {
            get { return _periodSettings; }
            set { _periodSettings = value; }
        }
    }
}
