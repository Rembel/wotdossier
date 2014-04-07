using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class TeamBattlesStatAdapter : IStatisticAdapter<TeamBattlesStatisticEntity>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public TeamBattlesStatAdapter(List<TankJson> tanks)
        {
            Battles_count = tanks.Sum(x => x.A7x7.battlesCount);
            Wins = tanks.Sum(x => x.A7x7.wins);
            Losses = tanks.Sum(x => x.A7x7.losses);
            Survived_battles = tanks.Sum(x => x.A7x7.survivedBattles);
            Xp = tanks.Sum(x => x.A7x7.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp/(double) Battles_count;
            }
            Max_xp = tanks.Max(x => x.A7x7.maxXP);
            Frags = tanks.Sum(x => x.A7x7.frags);
            Spotted = tanks.Sum(x => x.A7x7.spotted);
            Hits_percents = tanks.Sum(x => x.A7x7.hits)/((double) tanks.Sum(x => x.A7x7.shots))*100.0;
            Damage_dealt = tanks.Sum(x => x.A7x7.damageDealt);
            Damage_taken = tanks.Sum(x => x.A7x7.damageReceived);
            Capture_points = tanks.Sum(x => x.A7x7.capturePoints);
            Dropped_capture_points = tanks.Sum(x => x.A7x7.droppedCapturePoints);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier*x.A7x7.battlesCount)/(double) Battles_count;
            }

            #region [ Awards ]

            WolfAmongSheep = tanks.Sum(x => x.Achievements7x7.wolfAmongSheep);
            WolfAmongSheepMedal = tanks.Sum(x => x.Achievements7x7.wolfAmongSheepMedal);
            GeniusForWar = tanks.Sum(x => x.Achievements7x7.geniusForWar);
            GeniusForWarMedal = tanks.Sum(x => x.Achievements7x7.geniusForWarMedal);
            KingOfTheHill = tanks.Sum(x => x.Achievements7x7.kingOfTheHill);
            TacticalBreakthroughSeries = tanks.Sum(x => x.Achievements7x7.tacticalBreakthroughSeries);
            MaxTacticalBreakthroughSeries = tanks.Sum(x => x.Achievements7x7.maxTacticalBreakthroughSeries);
            ArmoredFist = tanks.Sum(x => x.Achievements7x7.armoredFist);
            TacticalBreakthrough = tanks.Sum(x => x.Achievements7x7.tacticalBreakthrough);

            GodOfWar = tanks.Sum(x => x.Achievements7x7.godOfWar);
            FightingReconnaissance = tanks.Sum(x => x.Achievements7x7.fightingReconnaissance);
            FightingReconnaissanceMedal = tanks.Sum(x => x.Achievements7x7.fightingReconnaissanceMedal);
            WillToWinSpirit = tanks.Sum(x => x.Achievements7x7.willToWinSpirit);
            CrucialShot = tanks.Sum(x => x.Achievements7x7.crucialShot);
            CrucialShotMedal = tanks.Sum(x => x.Achievements7x7.crucialShotMedal);
            ForTacticalOperations = tanks.Sum(x => x.Achievements7x7.forTacticalOperations);

            #endregion

            PerformanceRating = RatingHelper.GetPerformanceRating(tanks, tank => tank.A7x7);
            WN8Rating = RatingHelper.GetWN8Rating(tanks, tank => tank.A7x7);
            RBR = RatingHelper.GetRBR(tanks, tank => tank.A7x7);
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

        public int WolfAmongSheep { get; set; }

        public int WolfAmongSheepMedal { get; set; }

        public int GeniusForWar { get; set; }

        public int GeniusForWarMedal { get; set; }

        public int KingOfTheHill { get; set; }

        public int TacticalBreakthroughSeries { get; set; }

        public int MaxTacticalBreakthroughSeries { get; set; }

        public int ArmoredFist { get; set; }

        public int TacticalBreakthrough { get; set; }

        public int GodOfWar { get; set; }

        public int FightingReconnaissance { get; set; }

        public int FightingReconnaissanceMedal { get; set; }

        public int WillToWinSpirit { get; set; }

        public int CrucialShot { get; set; }

        public int CrucialShotMedal { get; set; }

        public int ForTacticalOperations { get; set; }

        #endregion

        public int Battles_count { get; set; }
        public DateTime Updated { get; set; }

        public virtual void Update(TeamBattlesStatisticEntity entity)
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
                entity.AchievementsIdObject = new TeamBattlesAchievementsEntity();
            }

            entity.AchievementsIdObject.WolfAmongSheep = WolfAmongSheep;
            entity.AchievementsIdObject.WolfAmongSheepMedal = WolfAmongSheepMedal;
            entity.AchievementsIdObject.GeniusForWar = GeniusForWar;
            entity.AchievementsIdObject.GeniusForWarMedal = GeniusForWarMedal;
            entity.AchievementsIdObject.KingOfTheHill = KingOfTheHill;
            entity.AchievementsIdObject.TacticalBreakthroughSeries = TacticalBreakthroughSeries;
            entity.AchievementsIdObject.MaxTacticalBreakthroughSeries = MaxTacticalBreakthroughSeries;
            entity.AchievementsIdObject.ArmoredFist = ArmoredFist;
            entity.AchievementsIdObject.TacticalBreakthrough = TacticalBreakthrough;

            entity.AchievementsIdObject.GodOfWar = GodOfWar;
            entity.AchievementsIdObject.FightingReconnaissance = FightingReconnaissance;
            entity.AchievementsIdObject.FightingReconnaissanceMedal = FightingReconnaissanceMedal;
            entity.AchievementsIdObject.WillToWinSpirit = WillToWinSpirit;
            entity.AchievementsIdObject.CrucialShot = CrucialShot;
            entity.AchievementsIdObject.CrucialShotMedal = CrucialShotMedal;
            entity.AchievementsIdObject.ForTacticalOperations = ForTacticalOperations;

            #endregion
        }
    }
}