using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract class PeriodStatisticViewModel : INotifyPropertyChanged
    {
        #region Fields
        
        private readonly IEnumerable<PeriodStatisticViewModel> _list;

        #endregion

        protected PeriodStatisticViewModel PrevStatisticSlice { get; set; }

        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public DateTime PrevStatisticSliceDate
        {
            get { return PrevStatisticSlice.Updated; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodStatisticViewModel"/> class.
        /// </summary>
        protected PeriodStatisticViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodStatisticViewModel"/> class.
        /// </summary>
        /// <param name="updated">The updated.</param>
        /// <param name="list">The list.</param>
        protected PeriodStatisticViewModel(DateTime updated, IEnumerable<PeriodStatisticViewModel> list)
        {
            _list = list;
            Updated = updated;

            AppSettings appSettings = SettingsReader.Get();
            PeriodStatisticViewModel prevStatistic = GetPrevStatistic(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.PrevDate);
            PrevStatisticSlice = prevStatistic ?? this;

            if (_list.Any())
            {
                EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            }
        }

        /// <summary>
        /// Called when [statistic period changed].
        /// </summary>
        /// <param name="eventArgs">The event arguments.</param>
        protected virtual void OnStatisticPeriodChanged(StatisticPeriodChangedEvent eventArgs)
        {
            StatisticPeriod statisticPeriod = eventArgs.StatisticPeriod;
            DateTime? prevDateTime = eventArgs.PrevDateTime;

            if (statisticPeriod != StatisticPeriod.LastNBattles)
            {
                PeriodStatisticViewModel prevStatistic = GetPrevStatistic(statisticPeriod, prevDateTime);
                SetPreviousStatistic(prevStatistic);
            }
        }

        private PeriodStatisticViewModel GetPrevStatistic(StatisticPeriod statisticPeriod, DateTime? prevDateTime)
        {
            PeriodStatisticViewModel prevStatistic = null;
            switch (statisticPeriod)
            {
                case StatisticPeriod.Recent:
                    prevStatistic = _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= Updated);
                    break;
                case StatisticPeriod.LastWeek:
                    prevStatistic = _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= DateTime.Now.AddDays(-7));
                    break;
                case StatisticPeriod.AllObservationPeriod:
                    prevStatistic = _list.OrderBy(x => x.Updated).FirstOrDefault();
                    break;
                case StatisticPeriod.Custom:
                    prevStatistic  = _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= prevDateTime) ??
                                     _list.OrderBy(x => x.Updated).FirstOrDefault();
                    break;
            }
            return prevStatistic;
        }

        /// <summary>
        /// Sets the previous statistic.
        /// </summary>
        /// <param name="prevStatistic">The previous statistic.</param>
        public void SetPreviousStatistic(PeriodStatisticViewModel prevStatistic)
        {
            PrevStatisticSlice = prevStatistic ?? this;

            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                OnPropertyChanged(propertyInfo.Name);
            }
        }

        /// <summary>
        /// Gets all statistic slices.
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllSlices<T>()
            where T : PeriodStatisticViewModel
        {
            List<T> list = new List<T>();
            list.AddRange((IEnumerable<T>) _list);
            list.Add((T) this);
            return list;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}