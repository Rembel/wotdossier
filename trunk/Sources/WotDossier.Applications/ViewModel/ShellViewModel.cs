﻿using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
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
using WotDossier.Domain.Player;
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
        public static readonly string PropSessionStatistic = TypeHelper<ShellViewModel>.PropertyName(v => v.SessionStatistic);
        public static readonly string PropMasterTanker = TypeHelper<ShellViewModel>.PropertyName(v => v.MasterTanker);
        public static readonly string PropTanks = TypeHelper<ShellViewModel>.PropertyName(v => v.Tanks);
        public static readonly string PropPeriodSelector = TypeHelper<ShellViewModel>.PropertyName(v => v.PeriodSelector);
        public static readonly string PropLastUsedTanksList = TypeHelper<ShellViewModel>.PropertyName(v => v.LastUsedTanksList);

        #region [ Properties and Fields ]

        private readonly DossierRepository _dossierRepository;
        private PlayerStatisticViewModel _playerStatistic;
        private PlayerStatisticViewModel _sessionStatistic;
        private List<TankRowMasterTanker> _masterTanker;
        private List<TankStatisticRowViewModel> _tanks = new List<TankStatisticRowViewModel>();
        private FraggsCountViewModel _fraggsCount = new FraggsCountViewModel();

        private PlayerStatisticViewModel _sessionStartStatistic;

        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged(PropPlayerStatistic);
            }
        }

        public PlayerStatisticViewModel SessionStatistic
        {
            get { return _sessionStatistic; }
            set
            {
                _sessionStatistic = value;
                RaisePropertyChanged(PropSessionStatistic);
            }
        }

        public List<TankRowMasterTanker> MasterTanker
        {
            get { return _masterTanker; }
            set
            {
                _masterTanker = value;
                RaisePropertyChanged(PropMasterTanker);
            }
        }

        public List<TankStatisticRowViewModel> Tanks
        {
            get
            {
                List<TankStatisticRowViewModel> tankStatisticRowViewModels = TankFilter.Filter(_tanks);
                if (tankStatisticRowViewModels.Count > 0)
                {
                    TotalTankStatisticRowViewModel totalRow =
                        new TotalTankStatisticRowViewModel(tankStatisticRowViewModels.ToList());
                    tankStatisticRowViewModels.Insert(0, totalRow);
                }
                FooterList<TankStatisticRowViewModel> statisticRowViewModels = new FooterList<TankStatisticRowViewModel>();
                statisticRowViewModels.AddRange(tankStatisticRowViewModels);
                return statisticRowViewModels;
            }
            set
            {
                _tanks = value;
                RaisePropertyChanged(PropTanks);
            }
        }

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

        public PlayerChartsViewModel ChartView { get; set; }

        public ReplaysViewModel ReplaysViewModel { get; set; }

        public List<TankStatisticRowViewModel> LastUsedTanksList
        {
            get
            {
                List<TankStatisticRowViewModel> list = _tanks.Where(x => x.LastBattle > PlayerStatistic.PreviousDate).ToList();
                return list;
            }
        }

        #endregion

        #region Commands

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand SettingsCommand { get; set; }

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }
        public DelegateCommand<object> AddToFavoriteCommand { get; set; }
        public DelegateCommand<object> RemoveFromFavoriteCommand { get; set; }

        public DelegateCommand<object> OpenClanCommand { get; set; }
        public DelegateCommand CompareCommand { get; set; }
        public DelegateCommand ExportToCsvCommand { get; set; }
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
            : this(view, false)
        {
            _dossierRepository = dossierRepository;

            LoadCommand = new DelegateCommand(OnLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            AddToFavoriteCommand = new DelegateCommand<object>(OnAddToFavorite, CanAddToFavorite);
            RemoveFromFavoriteCommand = new DelegateCommand<object>(OnRemoveFromFavorite, CanRemoveFromFavorite);

            OpenClanCommand = new DelegateCommand<object>(OnOpenClanCommand);
            CompareCommand = new DelegateCommand(OnCompare);
            ExportToCsvCommand = new DelegateCommand(OnExportToCsv);
            SearchPlayersCommand = new DelegateCommand(OnSearchPlayers);
            SearchClansCommand = new DelegateCommand(OnSearchClans);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(e));

            TankFilter = new TankFilterViewModel();
            TankFilter.PropertyChanged += TankFilterOnPropertyChanged;

            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            ProgressView = new ProgressControlViewModel();
            PeriodSelector = new PeriodSelectorViewModel();

            ChartView = new PlayerChartsViewModel();

            SetPeriodTabHeader();

            ReplaysViewModel = new ReplaysViewModel(_dossierRepository, ProgressView, ChartView);

            ViewTyped.Closing += ViewTypedOnClosing;
        }

        private void OnExportToCsv()
        {
            CsvExportProvider provider = new CsvExportProvider();
            provider.Export(_tanks, new List<Type>
            {
                typeof(ITankRowBase),
                typeof(ITankRowXP),
                typeof(ITankRowBattles),
                typeof(ITankRowFrags),
                typeof(ITankRowDamage),
                typeof(ITankRowBattleAwards),
                typeof(ITankRowSpecialAwards),
                typeof(ITankRowSeries),
                typeof(ITankRowMedals),
                typeof(ITankRowEpic),
                typeof(ITankRowTime),
                typeof(ITankRowPerformance)
            });
        }

        private void OnSearchClans()
        {
            var export = CompositionContainerFactory.Instance.Container.GetExport<ClanSearchViewModel>();
            if (export != null)
            {
                ClanSearchViewModel viewModel = export.Value;
                viewModel.Show();
            }
        }

        private void OnSearchPlayers()
        {
            var export = CompositionContainerFactory.Instance.Container.GetExport<PlayerSearchViewModel>();
            if (export != null)
            {
                PlayerSearchViewModel viewModel = export.Value;
                viewModel.Show();
            }
        }

        private void OnCompare()
        {
            var export = CompositionContainerFactory.Instance.Container.GetExport<PlayersCompareViewModel>();
            if (export != null)
            {
                PlayersCompareViewModel viewModel = export.Value;
                viewModel.Show();
            }
        }

        private void OnOpenClanCommand(object param)
        {
            Mouse.SetCursor(Cursors.Wait);
            ClanData clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), PlayerStatistic.Clan.Id);
            Mouse.SetCursor(Cursors.Arrow);
            if (clan != null)
            {
                var export = CompositionContainerFactory.Instance.Container.GetExport<ClanViewModel>();
                if (export != null)
                {
                    ClanViewModel viewModel = export.Value;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="isChild">if set to <c>true</c> [is child].</param>
        public ShellViewModel(IShellView view, bool isChild)
            : base(view, isChild)
        {
        }

        #endregion

        #region Handlers

        private void OnRemoveFromFavorite(object row)
        {
            TankStatisticRowViewModel model = row as TankStatisticRowViewModel;
            if (model != null)
            {
                SetFavorite(model, false);
            }
        }

        private bool CanRemoveFromFavorite(object row)
        {
            TankStatisticRowViewModel model = row as TankStatisticRowViewModel;
            return model != null && model.IsFavorite;
        }

        private void OnAddToFavorite(object data)
        {
            TankStatisticRowViewModel model = data as TankStatisticRowViewModel;
            if (model != null)
            {
                SetFavorite(model, true);
            }
        }

        private bool CanAddToFavorite(object row)
        {
            TankStatisticRowViewModel model = row as TankStatisticRowViewModel;
            return model != null && !model.IsFavorite;
        }

        private void OnRowDoubleClick(object rowData)
        {
            TankStatisticRowViewModel tankStatisticRowViewModel = rowData as TankStatisticRowViewModel;

            if (tankStatisticRowViewModel != null)
            {
                var export = CompositionContainerFactory.Instance.Container.GetExport<TankStatisticViewModel>();
                if (export != null)
                {
                    TankStatisticViewModel viewModel = export.Value;
                    viewModel.TankStatistic = tankStatisticRowViewModel;
                    AppSettings appSettings = SettingsReader.Get();
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        int battles = tankStatisticRowViewModel.BattlesCount - appSettings.PeriodSettings.LastNBattles;

                        TankStatisticRowViewModel model = tankStatisticRowViewModel.GetAll().OrderBy(x => x.BattlesCount).FirstOrDefault(x => x.BattlesCount >= battles);
                        tankStatisticRowViewModel.SetPreviousStatistic(model);
                    }

                    viewModel.Show();

                    //restore settings
                    if (appSettings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                    {
                        EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>()
                            .Publish(new StatisticPeriodChangedEvent(StatisticPeriod.LastNBattles, appSettings.PeriodSettings.PrevDate, appSettings.PeriodSettings.LastNBattles));
                    }
                }
            }
        }

        private void OnSettings()
        {
            var export = CompositionContainerFactory.Instance.Container.GetExport<SettingsViewModel>();
            if (export != null)
            {
                SettingsViewModel viewModel = export.Value;
                viewModel.Show();
            }
        }

        private List<DateTime> GetPreviousDates(PlayerStatisticViewModel playerStatisticViewModel)
        {
            return playerStatisticViewModel.GetAll().Select(x => x.Updated).OrderByDescending(x => x).Skip(1).ToList();
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
                        //set thread culture
                        SetUiCulture();

                        ServerStatWrapper serverStatistic = LoadPlayerServerStatistic(settings);

                        if (!string.IsNullOrEmpty(settings.PlayerName) && !string.IsNullOrEmpty(settings.Server))
                        {
                            FileInfo cacheFile = CacheHelper.GetCacheFile(settings.PlayerName, settings.Server);

                            if (cacheFile != null)
                            {
                                //convert dossier cache file to json
                                CacheHelper.BinaryCacheToJson(cacheFile);
                                List<TankJson> tanksV2 = WotApiClient.Instance.ReadTanksV2(cacheFile.FullName.Replace(".dat", ".json"));

                                //get tanks from dossier app spot
                                //string data = new Uri("http://wot-dossier.appspot.com/dossier-data/2587067").Get();
                                //List<TankJson> tanksV2 = WotApiClient.Instance.ReadDossierAppSpotTanks(data);

                                PlayerStatistic = InitPlayerStatisticViewModel(serverStatistic, tanksV2);

                                //init previous dates list
                                PeriodSelector.PropertyChanged -= PeriodSelectorOnPropertyChanged;
                                PeriodSelector.PrevDates = GetPreviousDates(PlayerStatistic);
                                PeriodSelector.PropertyChanged += PeriodSelectorOnPropertyChanged;

                                ProgressView.Report(bw, 25, string.Empty);

                                PlayerStatisticViewModel clone = PlayerStatistic.Clone();

                                if (_sessionStartStatistic == null)
                                {
                                    //save start session statistic
                                    _sessionStartStatistic = clone;
                                }
                                else
                                {
                                    clone.SetPreviousStatistic(_sessionStartStatistic);
                                }

                                SessionStatistic = clone;

                                InitTanksStatistic(tanksV2);

                                //trick for set "N last battles period"
                                if (settings.PeriodSettings.Period == StatisticPeriod.LastNBattles)
                                {
                                    PeriodSelectorOnPropertyChanged(null, null);
                                }

                                ProgressView.Report(bw, 50, string.Empty);

                                InitChart();
                                ProgressView.Report(bw, 100, string.Empty);
                            }
                            else
                            {
                                MessageBox.Show(Resources.Resources.WarningMsg_CanntFindPlayerDataInDossierCache, Resources.Resources.WindowCaption_Warning,
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error("Error on data load", e);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnDataLoad, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    ReplaysViewModel.LoadReplaysList();
                });
        }

        private void PeriodSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            AppSettings settings = SettingsReader.Get();
            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(settings.PeriodSettings.Period,
                    settings.PeriodSettings.PrevDate, settings.PeriodSettings.LastNBattles));
        }

        private static void SetUiCulture()
        {
            var culture = new CultureInfo(SettingsReader.Get().Language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private ServerStatWrapper LoadPlayerServerStatistic(AppSettings settings)
        {
            PlayerStat playerStat = null;
            try
            {
                int playerId = settings.PlayerId;
                if (!string.IsNullOrEmpty(settings.PlayerName))
                {
                    playerStat = WotApiClient.Instance.LoadPlayerStat(settings, playerId, false);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return new ServerStatWrapper(playerStat);
        }

        #endregion

        #region Initialize

        private PlayerStatisticViewModel InitPlayerStatisticViewModel(ServerStatWrapper serverStatistic, List<TankJson> tanks)
        {
            AppSettings settings = SettingsReader.Get();

            PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(serverStatistic.Ratings, tanks, settings.PlayerId);

            List<PlayerStatisticEntity> statisticEntities = _dossierRepository.GetPlayerStatistic(player.PlayerId).ToList();
            return StatisticViewModelFactory.Create(statisticEntities, tanks, player, serverStatistic);
        }

        private void InitTanksStatistic(List<TankJson> tanks)
        {
            AppSettings settings = SettingsReader.Get();
            PlayerEntity playerEntity = _dossierRepository.UpdateTankStatistic(settings.PlayerId, tanks);

            if (playerEntity == null)
            {
                MessageBox.Show(Resources.Resources.ErrorMsg_GetPlayerInfo, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IEnumerable<TankStatisticEntity> entities = _dossierRepository.GetTanksStatistic(playerEntity.Id);

            List<TankStatisticRowViewModel> tankStatisticRowViewModels = StatisticViewModelFactory.Create(entities);
            Tanks = tankStatisticRowViewModels;

            InitMasterTankerList(_tanks);

            FraggsCount.Init(_tanks);
        }

        private void InitMasterTankerList(IEnumerable<TankStatisticRowViewModel> tanks)
        {
            IEnumerable<int> killed =
                tanks.SelectMany(x => x.TankFrags).Select(x => x.TankUniqueId).Distinct().OrderBy(x => x);
            List<TankRowMasterTanker> masterTanker = Dictionaries.Instance.Tanks
                                                                 .Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                                                                 .Select(x => new TankRowMasterTanker(x.Value))
                                                                 .OrderBy(x => x.IsPremium)
                                                                 .ThenBy(x => x.Tier).ToList();
            MasterTanker = masterTanker;
        }

        #endregion

        #region [ Charts initialization ]

        private void InitChart()
        {
            if (PlayerStatistic != null)
            {
                ChartView.InitCharts(PlayerStatistic, _tanks);
                RaisePropertyChanged(PropLastUsedTanksList);
            }
        }

        
        #endregion

        private bool IsExistedtank(TankDescription tankDescription)
        {
            return !NotExistsedTanksList.Contains(tankDescription.UniqueId());
        }

        public List<int> NotExistsedTanksList
        {
            get
            {
                return new List<int>
                {
                    30251,//T-34-1 training
                    255,//Spectator
                    10226,//pziii_training
                    10227,//pzvib_tiger_ii_training
                    10228,//pzv_training
                    220,//t_34_85_training
                    20212,//m4a3e8_sherman_training
                    5,//KV
                    20009,//T23
                    222,//t44_122
                    223,//t44_85
                    210,//su_85i
                    30002,//Type 59 G
                    20211,//sexton_i
                    30003,//WZ-111
                };
            }
        }

        private void SetFavorite(TankStatisticRowViewModel model, bool favorite)
        {
            AppSettings settings = SettingsReader.Get();
            model.IsFavorite = favorite;
            _dossierRepository.SetFavorite(model.TankId, model.CountryId, settings.PlayerId, favorite);
        }

        private void OnStatisticPeriodChanged(StatisticPeriodChangedEvent args)
        {
            if (args.StatisticPeriod == StatisticPeriod.LastNBattles)
            {
                int battles = PlayerStatistic.BattlesCount - args.LastNBattles;

                PlayerStatisticViewModel viewModel = PlayerStatistic.GetAll().OrderBy(x => x.BattlesCount).FirstOrDefault(x => x.BattlesCount >= battles);
                if (viewModel != null)
                {
                    EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>()
                        .Publish(new StatisticPeriodChangedEvent(StatisticPeriod.Custom, viewModel.Updated, 0));
                }
            }
            else
            {
                ChartView.InitLastUsedTanksChart(PlayerStatistic, _tanks);
                RaisePropertyChanged(PropLastUsedTanksList);
            }

            SetPeriodTabHeader();
        }

        private void SetPeriodTabHeader()
        {
            AppSettings appSettings = SettingsReader.Get();
            PeriodTabHeader = Resources.Resources.ResourceManager.GetFormatedEnumResource(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.Period == StatisticPeriod.Custom ? (object)appSettings.PeriodSettings.PrevDate : appSettings.PeriodSettings.LastNBattles);
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

        public virtual void Show()
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ViewTyped.Show();
            UpdateChecker.CheckForUpdates();
        }

        private void OnShellViewActivated(object sender, EventArgs eventArgs)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            ((Action)OnLoad)();
        }

        private void ViewTypedOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            TankFilter.Save();
        }

        public bool Close()
        {
            bool close = false;
            if (IsCloseAllowed())
            {
                CloseView();
                close = true;
            }
            return close;
        }

        private bool IsCloseAllowed()
        {
            return true;
        }

        public virtual void CloseView()
        {
            ViewTyped.Close();
        }

    }
}