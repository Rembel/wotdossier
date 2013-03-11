using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using WotDossier.Applications;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Domain;
using WotDossier.Domain.Rows;
using Common.Logging;
using Path = System.IO.Path;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShellView
    {
        private string _curDirTemp;
        private FileInfo _last = null;
        SettingsReader _reader = new SettingsReader(WotDossierSettings.SettingsPath);

        protected static readonly ILog _log = LogManager.GetLogger("log");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
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

            if (files.Count() == 0)
            {
                return;
            }

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if(_last == null)
                {
                    _last = info;
                }
                else if(_last.LastWriteTime < info.LastWriteTime)
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

        private void ProcOnExited(object sender, EventArgs eventArgs)
        {
            Action act = () =>
            {
                Environment.CurrentDirectory = _curDirTemp;

                tabCommon.DataContext = Read.LoadPlayerStat(_reader.Read());

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

                IEnumerable<KeyValuePair<int, int>> killed = tanks.SelectMany(x => x.Frags).Select(x => new KeyValuePair<int, int>(x.TankId, x.CountryId)).Distinct();
                IEnumerable<TankRowMasterTanker> masterTanker = Read.TankDictionary.Where(x => !killed.Contains(x.Key) && IsExistedtank(x.Value)).Select(x => new TankRowMasterTanker(x.Value, Read.GetTankContour(x.Value))).OrderBy(x => x.IsPremium).ThenBy(x => x.Tier);

                dgBattles.DataContext = battles;
                dgXP.DataContext = xp;
                dgFrags.DataContext = frags;
                dgDamage.DataContext = damage;
                dgBattleAwards.DataContext = battleAwards;
                dgSpecialAwards.DataContext = specialAwards;
                dgSeries.DataContext = series;
                dgMedals.DataContext = medals;
                dgRatings.DataContext = ratings;
                dgPerformance.DataContext = performance;
                dgMasterTanker.DataContext = masterTanker;
                dgEpics.DataContext = epic;
            };

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }

        private bool IsExistedtank(TankInfo tankInfo)
        {
            return tankInfo.tankid <= 250 && !tankInfo.icon.Contains("training") && tankInfo.title != "KV" && tankInfo.title != "T23";
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel model = new SettingsViewModel(new Settings());
            model.Show();
        }
    }
}
