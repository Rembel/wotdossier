namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings
    {
        int Battles { get; set; }
        double Winrate { get; set; }
        int AverageDamage { get; set; }
        double KillDeathRatio { get; set; }
        int NewEffRating { get; set; }
        int WN6 { get; set; }
        int DamageRatingRev1 { get; set; }
        double KievArmorRating { get; set; }
        int MarkOfMastery { get; set; }
    }
}