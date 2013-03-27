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
            get { return BattlesCount - PrevPlayerStatistic.BattlesCount; }
        }

        public int WinsDelta
        {
            get { return Wins - PrevPlayerStatistic.Wins; }
        }

        public double WinsPercentDelta
        {
            get { return WinsPercent - PrevPlayerStatistic.WinsPercent; }
        }

        public int LossesDelta
        {
            get { return Losses - PrevPlayerStatistic.Losses; }
        }

        public double LossesPercentDelta
        {
            get { return LossesPercent - PrevPlayerStatistic.LossesPercent; }
        }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - PrevPlayerStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - PrevPlayerStatistic.SurvivedBattlesPercent; }
        }

        public int XpDelta
        {
            get { return Xp - PrevPlayerStatistic.Xp; }
        }

        public int BattleAvgXpDelta
        {
            get { return BattleAvgXp - PrevPlayerStatistic.BattleAvgXp; }
        }

        public int MaxXpDelta
        {
            get { return MaxXp - PrevPlayerStatistic.MaxXp; }
        }

        public int FragsDelta
        {
            get { return Frags - PrevPlayerStatistic.Frags; }
        }

        public int SpottedDelta
        {
            get { return Spotted - PrevPlayerStatistic.Spotted; }
        }

        public double HitsPercentsDelta
        {
            get { return HitsPercents - PrevPlayerStatistic.HitsPercents; }
        }

        public int DamageDealtDelta
        {
            get { return DamageDealt - PrevPlayerStatistic.DamageDealt; }
        }

        public int CapturePointsDelta
        {
            get { return CapturePoints - PrevPlayerStatistic.CapturePoints; }
        }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - PrevPlayerStatistic.DroppedCapturePoints; }
        }

        #endregion

        public T PrevPlayerStatistic { get; protected set; }

        public DateTime PreviousDate { get; protected set; }

        protected PeriodStatisticViewModel(DateTime updated, IEnumerable<T> list)
        {
            _list = list;
            Updated = updated;
            T prevPlayerStatistic = _list.Where(x => x.Updated <= Updated).OrderByDescending(x => x.Updated).FirstOrDefault();
            PrevPlayerStatistic = (T)((object)prevPlayerStatistic ?? this);
            PreviousDate = Updated;
        }

        public void SetPreviousDate(DateTime date)
        {
            PreviousDate = date;
            T prevPlayerStatistic = _list.OrderBy(x => x.Updated).FirstOrDefault(x => x.Updated <= date);
            PrevPlayerStatistic = (T)((object)prevPlayerStatistic ?? this);
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

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}