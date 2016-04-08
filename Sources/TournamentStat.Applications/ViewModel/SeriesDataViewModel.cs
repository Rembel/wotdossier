using System.Linq;
using TournamentStat.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class SeriesDataViewModel : ViewModel<ISeriesDataWindow>
    {
        public DelegateCommand OkCommand { get; set; }

        public TankStatisticRowViewModelBase Series { get; set; }

        public string Dossier
        {
            get { return Series.Dossier; }
            set { Series.Dossier = value; }
        }

        public string ReplaysUrlOwner
        {
            get { return Series.ReplaysUrlOwner; }
            set { Series.ReplaysUrlOwner = value; }
        }

        public string ReplaysUrl
        {
            get { return Series.ReplaysUrl; }
            set { Series.ReplaysUrl = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public SeriesDataViewModel(ISeriesDataWindow view)
            : base(view)
        {
            OkCommand = new DelegateCommand(OnSave);
        }

        private void OnSave()
        {
            TournamentStatSettings settings = SettingsReader.Get<TournamentStatSettings>();

            var player = settings.Players.First(x => x.PlayerId == Series.PlayerId);

            var tournamentSerie = player.Tanks.FirstOrDefault(x => x.TankUniqueId == Series.TankUniqueId && x.BattlesCount == Series.BattlesCount);

            if (tournamentSerie == null)
            {
                tournamentSerie = new TournamentTank(Series.Description);
                player.Tanks.Add(tournamentSerie);
            }

            tournamentSerie.BattlesCount = Series.BattlesCount;
            tournamentSerie.Dossier = Series.Dossier;
            tournamentSerie.ReplaysUrl = Series.ReplaysUrl;
            tournamentSerie.ReplaysUrlOwner = Series.ReplaysUrlOwner;

            SettingsReader.Save(settings);

            ViewTyped.DialogResult = true;
            ViewTyped.Close();
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }
    }
}
