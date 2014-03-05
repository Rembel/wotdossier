namespace WotDossier.Domain.Interfaces
{
    public interface ITankFilterable : ITankRowBase
    {
        int CountryId { get; set; }
        int Type { get; set; }
        bool IsPremium { get; set; }
        bool IsFavorite { get; set; }
    }
}