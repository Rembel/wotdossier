namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticRatings
    {
        //int DamageRatingRev1 { get; }
        int MarkOfMastery { get; set; }

        double EffRating { get; }
        double WN7Rating { get; }
        double WN8Rating { get; }
        double KievArmorRating { get; }

        double NoobRating { get; }
        double XEFF { get; }
        double XWN { get; }
        double PerformanceRating { get; set; }
        double RBR { get; set; }
    }
}