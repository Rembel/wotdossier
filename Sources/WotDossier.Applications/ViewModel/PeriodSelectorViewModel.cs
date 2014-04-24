using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.Foundation;

namespace WotDossier.Applications.ViewModel
{
    public class PeriodSelectorViewModel : Framework.Foundation.Model
    {
        private static readonly string PropPrevDate = TypeHelper.GetPropertyName<PeriodSelectorViewModel>(x => x.PrevDate);
        private static readonly string PropPeriod = TypeHelper.GetPropertyName<PeriodSelectorViewModel>(x => x.Period);
        private static readonly string PropLastNBattles = TypeHelper.GetPropertyName<PeriodSelectorViewModel>(x => x.LastNBattles);

        private List<string> _list = new List<string> { PropPrevDate, PropPeriod, PropLastNBattles };

        private List<ListItem<StatisticPeriod>> _periods = new List<ListItem<StatisticPeriod>>
        {
            new ListItem<StatisticPeriod>(StatisticPeriod.Recent, Resources.Resources.StatisticPeriod_Recent),
            new ListItem<StatisticPeriod>(StatisticPeriod.LastWeek, Resources.Resources.StatisticPeriod_LastWeek), 
            new ListItem<StatisticPeriod>(StatisticPeriod.AllObservationPeriod, Resources.Resources.StatisticPeriod_AllObservationPeriod),
            new ListItem<StatisticPeriod>(StatisticPeriod.LastNBattles, Resources.Resources.StatisticPeriod_LastNBattles_ComboItem),
            new ListItem<StatisticPeriod>(StatisticPeriod.Custom, Resources.Resources.StatisticPeriod_Custom_ComboItem)
        };

        /// <summary>
        /// Occurs when [period settings updated].
        /// </summary>
        public event Action PeriodSettingsUpdated;

        private PeriodSettings _periodSettings;
        /// <summary>
        /// Gets the period settings.
        /// </summary>
        /// <value>
        /// The period settings.
        /// </value>
        public PeriodSettings PeriodSettings
        {
            get { return _periodSettings; }
        }

        /// <summary>
        /// Gets or sets the periods.
        /// </summary>
        /// <value>
        /// The periods.
        /// </value>
        public List<ListItem<StatisticPeriod>> Periods
        {
            get { return _periods; }
            set { _periods = value; }
        }

        private List<DateTime> _prevDates;
        /// <summary>
        /// Gets or sets the previous dates.
        /// </summary>
        /// <value>
        /// The previous dates.
        /// </value>
        public List<DateTime> PrevDates
        {
            get { return _prevDates; }
            set
            {
                _prevDates = value;
                RaisePropertyChanged("PrevDates");
                RaisePropertyChanged(PropPrevDate);
            }
        }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public StatisticPeriod Period
        {
            get { return PeriodSettings.Period; }
            set
            {
                PeriodSettings.Period = value;
                RaisePropertyChanged("LastNBattlesVisible");
                RaisePropertyChanged("PeriodsVisible");
                RaisePropertyChanged(PropPeriod);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [last n battles visible].
        /// </summary>
        /// <value>
        /// <c>true</c> if [last n battles visible]; otherwise, <c>false</c>.
        /// </value>
        public bool LastNBattlesVisible
        {
            get { return Period == StatisticPeriod.LastNBattles; }
        }

        /// <summary>
        /// Gets a value indicating whether [periods visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [periods visible]; otherwise, <c>false</c>.
        /// </value>
        public bool PeriodsVisible
        {
            get { return Period == StatisticPeriod.Custom; }
        }

        /// <summary>
        /// Gets or sets the last n battles.
        /// </summary>
        /// <value>
        /// The last n battles.
        /// </value>
        public int LastNBattles
        {
            get { return PeriodSettings.LastNBattles; }
            set
            {
                PeriodSettings.LastNBattles = value;
                RaisePropertyChanged(PropLastNBattles);
            }
        }

        /// <summary>
        /// Gets or sets the previous date.
        /// </summary>
        /// <value>
        /// The previous date.
        /// </value>
        public DateTime? PrevDate
        {
            get { return PeriodSettings.PrevDate; }
            set
            {
                PeriodSettings.PrevDate = value;
                RaisePropertyChanged(PropPrevDate);
            }
        }

        public PeriodSelectorViewModel()
        {
            _periodSettings = SettingsReader.Get().PeriodSettings;
        }

        private void Save()
        {
            AppSettings appSettings = SettingsReader.Get();
            appSettings.PeriodSettings = PeriodSettings;
            SettingsReader.Save(appSettings);
        }

        /// <summary>
        /// Raises the <see cref="Model.PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Save();

            if (_list.Contains(e.PropertyName))
            {
                if (PeriodSettingsUpdated != null)
                {
                    PeriodSettingsUpdated();
                }
            }

            base.OnPropertyChanged(e);
        }
    }
}