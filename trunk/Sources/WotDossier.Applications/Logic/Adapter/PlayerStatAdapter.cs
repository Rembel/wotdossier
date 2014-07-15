using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class PlayerStatAdapter : AbstractStatisticAdapter<PlayerStatisticEntity>, IRandomBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.A15x15)
        {
            #region [ BattleAwards ]

            Warrior = tanks.Sum(x => x.Achievements.Warrior);
            Invader = tanks.Sum(x => x.Achievements.Invader);
            Sniper = tanks.Sum(x => x.Achievements.Sniper);
            Sniper2 = tanks.Sum(x => x.Achievements.Sniper2);
            MainGun = tanks.Sum(x => x.Achievements.MainGun);
            Defender = tanks.Sum(x => x.Achievements.Defender);
            SteelWall = tanks.Sum(x => x.Achievements.Steelwall);
            Confederate = tanks.Sum(x => x.Achievements.Supporter);
            Scout = tanks.Sum(x => x.Achievements.Scout);
            PatrolDuty = tanks.Sum(x => x.Achievements.Evileye);
            BrothersInArms = tanks.Sum(x => x.Achievements.MedalBrothersInArms);
            CrucialContribution = tanks.Sum(x => x.Achievements.MedalCrucialContribution);
            IronMan = tanks.Sum(x => x.Achievements.IronMan);
            LuckyDevil = tanks.Sum(x => x.Achievements.LuckyDevil);
            Sturdy = tanks.Sum(x => x.Achievements.Sturdy);

            #endregion

            #region [ Epic ]

            Boelter = tanks.Sum(x => x.Achievements.MedalWittmann);
            RadleyWalters = tanks.Sum(x => x.Achievements.MedalRadleyWalters);
            LafayettePool = tanks.Sum(x => x.Achievements.MedalLafayettePool);
            Orlik = tanks.Sum(x => x.Achievements.MedalOrlik);
            Oskin = tanks.Sum(x => x.Achievements.MedalOskin);
            Lehvaslaiho = tanks.Sum(x => x.Achievements.MedalLehvaslaiho);
            Nikolas = tanks.Sum(x => x.Achievements.MedalNikolas);
            Halonen = tanks.Sum(x => x.Achievements.MedalHalonen);
            Burda = tanks.Sum(x => x.Achievements.MedalBurda);
            Pascucci = tanks.Sum(x => x.Achievements.MedalPascucci);
            Dumitru = tanks.Sum(x => x.Achievements.MedalDumitru);
            TamadaYoshio = tanks.Sum(x => x.Achievements.MedalTamadaYoshio);
            Billotte = tanks.Sum(x => x.Achievements.MedalBillotte);
            BrunoPietro = tanks.Sum(x => x.Achievements.MedalBrunoPietro);
            Tarczay = tanks.Sum(x => x.Achievements.MedalTarczay);
            Kolobanov = tanks.Sum(x => x.Achievements.MedalKolobanov);
            Fadin = tanks.Sum(x => x.Achievements.MedalFadin);
            HeroesOfRassenay = tanks.Sum(x => x.Achievements.HeroesOfRassenay);
            DeLanglade = tanks.Sum(x => x.Achievements.MedalDeLanglade);

            #endregion

            #region [ Medals]

            //Kay = stat.data.achievements.medalKay;
            //Carius = stat.data.achievements.medalCarius;
            //Knispel = stat.data.achievements.medalKnispel;
            //Poppel = stat.data.achievements.medalPoppel;
            //Abrams = stat.data.achievements.medalAbrams;
            //Leclerk = stat.data.achievements.medalLeClerc;
            //Lavrinenko = stat.data.achievements.medalLavrinenko;
            //Ekins = stat.data.achievements.medalEkins;

            #endregion

            #region [ Series ]

            SharpshooterLongest = tanks.Max(x => x.Achievements.MaxSniperSeries);
            MasterGunnerLongest = tanks.Max(x => x.Achievements.MaxPiercingSeries);

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = tanks.Sum(x => x.Achievements.Kamikaze);
            Raider = tanks.Sum(x => x.Achievements.Raider);
            Bombardier = tanks.Sum(x => x.Achievements.Bombardier);
            Reaper = tanks.Max(x => x.Achievements.MaxKillingSeries);
            Invincible = tanks.Max(x => x.Achievements.MaxInvincibleSeries);
            Survivor = tanks.Max(x => x.Achievements.MaxDiehardSeries);
            //count Maus frags
            MouseTrap = tanks.Sum(x => x.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count)) / 10;
            Hunter = tanks.Sum(x => x.Achievements.FragsBeast) / 100;
            Sinai = tanks.Sum(x => x.Achievements.FragsSinai) / 100;
            PattonValley = tanks.Sum(x => x.Achievements.FragsPatton) / 100;
            Huntsman = tanks.Sum(x => x.Achievements.Huntsman);

            #endregion

            MarksOnGun = tanks.Sum(x => x.Achievements.MarksOnGun);
            MovingAvgDamage = tanks.Sum(x => x.Achievements.MovingAvgDamage);
            MedalMonolith = tanks.Sum(x => x.Achievements.MedalMonolith);
            MedalAntiSpgFire = tanks.Sum(x => x.Achievements.MedalAntiSpgFire);
            MedalGore = tanks.Sum(x => x.Achievements.MedalGore);
            MedalCoolBlood = tanks.Sum(x => x.Achievements.MedalCoolBlood);
            MedalStark = tanks.Sum(x => x.Achievements.MedalStark);
            DamageRating = tanks.Max(x => x.Achievements.DamageRating);
        }

        public PlayerStatAdapter(Player stat)
        {
            BattlesCount = stat.dataField.statistics.all.battles;
            Wins = stat.dataField.statistics.all.wins;
            Losses = stat.dataField.statistics.all.losses;
            SurvivedBattles = stat.dataField.statistics.all.survived_battles;
            Xp = stat.dataField.statistics.all.xp;
            BattleAvgXp = stat.dataField.statistics.all.battle_avg_xp;
            MaxXp = stat.dataField.statistics.max_xp;
            MaxDamage = stat.dataField.statistics.max_damage;
            Frags = stat.dataField.statistics.all.frags;
            Spotted = stat.dataField.statistics.all.spotted;
            HitsPercents = stat.dataField.statistics.all.hits_percents;
            DamageDealt = stat.dataField.statistics.all.damage_dealt;
            DamageTaken = stat.dataField.statistics.all.damage_received;
            CapturePoints = stat.dataField.statistics.all.capture_points;
            DroppedCapturePoints = stat.dataField.statistics.all.dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.dataField.updated_at).ToLocalTime();
            Created = Utils.UnixDateToDateTime((long)stat.dataField.created_at).ToLocalTime();
            if (BattlesCount > 0 && stat.dataField.vehicles != null)
            {
                int battlesCount = stat.dataField.vehicles.Sum(x => x.all.battles);
                if (battlesCount > 0)
                {
                    AvgLevel = stat.dataField.vehicles.Sum(x => (x.tank != null ? x.tank.level : 1)*x.all.battles)/(double) battlesCount;
                }
            }

            #region [ BattleAwards ]

            Warrior = stat.dataField.achievements.achievements.warrior;
            Invader = stat.dataField.achievements.achievements.invader;
            Sniper = stat.dataField.achievements.achievements.sniper;
            Defender = stat.dataField.achievements.achievements.defender;
            SteelWall = stat.dataField.achievements.achievements.steelwall;
            Confederate = stat.dataField.achievements.achievements.supporter;
            Scout = stat.dataField.achievements.achievements.scout;
            PatrolDuty = stat.dataField.achievements.achievements.evileye;
            BrothersInArms = stat.dataField.achievements.achievements.medalBrothersInArms;
            CrucialContribution = stat.dataField.achievements.achievements.medalCrucialContribution;
            IronMan = stat.dataField.achievements.achievements.ironMan;
            LuckyDevil = stat.dataField.achievements.achievements.luckyDevil;
            Sturdy = stat.dataField.achievements.achievements.sturdy;
            Sniper2 = stat.dataField.achievements.achievements.sniper2;
            MainGun = stat.dataField.achievements.achievements.main_gun;

            #endregion

            #region [ Epic ]

            Boelter = stat.dataField.achievements.achievements.medalBoelter;
            RadleyWalters = stat.dataField.achievements.achievements.medalRadleyWalters;
            LafayettePool = stat.dataField.achievements.achievements.medalLafayettePool;
            Orlik = stat.dataField.achievements.achievements.medalOrlik;
            Oskin = stat.dataField.achievements.achievements.medalOskin;
            Lehvaslaiho = stat.dataField.achievements.achievements.medalLehvaslaiho;
            Nikolas = stat.dataField.achievements.achievements.medalNikolas;
            Halonen = stat.dataField.achievements.achievements.medalHalonen;
            Burda = stat.dataField.achievements.achievements.medalBurda;
            Pascucci = stat.dataField.achievements.achievements.medalPascucci;
            Dumitru = stat.dataField.achievements.achievements.medalDumitru;
            TamadaYoshio = stat.dataField.achievements.achievements.medalTamadaYoshio;
            Billotte = stat.dataField.achievements.achievements.medalBillotte;
            BrunoPietro = stat.dataField.achievements.achievements.medalBrunoPietro;
            Tarczay = stat.dataField.achievements.achievements.medalTarczay;
            Kolobanov = stat.dataField.achievements.achievements.medalKolobanov;
            Fadin = stat.dataField.achievements.achievements.medalFadin;
            HeroesOfRassenay = stat.dataField.achievements.achievements.medalHeroesOfRassenay;
            DeLanglade = stat.dataField.achievements.achievements.medalDeLanglade;

            #endregion

            #region [ Medals]

            Kay = stat.dataField.achievements.achievements.medal_kay;
            Carius = stat.dataField.achievements.achievements.medalCarius;
            Knispel = stat.dataField.achievements.achievements.medalKnispel;
            Poppel = stat.dataField.achievements.achievements.medalPoppel;
            Abrams = stat.dataField.achievements.achievements.medalAbrams;
            Leclerk = stat.dataField.achievements.achievements.medalLeClerc;
            Lavrinenko = stat.dataField.achievements.achievements.medalLavrinenko;
            Ekins = stat.dataField.achievements.achievements.medalEkins;

            #endregion

            #region [ Series ]

            SharpshooterLongest = stat.dataField.achievements.max_series.titleSniper;
            MasterGunnerLongest = stat.dataField.achievements.max_series.armorPiercer;

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = stat.dataField.achievements.achievements.kamikaze;
            Raider = stat.dataField.achievements.achievements.raider;
            Bombardier = stat.dataField.achievements.achievements.bombardier;
            Reaper = stat.dataField.achievements.max_series.handOfDeath;
            Invincible = stat.dataField.achievements.max_series.invincible;
            Survivor = stat.dataField.achievements.max_series.diehard;
            MouseTrap = stat.dataField.achievements.achievements.mousebane;
            Hunter = stat.dataField.achievements.achievements.beasthunter;
            Sinai = stat.dataField.achievements.achievements.sinai;
            PattonValley = stat.dataField.achievements.achievements.pattonValley;
            Huntsman = stat.dataField.achievements.achievements.huntsman;

            #endregion

            MarksOnGun = stat.dataField.achievements.achievements.marksOnGun;
            MovingAvgDamage = stat.dataField.achievements.achievements.movingAvgDamage;
            MedalMonolith = stat.dataField.achievements.achievements.medalMonolith;
            MedalAntiSpgFire = stat.dataField.achievements.achievements.medalAntiSpgFire;
            MedalGore = stat.dataField.achievements.achievements.medalGore;
            MedalCoolBlood = stat.dataField.achievements.achievements.medalCoolBlood;
            MedalStark = stat.dataField.achievements.achievements.medalStark;

            //PerformanceRating = RatingHelper.PerformanceRating(tanks);
            if (stat.dataField.vehicles != null)
            {
                Tanks = stat.dataField.vehicles.Where(x => x.description != null).Select(x => (ITankStatisticRow)new TankStatisticRowViewModel(DataMapper.Map(x))).OrderByDescending(x => x.Tier).ToList();
                WN8Rating = RatingHelper.Wn8(Tanks);
                PerformanceRating = RatingHelper.PerformanceRating(Tanks);
            }
        }

        public List<ITankStatisticRow> Tanks { get; set; }

        #region Achievments

        #region [ BattleAwards ]

        public int Warrior { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Sniper2 { get; set; }

        public int MainGun { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int IronMan { get; set; }

        public int LuckyDevil { get; set; }

        public int Sturdy { get; set; }

        #endregion

        #region [ RowEpic ]

        public int Boelter { get; set; }

        public int RadleyWalters { get; set; }

        public int LafayettePool { get; set; }

        public int Orlik { get; set; }

        public int Oskin { get; set; }

        public int Lehvaslaiho { get; set; }

        public int Nikolas { get; set; }

        public int Halonen { get; set; }

        public int Burda { get; set; }

        public int Pascucci { get; set; }

        public int Dumitru { get; set; }

        public int TamadaYoshio { get; set; }

        public int Billotte { get; set; }

        public int BrunoPietro { get; set; }

        public int Tarczay { get; set; }

        public int Kolobanov { get; set; }

        public int Fadin { get; set; }

        public int HeroesOfRassenay { get; set; }

        public int DeLanglade { get; set; }

        #endregion

        #region [ SpecialAwards ]

        public int Kamikaze { get; set; }

        public int Raider { get; set; }

        public int Bombardier { get; set; }

        public int Reaper { get; set; }

        public int Sharpshooter { get; set; }

        public int Invincible { get; set; }

        public int Survivor { get; set; }

        public int MouseTrap { get; set; }

        public int Hunter { get; set; }

        public int Sinai { get; set; }

        public int PattonValley { get; set; }

        public int Huntsman { get; set; }

        #endregion

        #region [ Medals]

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

        #endregion

        #region [ Series ]

        public int ReaperLongest { get; set; }

        public int ReaperProgress { get; set; }

        public int SharpshooterLongest { get; set; }

        public int SharpshooterProgress { get; set; }

        public int MasterGunnerLongest { get; set; }

        public int MasterGunnerProgress { get; set; }

        public int InvincibleLongest { get; set; }

        public int InvincibleProgress { get; set; }

        public int SurvivorLongest { get; set; }

        public int SurvivorProgress { get; set; }

        #endregion

        public int MarksOnGun { get; set; }

        public int MovingAvgDamage { get; set; }

        public int MedalMonolith { get; set; }
        
        public int MedalAntiSpgFire { get; set; }
        
        public int MedalGore { get; set; }
        
        public int MedalCoolBlood { get; set; }
        
        public int MedalStark { get; set; }

        public int BattleHero { get; set; }

        public int DamageRating { get; set; }

        #endregion

        public override void Update(PlayerStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new PlayerAchievementsEntity();
            }

            #region [ BattleAwards ]

            entity.AchievementsIdObject.Warrior = Warrior;
            entity.AchievementsIdObject.Invader = Invader;
            entity.AchievementsIdObject.Sniper = Sniper;
            entity.AchievementsIdObject.Sniper2 = Sniper2;
            entity.AchievementsIdObject.MainGun = MainGun;
            entity.AchievementsIdObject.Defender = Defender;
            entity.AchievementsIdObject.SteelWall = SteelWall;
            entity.AchievementsIdObject.Confederate = Confederate;
            entity.AchievementsIdObject.Scout = Scout;
            entity.AchievementsIdObject.PatrolDuty = PatrolDuty;
            entity.AchievementsIdObject.BrothersInArms = BrothersInArms;
            entity.AchievementsIdObject.CrucialContribution = CrucialContribution;
            entity.AchievementsIdObject.IronMan = IronMan;
            entity.AchievementsIdObject.LuckyDevil = LuckyDevil;
            entity.AchievementsIdObject.Sturdy = Sturdy;

            #endregion

            #region [ Epic ]

            entity.AchievementsIdObject.Boelter = Boelter;
            entity.AchievementsIdObject.RadleyWalters = RadleyWalters;
            entity.AchievementsIdObject.LafayettePool = LafayettePool;
            entity.AchievementsIdObject.Orlik = Orlik;
            entity.AchievementsIdObject.Oskin = Oskin;
            entity.AchievementsIdObject.Lehvaslaiho = Lehvaslaiho;
            entity.AchievementsIdObject.Nikolas = Nikolas;
            entity.AchievementsIdObject.Halonen = Halonen;
            entity.AchievementsIdObject.Burda = Burda;
            entity.AchievementsIdObject.Pascucci = Pascucci;
            entity.AchievementsIdObject.Dumitru = Dumitru;
            entity.AchievementsIdObject.TamadaYoshio = TamadaYoshio;
            entity.AchievementsIdObject.Billotte = Billotte;
            entity.AchievementsIdObject.BrunoPietro = BrunoPietro;
            entity.AchievementsIdObject.Tarczay = Tarczay;
            entity.AchievementsIdObject.Kolobanov = Kolobanov;
            entity.AchievementsIdObject.Fadin = Fadin;
            entity.AchievementsIdObject.HeroesOfRassenay = HeroesOfRassenay;
            entity.AchievementsIdObject.DeLanglade = DeLanglade;

            #endregion

            #region [ Medals]

            entity.AchievementsIdObject.Kay = Kay;
            entity.AchievementsIdObject.Carius = Carius;
            entity.AchievementsIdObject.Knispel = Knispel;
            entity.AchievementsIdObject.Poppel = Poppel;
            entity.AchievementsIdObject.Abrams = Abrams;
            entity.AchievementsIdObject.Leclerk = Leclerk;
            entity.AchievementsIdObject.Lavrinenko = Lavrinenko;
            entity.AchievementsIdObject.Ekins = Ekins;

            #endregion

            #region [ Series ]

            entity.AchievementsIdObject.SharpshooterLongest = SharpshooterLongest;
            entity.AchievementsIdObject.MasterGunnerLongest = MasterGunnerLongest;

            #endregion

            #region [ SpecialAwards ]

            entity.AchievementsIdObject.Kamikaze = Kamikaze;
            entity.AchievementsIdObject.Raider = Raider;
            entity.AchievementsIdObject.Bombardier = Bombardier;
            entity.AchievementsIdObject.Reaper = Reaper;
            entity.AchievementsIdObject.Invincible = Invincible;
            entity.AchievementsIdObject.Survivor = Survivor;
            entity.AchievementsIdObject.MouseTrap = MouseTrap;
            entity.AchievementsIdObject.Hunter = Hunter;
            entity.AchievementsIdObject.Sinai = Sinai;
            entity.AchievementsIdObject.PattonValley = PattonValley;
            entity.AchievementsIdObject.Huntsman = Huntsman;

            #endregion

            entity.AchievementsIdObject.MarksOnGun = MarksOnGun; 
            entity.AchievementsIdObject.MovingAvgDamage = MovingAvgDamage;
            entity.AchievementsIdObject.MedalMonolith = MedalMonolith;
            entity.AchievementsIdObject.MedalAntiSpgFire = MedalAntiSpgFire;
            entity.AchievementsIdObject.MedalGore = MedalGore;
            entity.AchievementsIdObject.MedalCoolBlood = MedalCoolBlood;
            entity.AchievementsIdObject.MedalStark = MedalStark;
            entity.AchievementsIdObject.DamageRating = DamageRating;
        }
    }
}