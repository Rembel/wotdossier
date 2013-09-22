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
            new ListItem<StatisticPeriod>(StatisticPeriod.Custom, Resources.Resources.StatisticPeriod_Custom)
        };
        private List<DateTime> _prevDates;
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

        public List<ListItem<string>> Languages
        {
            get { return _languages; }
            set { _languages = value; }
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
            get { return AppSettings.Period; }
            set { AppSettings.Period = value; }
        }

        public DateTime? PrevDate
        {
            get { return AppSettings.PrevDate; }
            set { AppSettings.PrevDate = value; }
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
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public SettingsViewModel([Import(typeof(ISettingsView))]ISettingsView view, [Import]DossierRepository dossierRepository)
            : base(view)
        {
            _dossierRepository = dossierRepository;
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
            try
            {
                var player = WotApiClient.Instance.SearchPlayer(_appSettings);
                if (player != null)
                {
                    _appSettings.PlayerUniqueId = player.id;
                    _dossierRepository.GetOrCreatePlayer(player.name, player.id, Utils.UnixDateToDateTime((long)player.created_at));
                }

                SettingsReader.Save(_appSettings);
                EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(Period, PrevDate));
                ViewTyped.Close();
            }
            catch (Exception e)
            {
                _log.Error("Can't get player info from server", e);
                MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerData, Resources.Resources.WindowCaption_Error,
                                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

    public class ListItem<TId>
    {
        public TId Id { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ListItem(TId id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
