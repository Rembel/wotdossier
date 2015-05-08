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

        private readonly IEnumerable<StatisticSlice> _list;

        #endregion

        protected StatisticSlice PrevStatisticSlice { get; set; }

        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public DateTime PrevStatisticSliceDate
        {
            get { return PrevStatisticSlice.Date; }
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
        protected PeriodStatisticViewModel(DateTime updated, IEnumerable<StatisticSlice> list)
        {
            _list = list;
            Updated = updated;

            AppSettings appSettings = SettingsReader.Get();
            StatisticSlice prevStatistic = GetPrevStatistic(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.PrevDate);
            PrevStatisticSlice = prevStatistic ?? this.ToStatisticSlice();

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
                StatisticSlice prevStatistic = GetPrevStatistic(statisticPeriod, prevDateTime);
                SetPreviousStatistic(prevStatistic);
            }
        }

        private StatisticSlice GetPrevStatistic(StatisticPeriod statisticPeriod, DateTime? prevDateTime)
        {
            StatisticSlice prevStatistic = null;
            switch (statisticPeriod)
            {
                case StatisticPeriod.Recent:
                    prevStatistic = _list.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Date <= Updated);
                    break;
                case StatisticPeriod.LastWeek:
                    prevStatistic = _list.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Date <= DateTime.Now.AddDays(-7));
                    break;
                case StatisticPeriod.AllObservationPeriod:
                    prevStatistic = _list.OrderBy(x => x.Date).FirstOrDefault();
                    break;
                case StatisticPeriod.Custom:
                    prevStatistic  = _list.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Date <= prevDateTime) ??
                                     _list.OrderBy(x => x.Date).FirstOrDefault();
                    break;
            }
            return prevStatistic;
        }

        /// <summary>
        /// Sets the previous statistic.
        /// </summary>
        /// <param name="prevStatistic">The previous statistic.</param>
        public void SetPreviousStatistic(StatisticSlice prevStatistic)
        {
            PrevStatisticSlice = prevStatistic ?? this.ToStatisticSlice();

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
            list.AddRange(_list.Select(x => (T)x.Statistic));
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

    public class StatisticSlice
    {
        private readonly Lazy<PeriodStatisticViewModel> _lazyModel;
        private PeriodStatisticViewModel _statistic;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticSlice" /> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="lazyModel">The lazy model.</param>
        public StatisticSlice(DateTime date, Lazy<PeriodStatisticViewModel> lazyModel)
        {
            Date = date;
            _lazyModel = lazyModel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticSlice"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="model">The model.</param>
        public StatisticSlice(DateTime date, PeriodStatisticViewModel model)
        {
            Date = date;
            Statistic = model;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the statistic.
        /// </summary>
        public PeriodStatisticViewModel Statistic
        {
            get { return _statistic ?? (_statistic = _lazyModel.Value); }
            set { _statistic = value; }
        }
    }

    public static class StatisticSliceExtensions
    {
        public static StatisticSlice ToStatisticSlice(this PeriodStatisticViewModel statistic)
        {
            return new StatisticSlice(statistic.Updated, statistic);
        }

        public static StatisticSlice ToStatisticSlice(this PeriodStatisticViewModel statistic, Func<PeriodStatisticViewModel> valueFactory)
        {
            return new StatisticSlice(statistic.Updated, new Lazy<PeriodStatisticViewModel>(valueFactory));
        }
    }
}