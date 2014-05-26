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
        public TeamBattlesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.A7x7)
        {
            #region [ Awards ]

            WolfAmongSheep = tanks.Sum(x => x.Achievements7x7.wolfAmongSheep);
            WolfAmongSheepMedal = tanks.Sum(x => x.Achievements7x7.wolfAmongSheepMedal);
            GeniusForWar = tanks.Sum(x => x.Achievements7x7.geniusForWar);
            GeniusForWarMedal = tanks.Sum(x => x.Achievements7x7.geniusForWarMedal);
            KingOfTheHill = tanks.Sum(x => x.Achievements7x7.kingOfTheHill);
            TacticalBreakthroughSeries = tanks.Max(x => x.Achievements7x7.tacticalBreakthroughSeries);
            MaxTacticalBreakthroughSeries = tanks.Max(x => x.Achievements7x7.maxTacticalBreakthroughSeries);
            ArmoredFist = tanks.Sum(x => x.Achievements7x7.armoredFist);
            TacticalBreakthrough = tanks.Sum(x => x.Achievements7x7.tacticalBreakthrough);

            GodOfWar = tanks.Sum(x => x.Achievements7x7.godOfWar);
            FightingReconnaissance = tanks.Sum(x => x.Achievements7x7.fightingReconnaissance);
            FightingReconnaissanceMedal = tanks.Sum(x => x.Achievements7x7.fightingReconnaissanceMedal);
            WillToWinSpirit = tanks.Sum(x => x.Achievements7x7.willToWinSpirit);
            CrucialShot = tanks.Sum(x => x.Achievements7x7.crucialShot);
            CrucialShotMedal = tanks.Sum(x => x.Achievements7x7.crucialShotMedal);
            ForTacticalOperations = tanks.Sum(x => x.Achievements7x7.forTacticalOperations);

            PromisingFighter = tanks.Sum(x => x.Achievements7x7.promisingFighter);
            PromisingFighterMedal = tanks.Sum(x => x.Achievements7x7.promisingFighterMedal);
            HeavyFire = tanks.Sum(x => x.Achievements7x7.heavyFire);
            HeavyFireMedal = tanks.Sum(x => x.Achievements7x7.heavyFireMedal);
            Ranger = tanks.Sum(x => x.Achievements7x7.ranger);
            RangerMedal = tanks.Sum(x => x.Achievements7x7.rangerMedal);
            FireAndSteel = tanks.Sum(x => x.Achievements7x7.fireAndSteel);
            FireAndSteelMedal = tanks.Sum(x => x.Achievements7x7.fireAndSteelMedal);
            Pyromaniac = tanks.Sum(x => x.Achievements7x7.pyromaniac);
            PyromaniacMedal = tanks.Sum(x => x.Achievements7x7.pyromaniacMedal);
            NoMansLand = tanks.Sum(x => x.Achievements7x7.noMansLand);

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

        #endregion

        public override void Update(TeamBattlesStatisticEntity entity)
        {
            base.Update(entity);

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

            entity.AchievementsIdObject.PromisingFighter = PromisingFighter;
            entity.AchievementsIdObject.PromisingFighterMedal = PromisingFighterMedal;
            entity.AchievementsIdObject.HeavyFire = HeavyFire;
            entity.AchievementsIdObject.HeavyFireMedal = HeavyFireMedal;
            entity.AchievementsIdObject.Ranger = Ranger;
            entity.AchievementsIdObject.RangerMedal = RangerMedal;
            entity.AchievementsIdObject.FireAndSteel = FireAndSteel;
            entity.AchievementsIdObject.FireAndSteelMedal = FireAndSteelMedal;
            entity.AchievementsIdObject.Pyromaniac = Pyromaniac;
            entity.AchievementsIdObject.PyromaniacMedal = PyromaniacMedal;
            entity.AchievementsIdObject.NoMansLand = NoMansLand;
        }
    }
}