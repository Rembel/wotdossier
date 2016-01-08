using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class TeamBattlesStatAdapter : AbstractStatisticAdapter<TeamBattlesStatisticEntity>, ITeamBattlesAchievements
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public TeamBattlesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.A7x7 ?? new StatisticJson())
        {
            #region [ Awards ]


            var achievements7X7 = new Achievements7x7();
            Func<TankJson, Achievements7x7> achievementsPredicate = tankJson => tankJson.Achievements7x7 ?? achievements7X7;

            WolfAmongSheep = tanks.Sum(x => achievementsPredicate(x).WolfAmongSheep);
            WolfAmongSheepMedal = WolfAmongSheep/100;
            GeniusForWar = tanks.Sum(x => achievementsPredicate(x).GeniusForWar);
            GeniusForWarMedal = GeniusForWar/100;
            KingOfTheHill = tanks.Sum(x => achievementsPredicate(x).KingOfTheHill);
            TacticalBreakthroughSeries = tanks.Max(x => achievementsPredicate(x).TacticalBreakthroughSeries);
            MaxTacticalBreakthroughSeries = tanks.Max(x => achievementsPredicate(x).MaxTacticalBreakthroughSeries);
            ArmoredFist = tanks.Sum(x => achievementsPredicate(x).ArmoredFist);
            TacticalBreakthrough = tanks.Sum(x => achievementsPredicate(x).TacticalBreakthrough);

            GodOfWar = tanks.Sum(x => achievementsPredicate(x).GodOfWar);
            FightingReconnaissance = tanks.Sum(x => achievementsPredicate(x).FightingReconnaissance);
            FightingReconnaissanceMedal = FightingReconnaissance/50;
            WillToWinSpirit = tanks.Sum(x => achievementsPredicate(x).WillToWinSpirit);
            CrucialShot = tanks.Sum(x => achievementsPredicate(x).CrucialShot);
            CrucialShotMedal = CrucialShot/20;
            ForTacticalOperations = tanks.Sum(x => achievementsPredicate(x).ForTacticalOperations);

            PromisingFighter = tanks.Sum(x => achievementsPredicate(x).PromisingFighter);
            PromisingFighterMedal = PromisingFighter/100;
            HeavyFire = tanks.Sum(x => achievementsPredicate(x).HeavyFire);
            HeavyFireMedal = HeavyFire/100;
            Ranger = tanks.Sum(x => achievementsPredicate(x).Ranger);
            RangerMedal = Ranger/10;
            FireAndSteel = tanks.Sum(x => achievementsPredicate(x).FireAndSteel);
            FireAndSteelMedal = FireAndSteel/25;
            Pyromaniac = tanks.Sum(x => achievementsPredicate(x).Pyromaniac);
            PyromaniacMedal = Pyromaniac/10;
            NoMansLand = tanks.Sum(x => achievementsPredicate(x).NoMansLand);

            Guerrilla = tanks.Sum(x => achievementsPredicate(x).Guerrilla);
            GuerrillaMedal = Guerrilla/10;
            Infiltrator = tanks.Sum(x => achievementsPredicate(x).Infiltrator);
            InfiltratorMedal = Infiltrator/1000;
            Sentinel = tanks.Sum(x => achievementsPredicate(x).Sentinel);
            SentinelMedal = Sentinel/1000;
            PrematureDetonation = tanks.Sum(x => achievementsPredicate(x).PrematureDetonation);
            PrematureDetonationMedal = PrematureDetonation/10;
            BruteForce = tanks.Sum(x => achievementsPredicate(x).BruteForce);
            BruteForceMedal = BruteForce/10;
            AwardCount = tanks.Sum(x => achievementsPredicate(x).AwardCount);
            BattleTested = tanks.Sum(x => achievementsPredicate(x).BattleTested);

            #endregion
        }

        //public TeamBattlesStatAdapter(Player stat)
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

        public List<Vehicle> Vehicle { get; set; }

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

        public int PromisingFighter { get; set; }
        public int PromisingFighterMedal { get; set; }
        public int HeavyFire { get; set; }
        public int HeavyFireMedal { get; set; }
        public int Ranger { get; set; }
        public int RangerMedal { get; set; }
        public int FireAndSteel { get; set; }
        public int FireAndSteelMedal { get; set; }
        public int Pyromaniac { get; set; }
        public int PyromaniacMedal { get; set; }
        public int NoMansLand { get; set; }

        public int Guerrilla { get; set; }
        public int GuerrillaMedal { get; set; }
        public int Infiltrator { get; set; }
        public int InfiltratorMedal { get; set; }
        public int Sentinel { get; set; }
        public int SentinelMedal { get; set; }
        public int PrematureDetonation { get; set; }
        public int PrematureDetonationMedal { get; set; }
        public int BruteForce { get; set; }
        public int BruteForceMedal { get; set; }
        public int AwardCount { get; set; }
        public int BattleTested { get; set; }

        #endregion

        public override void Update(TeamBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new TeamBattlesAchievementsEntity{UId = Guid.NewGuid()};
                entity.AchievementsUId = entity.AchievementsIdObject.UId;
            }

            Mapper.Map<ITeamBattlesAchievements>(this, entity.AchievementsIdObject);
        }
    }
}