using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Ookii.Dialogs.Wpf;
using TournamentStat.Applications.View;
using WotDossier.Applications;
using WotDossier.Applications.BattleModeStrategies;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Resources;

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
                if (File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }

                using (new WaitCursor())
                {
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(dialog.FileName)))
                    {
                        AddParticipantsSheet(package, settings);
                        AddStatSheet(package, settings);
                        AddNominationsSheets(package, settings);

                        package.Save();
                    }
                }
            }
        }

        private void AddNominationsSheets(ExcelPackage package, TournamentStatSettings settings)
        {
            foreach (var nomination in settings.TournamentNominations)
            {
                AddNominationSheets(package, nomination);
            }
        }

        private void AddNominationSheets(ExcelPackage package, TournamentNomination nomination)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add(nomination.Nomination);
            int row = 2;

            int column = 1;

            excelWorksheet.Cells[1, column++].Value = "место";
            excelWorksheet.Cells[1, column++].Value = "игрок";
            excelWorksheet.Cells[1, column++].Value = "танк";
            excelWorksheet.Cells[1, column++].Value = "урон";

            TournamentStatistic.TournamentTankResults.SelectedNomination = nomination;

            int index = 1;

            foreach (TankStatisticRowViewModelBase tankStatistic in TournamentStatistic.TournamentTankResults.TankResult)
            {
                column = 1;

                excelWorksheet.Cells[row, column++].Value = index++;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PlayerName;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Tank;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod;

                row++;
            }
            
        }

        private void AddStatSheet(ExcelPackage package, TournamentStatSettings settings)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add("Stat");
            int row = 2;

            int column = 1;

            excelWorksheet.Cells[1, column++].Value = "№";
            excelWorksheet.Cells[1, column++].Value = "игрок";
            excelWorksheet.Cells[1, column++].Value = "танк";
            //excelWorksheet.Cells[1, column++].Value = "режим";
            //excelWorksheet.Cells[1, column++].Value = "голд";
            excelWorksheet.Cells[1, column++].Value = "дата";
            excelWorksheet.Cells[1, column++].Value = "бои";
            excelWorksheet.Cells[1, column++].Value = "ПП";
            excelWorksheet.Cells[1, column++].Value = "опыт";
            excelWorksheet.Cells[1, column++].Value = "фраги";
            excelWorksheet.Cells[1, column++].Value = "урон";
            excelWorksheet.Cells[1, column++].Value = "WN8";
            //excelWorksheet.Cells[1, column++].Value = "УпЗ";
            //excelWorksheet.Cells[1, column++].Value = "ППУ";
            //excelWorksheet.Cells[1, column++].Value = "захв";
            //excelWorksheet.Cells[1, column++].Value = "защ";
            //excelWorksheet.Cells[1, column++].Value = "свет";
            excelWorksheet.Cells[1, column++].Value = "твитч";
            excelWorksheet.Cells[1, column++].Value = "реплеи на ресурсе игрока";
            excelWorksheet.Cells[1, column++].Value = "реплеи на ресурсе организатора";
            excelWorksheet.Cells[1, column++].Value = "патч";

            excelWorksheet.Cells[1, column++].Value = "бои";
            excelWorksheet.Cells[1, column++].Value = "поб";
            excelWorksheet.Cells[1, column++].Value = "ПП";
            excelWorksheet.Cells[1, column++].Value = "урон";
            //excelWorksheet.Cells[1, column++].Value = "опыт";
            //excelWorksheet.Cells[1, column++].Value = "фраги";

            excelWorksheet.Cells[1, column++].Value = "бои";
            excelWorksheet.Cells[1, column++].Value = "поб";
            excelWorksheet.Cells[1, column++].Value = "ПП";
            excelWorksheet.Cells[1, column++].Value = "урон";
            //excelWorksheet.Cells[1, column++].Value = "опыт";
            //excelWorksheet.Cells[1, column++].Value = "фраги";

            //excelWorksheet.Cells[1, column++].Value = "УпЗ";
            //excelWorksheet.Cells[1, column++].Value = "ППУ";
            //excelWorksheet.Cells[1, column++].Value = "захв";
            //excelWorksheet.Cells[1, column++].Value = "защ";
            //excelWorksheet.Cells[1, column++].Value = "свет";
            //excelWorksheet.Cells[1, column++].Value = "dosssier";

            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            int index = 1;

            foreach (TankStatisticRowViewModelBase tankStatistic in TournamentStatistic.Series)
            {
                column = 1;

                excelWorksheet.Cells[row, column++].Value = index++;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PlayerName;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Tank;
                excelWorksheet.Cells[row, column].Value = tankStatistic.Updated;
                excelWorksheet.Cells[row, column++].Style.Numberformat.Format = "[$-419]d mmm;@";
                excelWorksheet.Cells[row, column++].Value = tankStatistic.BattlesCountDelta;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WinsPercentForPeriod;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgXpForPeriod;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgFragsForPeriod;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WN8RatingForPeriod;

                excelWorksheet.Cells[row, column++].Value = null;//"твитч";
                excelWorksheet.Cells[row, column++].Value = null;//"реплеи на ресурсе игрока";
                excelWorksheet.Cells[row, column++].Value = null;//"реплеи на ресурсе организатора";
                excelWorksheet.Cells[row, column++].Value = null;//"патч";

                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.BattlesCount;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.Wins;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.WinsPercent;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.DamageDealt;

                excelWorksheet.Cells[row, column++].Value = tankStatistic.BattlesCount;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Wins;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WinsPercent;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.DamageDealt;

                row++;
            }
        }

        private void AddParticipantsSheet(ExcelPackage package, TournamentStatSettings settings)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add("Participants");
            int row = 2;

            excelWorksheet.Cells[1, 1].Value = "номер";
            excelWorksheet.Cells[1, 2].Value = "игрок";
            excelWorksheet.Cells[1, 3].Value = "ссылка на профиль";
            excelWorksheet.Cells[1, 4].Value = "твитч";
            excelWorksheet.Cells[1, 5].Value = "моды";
            excelWorksheet.Cells[1, 6].Value = "дата";
            excelWorksheet.Cells[1, 7].Value = "танки";
            excelWorksheet.Cells[1, 8].Value = "призы";
            excelWorksheet.Cells[1, 9].Value = "серий";

            excelWorksheet.Cells[1, 1, 1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, 1, 1, 9].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            int index = 1;

            foreach (var participant in settings.Players)
            {
                excelWorksheet.Cells[row, 1].Value = index++;
                excelWorksheet.Cells[row, 2].Value = participant.PlayerName;
                var link = $"http://worldoftanks.ru/community/accounts/{participant.PlayerId}-{participant.PlayerName}/";
                excelWorksheet.Cells[row, 3].Hyperlink = new ExcelHyperLink(link);
                excelWorksheet.Cells[row, 3].Value = link;
                if (!string.IsNullOrEmpty(participant.TwitchUrl))
                {
                    excelWorksheet.Cells[row, 4].Value = new ExcelHyperLink(participant.TwitchUrl);
                }
                excelWorksheet.Cells[row, 5].Value = participant.Modes;
                excelWorksheet.Cells[row, 6].Value = participant.StartDate;
                excelWorksheet.Cells[row, 6].Style.Numberformat.Format = "[$-419]d mmm;@";
                excelWorksheet.Cells[row, 7].Value = string.Join(", ",
                    TournamentStatistic.Series.Where(x => x.PlayerName == participant.PlayerName)
                        .GroupBy(x => x.Tank)
                        .Select(x => x.Key));
                //excelWorksheet.Cells[row, 8].Value = participant.Tanks.Select();
                excelWorksheet.Cells[row, 9].Value =
                    TournamentStatistic.Series.Count(x => x.PlayerName == participant.PlayerName);

                row++;
            }

            row = row + 2;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "желтый цвет - игрок заявился, статистика на начало не снята, серию играть нельзя";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "зеленый цвет-игрок начал серию, статистика на начало зафиксирована";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Green);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "синий цвет-игрок закончил 1 серию на танке";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "фиолетовый цвет-игрок закончил 2+ серии на танке";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.DarkViolet);

            //for (int i = 1; i < 9; i++)
            //{
            //    excelWorksheet.Column(1).AutoFit();
            //}

            excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
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
                                !settings.Players.Exists(x => x.PlayerId == playerEntity.PlayerId))
                            {
                                var tournamentTanks = new List<TournamentTank>();
                                string twitchUrl = null;

                                if (settings.Players == null)
                                {
                                    settings.Players = new List<TournamentPlayer>();
                                }

                                settings.Players.Add(new TournamentPlayer
                                {
                                    PlayerId = playerEntity.PlayerId,
                                    PlayerName = playerEntity.Name,
                                    Tanks = tournamentTanks,
                                    TwitchUrl = twitchUrl,
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
                }

                LoadStat();
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

                StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(BattleMode.RandomCompany,
                    _dossierRepository);

                List<ITankStatisticRow> allSeries = new List<ITankStatisticRow>();

                foreach (var playerEntity in playerEntities)
                {
                    var rows = strategy.GetTanksStatistic(playerEntity.Id);
                    List<ITankStatisticRow> tankStatisticRows =
                        rows.Where(x => tankIds.Contains(x.TankUniqueId)).SelectMany(x => x.GetAll()).ToList();

                    //if several series on one tank
                    var groupByTankId = tankStatisticRows.GroupBy(x => x.TankUniqueId);

                    foreach (var tankSeries in groupByTankId)
                    {
                        foreach (var series in tankSeries)
                        {
                            series.PlayerId = playerEntity.Id;
                            series.PlayerName = playerEntity.Name;

                            //find series
                            var endSeries = tankSeries.FirstOrDefault(
                                x =>
                                    x.BattlesCount - series.BattlesCount >= 100 &&
                                    x.BattlesCount - series.BattlesCount < 105);
                            //set series start
                            endSeries?.SetPreviousStatistic(series);
                        }
                    }

                    allSeries.AddRange(
                        tankStatisticRows.Where(x => x.BattlesCountDelta >= 100 && x.BattlesCountDelta < 105));
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

            StatisticViewStrategyBase strategy = StatisticViewStrategyManager.Get(BattleMode.RandomCompany, _dossierRepository);

            strategy.UpdatePlayerStatistic(player.PlayerId, tanksCache, null);

            strategy.UpdateTankStatistic(player.PlayerId, tanksCache);
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
