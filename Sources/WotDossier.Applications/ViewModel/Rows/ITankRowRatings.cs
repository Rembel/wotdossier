namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings
    {
        int BattlesCount { get; set; }
        double WinsPercent { get; }
        double AvgDamageDealt { get; }
        double KillDeathRatio { get; }
        int DamageRatingRev1 { get; }
        int MarkOfMastery { get; set; }

        double EffRating { get; }
        double WN6Rating { get; }
        double WN7Rating { get; }
        double KievArmorRating { get; }
    }
}