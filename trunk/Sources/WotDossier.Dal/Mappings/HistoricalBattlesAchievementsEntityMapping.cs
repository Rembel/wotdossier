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
            Map(v => v.GuardsMan, "GuardsMan");
            Map(v => v.BothSidesWins, "BothSidesWins");
            Map(v => v.MakerOfHistory, "MakerOfHistory");
            Map(v => v.WeakVehiclesWins, "WeakVehiclesWins");
		
			//HasMany(v => v.PlayerStatisticEntities).KeyColumn(Column<PlayerStatisticEntity>(v => v.AchievementsId));
        }
    }
}
