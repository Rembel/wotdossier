using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public abstract class AbstractStatisticAdapter<T> : IStatisticAdapter<T> where T : StatisticEntity
    {
        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }
        public double BattleAvgXp { get; set; }
        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int MaxFrags { get; set; }
        public int Spotted { get; set; }
        public double HitsPercents { get; set; }
        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public int MaxDamage { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }
        public double AvgLevel { get; set; }
        public double RBR { get; set; }
        public double WN8Rating { get; set; }
        public double PerformanceRating { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public int MarkOfMastery { get; set; }

        protected AbstractStatisticAdapter()
        {
        }

        protected AbstractStatisticAdapter(List<TankJson> tanks, Func<TankJson, StatisticJson> statPredicate)
        {
            BattlesCount = tanks.Sum(x => statPredicate(x).battlesCount);
            Wins = tanks.Sum(x => statPredicate(x).wins);
            Losses = tanks.Sum(x => statPredicate(x).losses);
            SurvivedBattles = tanks.Sum(x => statPredicate(x).survivedBattles);
            Xp = tanks.Sum(x => statPredicate(x).xp);
            if (BattlesCount > 0)
            {
                BattleAvgXp = Xp / (double)BattlesCount;
            }
            MaxXp = tanks.Max(x => statPredicate(x).maxXP);
            Frags = tanks.Sum(x => statPredicate(x).frags);
            MaxFrags = tanks.Max(x => statPredicate(x).maxFrags);
            Spotted = tanks.Sum(x => statPredicate(x).spotted);
            HitsPercents = tanks.Sum(x => statPredicate(x).hits) / ((double)tanks.Sum(x => statPredicate(x).shots)) * 100.0;
            DamageDealt = tanks.Sum(x => statPredicate(x).damageDealt);
            DamageTaken = tanks.Sum(x => statPredicate(x).damageReceived);
            MaxDamage = tanks.Max(x => statPredicate(x).maxDamage);
            CapturePoints = tanks.Sum(x => statPredicate(x).capturePoints);
            DroppedCapturePoints = tanks.Sum(x => statPredicate(x).droppedCapturePoints);
            MarkOfMastery = tanks.Count(x => x.Achievements.MarkOfMastery == (int)Domain.MarkOfMastery.Master);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (BattlesCount > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier * statPredicate(x).battlesCount) / (double)BattlesCount;
            }

            PerformanceRating = RatingHelper.PerformanceRating(tanks, statPredicate);
            WN8Rating = RatingHelper.Wn8(tanks, statPredicate);
            RBR = RatingHelper.PersonalRating(tanks, statPredicate);
        }

        public virtual void Update(T entity)
        {
            entity.BattlesCount = BattlesCount;
            entity.Wins = Wins;
            entity.Losses = Losses;
            entity.SurvivedBattles = SurvivedBattles;
            entity.Xp = Xp;
            entity.BattleAvgXp = BattleAvgXp;
            entity.MaxXp = MaxXp;
            entity.MaxDamage = MaxDamage;
            entity.Frags = Frags;
            entity.MaxFrags = MaxFrags;
            entity.Spotted = Spotted;
            entity.HitsPercents = HitsPercents;
            entity.DamageDealt = DamageDealt;
            entity.DamageTaken = DamageTaken;
            entity.CapturePoints = CapturePoints;
            entity.DroppedCapturePoints = DroppedCapturePoints;
            entity.Updated = Updated;
            entity.AvgLevel = AvgLevel;
            entity.RBR = RBR;
            entity.WN8Rating = WN8Rating;
            entity.PerformanceRating = PerformanceRating;
            entity.MarkOfMastery = MarkOfMastery;
        }
    }
}