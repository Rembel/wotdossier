using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerStatisticEntity"/>.
    /// </summary>
    public class PlayerStatisticMapping : StatisticClassMapBase<PlayerStatisticEntity>
    {
        public PlayerStatisticMapping()
        {
			Map(v => v.RatingIntegratedValue, "Rating_IntegratedValue");
			Map(v => v.RatingIntegratedPlace, "Rating_IntegratedPlace");
			Map(v => v.RatingWinsRatioValue, "Rating_BattleAvgPerformanceValue");
			Map(v => v.RatingWinsRatioPlace, "Rating_BattleAvgPerformancePlace");
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
            Map(v => v.RatingMaxXpPlace, "Rating_MaxXpPlace");
            Map(v => v.RatingMaxXpValue, "Rating_MaxXpValue");
            Map(v => v.RatingHitsPercentsPlace, "Rating_HitsPercentsPlace");
            Map(v => v.RatingHitsPercentsValue, "Rating_HitsPercentsValue");

            Map(v => v.AchievementsId).ReadOnly();
            Map(v => v.AchievementsUId);
            Map(v => v.PlayerUId);
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();
            References(v => v.AchievementsIdObject).Column(Column(v => v.AchievementsId)).Insert().Update().Cascade.All().Fetch.Join();
        }
    }
}
