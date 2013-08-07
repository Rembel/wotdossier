using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.ViewModel
{
    public abstract class PeriodStatisticViewModel<T> : StatisticViewModelBase, INotifyPropertyChanged where T : StatisticViewModelBase
    {
        #region Constants

        public static readonly string PropBattlesCountDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BattlesCountDelta);
        public static readonly string PropWinsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WinsDelta);
        public static readonly string PropWinsPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WinsPercentDelta);
        public static readonly string PropLossesDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LossesDelta);
        public static readonly string PropLossesPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LossesPercentDelta);
        public static readonly string PropSurvivedBattlesDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivedBattlesPercentDelta);
        public static readonly string PropSurvivedBattlesPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivedBattlesDelta);
        public static readonly string PropXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.XpDelta);
        public static readonly string PropMaxXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.MaxXpDelta);
        public static readonly string PropFragsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.FragsDelta);
        public static readonly string PropSpottedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SpottedDelta);
        public static readonly string PropHitsPercentsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HitsPercentsDelta);
        public static readonly string PropDamageDealtDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DamageDealtDelta);
        public static readonly string PropCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.CapturePointsDelta);
        public static readonly string PropDroppedCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DroppedCapturePointsDelta);

        public static readonly string PropEffRatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.EffRatingDelta);
        public static readonly string PropKievArmorRatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KievArmorRatingDelta);
        public static readonly string PropWN6RatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WN6RatingDelta);

        public static readonly string PropAvgCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgCapturePointsDelta);
        public static readonly string PropAvgDamageDealtDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDamageDealtDelta);
        public static readonly string PropAvgDroppedCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDroppedCapturePointsDelta);
        public static readonly string PropAvgFragsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgFragsDelta);
        public static readonly string PropAvgSpottedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgSpottedDelta);
        public static readonly string PropAvgXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgXpDelta);

        public static readonly string PropAvgCapturePointsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgCapturePointsForPeriod);
        public static readonly string PropAvgDamageDealtForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDamageDealtForPeriod);
        public static readonly string PropAvgDroppedCapturePointsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDroppedCapturePointsForPeriod);
        public static readonly string PropAvgFragsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgFragsForPeriod);
        public static readonly string PropAvgSpottedForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgSpottedForPeriod);
        public static readonly string PropAvgXpForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgXpForPeriod);

        public static readonly string PropWN6RatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WN6RatingForPeriod);
        public static readonly string PropEffRatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.EffRatingForPeriod);
        public static readonly string PropKievArmorRatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KievArmorRatingForPeriod);

        public static readonly string PropPreviousDate = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.PreviousDate);

        #endregion

        #region Fields

        private readonly IEnumerable<T> _list;

        #endregion

        #region Common delta

        public int BattlesCountDelta
        {
            get { return BattlesCount - PrevStatistic.BattlesCount; }
        }

        public int WinsDelta
        {
            get { return Wins - PrevStatistic.Wins; }
        }

        public double WinsPercentDelta
        {
            get { return WinsPercent - PrevStatistic.WinsPercent; }
        }

        public int LossesDelta
        {
            get { return Losses - PrevStatistic.Losses; }
        }

        public double LossesPercentDelta
        {
            get { return LossesPercent - PrevStatistic.LossesPercent; }
        }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - PrevStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - PrevStatistic.SurvivedBattlesPercent; }
        }

        public int XpDelta
        {
            get { return Xp - PrevStatistic.Xp; }
        }

        public int MaxXpDelta
        {
            get { return MaxXp - PrevStatistic.MaxXp; }
        }

        public int FragsDelta
        {
            get { return Frags - PrevStatistic.Frags; }
        }

        public int SpottedDelta
        {
            get { return Spotted - PrevStatistic.Spotted; }
        }

        public double HitsPercentsDelta
        {
            get { return HitsPercents - PrevStatistic.HitsPercents; }
        }

        public int DamageDealtDelta
        {
            get { return DamageDealt - PrevStatistic.DamageDealt; }
        }

        public int CapturePointsDelta
        {
            get { return CapturePoints - PrevStatistic.CapturePoints; }
        }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - PrevStatistic.DroppedCapturePoints; }
        }

        public double WN6RatingDelta
        {
            get { return WN6Rating - PrevStatistic.WN6Rating; }
        }

        public double EffRatingDelta
        {
            get { return EffRating - PrevStatistic.EffRating; }
        }

        public double KievArmorRatingDelta
        {
            get { return KievArmorRating - PrevStatistic.KievArmorRating; }
        }

        public double XvmRatingDelta
        {
            get { return XEFF - PrevStatistic.XEFF; }
        }

        #endregion

        #region Average values

        public double AvgXpDelta
        {
            get
            {
                return AvgXp - PrevStatistic.AvgXp;
            }
        }

        public double AvgFragsDelta
        {
            get
            {
                return AvgFrags - PrevStatistic.AvgFrags;
            }
        }

        public double AvgSpottedDelta
        {
            get
            {
                return AvgSpotted - PrevStatistic.AvgSpotted;
            }
        }

        public double AvgDamageDealtDelta
        {
            get
            {
                return AvgDamageDealt - PrevStatistic.AvgDamageDealt;
            }
        }

        public double AvgCapturePointsDelta
        {
            get
            {
                return AvgCapturePoints - PrevStatistic.AvgCapturePoints;
            }
        }

        public double AvgDroppedCapturePointsDelta
        {
            get
            {
                return AvgDroppedCapturePoints - PrevStatistic.AvgDroppedCapturePoints;
            }
        }

        #endregion

        public T PrevStatistic { get; protected set; }

        public DateTime PreviousDate
        {
            get { return PrevStatistic.Updated; }
        }

        #region Statistic For Period

        public double WinsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return WinsDelta/(double) BattlesCountDelta*100.0;
                }
                return 0;
            }
        }

        public double LossesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return LossesDelta/(double) BattlesCountDelta*100.0;
                }
                return 0;
            }
        }

        public double SurvivedBattlesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SurvivedBattlesDelta/(double) BattlesCountDelta*100.0;
                }
                return 0;
            }
        }

        public double HitsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    //TODO
                    return 0;
                }
                return 0;
            }
        }

        public double TierForInterval
        {
            get
            {
                return (Tier * BattlesCount - PrevStatistic.Tier * PrevStatistic.BattlesCount) / BattlesCountDelta;
            }
        }

        public double EffRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcER(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                               AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double WN6RatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcWN6(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                                AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
                }
                return 0;
            }
        }

        public double KievArmorRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcKievArmorRating(BattlesCountDelta, AvgXpForPeriod, AvgDamageDealtForPeriod,
                                                            WinsPercentForPeriod/100.0,
                                                            AvgFragsForPeriod, AvgSpottedForPeriod,
                                                            AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double AvgXpForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return XpDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgFragsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return FragsDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgSpottedForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SpottedDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDamageDealtForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DamageDealtDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return CapturePointsDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DroppedCapturePointsDelta/(double) BattlesCountDelta;
                }
                return 0;
            }
        }

        #endregion

        protected PeriodStatisticViewModel(DateTime updated, List<T> list)
        {
            _list = list;
            Updated = updated;
            
            AppSettings appSettings = SettingsReader.Get();
            T prevPlayerStatistic = GetPrevStatistic(appSettings.Period, appSettings.PrevDate);
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);

            if (_list.Any())
            {
                EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            }
        }

        private void OnStatisticPeriodChanged(StatisticPeriodChangedEvent eventArgs)
        {
            StatisticPeriod statisticPeriod = eventArgs.StatisticPeriod;
            DateTime? prevDateTime = eventArgs.PrevDateTime;
            
            T prevStatistic = GetPrevStatistic(statisticPeriod, prevDateTime); 
            SetPreviousStatistic(prevStatistic);
        }

        private T GetPrevStatistic(StatisticPeriod statisticPeriod, DateTime? prevDateTime)
        {
            switch (statisticPeriod)
            {
                case StatisticPeriod.Recent:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= Updated);
                
                case StatisticPeriod.LastWeek:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= DateTime.Now.AddDays(-7));

                case StatisticPeriod.AllObservationPeriod:
                    return _list.OrderBy(x => x.Updated).FirstOrDefault();

                case StatisticPeriod.Custom:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= prevDateTime);
            }
            return null;
        }

        protected virtual void SetPreviousStatistic(T prevPlayerStatistic)
        {
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);

            OnPropertyChanged(PropBattlesCountDelta);
            OnPropertyChanged(PropWinsDelta);
            OnPropertyChanged(PropLossesDelta);
            OnPropertyChanged(PropSurvivedBattlesDelta);
            OnPropertyChanged(PropXpDelta);
            OnPropertyChanged(PropMaxXpDelta);
            OnPropertyChanged(PropFragsDelta);
            OnPropertyChanged(PropSpottedDelta);
            OnPropertyChanged(PropHitsPercentsDelta);
            OnPropertyChanged(PropDamageDealtDelta);
            OnPropertyChanged(PropCapturePointsDelta);
            OnPropertyChanged(PropDroppedCapturePointsDelta);
            OnPropertyChanged(PropWinsPercentDelta);
            OnPropertyChanged(PropLossesPercentDelta);
            OnPropertyChanged(PropSurvivedBattlesPercentDelta);

            OnPropertyChanged(PropAvgCapturePointsDelta);
            OnPropertyChanged(PropAvgDamageDealtDelta);
            OnPropertyChanged(PropAvgDroppedCapturePointsDelta);
            OnPropertyChanged(PropAvgFragsDelta);
            OnPropertyChanged(PropAvgSpottedDelta);
            OnPropertyChanged(PropAvgXpDelta);

            OnPropertyChanged(PropEffRatingDelta);
            OnPropertyChanged(PropKievArmorRatingDelta);
            OnPropertyChanged(PropWN6RatingDelta);

            OnPropertyChanged(PropAvgCapturePointsForPeriod);
            OnPropertyChanged(PropAvgDamageDealtForPeriod);
            OnPropertyChanged(PropAvgDroppedCapturePointsForPeriod);
            OnPropertyChanged(PropAvgFragsForPeriod);
            OnPropertyChanged(PropAvgSpottedForPeriod);
            OnPropertyChanged(PropAvgXpForPeriod);

            OnPropertyChanged(PropWN6RatingForPeriod);
            OnPropertyChanged(PropEffRatingForPeriod);
            OnPropertyChanged(PropKievArmorRatingForPeriod);

            OnPropertyChanged(PropPreviousDate);
        }

        public List<T> GetAll()
        {
            List<StatisticViewModelBase> list = new List<StatisticViewModelBase>();
            list.AddRange(_list);
            list.Add(this);
            return list.Cast<T>().ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StatisticPeriodChangedEvent : BaseEvent<StatisticPeriodChangedEvent>
    {
        public StatisticPeriod StatisticPeriod { get; set; }

        public DateTime? PrevDateTime { get; set; }

        public StatisticPeriodChangedEvent()
        {
        }

        public StatisticPeriodChangedEvent(StatisticPeriod statisticPeriod, DateTime? prevDate)
        {
            StatisticPeriod = statisticPeriod;
            PrevDateTime = prevDate;
        }
    }
}