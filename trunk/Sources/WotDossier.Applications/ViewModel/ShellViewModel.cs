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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.Events;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Update;
using WotDossier.Applications.View;
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

        #endregion

        #region Charts data

        private List<SellInfo> _lastUsedTanksDataSource;

        private EnumerableDataSource<DataPoint> _ratingDataSource;
        private EnumerableDataSource<DataPoint> _wn6RatingDataSource;
        private EnumerableDataSource<DataPoint> _winPercentDataSource;
        private EnumerableDataSource<DataPoint> _avgDamageDataSource;
        private EnumerableDataSource<DataPoint> _avgXpDataSource;
        private EnumerableDataSource<DataPoint> _killDeathRatioDataSource;
        private EnumerableDataSource<DataPoint> _survivePercentDataSource;
        private List<DataPoint> _efficiencyByTierDataSource;
        private List<GenericPoint<string, double>> _efficiencyByTypeDataSource;

        private EnumerableDataSource<DataPoint> _avgSpottedDataSource;
        private List<GenericPoint<string, double>> _efficiencyByCountryDataSource;
        private List<DataPoint> _replaysByMapDataSource;
        private double _maxMapBattles = 10;
        private double _maxWinReplayPercent = 100;
        private List<DataPoint> _winReplaysPercentByMapDataSource;
        private List<ReplayFile> _replays = new List<ReplayFile>();

        public List<TankStatisticRowViewModel> LastUsedTanksList
        {
            get
            {
                List<TankStatisticRowViewModel> list = _tanks.Where(x => x.LastBattle > PlayerStatistic.PreviousDate).ToList();
                return list;
            }
        }

        public List<SellInfo> LastUsedTanksDataSource
        {
            get { return _lastUsedTanksDataSource; }
            set
            {
                _lastUsedTanksDataSource = value;
                RaisePropertyChanged("LastUsedTanksDataSource");
                RaisePropertyChanged("LastUsedTanksList");
            }
        }

        public EnumerableDataSource<DataPoint> RatingDataSource
        {
            get { return _ratingDataSource; }
            set
            {
                _ratingDataSource = value;
                RaisePropertyChanged("RatingDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> WN6RatingDataSource
        {
            get { return _wn6RatingDataSource; }
            set
            {
                _wn6RatingDataSource = value;
                RaisePropertyChanged("WN6RatingDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> WinPercentDataSource
        {
            get { return _winPercentDataSource; }
            set
            {
                _winPercentDataSource = value;
                RaisePropertyChanged("WinPercentDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgDamageDataSource
        {
            get { return _avgDamageDataSource; }
            set
            {
                _avgDamageDataSource = value;
                RaisePropertyChanged("AvgDamageDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgXPDataSource
        {
            get { return _avgXpDataSource; }
            set
            {
                _avgXpDataSource = value;
                RaisePropertyChanged("AvgXPDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> AvgSpottedDataSource
        {
            get { return _avgSpottedDataSource; }
            set
            {
                _avgSpottedDataSource = value;
                RaisePropertyChanged("AvgSpottedDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> KillDeathRatioDataSource
        {
            get { return _killDeathRatioDataSource; }
            set
            {
                _killDeathRatioDataSource = value;
                RaisePropertyChanged("KillDeathRatioDataSource");
            }
        }

        public EnumerableDataSource<DataPoint> SurvivePercentDataSource
        {
            get { return _survivePercentDataSource; }
            set
            {
                _survivePercentDataSource = value;
                RaisePropertyChanged("SurvivePercentDataSource");
            }
        }

        public List<DataPoint> EfficiencyByTierDataSource
        {
            get { return _efficiencyByTierDataSource; }
            set
            {
                _efficiencyByTierDataSource = value;
                RaisePropertyChanged("EfficiencyByTierDataSource");
            }
        }

        public List<DataPoint> ReplaysByMapDataSource
        {
            get { return _replaysByMapDataSource; }
            set
            {
                _replaysByMapDataSource = value;
                RaisePropertyChanged("ReplaysByMapDataSource");
            }
        }

        public List<DataPoint> WinReplaysPercentByMapDataSource
        {
            get { return _winReplaysPercentByMapDataSource; }
            set
            {
                _winReplaysPercentByMapDataSource = value;
                RaisePropertyChanged("WinReplaysPercentByMapDataSource");
            }
        }

        public List<GenericPoint<string, double>> EfficiencyByTypeDataSource
        {
            get { return _efficiencyByTypeDataSource; }
            set
            {
                _efficiencyByTypeDataSource = value;
                RaisePropertyChanged("EfficiencyByTypeDataSource");
            }
        }

        public List<GenericPoint<string, double>> EfficiencyByCountryDataSource
        {
            get { return _efficiencyByCountryDataSource; }
            set
            {
                _efficiencyByCountryDataSource = value;
                RaisePropertyChanged("EfficiencyByCountryDataSource");
            }
        }

        public double MaxMapBattles
        {
            get { return _maxMapBattles; }
            set
            {
                _maxMapBattles = value;
                RaisePropertyChanged("MaxMapBattles");
            }
        }

        public double MaxWinReplayPercent
        {
            get { return _maxWinReplayPercent; }
            set
            {
                _maxWinReplayPercent = value;
                RaisePropertyChanged("MaxWinReplayPercent");
            }
        }

        public sealed class SellInfo : INotifyPropertyChanged
        {
            private double _winPercent;
            public double WinPercent
            {
                get { return _winPercent; }
                set { _winPercent = value; PropertyChanged.Raise(this, "WinPercent"); }
            }

            private string _tankName;
            public string TankName
            {
                get { return _tankName; }
                set { _tankName = value; PropertyChanged.Raise(this, "TankName"); }
            }

            private int _battles;
            public int Battles
            {
                get { return _battles; }
                set { _battles = value; PropertyChanged.Raise(this, "Battles"); }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
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
            ClanData clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), PlayerStatistic.Clan.id);
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
                viewModel.Show();
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

                Replay replay = WotApiClient.Instance.ReadReplay(jsonFile);
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
            List<DateTime> list = null;
            if (PlayerStatistic != null)
            {
                list = PlayerStatistic.GetAll().Select(x => x.Updated).OrderByDescending(x => x).Skip(1).ToList();
            }
            viewModel.PrevDates = list ?? new List<DateTime>();
            viewModel.Show();
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

                        if (settings != null && !string.IsNullOrEmpty(settings.PlayerName) && !string.IsNullOrEmpty(settings.Server))
                        {
                            FileInfo cacheFile = CacheHelper.GetCacheFile(settings.PlayerName);

                            List<TankJson> tanks;

                            if (cacheFile != null)
                            {
                                //convert dossier cache file to json
                                CacheHelper.BinaryCacheToJson(cacheFile);

                                tanks = LoadTanks(cacheFile);

                                PlayerStatistic = InitPlayerStatisticViewModel(serverStatistic, tanks);
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

                                InitTanksStatistic(tanks);

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

                        LoadReplaysList();
                    }
                    catch (Exception e)
                    {
                        _log.Error("Error on data load", e);
                        MessageBox.Show(Resources.Resources.Msg_ErrorOnDataLoad, Resources.Resources.WindowCaption_Error,
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
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
                    playerStat = WotApiClient.Instance.LoadPlayerStat(settings, playerId);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return new ServerStatWrapper(playerStat);
        }

        private static List<TankJson> LoadTanks(FileInfo cacheFile)
        {
            List<TankJson> tanks = WotApiClient.Instance.ReadTanks(cacheFile.FullName.Replace(".dat", ".json"));
            return tanks;
        }

        #endregion

        #region Initialize

        private void LoadReplaysList()
        {
            ReplaysFolders = ReplaysManager.GetFolders();
            Replays.Clear();

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
                                Replay data = WotApiClient.Instance.ReadReplay2Blocks(replay);
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

                    InitBattlesByMapChart(_replays);
                    InitWinReplaysPercentByMapChart(_replays);

                }, new ProgressDialogSettings(true, true, false));
        }

        private PlayerStatisticViewModel InitPlayerStatisticViewModel(ServerStatWrapper serverStatistic, List<TankJson> tanks)
        {
            AppSettings settings = SettingsReader.Get();

            PlayerEntity player = _dossierRepository.UpdatePlayerStatistic(serverStatistic.Ratings, tanks, settings.PlayerId);

            List<PlayerStatisticEntity> statisticEntities = _dossierRepository.GetPlayerStatistic(player.PlayerId).ToList();
            return StatisticViewModelFactory.Create(statisticEntities, tanks, player.Name, player.Creaded, serverStatistic.Clan);
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

            InitMasterTankerList(Tanks);

            FraggsCount.Init(Tanks);
        }

        private void InitMasterTankerList(List<TankStatisticRowViewModel> tanks)
        {
            IEnumerable<int> killed =
                tanks.SelectMany(x => x.TankFrags).Select(x => x.TankUniqueId).Distinct().OrderBy(x => x);
            List<TankRowMasterTanker> masterTanker = WotApiClient.Instance.TanksDictionary
                                                                 .Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value))
                                                                 .Select(x => new TankRowMasterTanker(x.Value, WotApiClient.Instance.GetTankIcon(x.Value)))
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
                List<PlayerStatisticViewModel> statisticViewModels = PlayerStatistic.GetAll();
                InitRatingChart(statisticViewModels);
                InitWinPercentChart(statisticViewModels);
                InitAvgDamageChart(statisticViewModels);
                InitAvgXPChart(statisticViewModels);
                InitAvgSpottedChart(statisticViewModels);
                InitKillDeathRatioChart(statisticViewModels);
                InitSurvivePercentChart(statisticViewModels);
                InitEfficiencyByTierChart(_tanks);
                InitEfficiencyByTypeChart(_tanks);
                InitEfficiencyByCountryChart(_tanks);
                InitLastUsedTanksChart();
            }
        }

        private void InitBattlesByMapChart(IEnumerable<ReplayFile> list)
        {
            List<DataPoint> dataSource = list.GroupBy(x => x.MapId).Select(x => new DataPoint(x.Count(), x.Key)).ToList();

            if (dataSource.Any())
            {
                ReplaysByMapDataSource = dataSource;

                double max = ReplaysByMapDataSource.Max(x => x.X);
                MaxMapBattles = max + 0.1*max;
            }
        }

        private void InitWinReplaysPercentByMapChart(IEnumerable<ReplayFile> list)
        {
            List<DataPoint> dataSource = list.GroupBy(x => x.MapId).Select(
                x => new DataPoint(
                    100 * x.Sum(y => (y.IsWinner == BattleStatus.Victory ? 1.0 : 0.0)) / x.Count(), x.Key)).ToList();

            if (dataSource.Any())
            {
                WinReplaysPercentByMapDataSource = dataSource;

                double max = WinReplaysPercentByMapDataSource.Max(x => x.X);
                MaxWinReplayPercent = max;
            }
        }

        private void InitEfficiencyByTierChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> dataSource = statisticViewModels.GroupBy(x => x.Tier).Select(x => new DataPoint(x.Key, RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTierDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByTypeChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.Type).Select(x => new GenericPoint<string, double>(x.Key.ToString(), RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByTypeDataSource = dataSource.ToList();
        }

        private void InitEfficiencyByCountryChart(List<TankStatisticRowViewModel> statisticViewModels)
        {
            IEnumerable<GenericPoint<string, double>> dataSource = statisticViewModels.GroupBy(x => x.CountryId).Select(x => new GenericPoint<string, double>(x.Key.ToString(), RatingHelper.CalcER(
                x.Average(y => y.AvgDamageDealt),
                x.Key,
                x.Average(y => y.AvgFrags),
                x.Average(y => y.AvgSpotted),
                x.Average(y => y.AvgCapturePoints),
                x.Average(y => y.AvgDroppedCapturePoints))));
            EfficiencyByCountryDataSource = dataSource.ToList();
        }

        private void InitSurvivePercentChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.SurvivedBattlesPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_Survive, point.X, point.Y));
            SurvivePercentDataSource = dataSource;
        }

        private void InitKillDeathRatioChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.KillDeathRatio));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_KillDeathRatio, point.X, point.Y));
            KillDeathRatioDataSource = dataSource;
        }

        private void InitAvgXPChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgXp));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_AvgXp, point.X, point.Y));
            AvgXPDataSource = dataSource;
        }

        private void InitAvgSpottedChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgSpotted));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.Chart_Tooltip_AvgSpotted, point.X, point.Y));
            AvgSpottedDataSource = dataSource;
        }

        private void InitRatingChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));

            RatingDataSource = dataSource;

            IEnumerable<DataPoint> wn6Points = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_Rating, point.X, point.Y));

            WN6RatingDataSource = dataSource;
        }

        private void InitWinPercentChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WinsPercent));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_WinPercent, point.X, point.Y));
            WinPercentDataSource = dataSource;
        }

        private void InitAvgDamageChart(List<PlayerStatisticViewModel> statisticViewModels)
        {
            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.AvgDamageDealt));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) { XMapping = x => x.X, YMapping = y => y.Y };
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(Resources.Resources.ChartTooltipFormat_AvgDamage, point.X, point.Y));
            AvgDamageDataSource = dataSource;
        }

        private void InitLastUsedTanksChart()
        {
            IEnumerable<TankStatisticRowViewModel> viewModels = _tanks.Where(x => x.Updated > PlayerStatistic.PreviousDate);
            IEnumerable<SellInfo> items = viewModels.Select(x => new SellInfo { TankName = x.Tank, WinPercent = x.WinsPercentForPeriod, Battles = x.BattlesCountDelta });
            LastUsedTanksDataSource = items.ToList();
        }

        #endregion

        private bool IsExistedtank(TankInfo tankInfo)
        {
            //TODO: refactoring
            return tankInfo.tankid <= 250 && !tankInfo.icon.Contains("training") && tankInfo.title != "KV" && tankInfo.title != "T23";
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
                InitLastUsedTanksChart();   
            }
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