using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="HistoricalBattlesAchievementsEntity"/>.
    /// </summary>
    public class HistoricalBattlesAchievementsEntityMapping : ClassMapBase<HistoricalBattlesAchievementsEntity>
    {
        public HistoricalBattlesAchievementsEntityMapping()
        {
            Map(v => v.GuardsMan);
            Map(v => v.BothSidesWins);
            Map(v => v.MakerOfHistory);
            Map(v => v.WeakVehiclesWins);
        }
    }
}
