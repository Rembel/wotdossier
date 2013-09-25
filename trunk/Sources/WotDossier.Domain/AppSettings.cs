using System;

namespace WotDossier.Domain
{
    public class AppSettings
    {
        private string _language = "ru-RU";
        private StatisticPeriod _period;
        private string _replaysUploadServerPath = "http://wotreplays.ru/site/upload";
        private string _replaysFolderPath;
        private DateTime? _prevDate;
        private bool _checkForUpdates = true;
        private int _lastNBattles = 100;
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public string Server { get; set; }

        public int LastNBattles
        {
            get { return _lastNBattles; }
            set { _lastNBattles = value; }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public StatisticPeriod Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public string ReplaysUploadServerPath
        {
            get { return _replaysUploadServerPath; }
            set { _replaysUploadServerPath = value; }
        }

        public string ReplaysFolderPath
        {
            get { return _replaysFolderPath; }
            set { _replaysFolderPath = value; }
        }

        public DateTime? PrevDate
        {
            get { return _prevDate; }
            set { _prevDate = value; }
        }

        public bool CheckForUpdates
        {
            get { return _checkForUpdates; }
            set { _checkForUpdates = value; }
        }
    }
}
