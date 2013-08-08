using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerStatisticEntity"/>.
    /// </summary>
    public class PlayerStatisticMapping : ClassMapBase<PlayerStatisticEntity>
    {
        public PlayerStatisticMapping()
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
			Map(v => v.CapturePoints, "CapturePoints");
			Map(v => v.DroppedCapturePoints, "DroppedCapturePoints");
			Map(v => v.BattlesCount, "BattlesCount");
			Map(v => v.RatingIntegratedValue, "Rating_IntegratedValue");
			Map(v => v.RatingIntegratedPlace, "Rating_IntegratedPlace");
			Map(v => v.RatingBattleAvgPerformanceValue, "Rating_BattleAvgPerformanceValue");
			Map(v => v.RatingBattleAvgPerformancePlace, "Rating_BattleAvgPerformancePlace");
			Map(v => v.RatingBattleAvgXpValue, "Rating_BattleAvgXpValue");
			Map(v => v.RatingBattleAvgXpPlace, "Rating_BattleAvgXpPlace");
			Map(v => v.RatingBattleWinsValue, "Rating_BattleWinsValue");
			Map(v => v.RatingBattleWinsPlace, "Rating_BattleWinsPlace");
			Map(v => v.RatingBattlesValue, "Rating_BattlesValue");
			Map(v => v.RatingBattlesPlace, "Rating_BattlesPlace");
			Map(v => v.RatingCapturedPointsValue, "Rating_CapturedPointsValue");
			Map(v => v.RatingCapturedPointsPlace, "Rating_CapturedPointsPlace");
			Map(v => v.RatingDamageDealtValue, "Rating_DamageDealtValue");
			Map(v => v.RatingDamageDealtPlace, "Rating_DamageDealtPlace");
			Map(v => v.RatingDroppedPointsValue, "Rating_DroppedPointsValue");
			Map(v => v.RatingDroppedPointsPlace, "Rating_DroppedPointsPlace");
			Map(v => v.RatingFragsValue, "Rating_FragsValue");
			Map(v => v.RatingFragsPlace, "Rating_FragsPlace");
			Map(v => v.RatingSpottedValue, "Rating_SpottedValue");
			Map(v => v.RatingSpottedPlace, "Rating_SpottedPlace");
			Map(v => v.RatingXpValue, "Rating_XpValue");
			Map(v => v.RatingXpPlace, "Rating_XpPlace");
            Map(v => v.AvgLevel, "AvgLevel");
			Map(v=>v.PlayerId, "PlayerId").Insert();
            Map(v => v.AchievementsId, "AchievementsId").ReadOnly();
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();
            References(v => v.AchievementsIdObject).Column(Column(v => v.AchievementsId)).Insert().Update().Cascade.All();
        }
    }
}
