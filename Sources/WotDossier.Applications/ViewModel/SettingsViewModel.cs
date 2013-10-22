using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using Common.Logging;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Player;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private readonly DossierRepository _dossierRepository;
        private static readonly ILog _log = LogManager.GetLogger("ShellViewModel");

        private AppSettings _appSettings;
        private List<string> _servers = new List<string> { "ru", "eu" };
        private List<ListItem<string>> _languages = new List<ListItem<string>>
        {
            new ListItem<string>("ru-RU", Resources.Resources.Language_Russian),
            new ListItem<string>("en-US", Resources.Resources.Language_English),
        };
        private List<ListItem<StatisticPeriod>> _periods = new List<ListItem<StatisticPeriod>>
        {
            new ListItem<StatisticPeriod>(StatisticPeriod.Recent, Resources.Resources.StatisticPeriod_Recent),
            new ListItem<StatisticPeriod>(StatisticPeriod.LastWeek, Resources.Resources.StatisticPeriod_LastWeek), 
            new ListItem<StatisticPeriod>(StatisticPeriod.AllObservationPeriod, Resources.Resources.StatisticPeriod_AllObservationPeriod),
            //new ListItem<StatisticPeriod>(StatisticPeriod.LastNBattles, Resources.Resources.StatisticPeriod_LastNBattles),
            new ListItem<StatisticPeriod>(StatisticPeriod.Custom, Resources.Resources.StatisticPeriod_Custom)
        };
        private List<DateTime> _prevDates;
        private bool _nameChanged;
        public DelegateCommand SaveCommand { get; set; }

        public AppSettings AppSettings
        {
            get { return _appSettings; }
        }

        public List<string> Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }

        public List<ListItem<string>> Languages
        {
            get { return _languages; }
            set { _languages = value; }
        }

        public string PlayerName
        {
            get { return AppSettings.PlayerName; }
            set
            {
                AppSettings.PlayerName = value;
                _nameChanged = true;
            }
        }

        public List<ListItem<StatisticPeriod>> Periods
        {
            get { return _periods; }
            set { _periods = value; }
        }

        public List<DateTime> PrevDates
        {
            get { return _prevDates; }
            set { _prevDates = value; }
        }

        public StatisticPeriod Period
        {
            get { return AppSettings.PeriodSettings.Period; }
            set
            {
                AppSettings.PeriodSettings.Period = value;
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
            get { return AppSettings.PeriodSettings.LastNBattles; }
            set { AppSettings.PeriodSettings.LastNBattles = value; }
        }

        public DateTime? PrevDate
        {
            get { return AppSettings.PeriodSettings.PrevDate; }
            set { AppSettings.PeriodSettings.PrevDate = value; }
        }

        public bool CheckForUpdates
        {
            get { return AppSettings.CheckForUpdates; }
            set { AppSettings.CheckForUpdates = value; }
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
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository">The dossier repository.</param>
        [ImportingConstructor]
        public SettingsViewModel([Import(typeof(ISettingsView))]ISettingsView view, [Import]DossierRepository dossierRepository)
            : base(view)
        {
            _dossierRepository = dossierRepository;
            SaveCommand = new DelegateCommand(OnSave);
            _appSettings = SettingsReader.Get();
        }

        private void OnSave()
        {
            if (_nameChanged)
            {
                PlayerSearchJson player = null;
                
                player = WotApiClient.Instance.SearchPlayer(_appSettings, _appSettings.PlayerName);
                
                if (player != null)
                {
                    _appSettings.PlayerId = player.id;
                    double createdAt = WotApiClient.Instance.LoadPlayerStat(_appSettings, player.id).dataField.created_at;
                    _dossierRepository.GetOrCreatePlayer(player.nickname, player.id, Utils.UnixDateToDateTime((long)createdAt));
                }
                else
                {
                    _appSettings.PlayerId = 0;
                    MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerData, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            SettingsReader.Save(_appSettings);
            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(Period, PrevDate, LastNBattles));
            ViewTyped.Close();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> then this object is a child of another ViewModel.</param>
        public SettingsViewModel(ISettingsView view, bool isChild)
            : base(view, isChild)
        {
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }
    }
}
