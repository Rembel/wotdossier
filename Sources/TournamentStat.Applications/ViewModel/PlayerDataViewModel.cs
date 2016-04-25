using System.Linq;
using TournamentStat.Applications.View;
using WotDossier.Dal;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class PlayerDataViewModel : ViewModel<IPlayerDataWindow>
    {
        public DelegateCommand OkCommand { get; set; }

        public TournamentPlayer Player { get; set; }

        public string TwitchUrl
        {
            get { return Player.TwitchUrl; }
            set { Player.TwitchUrl = value; }
        }

        public string Mods
        {
            get { return Player.Mods; }
            set { Player.Mods = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public PlayerDataViewModel(IPlayerDataWindow view)
            : base(view)
        {
            OkCommand = new DelegateCommand(OnSave);
        }

        private void OnSave()
        {
            TournamentStatSettings settings = SettingsReader.Get<TournamentStatSettings>();

            var player = settings.Players.First(x => x.PlayerId == Player.PlayerId);

            player.TwitchUrl = Player.TwitchUrl ?? player.TwitchUrl;
            player.Mods = Player.Mods ?? player.Mods;

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
