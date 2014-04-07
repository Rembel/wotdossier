using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class HistoricalBattlesStatAdapter : IStatisticAdapter<HistoricalBattlesStatisticEntity>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public HistoricalBattlesStatAdapter(List<TankJson> tanks)
        {
            Battles_count = tanks.Sum(x => x.Historical.battlesCount);
            Wins = tanks.Sum(x => x.Historical.wins);
            Losses = tanks.Sum(x => x.Historical.losses);
            Survived_battles = tanks.Sum(x => x.Historical.survivedBattles);
            Xp = tanks.Sum(x => x.Historical.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp/(double) Battles_count;
            }
            Max_xp = tanks.Max(x => x.Historical.maxXP);
            Frags = tanks.Sum(x => x.Historical.frags);
            Spotted = tanks.Sum(x => x.Historical.spotted);
            Hits_percents = tanks.Sum(x => x.Historical.hits)/((double) tanks.Sum(x => x.Historical.shots))*100.0;
            Damage_dealt = tanks.Sum(x => x.Historical.damageDealt);
            Damage_taken = tanks.Sum(x => x.Historical.damageReceived);
            Capture_points = tanks.Sum(x => x.Historical.capturePoints);
            Dropped_capture_points = tanks.Sum(x => x.Historical.droppedCapturePoints);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier*x.Historical.battlesCount)/(double) Battles_count;
            }

            #region [ Awards ]

            GuardsMan = tanks.Sum(x => x.AchievementsHistorical.guardsman);
            MakerOfHistory = tanks.Sum(x => x.AchievementsHistorical.makerOfHistory);
            BothSidesWins = tanks.Sum(x => x.AchievementsHistorical.bothSidesWins);
            WeakVehiclesWins = tanks.Sum(x => x.AchievementsHistorical.weakVehiclesWins);

            #endregion

            PerformanceRating = RatingHelper.GetPerformanceRating(tanks, tank => tank.Historical);
            WN8Rating = RatingHelper.GetWN8Rating(tanks, tank => tank.Historical);
            RBR = RatingHelper.GetRBR(tanks, tank => tank.Historical);
        }

        //public TeamBattlesStatAdapter(PlayerStat stat)
        //{
        //    Battles_count = stat.dataField.statistics.all.battles;
        //    Wins = stat.dataField.statistics.all.wins;
        //    Losses = stat.dataField.statistics.all.losses;
        //    Survived_battles = stat.dataField.statistics.all.survived_battles;
        //    Xp = stat.dataField.statistics.all.xp;
        //    Battle_avg_xp = stat.dataField.statistics.all.battle_avg_xp;
        //    Max_xp = stat.dataField.statistics.max_xp;
        //    Frags = stat.dataField.statistics.all.frags;
        //    Spotted = stat.dataField.statistics.all.spotted;
        //    Hits_percents = stat.dataField.statistics.all.hits_percents;
        //    Damage_dealt = stat.dataField.statistics.all.damage_dealt;
        //    Capture_points = stat.dataField.statistics.all.capture_points;
        //    Dropped_capture_points = stat.dataField.statistics.all.dropped_capture_points;
        //    Updated = Utils.UnixDateToDateTime((long)stat.dataField.updated_at).ToLocalTime();
        //    Created = Utils.UnixDateToDateTime((long)stat.dataField.created_at).ToLocalTime();
        //    if (Battles_count > 0 && stat.dataField.vehicles != null)
        //    {
        //        int battlesCount = stat.dataField.vehicles.Sum(x => x.statistics.all.battles);
        //        if (battlesCount > 0)
        //        {
        //            AvgLevel = stat.dataField.vehicles.Sum(x => (x.tank != null ? x.tank.level : 1)*x.statistics.all.battles)/(double) battlesCount;
        //        }
        //    }

        //    #region [ IRowBattleAwards ]

        //    WolfAmongSheep = _tanksV2.Sum(x => x.Achievements7x7.wolfAmongSheep);
        //    WolfAmongSheepMedal = _tanksV2.Sum(x => x.Achievements7x7.wolfAmongSheepMedal);
        //    GeniusForWar = _tanksV2.Sum(x => x.Achievements7x7.geniusForWar);
        //    GeniusForWarMedal = _tanksV2.Sum(x => x.Achievements7x7.geniusForWarMedal);
        //    KingOfTheHill = _tanksV2.Sum(x => x.Achievements7x7.kingOfTheHill);
        //    TacticalBreakthroughSeries = _tanksV2.Sum(x => x.Achievements7x7.tacticalBreakthroughSeries);
        //    MaxTacticalBreakthroughSeries = _tanksV2.Sum(x => x.Achievements7x7.maxTacticalBreakthroughSeries);
        //    ArmoredFist = _tanksV2.Sum(x => x.Achievements7x7.armoredFist);
        //    TacticalBreakthrough = _tanksV2.Sum(x => x.Achievements7x7.tacticalBreakthrough);

        //    #endregion

        //    Vehicle = stat.dataField.vehicles;
        //}

        public List<VehicleStat> Vehicle { get; set; }

        public DateTime Created { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Survived_battles { get; set; }

        public int Xp { get; set; }

        public double Battle_avg_xp { get; set; }

        public int Max_xp { get; set; }

        public int Frags { get; set; }

        public int Spotted { get; set; }

        public double Hits_percents { get; set; }

        public int Damage_dealt { get; set; }

        public int Damage_taken { get; set; }

        public int Capture_points { get; set; }

        public int Dropped_capture_points { get; set; }

        public double AvgLevel { get; set; }

        public double RBR { get; set; }

        public double WN8Rating { get; set; }

        public double PerformanceRating { get; set; }

        #region Achievments

        public int GuardsMan { get; set; }

        public int MakerOfHistory { get; set; }

        public int BothSidesWins { get; set; }

        public int WeakVehiclesWins { get; set; }

        #endregion

        public int Battles_count { get; set; }
        public DateTime Updated { get; set; }

        public virtual void Update(HistoricalBattlesStatisticEntity entity)
        {
            #region CommonJson init

            entity.BattlesCount = Battles_count;
            entity.Wins = Wins;
            entity.Losses = Losses;
            entity.SurvivedBattles = Survived_battles;
            entity.Xp = Xp;
            entity.BattleAvgXp = Battle_avg_xp;
            entity.MaxXp = Max_xp;
            entity.Frags = Frags;
            entity.Spotted = Spotted;
            entity.HitsPercents = Hits_percents;
            entity.DamageDealt = Damage_dealt;
            entity.DamageTaken = Damage_taken;
            entity.CapturePoints = Capture_points;
            entity.DroppedCapturePoints = Dropped_capture_points;
            entity.Updated = Updated;
            entity.AvgLevel = AvgLevel;
            entity.RBR = RBR;
            entity.WN8Rating = WN8Rating;
            entity.PerformanceRating = PerformanceRating;

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new HistoricalBattlesAchievementsEntity();
            }

            entity.AchievementsIdObject.WeakVehiclesWins = WeakVehiclesWins;
            entity.AchievementsIdObject.GuardsMan = GuardsMan;
            entity.AchievementsIdObject.MakerOfHistory = MakerOfHistory;
            entity.AchievementsIdObject.BothSidesWins = BothSidesWins;

            #endregion
        }
    }
}