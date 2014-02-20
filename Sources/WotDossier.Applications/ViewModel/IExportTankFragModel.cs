namespace WotDossier.Applications.ViewModel
{
    internal interface IExportTankFragModel
    {
        string Tank { get; set; }
        double Tier { get; set; }
        int CountryId { get; set; }
        int Type { get; set; }
        int TankId { get; set; }
        string FragTank { get; set; }
        double FragTier { get; set; }
        int FragCountryId { get; set; }
        int FragType { get; set; }
        int FragTankId { get; set; }
        int Count { get; set; }
    }
}