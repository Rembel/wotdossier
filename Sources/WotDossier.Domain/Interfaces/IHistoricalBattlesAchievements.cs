namespace WotDossier.Domain.Interfaces
{
    public interface IHistoricalBattlesAchievements
    {
        int GuardsMan { get; set; }
        int MakerOfHistory { get; set; }
        int BothSidesWins { get; set; }
        int WeakVehiclesWins { get; set; }
    }
}