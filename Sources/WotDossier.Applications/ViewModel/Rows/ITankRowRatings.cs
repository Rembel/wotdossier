namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings
    {
        int BattlesCount { get; set; }
        double WinsPercent { get; }
        double AvgDamageDealt { get; }
        double KillDeathRatio { get; }
        double EffRating { get; }
        double WN6Rating { get; }
        int DamageRatingRev1 { get; }
        double KievArmorRating { get; }
        int MarkOfMastery { get; set; }
    }
}