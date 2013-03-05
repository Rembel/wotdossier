using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WotDossier.Applications;
using WotDossier.Domain;
using WotDossier.Domain.Rows;
using Path = System.IO.Path;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _curDirTemp;
        private FileInfo _last = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] files = Directory.GetFiles(appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache", "*.dat");

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

                Read read = new Read();

                List<Tank> tanks = read.Start(_last.FullName.Replace(".dat", ".json"));

                IEnumerable<TankRowBattles> battles = tanks.Select(x => new TankRowBattles(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowXP> xp = tanks.Select(x => new TankRowXP(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowFrags> frags = tanks.Select(x => new TankRowFrags(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowDamage> damage = tanks.Select(x => new TankRowDamage(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowBattleAwards> battleAwards = tanks.Select(x => new TankRowBattleAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowSpecialAwards> specialAwards = tanks.Select(x => new TankRowSpecialAwards(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowSeries> series = tanks.Select(x => new TankRowSeries(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowMedals> medals = tanks.Select(x => new TankRowMedals(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);
                IEnumerable<TankRowRatings> ratings = tanks.Select(x => new TankRowRatings(x)).OrderByDescending(x => x.Tier).ThenBy(x => x.Tank);

                dgBattles.DataContext = battles;
                dgXP.DataContext = xp;
                dgFrags.DataContext = frags;
                dgDamage.DataContext = damage;
                dgBattleAwards.DataContext = battleAwards;
                dgSpecialAwards.DataContext = specialAwards;
                dgSeries.DataContext = series;
                dgMedals.DataContext = medals;
                dgRatings.DataContext = ratings;
            };

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(act);
        }
    }
}
