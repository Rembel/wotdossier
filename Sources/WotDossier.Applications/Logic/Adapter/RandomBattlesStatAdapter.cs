using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class RandomBattlesStatAdapter : AbstractStatisticAdapter<RandomBattlesStatisticEntity>, IRandomBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RandomBattlesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.A15x15)
        {
            Func<TankJson, AchievementsJson> achievementsPredicate = tankJson => tankJson.Achievements ?? new AchievementsJson();

                #region [ BattleAwards ]

            Warrior = tanks.Sum(x => achievementsPredicate(x).Warrior);
            Invader = tanks.Sum(x => achievementsPredicate(x).Invader);
            Sniper = tanks.Sum(x => achievementsPredicate(x).Sniper);
            Sniper2 = tanks.Sum(x => achievementsPredicate(x).Sniper2);
            MainGun = tanks.Sum(x => achievementsPredicate(x).MainGun);
            Defender = tanks.Sum(x => achievementsPredicate(x).Defender);
            SteelWall = tanks.Sum(x => achievementsPredicate(x).SteelWall);
            Confederate = tanks.Sum(x => achievementsPredicate(x).Confederate);
            Scout = tanks.Sum(x => achievementsPredicate(x).Scout);
            PatrolDuty = tanks.Sum(x => achievementsPredicate(x).PatrolDuty);
            BrothersInArms = tanks.Sum(x => achievementsPredicate(x).BrothersInArms);
            CrucialContribution = tanks.Sum(x => achievementsPredicate(x).CrucialContribution);
            IronMan = tanks.Sum(x => achievementsPredicate(x).IronMan);
            LuckyDevil = tanks.Sum(x => achievementsPredicate(x).LuckyDevil);
            Sturdy = tanks.Sum(x => achievementsPredicate(x).Sturdy);

            #endregion

            #region [ Epic ]

            Boelter = tanks.Sum(x => achievementsPredicate(x).Boelter);
            RadleyWalters = tanks.Sum(x => achievementsPredicate(x).RadleyWalters);
            LafayettePool = tanks.Sum(x => achievementsPredicate(x).LafayettePool);
            Orlik = tanks.Sum(x => achievementsPredicate(x).Orlik);
            Oskin = tanks.Sum(x => achievementsPredicate(x).Oskin);
            Lehvaslaiho = tanks.Sum(x => achievementsPredicate(x).Lehvaslaiho);
            Nikolas = tanks.Sum(x => achievementsPredicate(x).Nikolas);
            Halonen = tanks.Sum(x => achievementsPredicate(x).Halonen);
            Burda = tanks.Sum(x => achievementsPredicate(x).Burda);
            Pascucci = tanks.Sum(x => achievementsPredicate(x).Pascucci);
            Dumitru = tanks.Sum(x => achievementsPredicate(x).Dumitru);
            TamadaYoshio = tanks.Sum(x => achievementsPredicate(x).TamadaYoshio);
            Billotte = tanks.Sum(x => achievementsPredicate(x).Billotte);
            BrunoPietro = tanks.Sum(x => achievementsPredicate(x).BrunoPietro);
            Tarczay = tanks.Sum(x => achievementsPredicate(x).Tarczay);
            Kolobanov = tanks.Sum(x => achievementsPredicate(x).Kolobanov);
            Fadin = tanks.Sum(x => achievementsPredicate(x).Fadin);
            HeroesOfRassenay = tanks.Sum(x => achievementsPredicate(x).HeroesOfRassenay);
            DeLanglade = tanks.Sum(x => achievementsPredicate(x).DeLanglade);

            #endregion

            #region [ Series ]

            SharpshooterLongest = tanks.Max(x => achievementsPredicate(x).SharpshooterLongest);
            MasterGunnerLongest = tanks.Max(x => achievementsPredicate(x).MasterGunnerLongest);

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = tanks.Sum(x => achievementsPredicate(x).Kamikaze);
            Raider = tanks.Sum(x => achievementsPredicate(x).Raider);
            Bombardier = tanks.Sum(x => achievementsPredicate(x).Bombardier);
            Reaper = tanks.Max(x => achievementsPredicate(x).ReaperLongest);
            Invincible = tanks.Max(x => achievementsPredicate(x).InvincibleLongest);
            Survivor = tanks.Max(x => achievementsPredicate(x).SurvivorLongest);
            //count Maus frags
            Func<TankJson, IEnumerable<FragsJson>> fragsPredicate = tankJson => tankJson.Frags ?? new List<FragsJson>();

            MouseTrap = tanks.Sum(x => fragsPredicate(x).Where(f => f.TankUniqueId == 10027).Sum(s => s.Count)) / 10;
            Hunter = tanks.Sum(x => achievementsPredicate(x).FragsBeast) / 100;
            Sinai = tanks.Sum(x => achievementsPredicate(x).FragsSinai) / 100;
            PattonValley = tanks.Sum(x => achievementsPredicate(x).FragsPatton) / 100;
            Huntsman = tanks.Sum(x => achievementsPredicate(x).Huntsman);

            #endregion

            MarksOnGun = tanks.Max(x => achievementsPredicate(x).MarksOnGun);
            MovingAvgDamage = (int) tanks.Average(x => achievementsPredicate(x).MovingAvgDamage);
            MedalMonolith = tanks.Sum(x => achievementsPredicate(x).MedalMonolith);
            MedalAntiSpgFire = tanks.Sum(x => achievementsPredicate(x).MedalAntiSpgFire);
            MedalGore = tanks.Sum(x => achievementsPredicate(x).MedalGore);
            MedalCoolBlood = tanks.Sum(x => achievementsPredicate(x).MedalCoolBlood);
            MedalStark = tanks.Sum(x => achievementsPredicate(x).MedalStark);
            DamageRating = tanks.Max(x => achievementsPredicate(x).DamageRating);

            Impenetrable = tanks.Sum(x => achievementsPredicate(x).Impenetrable);
            MaxAimerSeries = tanks.Max(x => achievementsPredicate(x).MaxAimerSeries);
            ShootToKill = tanks.Sum(x => achievementsPredicate(x).ShootToKill);
            Fighter = tanks.Sum(x => achievementsPredicate(x).Fighter);
            Duelist = tanks.Sum(x => achievementsPredicate(x).Duelist);
            Demolition = tanks.Sum(x => achievementsPredicate(x).Demolition);
            Arsonist = tanks.Sum(x => achievementsPredicate(x).Arsonist);
            Bonecrusher = tanks.Sum(x => achievementsPredicate(x).Bonecrusher);
            Charmed = tanks.Sum(x => achievementsPredicate(x).Charmed);
            Even = tanks.Sum(x => achievementsPredicate(x).Even);

            MasterGunner = tanks.Sum(x => achievementsPredicate(x).MasterGunner);
            Alaric = tanks.Sum(x => achievementsPredicate(x).Alaric);
        }

        public RandomBattlesStatAdapter(Player stat)
        {
            BattlesCount = stat.dataField.statistics.all.battles;
            Wins = stat.dataField.statistics.all.wins;
            Losses = stat.dataField.statistics.all.losses;
            SurvivedBattles = stat.dataField.statistics.all.survived_battles;
            Xp = stat.dataField.statistics.all.xp;
            BattleAvgXp = stat.dataField.statistics.all.battle_avg_xp;
            MaxXp = stat.dataField.statistics.all.max_xp;
            MaxDamage = stat.dataField.statistics.all.max_damage;
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
                    AvgLevel = stat.dataField.vehicles.Sum(x => (x.description != null ? x.description.Tier : 1)*x.all.battles)/(double) battlesCount;
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
            HeroesOfRassenay = stat.dataField.achievements.achievements.heroesOfRassenay;
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

            if (stat.dataField.vehicles != null)
            {
                Tanks = stat.dataField.vehicles.Where(x => x.description != null).Select(x => (ITankStatisticRow)new RandomBattlesTankStatisticRowViewModel(Dal.DataMapper.Map(x), new List<StatisticSlice>())).OrderByDescending(x => x.Tier).ToList();
                WN8Rating = RatingHelper.Wn8(Tanks);
                PerformanceRating = RatingHelper.PerformanceRating(Tanks);

                //double battlesCount88 = ???;

                //double avgOriginalXp = stat.dataField.statistics.all.base_xp;
                //double avgDamageAssistedRadio = stat.dataField.statistics.all.avg_damage_assisted_radio;
                //double avgDamageAssistedTrack = stat.dataField.statistics.all.avg_damage_assisted_track;

                //RBR = RatingHelper.PersonalRating(BattlesCount, battlesCount88, Wins / (double)BattlesCount,
                //    SurvivedBattles / (double)BattlesCount, DamageDealt / (double)BattlesCount, avgOriginalXp, avgDamageAssistedRadio, avgDamageAssistedTrack);
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

        public int Lumberjack { get; set; }
        public int MasterGunner { get; set; }
        public int Alaric { get; set; }

        public int Impenetrable { get; set; }
        public int MaxAimerSeries { get; set; }
        public int ShootToKill { get; set; }
        public int Fighter { get; set; }
        public int Duelist { get; set; }
        public int Demolition { get; set; }
        public int Arsonist { get; set; }
        public int Bonecrusher { get; set; }
        public int Charmed { get; set; }
        public int Even { get; set; }

        #endregion

        public override void Update(RandomBattlesStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new RandomBattlesAchievementsEntity { UId = Guid.NewGuid() };
                entity.AchievementsUId = entity.AchievementsIdObject.UId;
            }

            Mapper.Map<IRandomBattlesAchievements>(this, entity.AchievementsIdObject);
        }
    }
}