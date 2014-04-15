namespace WotDossier.Domain.Server
{
    public class Statistics
    {
        public StatisticPart clan { get; set; }
        public StatisticPart all { get; set; }
        public StatisticPart company { get; set; }
        public int max_xp { get; set; }
    }
}