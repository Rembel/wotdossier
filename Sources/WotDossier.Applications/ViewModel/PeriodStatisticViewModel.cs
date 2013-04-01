using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Common;

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
        public static readonly string PropBattleAvgXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BattleAvgXpDelta);
        public static readonly string PropMaxXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.MaxXpDelta);
        public static readonly string PropFragsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.FragsDelta);
        public static readonly string PropSpottedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SpottedDelta);
        public static readonly string PropHitsPercentsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HitsPercentsDelta);
        public static readonly string PropDamageDealtDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DamageDealtDelta);
        public static readonly string PropCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.CapturePointsDelta);
        public static readonly string PropDroppedCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DroppedCapturePointsDelta);

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

        public int BattleAvgXpDelta
        {
            get { return BattleAvgXp - PrevStatistic.BattleAvgXp; }
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

        #endregion

        public T PrevStatistic { get; protected set; }

        public DateTime PreviousDate { get; protected set; }
        public int BattleAvgXpForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return XpDelta / BattlesCountDelta;
                }
                return 0;
            }
        }
        public double WinsPercentForPeriod
        {
            get
            {
                return WinsDelta / (double)BattlesCountDelta * 100.0;
            }
        }
        public double LossesPercentForPeriod
        {
            get
            {
                return LossesDelta / (double)BattlesCountDelta * 100.0;
            }
        }
        public double SurvivedBattlesPercentForPeriod
        {
            get
            {
                return SurvivedBattlesDelta / (double)BattlesCountDelta * 100.0;
            }
        }

        protected PeriodStatisticViewModel(DateTime updated, IEnumerable<T> list)
        {
            _list = list;
            Updated = updated;
            T prevPlayerStatistic = _list.Where(x => x.Updated <= Updated).OrderByDescending(x => x.Updated).FirstOrDefault();
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);
            PreviousDate = Updated;
        }

        public void SetPreviousDate(DateTime date)
        {
            PreviousDate = date;
            T prevPlayerStatistic = _list.OrderBy(x => x.Updated).FirstOrDefault(x => x.Updated <= date);
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);
            OnPropertyChanged(PropBattlesCountDelta);
            OnPropertyChanged(PropWinsDelta);
            OnPropertyChanged(PropLossesDelta);
            OnPropertyChanged(PropSurvivedBattlesDelta);
            OnPropertyChanged(PropXpDelta);
            OnPropertyChanged(PropBattleAvgXpDelta);
            OnPropertyChanged(PropMaxXpDelta);
            OnPropertyChanged(PropFragsDelta);
            OnPropertyChanged(PropSpottedDelta);
            OnPropertyChanged(PropHitsPercentsDelta);
            OnPropertyChanged(PropDamageDealtDelta);
            OnPropertyChanged(PropCapturePointsDelta);
            OnPropertyChanged(PropDroppedCapturePointsDelta);
        }

        public List<StatisticViewModelBase> GetAll()
        {
            List<StatisticViewModelBase> list = new List<StatisticViewModelBase>();
            list.AddRange(_list);
            list.Add(this);
            return list;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}