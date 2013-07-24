namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings : IRating
    {
        int BattlesCount { get; set; }
        double WinsPercent { get; }
        double AvgDamageDealt { get; }
        double KillDeathRatio { get; }
        int DamageRatingRev1 { get; }
        int MarkOfMastery { get; set; }
    }
}