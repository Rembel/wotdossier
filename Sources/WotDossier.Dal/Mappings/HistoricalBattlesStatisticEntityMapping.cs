using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="HistoricalBattlesStatisticEntity"/>.
    /// </summary>
    public class HistoricalBattlesStatisticEntityMapping : ClassMapBase<HistoricalBattlesStatisticEntity>
    {
        public HistoricalBattlesStatisticEntityMapping()
        {
			Map(v => v.Updated, "Updated");
			Map(v => v.Wins, "Wins");
			Map(v => v.Losses, "Losses");
			Map(v => v.SurvivedBattles, "SurvivedBattles");
			Map(v => v.Xp, "Xp");
			Map(v => v.BattleAvgXp, "BattleAvgXp");
			Map(v => v.MaxXp, "MaxXp");
			Map(v => v.Frags, "Frags");
			Map(v => v.Spotted, "Spotted");
			Map(v => v.HitsPercents, "HitsPercents");
			Map(v => v.DamageDealt, "DamageDealt");
            Map(v => v.DamageTaken, "DamageTaken");
			Map(v => v.CapturePoints, "CapturePoints");
			Map(v => v.DroppedCapturePoints, "DroppedCapturePoints");
			Map(v => v.BattlesCount, "BattlesCount");
            Map(v => v.AvgLevel, "AvgLevel");
            Map(v => v.RBR, "RBR");
            Map(v => v.WN8Rating, "WN8Rating");
            Map(v => v.PerformanceRating, "PerformanceRating");
			Map(v=>v.PlayerId, "PlayerId").Insert();
            Map(v => v.AchievementsId, "AchievementsId").ReadOnly();
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();
            References(v => v.AchievementsIdObject).Column(Column(v => v.AchievementsId)).Insert().Update().Cascade.All().Fetch.Join();
        }
    }
}