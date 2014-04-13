using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class PlayerStatAdapter : AbstractStatisticAdapter<PlayerStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.A15x15)
        {
            #region [ BattleAwards ]

            Warrior = tanks.Sum(x => x.Achievements.warrior);
            Invader = tanks.Sum(x => x.Achievements.invader);
            Sniper = tanks.Sum(x => x.Achievements.sniper);
            Sniper2 = tanks.Sum(x => x.Achievements.sniper2);
            MainGun = tanks.Sum(x => x.Achievements.mainGun);
            Defender = tanks.Sum(x => x.Achievements.defender);
            SteelWall = tanks.Sum(x => x.Achievements.steelwall);
            Confederate = tanks.Sum(x => x.Achievements.supporter);
            Scout = tanks.Sum(x => x.Achievements.scout);
            PatrolDuty = tanks.Sum(x => x.Achievements.evileye);
            BrothersInArms = tanks.Sum(x => x.Achievements.medalBrothersInArms);
            CrucialContribution = tanks.Sum(x => x.Achievements.medalCrucialContribution);
            IronMan = tanks.Sum(x => x.Achievements.ironMan);
            LuckyDevil = tanks.Sum(x => x.Achievements.luckyDevil);
            Sturdy = tanks.Sum(x => x.Achievements.sturdy);

            #endregion

            #region [ Epic ]

            Boelter = tanks.Sum(x => x.Achievements.medalWittmann);
            RadleyWalters = tanks.Sum(x => x.Achievements.medalRadleyWalters);
            LafayettePool = tanks.Sum(x => x.Achievements.medalLafayettePool);
            Orlik = tanks.Sum(x => x.Achievements.medalOrlik);
            Oskin = tanks.Sum(x => x.Achievements.medalOskin);
            Lehvaslaiho = tanks.Sum(x => x.Achievements.medalLehvaslaiho);
            Nikolas = tanks.Sum(x => x.Achievements.medalNikolas);
            Halonen = tanks.Sum(x => x.Achievements.medalHalonen);
            Burda = tanks.Sum(x => x.Achievements.medalBurda);
            Pascucci = tanks.Sum(x => x.Achievements.medalPascucci);
            Dumitru = tanks.Sum(x => x.Achievements.medalDumitru);
            TamadaYoshio = tanks.Sum(x => x.Achievements.medalTamadaYoshio);
            Billotte = tanks.Sum(x => x.Achievements.medalBillotte);
            BrunoPietro = tanks.Sum(x => x.Achievements.medalBrunoPietro);
            Tarczay = tanks.Sum(x => x.Achievements.medalTarczay);
            Kolobanov = tanks.Sum(x => x.Achievements.medalKolobanov);
            Fadin = tanks.Sum(x => x.Achievements.medalFadin);
            HeroesOfRassenay = tanks.Sum(x => x.Achievements.heroesOfRassenay);
            DeLanglade = tanks.Sum(x => x.Achievements.medalDeLanglade);

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

            SharpshooterLongest = tanks.Max(x => x.Achievements.maxSniperSeries);
            MasterGunnerLongest = tanks.Max(x => x.Achievements.maxPiercingSeries);

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = tanks.Sum(x => x.Achievements.kamikaze);
            Raider = tanks.Sum(x => x.Achievements.raider);
            Bombardier = tanks.Sum(x => x.Achievements.bombardier);
            Reaper = tanks.Max(x => x.Achievements.maxKillingSeries);
            Invincible = tanks.Max(x => x.Achievements.maxInvincibleSeries);
            Survivor = tanks.Max(x => x.Achievements.maxDiehardSeries);
            //count Maus frags
            MouseTrap = tanks.Sum(x => x.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count)) / 10;
            Hunter = tanks.Sum(x => x.Achievements.fragsBeast) / 100;
            Sinai = tanks.Sum(x => x.Achievements.fragsSinai) / 100;
            PattonValley = tanks.Sum(x => x.Achievements.fragsPatton) / 100;
            Huntsman = tanks.Sum(x => x.Achievements.huntsman);

            #endregion

            MarksOnGun = tanks.Sum(x => x.Achievements.marksOnGun);
            MovingAvgDamage = tanks.Sum(x => x.Achievements.movingAvgDamage);
            MedalMonolith = tanks.Sum(x => x.Achievements.medalMonolith);
            MedalAntiSpgFire = tanks.Sum(x => x.Achievements.medalAntiSpgFire);
            MedalGore = tanks.Sum(x => x.Achievements.medalGore);
            MedalCoolBlood = tanks.Sum(x => x.Achievements.medalCoolBlood);
            MedalStark = tanks.Sum(x => x.Achievements.medalStark);
        }

        public PlayerStatAdapter(PlayerStat stat)
        {
            BattlesCount = stat.dataField.statistics.all.battles;
            Wins = stat.dataField.statistics.all.wins;
            Losses = stat.dataField.statistics.all.losses;
            SurvivedBattles = stat.dataField.statistics.all.survived_battles;
            Xp = stat.dataField.statistics.all.xp;
            BattleAvgXp = stat.dataField.statistics.all.battle_avg_xp;
            MaxXp = stat.dataField.statistics.max_xp;
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

            Warrior = stat.dataField.achievements.warrior;
            Invader = stat.dataField.achievements.invader;
            Sniper = stat.dataField.achievements.sniper;
            Defender = stat.dataField.achievements.defender;
            SteelWall = stat.dataField.achievements.steelwall;
            Confederate = stat.dataField.achievements.supporter;
            Scout = stat.dataField.achievements.scout;
            PatrolDuty = stat.dataField.achievements.evileye;
            BrothersInArms = stat.dataField.achievements.medalBrothersInArms;
            CrucialContribution = stat.dataField.achievements.medalCrucialContribution;
            IronMan = stat.dataField.achievements.ironMan;
            LuckyDevil = stat.dataField.achievements.luckyDevil;
            Sturdy = stat.dataField.achievements.sturdy;
            Sniper2 = stat.dataField.achievements.sniper2;
            MainGun = stat.dataField.achievements.main_gun;

            #endregion

            #region [ Epic ]

            Boelter = stat.dataField.achievements.medalBoelter;
            RadleyWalters = stat.dataField.achievements.medalRadleyWalters;
            LafayettePool = stat.dataField.achievements.medalLafayettePool;
            Orlik = stat.dataField.achievements.medalOrlik;
            Oskin = stat.dataField.achievements.medalOskin;
            Lehvaslaiho = stat.dataField.achievements.medalLehvaslaiho;
            Nikolas = stat.dataField.achievements.medalNikolas;
            Halonen = stat.dataField.achievements.medalHalonen;
            Burda = stat.dataField.achievements.medalBurda;
            Pascucci = stat.dataField.achievements.medalPascucci;
            Dumitru = stat.dataField.achievements.medalDumitru;
            TamadaYoshio = stat.dataField.achievements.medalTamadaYoshio;
            Billotte = stat.dataField.achievements.medalBillotte;
            BrunoPietro = stat.dataField.achievements.medalBrunoPietro;
            Tarczay = stat.dataField.achievements.medalTarczay;
            Kolobanov = stat.dataField.achievements.medalKolobanov;
            Fadin = stat.dataField.achievements.medalFadin;
            HeroesOfRassenay = stat.dataField.achievements.medalHeroesOfRassenay;
            DeLanglade = stat.dataField.achievements.medalDeLanglade;

            #endregion

            #region [ Medals]

            Kay = stat.dataField.achievements.medal_kay;
            Carius = stat.dataField.achievements.medalCarius;
            Knispel = stat.dataField.achievements.medalKnispel;
            Poppel = stat.dataField.achievements.medalPoppel;
            Abrams = stat.dataField.achievements.medalAbrams;
            Leclerk = stat.dataField.achievements.medalLeClerc;
            Lavrinenko = stat.dataField.achievements.medalLavrinenko;
            Ekins = stat.dataField.achievements.medalEkins;

            #endregion

            #region [ Series ]

            SharpshooterLongest = stat.dataField.achievements.max_sniper_series;
            MasterGunnerLongest = stat.dataField.achievements.max_piercing_series;

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = stat.dataField.achievements.kamikaze;
            Raider = stat.dataField.achievements.raider;
            Bombardier = stat.dataField.achievements.bombardier;
            Reaper = stat.dataField.achievements.max_killing_series;
            Invincible = stat.dataField.achievements.max_invincible_series;
            Survivor = stat.dataField.achievements.max_diehard_series;
            MouseTrap = stat.dataField.achievements.mousebane;
            Hunter = stat.dataField.achievements.beasthunter;
            Sinai = stat.dataField.achievements.sinai;
            PattonValley = stat.dataField.achievements.pattonValley;
            Huntsman = stat.dataField.achievements.huntsman;

            #endregion

            MarksOnGun = stat.dataField.achievements.marksOnGun;
            MovingAvgDamage = stat.dataField.achievements.movingAvgDamage;
            MedalMonolith = stat.dataField.achievements.medalMonolith;
            MedalAntiSpgFire = stat.dataField.achievements.medalAntiSpgFire;
            MedalGore = stat.dataField.achievements.medalGore;
            MedalCoolBlood = stat.dataField.achievements.medalCoolBlood;
            MedalStark = stat.dataField.achievements.medalStark;

            Vehicle = stat.dataField.vehicles;
        }

        public List<VehicleStat> Vehicle { get; set; }

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
            entity.AchievementsIdObject.CoolHeaded = IronMan;
            entity.AchievementsIdObject.LuckyDevil = LuckyDevil;
            entity.AchievementsIdObject.Spartan = Sturdy;

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
            entity.AchievementsIdObject.Ranger = Huntsman;

            #endregion

            entity.AchievementsIdObject.MarksOnGun = MarksOnGun;
            entity.AchievementsIdObject.MovingAvgDamage = MovingAvgDamage;
            entity.AchievementsIdObject.MedalMonolith = MedalMonolith;
            entity.AchievementsIdObject.MedalAntiSpgFire = MedalAntiSpgFire;
            entity.AchievementsIdObject.MedalGore = MedalGore;
            entity.AchievementsIdObject.MedalCoolBlood = MedalCoolBlood;
            entity.AchievementsIdObject.MedalStark = MedalStark;
        }
    }
}