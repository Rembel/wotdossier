using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Rows
{
    public class TankStatisticRowViewModelBase<T> : PeriodStatisticViewModel<T>, ITankStatisticRow where T : StatisticViewModelBase
    {
        private DateTime _lastBattle;
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

        #region [ IStatisticBattles ]

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

        #region [ IStatisticDamage ]

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
       
        #region [ IStatisticFrags ]

        public int Tier8Frags { get; set; }

        public int BeastFrags { get; set; }

        public int SinaiFrags { get; set; }

        public int PattonFrags { get; set; }

        public int MouseFrags { get; set; }

        #endregion

        #region [ IStatisticPerformance ]

        public int Shots { get; set; }

        public int Hits { get; set; }

        #endregion

        #region [ IStatisticRatings ]

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

        #region [ IStatisticTime ]
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
            return GetAllSlices().Cast<ITankStatisticRow>();
        }

        public void SetPreviousStatistic(ITankStatisticRow model)
        {
            SetPreviousStatistic((T) model);
        }

        public ITankStatisticRow GetPreviousStatistic()
        {
            return (ITankStatisticRow) PrevStatistic;
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

                    return RatingHelper.Wn8(AvgDamageDealt, expDamage, AvgFrags, expFrags, AvgSpotted, expSpotted, AvgDroppedCapturePoints, expDef, WinsPercent, expWinRate);
                }
                return 0;
            }
        }

        public override double RBR
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.PersonalRating(BattlesCount, BattlesCount88, Wins/(double)BattlesCount,
                        SurvivedBattles/(double)BattlesCount, DamageDealt/(double)BattlesCount, AvgOriginalXP, AvgDamageAssistedRadio, AvgDamageAssistedTrack);
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

        public override double PerformanceRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.PerformanceRating(BattlesCountDelta, WinsDelta,
                        BattlesCountDelta * Description.Expectancy.PRNominalDamage,
                        DamageDealtDelta, Tier, false);
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

                    return RatingHelper.Wn8(AvgDamageDealtForPeriod, expDamage, AvgFragsForPeriod, expFrags, AvgSpottedForPeriod, expSpotted, AvgDroppedCapturePointsForPeriod, expDef, WinsPercentForPeriod, expWinRate);
                }
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TankStatisticRowViewModelBase{T}"/> class.
        /// </summary>
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
            Mileage = tank.Common.mileage / 1000;

            MarkOfMastery = tank.Achievements.markOfMastery;
            
            Updated = Utils.UnixDateToDateTime(tank.Common.updated);
        }

        public TankDescription Description { get; set; }

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