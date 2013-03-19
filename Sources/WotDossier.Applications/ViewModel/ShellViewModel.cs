using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Domain.Rows;
using WotDossier.Framework.Applications;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Common.Logging;

namespace WotDossier.Applications.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class ShellViewModel : ViewModel<IShellView>
    {
        private const string BATTLES_RATING_MARKER_TOOLTIP_FORMAT = "Battles: {0}\nRating: {1:0.00}";
        private readonly DossierRepository _dossierRepository;
        private string _curDirTemp;
        private FileInfo _last = null;
        private readonly SettingsReader _reader = new SettingsReader(WotDossierSettings.SettingsPath);

        protected static readonly ILog _log = LogManager.GetLogger("log");
        private PlayerStatisticViewModel _playerStatistic;
        private IEnumerable<TankRowBattles> _battles;
        private IEnumerable<TankRowXP> _xp;
        private IEnumerable<TankRowFrags> _frags;
        private IEnumerable<TankRowDamage> _damage;
        private IEnumerable<TankRowBattleAwards> _battleAwards;
        private IEnumerable<TankRowSpecialAwards> _specialAwards;
        private IEnumerable<TankRowSeries> _series;
        private IEnumerable<TankRowMedals> _medals;
        private IEnumerable<TankRowRatings> _ratings;
        private IEnumerable<TankRowPerformance> _performance;
        private IEnumerable<TankRowMasterTanker> _masterTanker;
        private IEnumerable<TankRowEpic> _epics;
        private IEnumerable<TankRowTime> _time;
        
        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged("PlayerStatistic");
            }
        }

        public ChartPlotter Chart
        {
            get { return ViewTyped.Chart; }
        }
        
        public DelegateCommand LoadCommand { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository"></param>
        public ShellViewModel(IShellView view, DossierRepository dossierRepository)
            : this(view, false)
        {
            _dossierRepository = dossierRepository;
            LoadCommand = new DelegateCommand(OnLoad);

            WeakEventHandler.SetAnyGenericHandler<ShellViewModel, CancelEventArgs>(
                h => view.Closing += new CancelEventHandler(h), h => view.Closing -= new CancelEventHandler(h), this, (s, e) => s.ViewClosing(s, e));
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

        private void OnLoad()
        {
            PlayerStatistic = GetPlayerStatistic();

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache", "*.dat");
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.Error("Путь к файлам кэша не найден", ex);
            }

            if (!files.Any())
            {
                return;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (_last == null)
                {
                    _last = info;
                }
                else if (_last.LastWriteTime < info.LastWriteTime)
                {
                    _last = info;
                }
            }

            _curDirTemp = Environment.CurrentDirectory;

            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = directoryName + @"\External";
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = directoryName + @"\External\wotdc2j.exe";
            proc.StartInfo.Arguments = string.Format("{0} -f -r", _last.FullName);
            proc.Start();

            Thread.Sleep(1000);

            ProcOnExited(null, null);
        }

        private PlayerStatisticViewModel GetPlayerStatistic()
        {
            AppSettings settings = _reader.Get();
            if (settings == null || string.IsNullOrEmpty(settings.PlayerId) || string.IsNullOrEmpty(settings.Server))
            {
                return null;
            }

            PlayerStat playerStat = Read.LoadPlayerStat(settings);

            PlayerEntity player;

            if (playerStat != null)
            {
                player = _dossierRepository.GetOrCreatePlayer(settings.PlayerId, playerStat);

                var playerEntities = _dossierRepository.GetPlayerStatistic(settings.PlayerId).ToList();

                PlayerStatisticEntity currentStatistic = playerEntities.OrderByDescending(x => x.Updated).First();
                IEnumerable<PlayerStatisticViewModel> statisticViewModels = playerEntities.Where(x => x.Id != currentStatistic.Id)
                    .Select(x => new PlayerStatisticViewModel(x));

                IEnumerable<PlayerStatisticViewModel> viewModels = playerEntities.Select(x => new PlayerStatisticViewModel(x));

                InitChart(viewModels);

                PlayerStatisticViewModel playerStatisticViewModel = new PlayerStatisticViewModel(currentStatistic, statisticViewModels);
                playerStatisticViewModel.Name = player.Name;
                playerStatisticViewModel.Created = player.Creaded;

                if (playerStat.data.clan != null)
                {
                    playerStatisticViewModel.Clan = new PlayerStatisticClanViewModel(playerStat.data.clan);
                }

                return playerStatisticViewModel;
            }
            return null;
        }

        private void InitChart(IEnumerable<PlayerStatisticViewModel> statisticViewModels)
        {
            DataRect dataRect = DataRect.Create(0, 0, 100000, 2500);
            Chart.Viewport.Domain = dataRect;
            Chart.Viewport.Visible = dataRect;

            IEnumerable<DataPoint> erPoints = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.EffRating));
            var dataSource = new EnumerableDataSource<DataPoint>(erPoints) {XMapping = x => x.X, YMapping = y => y.Y};
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(BATTLES_RATING_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            SolidColorBrush brush = new SolidColorBrush {Color = Colors.Blue};
            Pen lineThickness = new Pen(brush, 2);
            ElementPointMarker circlePointMarker = new CircleElementPointMarker {Size = 7, Fill = brush, Brush = brush};
            LineAndMarker<ElementMarkerPointsGraph> lineGraph = Chart.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                                                                   new PenDescription("РЭ"));

            IEnumerable<DataPoint> wn6Points = statisticViewModels.Select(x => new DataPoint(x.BattlesCount, x.WN6Rating));
            dataSource = new EnumerableDataSource<DataPoint>(wn6Points) {XMapping = x => x.X, YMapping = y => y.Y};
            dataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                                  point => String.Format(BATTLES_RATING_MARKER_TOOLTIP_FORMAT, point.X, point.Y));
            brush = new SolidColorBrush {Color = Colors.Green};
            lineThickness = new Pen(brush, 2);
            circlePointMarker = new CircleElementPointMarker() {Size = 7, Fill = brush, Brush = brush};
            LineAndMarker<ElementMarkerPointsGraph> lineGraph1 = Chart.AddLineGraph(dataSource, lineThickness, circlePointMarker,
                                                                                    new PenDescription("WN6"));

            Chart.FitToView();
        }

        private void ProcOnExited(object sender, EventArgs eventArgs)
        {
            Action act = () =>
            {
                Environment.CurrentDirectory = _curDirTemp;

                List<Tank> tanks = Read.ReadTanks(_last.FullName.Replace(".dat", ".json"));

                IEnumerable<TankRowBattles> battles = tanks.Select(x => new TankRowBattles(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowXP> xp = tanks.Select(x => new TankRowXP(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowFrags> frags = tanks.Select(x => new TankRowFrags(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowDamage> damage = tanks.Select(x => new TankRowDamage(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowBattleAwards> battleAwards = tanks.Select(x => new TankRowBattleAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowSpecialAwards> specialAwards = tanks.Select(x => new TankRowSpecialAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowSeries> series = tanks.Select(x => new TankRowSeries(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowMedals> medals = tanks.Select(x => new TankRowMedals(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowRatings> ratings = tanks.Select(x => new TankRowRatings(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowPerformance> performance = tanks.Select(x => new TankRowPerformance(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowEpic> epic = tanks.Select(x => new TankRowEpic(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowTime> time = tanks.Select(x => new TankRowTime(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                IEnumerable<KeyValuePair<int, int>> killed = tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
                IEnumerable<TankRowMasterTanker> masterTanker = Read.TankDictionary.Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value)).Select(x => new TankRowMasterTanker(x.Value, Read.GetTankContour(x.Value))).OrderBy(x => x.IsPremium).ThenBy(x => x.Tier);

                Battles= battles;
                Xp= xp;
                Frags= frags;
                Damage= damage;
                BattleAwards= battleAwards;
                SpecialAwards= specialAwards;
                Series= series;
                Medals= medals;
                Ratings= ratings;
                Performance= performance;
                MasterTanker= masterTanker;
                Epics= epic;
                Time = time;
            };

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }

        public IEnumerable<TankRowTime> Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged("Time");
            }
        }

        public IEnumerable<TankRowEpic> Epics
        {
            get { return _epics; }
            set
            {
                _epics = value;
                RaisePropertyChanged("Epics");
            }
        }

        public IEnumerable<TankRowMasterTanker> MasterTanker
        {
            get { return _masterTanker; }
            set
            {
                _masterTanker = value;
                RaisePropertyChanged("MasterTanker");
            }
        }

        public IEnumerable<TankRowPerformance> Performance
        {
            get { return _performance; }
            set
            {
                _performance = value;
                RaisePropertyChanged("Performance");
            }
        }

        public IEnumerable<TankRowRatings> Ratings
        {
            get { return _ratings; }
            set
            {
                _ratings = value;
                RaisePropertyChanged("Ratings");
            }
        }

        public IEnumerable<TankRowMedals> Medals
        {
            get { return _medals; }
            set
            {
                _medals = value;
                RaisePropertyChanged("Medals");
            }
        }

        public IEnumerable<TankRowSeries> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                RaisePropertyChanged("Series");
            }
        }

        public IEnumerable<TankRowSpecialAwards> SpecialAwards
        {
            get { return _specialAwards; }
            set
            {
                _specialAwards = value;
                RaisePropertyChanged("SpecialAwards");
            }
        }

        public IEnumerable<TankRowBattleAwards> BattleAwards
        {
            get { return _battleAwards; }
            set
            {
                _battleAwards = value;
                RaisePropertyChanged("BattleAwards");
            }
        }

        public IEnumerable<TankRowDamage> Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
                RaisePropertyChanged("Damage");
            }
        }

        public IEnumerable<TankRowFrags> Frags
        {
            get { return _frags; }
            set
            {
                _frags = value;
                RaisePropertyChanged("Frags");
            }
        }

        public IEnumerable<TankRowXP> Xp
        {
            get { return _xp; }
            set
            {
                _xp = value;
                RaisePropertyChanged("Xp");
            }
        }

        public IEnumerable<TankRowBattles> Battles
        {
            get { return _battles; }
            set
            {
                _battles = value;
                RaisePropertyChanged("Battles");
            }
        }

        private bool IsExistedtank(TankInfo tankInfo)
        {
            return tankInfo.tankid <= 250 && !tankInfo.icon.Contains("training") && tankInfo.title != "KV" && tankInfo.title != "T23";
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                e.Cancel = !IsCloseAllowed();
            }
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

    public interface IDataPoint
    {
        double X { get; }
        double Y { get; }
    }

    public class DataPoint : IDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}