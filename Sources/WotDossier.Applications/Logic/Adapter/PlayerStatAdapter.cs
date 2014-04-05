using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class PlayerStatAdapter : IStatisticAdapter<PlayerStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks)
        {
            Battles_count = tanks.Sum(x => x.A15x15.battlesCount);
            Wins = tanks.Sum(x => x.A15x15.wins);
            Losses = tanks.Sum(x => x.A15x15.losses);
            Survived_battles = tanks.Sum(x => x.A15x15.survivedBattles);
            Xp = tanks.Sum(x => x.A15x15.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp / (double)Battles_count;
            }
            Max_xp = tanks.Max(x => x.A15x15.maxXP);
            Frags = tanks.Sum(x => x.A15x15.frags);
            Spotted = tanks.Sum(x => x.A15x15.spotted);
            Hits_percents = tanks.Sum(x => x.A15x15.hits) / ((double)tanks.Sum(x => x.A15x15.shots)) * 100.0;
            Damage_dealt = tanks.Sum(x => x.A15x15.damageDealt);
            Damage_taken = tanks.Sum(x => x.A15x15.damageReceived);
            Capture_points = tanks.Sum(x => x.A15x15.capturePoints);
            Dropped_capture_points = tanks.Sum(x => x.A15x15.droppedCapturePoints);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier * x.A15x15.battlesCount) / (double)Battles_count;
            }

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
            CoolHeaded = tanks.Sum(x => x.Achievements.ironMan);
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

            PerformanceRating = RatingHelper.GetPerformanceRating(tanks);
            WN8Rating = RatingHelper.GetWN8Rating(tanks);
            RBR = RatingHelper.GetRBR(tanks);
        }

        public PlayerStatAdapter(PlayerStat stat)
        {
            Battles_count = stat.dataField.statistics.all.battles;
            Wins = stat.dataField.statistics.all.wins;
            Losses = stat.dataField.statistics.all.losses;
            Survived_battles = stat.dataField.statistics.all.survived_battles;
            Xp = stat.dataField.statistics.all.xp;
            Battle_avg_xp = stat.dataField.statistics.all.battle_avg_xp;
            Max_xp = stat.dataField.statistics.max_xp;
            Frags = stat.dataField.statistics.all.frags;
            Spotted = stat.dataField.statistics.all.spotted;
            Hits_percents = stat.dataField.statistics.all.hits_percents;
            Damage_dealt = stat.dataField.statistics.all.damage_dealt;
            Capture_points = stat.dataField.statistics.all.capture_points;
            Dropped_capture_points = stat.dataField.statistics.all.dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.dataField.updated_at).ToLocalTime();
            Created = Utils.UnixDateToDateTime((long)stat.dataField.created_at).ToLocalTime();
            if (Battles_count > 0 && stat.dataField.vehicles != null)
            {
                int battlesCount = stat.dataField.vehicles.Sum(x => x.statistics.all.battles);
                if (battlesCount > 0)
                {
                    AvgLevel = stat.dataField.vehicles.Sum(x => (x.tank != null ? x.tank.level : 1)*x.statistics.all.battles)/(double) battlesCount;
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
            BrothersInArms = stat.dataField.achievements.medal_brothers_in_arms;
            CrucialContribution = stat.dataField.achievements.medal_crucial_contribution;
            CoolHeaded = stat.dataField.achievements.iron_man;
            LuckyDevil = stat.dataField.achievements.lucky_devil;
            Sturdy = stat.dataField.achievements.sturdy;
            Sniper2 = stat.dataField.achievements.sniper2;
            MainGun = stat.dataField.achievements.main_gun;

            #endregion

            #region [ Epic ]

            Boelter = stat.dataField.achievements.medal_boelter;
            RadleyWalters = stat.dataField.achievements.medal_radley_walters;
            LafayettePool = stat.dataField.achievements.medal_lafayette_pool;
            Orlik = stat.dataField.achievements.medal_orlik;
            Oskin = stat.dataField.achievements.medal_oskin;
            Lehvaslaiho = stat.dataField.achievements.medal_lehvaslaiho;
            Nikolas = stat.dataField.achievements.medal_nikolas;
            Halonen = stat.dataField.achievements.medal_halonen;
            Burda = stat.dataField.achievements.medal_burda;
            Pascucci = stat.dataField.achievements.medal_pascucci;
            Dumitru = stat.dataField.achievements.medal_dumitru;
            TamadaYoshio = stat.dataField.achievements.medal_tamada_yoshio;
            Billotte = stat.dataField.achievements.medal_billotte;
            BrunoPietro = stat.dataField.achievements.medal_bruno_pietro;
            Tarczay = stat.dataField.achievements.medal_tarczay;
            Kolobanov = stat.dataField.achievements.medal_kolobanov;
            Fadin = stat.dataField.achievements.medal_fadin;
            HeroesOfRassenay = stat.dataField.achievements.medal_heroes_of_rassenay;
            DeLanglade = stat.dataField.achievements.medal_delanglade;

            #endregion

            #region [ Medals]

            Kay = stat.dataField.achievements.medal_kay;
            Carius = stat.dataField.achievements.medal_carius;
            Knispel = stat.dataField.achievements.medal_knispel;
            Poppel = stat.dataField.achievements.medal_poppel;
            Abrams = stat.dataField.achievements.medal_abrams;
            Leclerk = stat.dataField.achievements.medal_le_clerc;
            Lavrinenko = stat.dataField.achievements.medal_lavrinenko;
            Ekins = stat.dataField.achievements.medal_ekins;

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
            PattonValley = stat.dataField.achievements.patton_valley;
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

        public DateTime Created { get; set; }

        public int Battles_count { get; set; }

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

        public DateTime Updated { get; set; }

        public double AvgLevel { get; set; }

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

        public int CoolHeaded { get; set; }

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

        public double RBR { get; set; }

        public double WN8Rating { get; set; }

        public double PerformanceRating { get; set; }

        #endregion

        public virtual void Update(PlayerStatisticEntity entity)
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
            entity.AchievementsIdObject.CoolHeaded = CoolHeaded;
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

            #endregion
        }
    }
}