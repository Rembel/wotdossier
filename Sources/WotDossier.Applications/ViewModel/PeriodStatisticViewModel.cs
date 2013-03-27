using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    public abstract class PeriodStatisticViewModel<T> : StatisticViewModelBase, INotifyPropertyChanged where T : StatisticViewModelBase
    {
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

        public int HitsPercentsDelta
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
            //OnPropertyChanged(PropBattlesCountDelta);
            //OnPropertyChanged(PropWinsDelta);
            //OnPropertyChanged(PropLossesDelta);
            //OnPropertyChanged(PropSurvivedBattlesDelta);
            //OnPropertyChanged(PropXpDelta);
            //OnPropertyChanged(PropBattleAvgXpDelta);
            //OnPropertyChanged(PropMaxXpDelta);
            //OnPropertyChanged(PropFragsDelta);
            //OnPropertyChanged(PropSpottedDelta);
            //OnPropertyChanged(PropHitsPercentsDelta);
            //OnPropertyChanged(PropDamageDealtDelta);
            //OnPropertyChanged(PropCapturePointsDelta);
            //OnPropertyChanged(PropDroppedCapturePointsDelta);
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