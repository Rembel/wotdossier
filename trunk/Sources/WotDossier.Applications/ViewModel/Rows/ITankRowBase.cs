using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public interface ITankRowBase
    {
        int Tier { get; set; }
        TankIcon Icon { get; set; }
        string Tank { get; set; }
        int TankType { get; set; }
        int CountryId { get; set; }
    }
}