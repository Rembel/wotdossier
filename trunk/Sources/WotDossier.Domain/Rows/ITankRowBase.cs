using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public interface ITankRowBase
    {
        int Tier { get; set; }
        TankContour Icon { get; set; }
        string Tank { get; set; }
        int TankType { get; set; }
        int CountryId { get; set; }
    }
}