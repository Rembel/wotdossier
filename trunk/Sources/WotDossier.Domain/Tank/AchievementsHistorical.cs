using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    public class AchievementsHistorical : IHistoricalBattlesAchievements
    {
        public int GuardsMan { get; set; }
        public int MakerOfHistory { get; set; }
        public int BothSidesWins { get; set; }
        public int WeakVehiclesWins { get; set; }
    }
}