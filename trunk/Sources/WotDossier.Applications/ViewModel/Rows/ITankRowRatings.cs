namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings
    {
        int BattlesCount { get; set; }
        double Winrate { get; set; }
        int AverageDamage { get; set; }
        double KillDeathRatio { get; set; }
        double EffRating { get; set; }
        double WN6Rating { get; set; }
        int DamageRatingRev1 { get; set; }
        double KievArmorRating { get; set; }
        int MarkOfMastery { get; set; }
    }
}