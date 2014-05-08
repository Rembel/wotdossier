using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
    /// <summary>
    /// 	Represents base mapping class for entity.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class StatisticClassMapBase<T> : ClassMapBase<T>
        where T : StatisticEntity
    {
        public StatisticClassMapBase()
        {
            Map(v => v.Updated, StatisticEntity.PropUpdated);
            Map(v => v.Wins, StatisticEntity.PropWins);
            Map(v => v.Losses, StatisticEntity.PropLosses);
            Map(v => v.SurvivedBattles, StatisticEntity.PropSurvivedBattles);
            Map(v => v.Xp, StatisticEntity.PropXp);
            Map(v => v.BattleAvgXp, StatisticEntity.PropBattleAvgXp);
            Map(v => v.MaxXp, StatisticEntity.PropMaxXp);
            Map(v => v.Frags, StatisticEntity.PropFrags);
            Map(v => v.Spotted, StatisticEntity.PropSpotted);
            Map(v => v.HitsPercents, StatisticEntity.PropHitsPercents);
            Map(v => v.DamageDealt, StatisticEntity.PropDamageDealt);
            Map(v => v.DamageTaken, StatisticEntity.PropDamageTaken);
            Map(v => v.CapturePoints, StatisticEntity.PropCapturePoints);
            Map(v => v.DroppedCapturePoints, StatisticEntity.PropDroppedCapturePoints);
            Map(v => v.BattlesCount, StatisticEntity.PropBattlesCount);
            Map(v => v.AvgLevel, StatisticEntity.PropAvgLevel);
            Map(v => v.PlayerId, StatisticEntity.PropPlayerId).Insert();

            Map(v => v.RBR, StatisticEntity.PropRBR);
            Map(v => v.WN8Rating, StatisticEntity.PropWN8Rating);
            Map(v => v.PerformanceRating, StatisticEntity.PropPerformanceRating);
        }
    }
}