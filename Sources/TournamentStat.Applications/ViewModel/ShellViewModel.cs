using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using TournamentStat.Applications.Logic;
using TournamentStat.Applications.View;
using WotDossier.Applications;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Resources;
using TournamentStat.Applications.BattleModeStrategies;

namespace TournamentStat.Applications.ViewModel
{
    public class ShellViewModel : ViewModel<IShellView>
    {
        private readonly DossierRepository _dossierRepository;

        public ICommand LoadCommand { get; set; }

        public ICommand SettingsCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        public TournamentStatViewModel TournamentStatistic { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        public ShellViewModel(IShellView view, DossierRepository dossierRepository)
            : base(view, false)
        {
            _dossierRepository = dossierRepository;
            LoadCommand = new DelegateCommand(AddDossierCache, CanLoad);
            SettingsCommand = new DelegateCommand(OnSettings);
            ExportCommand = new DelegateCommand(OnExport);
            //AboutCommand = new DelegateCommand(OnAbout);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(e));

            TournamentStatistic = new TournamentStatViewModel();
            
            //ProgressView = new ProgressControlViewModel();

            //ChartView = new PlayerChartsViewModel();

            //ReplaysViewModel = new ReplaysViewModel(dossierRepository, ProgressView, ChartView);

            //EventAggregatorFactory.EventAggregator.GetEvent<ReplayManagerRefreshEvent>().Subscribe(OnReplayManagerRefresh);
        }

        private void OnExport()
        {
            var settings = SettingsReader.Get<TournamentStatSettings>();

            VistaSaveFileDialog dialog = new VistaSaveFileDialog();
            dialog.FileName = settings.TournamentName + ".xlsx";
            var result = dialog.ShowDialog();
            if (result == true)
            {
                var fileName = dialog.FileName;
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                new ExcelExportProvider().Export(fileName, settings, TournamentStatistic.Series);

                using (var proc = new Process())
                {
                    //proc.StartInfo.CreateNoWindow = true;
                    //proc.StartInfo.UseShellExecute = false;
                    //proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.FileName = fileName;
                    proc.Start();
                }
            }
        }
        
        #endregion

        private void OnSettings()
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<SettingsViewModel>();
            if (viewModel?.Show() == true)
            {
                LoadStat();
            }
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public virtual void Show()
        {
            ViewTyped.Loaded += OnShellViewActivated;
            ViewTyped.Show();

            //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
            //{
            //    AppUpdater.Update();
            //}, null);
        }

        private void OnShellViewActivated(object sender, EventArgs eventArgs)
        {
            ViewTyped.Loaded -= OnShellViewActivated;
            LoadStat();
        }

        private void AddDossierCache()
        {
            TournamentStatSettings settings = SettingsReader.Get<TournamentStatSettings>();

            if (settings == null || string.IsNullOrEmpty(settings.TournamentName))
            {
                MessageBox.Show("Specify tournament name", Resources.WindowCaption_Warning,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectCacheFile = SelectCacheFile();

            using (new WaitCursor())
            {
                if (!string.IsNullOrEmpty(selectCacheFile))
                {
                    FileInfo cacheFile = new FileInfo(selectCacheFile);
                    var playerName = CacheFileHelper.GetPlayerName(cacheFile);

                    PlayerSearchJson player = WotApiClient.Instance.SearchPlayer(playerName, settings);

                    Player playerStat = null;

                    if (player != null)
                    {
                        playerStat = WotApiClient.Instance.LoadPlayerStat(player.account_id, settings,
                            PlayerStatLoadOptions.LoadCommon, new[] {"account_id", "nickname", "created_at"});

                        if (playerStat != null)
                        {
                            double createdAt = playerStat.dataField.created_at;
                            var playerEntity = _dossierRepository.GetOrCreatePlayer(player.nickname, player.account_id,
                                Utils.UnixDateToDateTime((long) createdAt), settings.Server);

                            UpdateLocalDatabase(cacheFile, playerEntity);

                            if (settings.Players == null ||
                                !settings.Players.Exists(x => x.AccountId == playerEntity.AccountId))
                            {
                                var tournamentTanks = new List<TournamentTank>();

                                if (settings.Players == null)
                                {
                                    settings.Players = new List<TournamentPlayer>();
                                }

                                settings.Players.Add(new TournamentPlayer
                                {
                                    PlayerId = playerEntity.Id,
                                    AccountId = playerEntity.AccountId,
                                    PlayerName = playerEntity.Name,
                                    Tanks = tournamentTanks,
                                    StartDate = DateTime.Now
                                });
                                SettingsReader.Save(settings);
                            }
                            else
                            {
                                //set replays link
                            }
                        }
                    }

                    LoadStat();
                }
            }
        }

        private void LoadStat()
        {
            using (new WaitCursor())
            {
                TournamentStatSettings settings = SettingsReader.Get<TournamentStatSettings>();

                var playerEntities = _dossierRepository.GetPlayers();

                var tankIds =
                    settings.TournamentNominations.SelectMany(x => x.TournamentTanks.Select(t => t.TankUniqueId))
                        .ToList();

                BattleModeStrategies.StatisticViewStrategyBase strategy = new RandomStatisticViewStrategy(_dossierRepository);

                List<ITankStatisticRow> allSeries = new List<ITankStatisticRow>();

                foreach (var playerEntity in playerEntities)
                {
                    var player = settings.Players.First(x => x.AccountId == playerEntity.AccountId);
                    
                    var rows = strategy.GetTanksStatistic(playerEntity.Id, settings.TournamentNominations.SelectMany(x => x.TournamentTanks));
                    List<ITankStatisticRow> tankStatisticRows =
                        rows.Where(x => tankIds.Contains(x.TankUniqueId)).SelectMany(x => x.GetAll()).ToList();

                    //if several series on one tank
                    var groupByTankId = tankStatisticRows.GroupBy(x => x.TankUniqueId);

                    foreach (var tankSeries in groupByTankId)
                    {
                        foreach (TankStatisticRowViewModelBase series in tankSeries)
                        {
                            series.PlayerId = playerEntity.Id;
                            series.PlayerName = playerEntity.Name;
                            series.TwitchUrl = player.TwitchUrl;

                            var tournamentTank = player.Tanks.FirstOrDefault(x => x.BattlesCount == series.BattlesCount);
                            series.Dossier = tournamentTank?.Dossier;
                            series.ReplaysUrl = tournamentTank?.ReplaysUrl;
                            series.ReplaysUrlOwner = tournamentTank?.ReplaysUrlOwner;

                            //find series
                            var endSeries = tankSeries.FirstOrDefault(
                                x =>
                                    x.BattlesCount - series.BattlesCount >= 100 &&
                                    x.BattlesCount - series.BattlesCount < 105);
                            //set series start
                            endSeries?.SetPreviousStatistic(series);
                        }
                    }

                    var statisticRows = tankStatisticRows.Where(x => x.BattlesCountDelta >= 100 && x.BattlesCountDelta < 105);

                    
                    allSeries.AddRange(statisticRows);
                }

                TournamentStatistic.Series = allSeries;

                TournamentStatistic.TournamentTankResults =
                    new TournamentTankResultsViewModel(settings.TournamentNominations, allSeries);
            }
        }

        private void UpdateLocalDatabase(FileInfo cacheFile, PlayerEntity player)
        {
            //convert dossier cache file to json
            string jsonFile = CacheFileHelper.BinaryCacheToJson(cacheFile);
            var tanksCache = CacheFileHelper.ReadTanksCache(jsonFile);

            StatisticViewStrategyBase strategy = new RandomStatisticViewStrategy(_dossierRepository);

            strategy.UpdatePlayerStatistic(player.AccountId, tanksCache, null);

            strategy.UpdateTankStatistic(player.AccountId, tanksCache);
        }

        private string SelectCacheFile()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.InitialDirectory = Folder.GetDefaultDossierCacheFolder();
            bool? showDialog = dialog.ShowDialog();
            if (showDialog == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        private bool CanLoad()
        {
            return true;
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

        private void ViewClosing(CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                e.Cancel = !IsCloseAllowed();
            }
        }
    }
}
