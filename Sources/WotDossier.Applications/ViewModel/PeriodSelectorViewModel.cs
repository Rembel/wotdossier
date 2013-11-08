using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Foundation;

namespace WotDossier.Applications.ViewModel
{
    public class PeriodSelectorViewModel : Framework.Foundation.Model
    {
        private PeriodSettings _periodSettings;
        private List<ListItem<StatisticPeriod>> _periods = new List<ListItem<StatisticPeriod>>
        {
            new ListItem<StatisticPeriod>(StatisticPeriod.Recent, Resources.Resources.StatisticPeriod_Recent),
            new ListItem<StatisticPeriod>(StatisticPeriod.LastWeek, Resources.Resources.StatisticPeriod_LastWeek), 
            new ListItem<StatisticPeriod>(StatisticPeriod.AllObservationPeriod, Resources.Resources.StatisticPeriod_AllObservationPeriod),
            new ListItem<StatisticPeriod>(StatisticPeriod.LastNBattles, Resources.Resources.StatisticPeriod_LastNBattles_ComboItem),
            new ListItem<StatisticPeriod>(StatisticPeriod.Custom, Resources.Resources.StatisticPeriod_Custom)
        };
        private List<DateTime> _prevDates;

        public PeriodSettings PeriodSettings
        {
            get { return _periodSettings; }
        }

        public List<ListItem<StatisticPeriod>> Periods
        {
            get { return _periods; }
            set { _periods = value; }
        }

        public List<DateTime> PrevDates
        {
            get { return _prevDates; }
            set
            {
                _prevDates = value;
                RaisePropertyChanged("PrevDates");
                RaisePropertyChanged("PrevDate");
            }
        }

        public StatisticPeriod Period
        {
            get { return PeriodSettings.Period; }
            set
            {
                PeriodSettings.Period = value;
                RaisePropertyChanged("LastNBattlesVisible");
                RaisePropertyChanged("PeriodsVisible");
            }
        }

        public bool LastNBattlesVisible
        {
            get { return Period == StatisticPeriod.LastNBattles; }
        }

        public bool PeriodsVisible
        {
            get { return Period == StatisticPeriod.Custom; }
        }

        public int LastNBattles
        {
            get { return PeriodSettings.LastNBattles; }
            set
            {
                PeriodSettings.LastNBattles = value;
                RaisePropertyChanged("LastNBattles");
            }
        }

        public DateTime? PrevDate
        {
            get { return PeriodSettings.PrevDate; }
            set
            {
                PeriodSettings.PrevDate = value;
                RaisePropertyChanged("PrevDate");
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
            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(Period, PrevDate, LastNBattles));
        }

        /// <summary>
        /// Raises the <see cref="Model.PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Save();
            base.OnPropertyChanged(e);
        }
    }
}