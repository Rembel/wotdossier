using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract partial class StatisticViewModelBase : PeriodStatisticViewModel, IStatisticBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticViewModelBase"/> class.
        /// </summary>
        protected StatisticViewModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticViewModelBase"/> class.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="list">The list.</param>
        protected StatisticViewModelBase(DateTime updated, IEnumerable<StatisticSlice> list)
            : base(updated, list ?? new List<StatisticSlice>())
        {
        }

        #region Statistic

        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }

        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int MaxFrags { get; set; }
        public int Spotted { get; set; }

        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public int MaxDamage { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }

        public int MarkOfMastery { get; set; }
        public TimeSpan PlayTime { get; set; }

        public double Tier { get; set; }

        public int Draws
        {
            get { return BattlesCount - Wins - Losses; }
        }

        public double KillDeathRatio
        {
            get
            {
                if (BattlesCount - SurvivedBattles > 0)
                {
                    return Frags / (double)(BattlesCount - SurvivedBattles);
                }
                return 0;
            }
        }

        public double DamageRatio
        {
            get
            {
                if (DamageTaken > 0)
                {
                    return DamageDealt / (double)DamageTaken;
                }
                return 0;
            }
        }

        #region Percents

        public double HitsPercents { get; set; }

        public double WinsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Wins/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        public double DrawsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Draws / (double)BattlesCount * 100.0;
                }
                return 0;
            }
        }

        public double LossesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Losses/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }
        public double SurvivedBattlesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedBattles/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region Average values

        public double AvgXp
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Xp/(double) BattlesCount;
                }
                return 0;
            }
        }

        public double AvgFrags
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

        public double AvgSpotted
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Spotted / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDamageTaken
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DamageTaken / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDamageDealt
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DamageDealt / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return CapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DroppedCapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        #endregion

        #region Ratings
        
        public double WN7Rating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.Wn7(BattlesCount, AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);
                }
                return 0;
            }
        }

        public double NoobRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.WotNoobsRating(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgCapturePoints, AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double EffRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.EffectivityRating(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgCapturePoints, AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double KievArmorRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.KievArmorRating(BattlesCount, AvgXp, AvgDamageDealt, WinsPercent/100.0,
                        AvgFrags, AvgSpotted, AvgCapturePoints,
                        AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double XEFF
        {
            get { return RatingHelper.Xwn8(WN8Rating); }
        }

        public double XWN
        {
            get { return RatingHelper.Xwn6(WN7Rating); }
        }

        private double _performanceRating;
        private double _rbr;

        public virtual double PerformanceRating
        {
            get { return _performanceRating; }
            set
            {
                _performanceRating = value;
                OnPropertyChanged("PerformanceRating");
            }
        }

        private double _wn8Rating;
        public virtual double WN8Rating
        {
            get { return _wn8Rating; }
            set
            {
                _wn8Rating = value;
                OnPropertyChanged("WN8Rating");
            }
        }

        public virtual double RBR
        {
            get { return _rbr; }
            set
            {
                _rbr = value;
                OnPropertyChanged("RBR");
            }
        }

        #endregion

        #endregion

        private StatisticViewModelBase TypedPrevStatistic
        {
            get { return (StatisticViewModelBase)PrevStatisticSlice.Statistic; }
        }
    }
}