﻿using System.Windows.Threading;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using WotDossier.Applications.BattleModeStrategies;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Common.Collections;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private static readonly ILog _log = LogManager.GetLogger("ShellViewModel");

        private static readonly string PropPeriodTabHeader = TypeHelper.GetPropertyName<ShellViewModel>(x => x.PeriodTabHeader);
        public static readonly string PropPlayerStatistic = TypeHelper<ShellViewModel>.PropertyName(v => v.PlayerStatistic);
        public static readonly string PropMasterTanker = TypeHelper<ShellViewModel>.PropertyName(v => v.MasterTanker);
        public static readonly string PropTanks = TypeHelper<ShellViewModel>.PropertyName(v => v.Tanks);
        public static readonly string PropPeriodSelector = TypeHelper<ShellViewModel>.PropertyName(v => v.PeriodSelector);
        public static readonly string PropBattleModeSelector = TypeHelper<ShellViewModel>.PropertyName(v => v.BattleModeSelector);
        public static readonly string PropLastUsedTanksList = TypeHelper<ShellViewModel>.PropertyName(v => v.LastUsedTanksList);

        #region [ Properties and Fields ]

        private readonly DossierRepository _dossierRepository;
        
        private PlayerStatisticViewModel _playerStatistic;
        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged(PropPlayerStatistic);
            }
        }

        private List<TankRowMasterTanker> _masterTanker;
        public List<TankRowMasterTanker> MasterTanker
        {
            get { return _masterTanker; }
            set
            {
                _masterTanker = value;
                RaisePropertyChanged(PropMasterTanker);
            }
        }

        private List<ITankStatisticRow> _tanks = new List<ITankStatisticRow>();
        public List<ITankStatisticRow> Tanks
        {
            get
            {
                List<ITankStatisticRow> tankStatisticRowViewModels = TankFilter.Filter(_tanks);
                if (tankStatisticRowViewModels.Count > 0)
                {
                    TotalTankStatisticRowViewModel totalRow =
                        new TotalTankStatisticRowViewModel(tankStatisticRowViewModels.ToList());
                    tankStatisticRowViewModels.Insert(0, totalRow);
                }
                FooterList<ITankStatisticRow> statisticRowViewModels = new FooterList<ITankStatisticRow>();
                statisticRowViewModels.AddRange(tankStatisticRowViewModels);
                return statisticRowViewModels;
            }
            set
            {
                _tanks = value;
                RaisePropertyChanged(PropTanks);
            }
        }

        private FraggsCountViewModel _fraggsCount = new FraggsCountViewModel();
        public FraggsCountViewModel FraggsCount
        {
            get { return _fraggsCount; }
            set { _fraggsCount = value; }
        }

        public TankFilterViewModel TankFilter { get; set; }

        public ProgressControlViewModel ProgressView { get; set; }

        private string _periodTabHeader;
        /// <summary>
        /// Gets or sets the period tab header.
        /// </summary>
        /// <value>
        /// The period tab header.
        /// </value>
        public string PeriodTabHeader
        {
            get { return _periodTabHeader; }
            set
            {
                _periodTabHeader = value;
                RaisePropertyChanged(PropPeriodTabHeader);
            }
        }

        private PeriodSelectorViewModel _periodSelector;
        public PeriodSelectorViewModel PeriodSelector
        {
            get { return _periodSelector; }
            set
            {
                _periodSelector = value;
                RaisePropertyChanged(PropPeriodSelector);
            }
        }

        private BattleModeSelectorViewModel _battleModeSelector;
        public BattleModeSelectorViewModel BattleModeSelector
        {
            get { return _battleModeSelector; }
            set
            {
                _battleModeSelector = value;
                RaisePropertyChanged(PropBattleModeSelector);
            }
        }

        public PlayerChartsViewModel ChartView { get; set; }

        public ReplaysViewModel ReplaysViewModel { get; set; }

        public List<ITankStatisticRow> LastUsedTanksList
        {
            get
            {
                List<ITankStatisticRow> list = _tanks.Where(x => x.LastBattle > PlayerStatistic.PreviousDate).ToList();
                return list;
            }
        }

        private bool _loadInProgress;
        public bool LoadInProgress
        {
            get { return _loadInProgress; }
            set
            {
                _loadInProgress = value;
                RaisePropertyChanged("LoadCommand");
            }
        }

        #endregion

        #region Commands

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand SettingsCommand { get; set; }
        public DelegateCommand AboutCommand { get; set; }

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }
        public DelegateCommand<object> AddToFavoriteCommand { get; set; }
        public DelegateCommand<object> RemoveFromFavoriteCommand { get; set; }

        public DelegateCommand<object> OpenClanCommand { get; set; }
        public DelegateCommand CompareCommand { get; set; }
        public DelegateCommand ExportToCsvCommand { get; set; }
        public DelegateCommand ExportFragsToCsvCommand { get; set; }
        public DelegateCommand SearchPlayersCommand { get; set; }
        public DelegateCommand SearchClansCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        [ImportingConstructor]
        public ShellViewModel([Import(typeof(IShellView))]IShellView view, [Import]DossierRepository dossierRepository)
            : base(view, false)
        {
            _dossierRepository = dossierRepository;

            LoadCommand = new DelegateCommand(OnLoad, CanLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            AboutCommand = new DelegateCommand(OnAbout);
            
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            AddToFavoriteCommand = new DelegateCommand<object>(OnAddToFavorite, CanAddToFavorite);
            RemoveFromFavoriteCommand = new DelegateCommand<object>(OnRemoveFromFavorite, CanRemoveFromFavorite);

            OpenClanCommand = new DelegateCommand<object>(OnOpenClanCommand);
            CompareCommand = new DelegateCommand(OnCompare);
            ExportToCsvCommand = new DelegateCommand(OnExportToCsv);
            ExportFragsToCsvCommand = new DelegateCommand(OnExportFragsToCsv);
            SearchPlayersCommand = new DelegateCommand(OnSearchPlayers);
            SearchClansCommand = new DelegateCommand(OnSearchClans);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(e));

            TankFilter = new TankFilterViewModel();
            TankFilter.PropertyChanged += TankFilterOnPropertyChanged;

            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            ProgressView = new ProgressControlViewModel();
            PeriodSelector = new PeriodSelectorViewModel();
            BattleModeSelector = new BattleModeSelectorViewModel();

            BattleModeSelector.PropertyChanged += (sender, args) => OnLoad();

            ChartView = new PlayerChartsViewModel();

            SetPeriodTabHeader();

            ReplaysViewModel = new ReplaysViewModel(_dossierRepository, ProgressView, ChartView);

            ViewTyped.Closing += ViewTypedOnClosing;
        }

        private bool CanLoad()
        {
            return !LoadInProgress;
        }

        private void OnExportToCsv()
        {
            CsvExportProvider provider = new CsvExportProvider();
            provider.Export(_tanks, new List<Type>
            {
                typeof(ITankRowBase),
                typeof(IStatisticXp),
                typeof(ITankRowBattles),
                typeof(ITankRowFrags),
                typeof(ITankRowDamage),
                typeof(IStatisticBattleAwards),
                typeof(IStatisticSpecialAwards),
                typeof(IStatisticSeries),
                typeof(IStatisticMedals),
                typeof(IStatisticEpic),
                typeof(ITankRowTime),
                typeof(ITankRowPerformance)
            });
        }

        private void OnExportFragsToCsv()
        {
            CsvExportProvider provider = new CsvExportProvider();
            provider.Export(_tanks.SelectMany(x => x.TankFrags.Select(f => new ExportTankFragModel(x, f))).ToList(),
                new List<Type>
                {
                    typeof (IExportTankFragModel),
                });
        }

        private void OnSearchClans()
        {
            var export = CompositionContainerFactory.Instance.GetExport<ClanSearchViewModel>();
            if (export != null)
            {
                ClanSearchViewModel viewModel = export;
                viewModel.Show();
            }
        }

        private void OnSearchPlayers()
        {
            var export = CompositionContainerFactory.Instance.GetExport<PlayerSearchViewModel>();
            if (export != null)
            {
                PlayerSearchViewModel viewModel = export;
                viewModel.Show();
            }
        }

        private void OnCompare()
        {
            var export = CompositionContainerFactory.Instance.GetExport<PlayersCompareViewModel>();
            if (export != null)
            {
                PlayersCompareViewModel viewModel = export;
                viewModel.Show();
            }
        }

        private void OnOpenClanCommand(object param)
        {
            ClanData clan;
            using (new WaitCursor())
            {
                clan = WotApiClient.Instance.LoadClan(PlayerStatistic.Clan.Id, SettingsReader.Get());
            }
            if (clan != null)
            {
                var export = CompositionContainerFactory.Instance.GetExport<ClanViewModel>();
                if (export != null)
                {
                    ClanViewModel viewModel = export;
                    viewModel.Init(clan);
                    viewModel.Show();
                }
            }
            else
            {
                MessageBox.Show(Resources.Resources.Msg_CantGetClanDataFromServer, Resources.Resources.WindowCaption_Information,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Handlers

        private void OnRemoveFromFavorite(object row)
        {
            ITankStatisticRow model = row as ITankStatisticRow;
            if (model != null)
            {
                SetFavorite(model, false);
            }
        }

        private bool CanRemoveFromFavorite(object row)
        {
            ITankStatisticRow model = row as ITankStatisticRow;
            return model != null && model.IsFavorite;
        }

        private void OnAddToFavorite(object data)
        {
            ITankStatisticRow model = data as ITankStatisticRow;
            if (model != null)
            {
                SetFavorite(model, true);
            }
        }

        private bool CanAddToFavorite(object row)
        {
            ITankStatisticRow model = row as ITankStatisticRow;
            return model != null && !model.IsFavorite;
        }

        private void OnRowDoubleClick(object rowData)
        {
            ITankStatisticRow tankStatisticRowViewModel = rowData as ITankStatisticRow;

            //NRE if row of type TotalTankStatisticRowViewModel
            if (tankStatisticRowViewModel != null && !(tankStatisticRowViewModel is TotalTankStatisticRowViewModel))
            {
                TankStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<TankStatisticViewModel>();
                if (viewModel != null)
                {
                    viewModel.TankStatistic = tankStatisticRowViewModel;
                    AppSettings appSettings = SettingsReader.Get();

                    ITankStatisticRow temp = tankStatisticRowViewModel.GetPreviousStatistic();

                    // configure LastNBattles stat for tank
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        int battles = tankStatisticRowViewModel.BattlesCount - appSettings.PeriodSettings.LastNBattles;
                        ITankStatisticRow model = tankStatisticRowViewModel.GetAll().OrderBy(x => x.BattlesCount).FirstOrDefault(x => x.BattlesCount >= battles);
                        tankStatisticRowViewModel.SetPreviousStatistic(model);
                    }

                    viewModel.Show();

                    //restore settings 
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        tankStatisticRowViewModel.SetPreviousStatistic(temp);
                    }
                }
            }
        }

        private void OnSettings()
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<SettingsViewModel>();
            if (viewModel != null)
            {
                viewModel.Show();
            }
        }

        private void OnAbout()
        {
            AboutViewModel viewModel = CompositionContainerFactory.Instance.GetExport<AboutViewModel>();
            if (viewModel != null)
            {
                viewModel.Show();
            }
        }

        private List<DateTime> GetPreviousDates(PlayerStatisticViewModel playerStatisticViewModel)
        {
            return playerStatisticViewModel.GetAllSlices().Select(x => x.Updated).OrderByDescending(x => x).Skip(1).ToList();
        }

        #endregion

        #region load

        private void OnLoad()
        {
            AppSettings settings = SettingsReader.Get();

            if (settings == null || string.IsNullOrEmpty(settings.PlayerName) || string.IsNullOrEmpty(settings.Server))
            {
                MessageBox.Show(Resources.Resources.WarningMsg_SpecifyPlayerName, Resources.Resources.WindowCaption_Warning,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProgressView.Execute(Resources.Resources.ProgressTitle_Loading_replays,
                (bw, we) =>
                {
                    try
                    {
                        LoadInProgress = true;

                        //set thread culture
                        SetUiCulture();

                        ServerStatWrapper serverStatistic = LoadPlayerServerStatistic(settings);
                        
                        if (!string.IsNullOrEmpty(settings.PlayerName) && !string.IsNullOrEmpty(settings.Server))
                        {
                            FileInfo cacheFile = CacheHelper.GetCacheFile(settings.PlayerName, settings.Server);

                            if (cacheFile != null)
                            {
                                //convert dossier cache file to json
                                string jsonFile = CacheHelper.BinaryCacheToJson(cacheFile);
                                List<TankJson> tanks = WotApiClient.Instance.ReadTanksCache(jsonFile);

                                //get tanks from dossier app spot
                                //string data = new Uri("http://wot-dossier.appspot.com/dossier-data/2587067").Get();
                                //List<TankJson> tanksV2 = WotApiClient.Instance.ReadDossierAppSpotTanks(data);

                                InitPlayerStatistic(serverStatistic, tanks);

                                ProgressView.Report(bw, 25, string.Empty);

                                InitTanksStatistic(tanks);

                                //trick for set "N last battles period"
                                if (settings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                                {
                                    PeriodSelectorOnPropertyChanged();
                                }

                                ProgressView.Report(bw, 50, string.Empty);

                                InitChart();

                                InitLastUsedTankList();

                                ProgressView.Report(bw, 100, string.Empty);
                            }
                            else
                            {
                                MessageBox.Show(Resources.Resources.WarningMsg_CanntFindPlayerDataInDossierCache,
                                    Resources.Resources.WindowCaption_Warning,
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error("Error on data load", e);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnDataLoad, Resources.Resources.WindowCaption_Error,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        LoadInProgress = false;
                    }
                });

            ReplaysViewModel.LoadReplaysList();
        }

        private void InitLastUsedTankList()
        {
            _log.Trace("InitLastUsedTankList start");

            if (PlayerStatistic != null)
            {
                RaisePropertyChanged(PropLastUsedTanksList);
                var lastUsedTanksList = LastUsedTanksList;
                PlayerStatistic.WN8RatingForPeriod = RatingHelper.Wn8ForPeriod(lastUsedTanksList);
                PlayerStatistic.PerformanceRatingForPeriod = RatingHelper.PerformanceRatingForPeriod(lastUsedTanksList);
            }

            _log.Trace("InitLastUsedTankList end");
        }

        private void InitPlayerStatistic(ServerStatWrapper serverStatistic, List<TankJson> tanks)
        {
            _log.Trace("ShellViewModel.InitPlayerStatistic start");
            PlayerStatistic = InitPlayerStatisticViewModel(serverStatistic, tanks);
            
            //init previous dates list
            PeriodSelector.PeriodSettingsUpdated -= PeriodSelectorOnPropertyChanged;
            PeriodSelector.PrevDates = GetPreviousDates(PlayerStatistic);
            PeriodSelector.PeriodSettingsUpdated += PeriodSelectorOnPropertyChanged;
            _log.Trace("ShellViewModel.InitPlayerStatistic end");
        }

        private void InitTanksStatistic(List<TankJson> tanks)
        {
            _log.Trace("ShellViewModel.InitTanksStatistic start");
            AppSettings settings = SettingsReader.Get();

            StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(BattleModeSelector.BattleMode, _dossierRepository);

            PlayerEntity playerEntity = strategy.UpdateTankStatistic(settings.PlayerId, tanks);

            if (playerEntity == null)
            {
                MessageBox.Show(string.Format(Resources.Resources.Msg_ErrorOnGetLocalPlayerInfo, settings.PlayerName), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Tanks = strategy.GetTanksStatistic(playerEntity.Id);

            MasterTanker = strategy.GetMasterTankerList(_tanks);

            FraggsCount.Init(_tanks);
            _log.Trace("ShellViewModel.InitTanksStatistic end");
        }

        private ServerStatWrapper LoadPlayerServerStatistic(AppSettings settings)
        {
            _log.Trace("ShellViewModel.LoadPlayerServerStatistic start");
            Player player = null;
            try
            {
                int playerId = settings.PlayerId;
                if (!string.IsNullOrEmpty(settings.PlayerName))
                {
                    player = WotApiClient.Instance.LoadPlayerStat(playerId, false, settings);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            _log.Trace("ShellViewModel.LoadPlayerServerStatistic end");
            return new ServerStatWrapper(player);
        }

        #endregion

        #region Initialize

        private PlayerStatisticViewModel InitPlayerStatisticViewModel(ServerStatWrapper serverStatistic, List<TankJson> tanks)
        {
            AppSettings settings = SettingsReader.Get();

            StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(BattleModeSelector.BattleMode, _dossierRepository);

            int playerId = settings.PlayerId;

            PlayerEntity player = strategy.UpdatePlayerStatistic(playerId, tanks, serverStatistic.Ratings);
            
            return strategy.GetPlayerStatistic(player, tanks, serverStatistic);
        }

        #endregion

        #region [ Charts initialization ]

        private void InitChart()
        {
            if (PlayerStatistic != null)
            {
                ChartView.InitCharts(PlayerStatistic, _tanks);
            }
        }

        
        #endregion

        private void SetFavorite(ITankStatisticRow model, bool favorite)
        {
            AppSettings settings = SettingsReader.Get();
            model.IsFavorite = favorite;
            _dossierRepository.SetFavorite(model.TankId, model.CountryId, settings.PlayerId, favorite);
        }

        private void OnStatisticPeriodChanged(StatisticPeriodChangedEvent args)
        {
            ChartView.InitLastUsedTanksChart(PlayerStatistic, _tanks);
            InitLastUsedTankList();
            
            SetPeriodTabHeader();
        }

        private void SetPeriodTabHeader()
        {
            AppSettings appSettings = SettingsReader.Get();
            PeriodTabHeader = Resources.Resources.ResourceManager.GetFormatedEnumResource(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.Period == StatisticPeriod.Custom ? (object)appSettings.PeriodSettings.PrevDate : appSettings.PeriodSettings.LastNBattles);
        }

        private void PeriodSelectorOnPropertyChanged()
        {
            AppSettings settings = SettingsReader.Get();

            if (settings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
            {
                //convert LastNBattles period -> Custom
                int battles = PlayerStatistic.BattlesCount - settings.PeriodSettings.LastNBattles;

                StatisticViewModelBase viewModel = PlayerStatistic.GetAllSlices().OrderBy(x => x.BattlesCount).FirstOrDefault(x => x.BattlesCount >= battles);
                if (viewModel != null)
                {
                    EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>()
                        .Publish(new StatisticPeriodChangedEvent(StatisticPeriod.Custom, viewModel.Updated, 0));
                }
            }
            else
            {
                EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(settings.PeriodSettings.Period,
                    settings.PeriodSettings.PrevDate, settings.PeriodSettings.LastNBattles));   
            }
        }

        private static void SetUiCulture()
        {
            var culture = new CultureInfo(SettingsReader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void TankFilterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaisePropertyChanged(PropTanks);
        }

        private void ViewClosing(CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                e.Cancel = !IsCloseAllowed();
            }
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public virtual void Show()
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ViewTyped.Show();

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
            {
                UpdateChecker.CheckForUpdates();
            }, null);
        }

        private void OnShellViewActivated(object sender, EventArgs eventArgs)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            OnLoad();
        }

        private void ViewTypedOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            TankFilter.Save();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool close = false;
            if (IsCloseAllowed())
            {
                ViewTyped.Close();
                close = true;
            }
            return close;
        }

        private bool IsCloseAllowed()
        {
            return true;
        }
    }
}