namespace WotDossier.Domain
{
    public class AppSettings
    {
        private string _language = "ru-RU";
        private StatisticPeriod _period;
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
    }
}
