namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowRatings
    {
        int DamageRatingRev1 { get; }
        int MarkOfMastery { get; set; }

        double EffRating { get; }
        double WN7Rating { get; }
        double WN8Rating { get; }
        double KievArmorRating { get; }
    }
}