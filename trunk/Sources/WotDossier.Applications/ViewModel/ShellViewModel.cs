using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.Events;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Logic.Export;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Common.Logging;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel<IShellView>
    {
        #region [ Properties and Fields ]

        private static readonly ILog _log = LogManager.GetLogger("ShellViewModel");

        private readonly DossierRepository _dossierRepository;
        private PlayerStatisticViewModel _playerStatistic;
        private PlayerStatisticViewModel _sessionStatistic;
        private List<TankRowMasterTanker> _masterTanker;
        private List<TankStatisticRowViewModel> _tanks = new List<TankStatisticRowViewModel>();
        private FraggsCountViewModel _fraggsCount = new FraggsCountViewModel();

        //private ObservableCollection<ReplayFile> _replays;
        private PlayerStatisticViewModel _sessionStartStatistic;
        private ProgressControlViewModel _progressView;
        private List<ReplayFolder> _replaysFolders;
        private ReplaysManager _replaysManager;

        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged("PlayerStatistic");
            }
        }

        public PlayerStatisticViewModel SessionStatistic
        {
            get { return _sessionStatistic; }
            set
            {
                _sessionStatistic = value;
                RaisePropertyChanged("SessionStatistic");
            }
        }

        public List<TankRowMasterTanker> MasterTanker
        {
            get { return _masterTanker; }
            set
            {
                _masterTanker = value;
                RaisePropertyChanged("MasterTanker");
            }
        }

        public List<TankStatisticRowViewModel> Tanks
        {
            get { return TankFilter.Filter(_tanks); }
            set
            {
                _tanks = value;
                RaisePropertyChanged("Tanks");
            }
        }

        private List<ReplayFile> _replays = new List<ReplayFile>();
        public List<ReplayFile> Replays
        {
            get { return ReplayFilter.Filter(_replays); }
            set
            {
                _replays = value;
                RaisePropertyChanged("Replays");
            }
        }

        public FraggsCountViewModel FraggsCount
        {
            get { return _fraggsCount; }
            set { _fraggsCount = value; }
        }

        public ReplaysFilterViewModel ReplayFilter { get; set; }
        public TankFilterViewModel TankFilter { get; set; }

        public ProgressControlViewModel ProgressView
        {
            get { return _progressView; }
            set { _progressView = value; }
        }

        public List<ReplayFolder> ReplaysFolders
        {
            get { return _replaysFolders; }
            set
            {
                _replaysFolders = value;
                RaisePropertyChanged("ReplaysFolders");
            }
        }

        public ReplaysManager ReplaysManager
        {
            get { return _replaysManager; }
            set { _replaysManager = value; }
        }

        private static readonly string PropPeriodTabHeader = TypeHelper.GetPropertyName<ShellViewModel>(x => x.PeriodTabHeader);

        private string _periodTabHeader;
        private PeriodSelectorViewModel _periodSelector;

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

        public PeriodSelectorViewModel PeriodSelector
        {
            get { return _periodSelector; }
            set
            {
                _periodSelector = value;
                RaisePropertyChanged("PeriodSelector");
            }
        }

        public PlayerChartsViewModel ChartView { get; set; }

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

        public DelegateCommand<ReplayFile> ReplayUploadCommand { get; set; }
        public DelegateCommand<ReplayFile> ReplayDeleteCommand { get; set; }
        public DelegateCommand<ReplayFile> CopyLinkToClipboardCommand { get; set; }
        public DelegateCommand<ReplayFile> PlayReplayCommand { get; set; }
        public DelegateCommand<object> ReplayRowDoubleClickCommand { get; set; }
        public DelegateCommand<object> ReplayRowsDeleteCommand { get; set; }

        public DelegateCommand<ReplayFolder> AddFolderCommand { get; set; }
        public DelegateCommand<ReplayFolder> DeleteFolderCommand { get; set; }

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
        public ShellViewModel([Import(typeof(IShellView))]IShellView view, [Import]DossierRepository dossierRepository, [Import]ReplaysManager replaysManager)
            : this(view, false)
        {
            _dossierRepository = dossierRepository;
            _replaysManager = replaysManager;

            LoadCommand = new DelegateCommand(OnLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            
            ReplayRowDoubleClickCommand = new DelegateCommand<object>(OnReplayRowDoubleClick);
            ReplayUploadCommand = new DelegateCommand<ReplayFile>(OnUploadReplay, CanUploadReplay);
            ReplayDeleteCommand = new DelegateCommand<ReplayFile>(OnReplayRowDelete);
            ReplayRowsDeleteCommand = new DelegateCommand<object>(OnReplayRowsDelete);
            CopyLinkToClipboardCommand = new DelegateCommand<ReplayFile>(OnCopyLinkToClipboard, CanCopyLinkToClipboard);
            PlayReplayCommand = new DelegateCommand<ReplayFile>(OnPlayReplay);

            AddFolderCommand = new DelegateCommand<ReplayFolder>(OnAddFolder);
            DeleteFolderCommand = new DelegateCommand<ReplayFolder>(OnDeleteFolderCommand);

            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            AddToFavoriteCommand = new DelegateCommand<object>(OnAddToFavorite, CanAddToFavorite);
            RemoveFromFavoriteCommand = new DelegateCommand<object>(OnRemoveFromFavorite, CanRemoveFromFavorite);

            OpenClanCommand = new DelegateCommand<object>(OnOpenClanCommand);
            CompareCommand = new DelegateCommand(OnCompare);
            ExportToCsvCommand = new DelegateCommand(OnExportToCsv);
            SearchPlayersCommand = new DelegateCommand(OnSearchPlayers);
            SearchClansCommand = new DelegateCommand(OnSearchClans);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(s, e));

            ReplayFilter = new ReplaysFilterViewModel();
            ReplayFilter.PropertyChanged += ReplayFilterOnPropertyChanged;
            TankFilter = new TankFilterViewModel();
            TankFilter.PropertyChanged += TankFilterOnPropertyChanged;

            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            EventAggregatorFactory.EventAggregator.GetEvent<ReplayFileMoveEvent>().Subscribe(OnReplayFileMove);
            ProgressView = new ProgressControlViewModel();
            PeriodSelector = new PeriodSelectorViewModel();

            ChartView = new PlayerChartsViewModel();

            SetPeriodTabHeader();

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
            ClanSearchViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ClanSearchViewModel>().Value;
            viewModel.Show();
        }

        private void OnSearchPlayers()
        {
            PlayerSearchViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<PlayerSearchViewModel>().Value;
            viewModel.Show();
        }

        private void OnCompare()
        {
            PlayersCompareViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<PlayersCompareViewModel>().Value;
            viewModel.Show();
        }

        private void OnOpenClanCommand(object param)
        {
            Mouse.SetCursor(Cursors.Wait);
            ClanData clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), PlayerStatistic.Clan.Id);
            Mouse.SetCursor(Cursors.Arrow);
            if (clan != null)
            {
                ClanViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ClanViewModel>().Value;
                viewModel.Init(clan);
                viewModel.Show();
            }
            else
            {
                MessageBox.Show(Resources.Resources.Msg_CantGetClanDataFromServer, Resources.Resources.WindowCaption_Information,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnPlayReplay(ReplayFile replay)
        {
            if (replay != null && File.Exists(replay.FileInfo.FullName))
            {
                AppSettings appSettings = SettingsReader.Get();
                string path = appSettings.PathToWotExe;
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    VistaOpenFileDialog dialog = new VistaOpenFileDialog();
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;
                    dialog.DefaultExt = ".exe"; // Default file extension
                    dialog.Filter = "WorldOfTanks (.exe)|*.exe"; // Filter files by extension 
                    dialog.Multiselect = false;
                    dialog.Title = Resources.Resources.WindowCaption_SelectPathToWorldOfTanksExecutable;
                    bool? showDialog = dialog.ShowDialog();
                    if (showDialog == true)
                    {
                        path = appSettings.PathToWotExe = dialog.FileName;
                        SettingsReader.Save(appSettings);
                    }
                }

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    try
                    {
                        Process proc = new Process();
                        proc.EnableRaisingEvents = false;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.FileName = path;
                        proc.StartInfo.Arguments = string.Format("\"{0}\"", replay.FileInfo.FullName);
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on play replay ({0} {1})", e, path, replay.FileInfo.FullName);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnPlayReplay, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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

        private void OnCopyLinkToClipboard(ReplayFile model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Link))
            {
                Clipboard.SetText(model.Link);
            }
        }

        private bool CanCopyLinkToClipboard(ReplayFile model)
        {
            return  model != null && !string.IsNullOrEmpty(model.Link);
        }

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

        private void OnReplayRowDelete(ReplayFile replayFile)
        {
            if (replayFile != null)
            {
                if (replayFile.FileInfo.Exists)
                {
                    try
                    {
#if !DEBUG
                        replayFile.FileInfo.Delete();
#endif
                        _replays.Remove(replayFile);
                        RaisePropertyChanged("Replays");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.FileInfo.Name);
                        MessageBox.Show(string.Format(Resources.Resources.ErrorMsg_ErrorOnFileDeletion, replayFile.FileInfo.Name),
                            Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OnReplayRowsDelete(object rowData)
        {
            ObservableCollection<object> selectedItems = rowData as ObservableCollection<object> ?? new ObservableCollection<object>();
            IEnumerable<ReplayFile> replays = selectedItems.Cast<ReplayFile>().ToList();
            bool error = false;
            foreach (ReplayFile replayFile in replays)
            {
                try
                {
#if !DEBUG
                        replayFile.FileInfo.Delete();
#endif
                    _replays.Remove(replayFile);
                }
                catch (Exception e)
                {
                    _log.ErrorFormat("Error on file deletion - {0}", e, replayFile.FileInfo.Name);
                    error = true;
                }
            }

            RaisePropertyChanged("Replays");

            if (error)
            {
                MessageBox.Show(Resources.Resources.ErrorMsg_ErrorOnFilesDeletion,
                    Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnUploadReplay(ReplayFile replayFile)
        {
            if (replayFile != null)
            {
                UploadReplayViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<UploadReplayViewModel>().Value;
                viewModel.ReplayFile = replayFile;
                viewModel.Show();
            }
        }

        private bool CanUploadReplay(object row)
        {
            ReplayFile model = row as ReplayFile;
            return model != null && string.IsNullOrEmpty(model.Link);
        }

        private void OnRowDoubleClick(object rowData)
        {
            TankStatisticRowViewModel tankStatisticRowViewModel = rowData as TankStatisticRowViewModel;

            if (tankStatisticRowViewModel != null)
            {
                TankStatisticViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<TankStatisticViewModel>().Value;
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

        private void OnReplayRowDoubleClick(object rowData)
        {
            ReplayFile replayFile = rowData as ReplayFile;

            if (replayFile != null)
            {
                string jsonFile = replayFile.FileInfo.FullName.Replace(replayFile.FileInfo.Extension, ".json");
                //convert dossier cache file to json
                if (!File.Exists(jsonFile))
                {
                    CacheHelper.ReplayToJson(replayFile.FileInfo);
                }

                if (!File.Exists(jsonFile))
                {
                    MessageBox.Show(Resources.Resources.Msg_Error_on_replay_file_read, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Domain.Replay.Replay replay = WotApiClient.Instance.ReadReplay(jsonFile);
                if (replay != null && replay.datablock_battle_result != null && replay.CommandResult != null)
                {
                    ReplayViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ReplayViewModel>().Value;
                    viewModel.Init(replay, replayFile);
                    viewModel.Show();
                }
                else
                {
                    MessageBox.Show(Resources.Resources.Msg_File_incomplete_or_not_supported, Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnSettings()
        {
            SettingsViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<SettingsViewModel>().Value;
            viewModel.Show();
        }

        private List<DateTime> GetPreviousDates(PlayerStatisticViewModel playerStatisticViewModel)
        {
            return playerStatisticViewModel.GetAll().Select(x => x.Updated).OrderByDescending(x => x).Skip(1).ToList();
        }

        private void OnDeleteFolderCommand(ReplayFolder folder)
        {
            ReplayFolder root = ReplaysFolders.FirstOrDefault();
            ReplayFolder parent = FindParentFolder(root, folder);
            if (parent != null)
            {
                parent.Folders.Remove(folder);
                ReplaysManager.SaveFolder(root);
            }
        }

        private void OnAddFolder(ReplayFolder folder)
        {
            AddReplayFolderViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<AddReplayFolderViewModel>().Value;
            viewModel.Show();

            if (viewModel.ReplayFolder != null)
            {
                ReplayFolder root = ReplaysFolders.FirstOrDefault();
                folder.Folders.Add(viewModel.ReplayFolder);
                ReplaysManager.SaveFolder(root);
                ProcessReplaysFolders(new List<ReplayFolder> { viewModel.ReplayFolder });
            }
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

            ProgressView.Execute((Window)ViewTyped, Resources.Resources.ProgressTitle_Loading_replays,
                (bw, we) =>
                {
                    try
                    {
                        //set thread culture
                        SetUICulture();

                        ServerStatWrapper serverStatistic = LoadPlayerServerStatistic(settings);

                        if (!string.IsNullOrEmpty(settings.PlayerName) && !string.IsNullOrEmpty(settings.Server))
                        {
                            FileInfo cacheFile = CacheHelper.GetCacheFile(settings.PlayerName);

                            List<TankJson> tanksV2;

                            if (cacheFile != null)
                            {
                                //convert dossier cache file to json
                                CacheHelper.BinaryCacheToJson(cacheFile);

                                tanksV2 = WotApiClient.Instance.ReadTanksV2(cacheFile.FullName.Replace(".dat", ".json"));

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

                    LoadReplaysList();
                });
        }

        private void PeriodSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            AppSettings settings = SettingsReader.Get();
            EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Publish(new StatisticPeriodChangedEvent(settings.PeriodSettings.Period,
                    settings.PeriodSettings.PrevDate, settings.PeriodSettings.LastNBattles));
        }

        private static void SetUICulture()
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

        private void LoadReplaysList()
        {
            ReplaysFolders = ReplaysManager.GetFolders();
            _replays.Clear();

            List<ReplayFolder> replayFolders = ReplaysFolders.GetAll();

            ProcessReplaysFolders(replayFolders);
        }

        private void ProcessReplaysFolders(List<ReplayFolder> replayFolders)
        {
            ProgressDialogResult result = ProgressView.Execute((Window) ViewTyped,
                Resources.Resources.ProgressTitle_Loading_replays, (bw, we) =>
                {
                    List<ReplayFile> replayFiles = new List<ReplayFile>();

                    foreach (var replayFolder in replayFolders)
                    {
                        string folderPath = replayFolder.Path;

                        if (string.IsNullOrEmpty(folderPath))
                        {
                            continue;
                        }

                        if (Directory.Exists(folderPath))
                        {
                            string[] files = Directory.GetFiles(folderPath, "*.wotreplay");
                            List<FileInfo> replays =
                                files.Select(x => new FileInfo(Path.Combine(folderPath, x))).Where(x => x.Length > 0).ToList();

                            int count = replays.Count();

                            int index = 0;
                            foreach (FileInfo replay in replays)
                            {
                                Domain.Replay.Replay data = WotApiClient.Instance.ReadReplay2Blocks(replay);
                                if (data != null)
                                {
                                    replayFiles.Add(new ReplayFile(replay, data, replayFolder.Id));
                                }
                                index++;
                                int percent = (index + 1)*100/count;
                                if (ProgressView.ReportWithCancellationCheck(bw, we, percent,
                                    Resources.Resources.ProgressLabel_Processing_file_format, index + 1, count, replay.Name))
                                {
                                    return;
                                }
                            }

                            // So this check in order to avoid default processing after the Cancel button has been pressed.
                            // This call will set the Cancelled flag on the result structure.
                            ProgressView.CheckForPendingCancellation(bw, we);
                        }
                    }

                    IList<ReplayEntity> dbReplays = _dossierRepository.GetReplays();

                    dbReplays.Join(replayFiles, x => new { x.PlayerId, x.ReplayId }, y => new { y.PlayerId, y.ReplayId },
                        (x, y) => new { ReplayEntity = x, ReplayFile = y })
                        .ToList()
                        .ForEach(x => x.ReplayFile.Link = x.ReplayEntity.Link);

                    _replays.AddRange(replayFiles.OrderByDescending(x => x.PlayTime).ToList());
                    RaisePropertyChanged("Replays");

                    ReplayFilter.SelectedFolder = replayFolders.FirstOrDefault();

                    ChartView.InitBattlesByMapChart(_replays);
                    ChartView.InitWinReplaysPercentByMapChart(_replays);

                }, new ProgressDialogSettings(true, true, false));
        }

        private PlayerStatisticViewModel InitPlayerStatisticViewModel(ServerStatWrapper serverStatistic, List<TankJson> tanks)
        {
            AppSettings settings = SettingsReader.Get();

            PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(serverStatistic.Ratings, tanks, settings.PlayerId);

            List<PlayerStatisticEntity> statisticEntities = _dossierRepository.GetPlayerStatistic(player.PlayerId).ToList();
            return StatisticViewModelFactory.Create(statisticEntities, tanks, player.Name, player.Creaded, serverStatistic);
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

            Tanks = StatisticViewModelFactory.Create(entities);

            InitMasterTankerList(_tanks);

            FraggsCount.Init(_tanks);
        }

        private void InitMasterTankerList(List<TankStatisticRowViewModel> tanks)
        {
            IEnumerable<int> killed =
                tanks.SelectMany(x => x.TankFrags).Select(x => x.TankUniqueId).Distinct().OrderBy(x => x);
            List<TankRowMasterTanker> masterTanker = WotApiClient.Instance.TanksDictionary
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
                RaisePropertyChanged("LastUsedTanksList");
            }
        }

        
        #endregion

        private bool IsExistedtank(TankDescription tankDescription)
        {
            //TODO: refactoring
            return tankDescription.TankId <= 250 && !tankDescription.Icon.Icon.Contains("training") && tankDescription.Title != "KV" && tankDescription.Title != "T23";
        }

        private ReplayFolder FindParentFolder(ReplayFolder parent, ReplayFolder folder)
        {
            if (parent.Folders.Contains(folder))
            {
                return parent;
            }
            return parent.Folders.Select(child => FindParentFolder(child, folder)).FirstOrDefault(foundItem => foundItem != null);
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
                RaisePropertyChanged("LastUsedTanksList");
            }

            SetPeriodTabHeader();
        }

        private void SetPeriodTabHeader()
        {
            AppSettings appSettings = SettingsReader.Get();
            PeriodTabHeader = Resources.Resources.ResourceManager.GetFormatedEnumResource(appSettings.PeriodSettings.Period, appSettings.PeriodSettings.Period == StatisticPeriod.Custom ? (object)appSettings.PeriodSettings.PrevDate : appSettings.PeriodSettings.LastNBattles);
        }

        private void OnReplayFileMove(ReplayFileMoveEventArgs eventArgs)
        {
            if (ReplayFilter.SelectedFolder != eventArgs.TargetFolder)
            {
                ReplaysManager.Move(eventArgs.ReplayFile, eventArgs.TargetFolder);
                eventArgs.ReplayFile.FolderId = eventArgs.TargetFolder.Id;
                RaisePropertyChanged("Replays");
            }
        }

        private void TankFilterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaisePropertyChanged("Tanks");
        }

        private void ReplayFilterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RaisePropertyChanged("Replays");
        }

        private void ViewClosing(object sender, CancelEventArgs e)
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