namespace WotDossier.Domain
{
    public class AppSettings
    {
        private string _language = "ru-RU";
        private StatisticPeriod _period;
        private string _replaysUploadServerPath = "http://wotreplays.ru/site/upload";
        private string _replaysFolderPath;
        public string PlayerId { get; set; }
        public string Server { get; set; }

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
    }
}
