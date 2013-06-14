using System.Collections.Generic;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.View;
using WotDossier.Domain;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private AppSettings _appSettings;
        private List<string> _servers = new List<string>{"ru", "eu"};
        private List<string> _languages = new List<string>{"ru-RU", "en-US"};
        private List<StatisticPeriod> _periods = new List<StatisticPeriod>{StatisticPeriod.Recent, StatisticPeriod.LastWeek, StatisticPeriod.AllObservationPeriod};
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand SelectReplaysFolderCommand { get; set; }

        public AppSettings AppSettings
        {
            get { return _appSettings; }
        }

        public List<string> Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }

        public List<string> Languages
        {
            get { return _languages; }
            set { _languages = value; }
        }

        public List<StatisticPeriod> Periods
        {
            get { return _periods; }
            set { _periods = value; }
        }

        public StatisticPeriod Period
        {
            get { return AppSettings.Period; }
            set
            {
                AppSettings.Period = value;
            }
        }

        public string ReplaysFolderPath
        {
            get { return AppSettings.ReplaysFolderPath; }
            set
            {
                AppSettings.ReplaysFolderPath = value;
                RaisePropertyChanged("ReplaysFolderPath");
            }
        }

        public SettingsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public SettingsViewModel(ISettingsView view) : base(view)
        {
            SaveCommand = new DelegateCommand(OnSave);
            SelectReplaysFolderCommand = new DelegateCommand(OnSelectReplaysFolder);
            _appSettings = SettingsReader.Get();
        }

        private void OnSelectReplaysFolder()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            bool? showDialog = dialog.ShowDialog();
            if (showDialog == true)
            {
                ReplaysFolderPath = dialog.SelectedPath;
            }
        }

        private void OnSave()
        {
            SettingsReader.Save(_appSettings);
            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(Period));
            ViewTyped.Close();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> then this object is a child of another ViewModel.</param>
        public SettingsViewModel(ISettingsView view, bool isChild) : base(view, isChild)
        {
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }
    }
}
