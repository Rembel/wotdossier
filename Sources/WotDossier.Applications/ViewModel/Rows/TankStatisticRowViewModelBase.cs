using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModelBase<T> : PeriodStatisticViewModel<T>, ITankStatisticRow where T : StatisticViewModelBase
    {
        private DateTime _lastBattle;
        private IEnumerable<FragsJson> _tankFrags;
        private bool _isFavorite;

        #region Common

        public TankIcon Icon { get; set; }

        public string Tank { get; set; }

        public int Type { get; set; }

        public int CountryId { get; set; }

        public int TankId { get; set; }

        public int TankUniqueId { get; set; }

        public bool IsPremium { get; set; }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        #endregion

        #region [ ITankRowBattles ]

        public int SurvivedAndWon { get; set; }

        public double SurvivedAndWonPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedAndWon/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region [ ITankRowDamage ]

        public int DamagePerHit
        {
            get
            {
                if (Hits > 0)
                {
                    return DamageDealt/Hits;
                }
                return 0;
            }
        }

        #endregion
       
        #region [ ITankRowFrags ]

        public int MaxFrags { get; set; }

        public double FragsPerBattle
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Frags/(double) BattlesCount;
                }
                return 0;
            }
        }

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        public int PattonFrags { get; set; }

        #endregion

        #region [ ITankRowPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ ITankRowRatings ]

        public int DamageRatingRev1
        {
            get
            {
                if (DamageTaken > 0)
                {
                    return (int) (DamageDealt/(double) DamageTaken*100);
                }
                return 0;
            }
        }

        public int MarkOfMastery { get; set; }

        #endregion

        #region [ ITankRowTime ]
        public DateTime LastBattle
        {
            get { return _lastBattle; }
            set { _lastBattle = value; }
        }

        public TimeSpan AverageBattleTime { get; set; }
        #endregion

        public int OriginalXP { get; set; }
        public int DamageAssistedTrack { get; set; }
        public int DamageAssistedRadio { get; set; }
        public double Mileage { get; set; }
        public int ShotsReceived { get; set; }
        public int NoDamageShotsReceived { get; set; }
        public int PiercedReceived { get; set; }
        public int HeHitsReceived { get; set; }
        public int HeHits { get; set; }
        public int Pierced { get; set; }
        public int XpBefore88 { get; set; }
        public int BattlesCountBefore88 { get; set; }
        public int BattlesCount88 { get; set; }
        public int MaxDamage { get; set; }

        IEnumerable<ITankStatisticRow> ITankStatisticRow.GetAll()
        {
            return GetAll().Cast<ITankStatisticRow>();
        }

        public void SetPreviousStatistic(ITankStatisticRow model)
        {
            SetPreviousStatistic((T) model);
        }

        public int DamageAssisted
        {
            get
            {
                return DamageAssistedTrack + DamageAssistedRadio;
            }
        }

        public double AvgDamageAssistedTrack
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedTrack / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssistedRadio
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssistedRadio / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgDamageAssisted
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return DamageAssisted / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public double AvgOriginalXP
        {
            get
            {
                if (BattlesCount88 > 0)
                {
                    return OriginalXP / (double)BattlesCount88;
                }
                return 0;
            }
        }

        public override double WN8Rating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    double expDamage = BattlesCount * Description.Expectancy.Wn8NominalDamage / BattlesCount;
                    double expSpotted = BattlesCount * Description.Expectancy.Wn8NominalSpotted / BattlesCount;
                    double expDef = BattlesCount * Description.Expectancy.Wn8NominalDefence / BattlesCount;
                    double expWinRate = BattlesCount * Description.Expectancy.Wn8NominalWinRate / 100.0 / BattlesCount;
                    double expFrags = BattlesCount * Description.Expectancy.Wn8NominalFrags / BattlesCount;

                    return RatingHelper.CalcWN8(AvgDamageDealt, expDamage, AvgFrags, expFrags, AvgSpotted, expSpotted, AvgDroppedCapturePoints, expDef, WinsPercent, expWinRate);
                }
                return 0;
            }
        }

        public override double PerformanceRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.PerformanceRating(BattlesCount, Wins,
                        BattlesCount*Description.Expectancy.PRNominalDamage,
                        DamageDealt, Tier);
                }
                return 0;
            }
        }

        public override double WN8RatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    double expDamage = BattlesCountDelta * Description.Expectancy.Wn8NominalDamage / BattlesCountDelta;
                    double expSpotted = BattlesCountDelta * Description.Expectancy.Wn8NominalSpotted / BattlesCountDelta;
                    double expDef = BattlesCountDelta * Description.Expectancy.Wn8NominalDefence / BattlesCountDelta;
                    double expWinRate = BattlesCountDelta * Description.Expectancy.Wn8NominalWinRate / 100.0 / BattlesCountDelta;
                    double expFrags = BattlesCountDelta * Description.Expectancy.Wn8NominalFrags / BattlesCountDelta;

                    return RatingHelper.CalcWN8(AvgDamageDealtForPeriod, expDamage, AvgFragsForPeriod, expFrags, AvgSpottedForPeriod, expSpotted, AvgDroppedCapturePointsForPeriod, expDef, WinsPercentForPeriod, expWinRate);
                }
                return 0;
            }
        }

        protected TankStatisticRowViewModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModel"/> class.
        /// </summary>
        /// <param name="tank">The tank.</param>
        public TankStatisticRowViewModelBase(TankJson tank)
            : this(tank, new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankStatisticRowViewModelBase(TankJson tank, List<T> list)
            : base(Utils.UnixDateToDateTime(tank.Common.updated), list)
        {
            Tier = tank.Common.tier;
            Type = tank.Common.type;
            Tank = tank.Common.tanktitle;
            Icon = tank.Description.Icon;
            Description = tank.Description;
            CountryId = tank.Common.countryid;
            TankId = tank.Common.tankid;
            TankUniqueId = tank.UniqueId();
            TankFrags = tank.Frags;
            Mileage = tank.Common.mileage / 1000;    
            
            #region [ ITankRowBattleAwards ]
            BattleHero = tank.Achievements.battleHeroes;
            Warrior = tank.Achievements.warrior;
            Invader = tank.Achievements.invader;
            Sniper = tank.Achievements.sniper;
            Defender = tank.Achievements.defender;
            SteelWall = tank.Achievements.steelwall;
            Confederate = tank.Achievements.supporter;
            Scout = tank.Achievements.scout;
            PatrolDuty = tank.Achievements.evileye;
            BrothersInArms = tank.Achievements.medalBrothersInArms;
            CrucialContribution = tank.Achievements.medalCrucialContribution;
            CoolHeaded = tank.Achievements.ironMan;
            LuckyDevil = tank.Achievements.luckyDevil;
            Spartan = tank.Achievements.sturdy;
            Ranger = tank.Achievements.huntsman;
            #endregion

            #region [ ITankRowEpic ]
            Boelter = tank.Achievements.medalWittmann;
            RadleyWalters = tank.Achievements.medalRadleyWalters;
            LafayettePool = tank.Achievements.medalLafayettePool;
            Orlik = tank.Achievements.medalOrlik;
            Oskin = tank.Achievements.medalOskin;
            Lehvaslaiho = tank.Achievements.medalLehvaslaiho;
            Nikolas = tank.Achievements.medalNikolas;
            Halonen = tank.Achievements.medalHalonen;
            Burda = tank.Achievements.medalBurda;
            Pascucci = tank.Achievements.medalPascucci;
            Dumitru = tank.Achievements.medalDumitru;
            TamadaYoshio = tank.Achievements.medalTamadaYoshio;
            Billotte = tank.Achievements.medalBillotte;
            BrunoPietro = tank.Achievements.medalBrunoPietro;
            Tarczay = tank.Achievements.medalTarczay;
            Kolobanov = tank.Achievements.medalKolobanov;
            Fadin = tank.Achievements.medalFadin;
            HeroesOfRassenay = tank.Achievements.heroesOfRassenay;
            DeLanglade = tank.Achievements.medalDeLanglade;
            #endregion

            #region [ ITankRowMedals]
            Kay = tank.Achievements.medalKay;
            Carius = tank.Achievements.medalCarius;
            Knispel = tank.Achievements.medalKnispel;
            Poppel = tank.Achievements.medalPoppel;
            Abrams = tank.Achievements.medalAbrams;
            Leclerk = tank.Achievements.medalLeClerc;
            Lavrinenko = tank.Achievements.medalLavrinenko;
            Ekins = tank.Achievements.medalEkins;
            #endregion

            #region [ ITankRowSeries ]
            ReaperLongest = tank.Achievements.maxKillingSeries;
            ReaperProgress = tank.Achievements.killingSeries;
            SharpshooterLongest = tank.Achievements.maxSniperSeries;
            SharpshooterProgress = tank.Achievements.sniperSeries;
            MasterGunnerLongest = tank.Achievements.maxPiercingSeries;
            MasterGunnerProgress = tank.Achievements.piercingSeries;
            InvincibleLongest = tank.Achievements.maxInvincibleSeries;
            InvincibleProgress = tank.Achievements.invincibleSeries;
            SurvivorLongest = tank.Achievements.maxDiehardSeries;
            SurvivorProgress = tank.Achievements.diehardSeries;
            #endregion

            #region [ ITankRowSpecialAwards ]
            Kamikaze = tank.Achievements.kamikaze;
            Raider = tank.Achievements.raider;
            Bombardier = tank.Achievements.bombardier;
            Reaper = tank.Achievements.maxKillingSeries;
            Sharpshooter = tank.Achievements.maxSniperSeries;
            Invincible = tank.Achievements.maxInvincibleSeries;
            Survivor = tank.Achievements.maxDiehardSeries;
            MouseTrap = tank.Achievements.mousebane;
            Hunter = tank.Achievements.beasthunter;
            Sinai = tank.Achievements.sinai;
            PattonValley = tank.Achievements.pattonValley;
            #endregion

            #region [ ITankRowRatings ]
            MarkOfMastery = tank.Achievements.markOfMastery;
            #endregion

            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
        }

        public TankDescription Description { get; set; }

        public IEnumerable<FragsJson> TankFrags
        {
            get { return _tankFrags; }
            set { _tankFrags = value; }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Tank;
        }
    }
}