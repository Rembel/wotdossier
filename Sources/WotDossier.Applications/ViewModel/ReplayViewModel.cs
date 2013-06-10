using System.Collections.Generic;
using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        private Replay _replay;
        private List<object> _combatEffects = new List<object> {1, 2, 3, 4, 5, 6};
        private List<object> _firstTeam = new List<object> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
        private List<object> _secondTeam = new List<object> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        private string _mapName;
        private string _mapDisplayName;
        private TankIcon _tankIcon;

        public Replay Replay
        {
            get { return _replay; }
            set { _replay = value; }
        }

        public List<object> CombatEffects
        {
            get { return _combatEffects; }
            set { _combatEffects = value; }
        }

        public List<object> FirstTeam
        {
            get { return _firstTeam; }
            set { _firstTeam = value; }
        }

        public List<object> SecondTeam
        {
            get { return _secondTeam; }
            set { _secondTeam = value; }
        }

        public TankIcon TankIcon
        {
            get { return _tankIcon; }
            set { _tankIcon = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewModel([Import(typeof(IReplayView))]IReplayView view)
            : base(view)
        {
        }

        public void Show()
        {
            ViewTyped.Show();
        }

        public void Init(Replay replay)
        {
            Replay = replay;
            MapName = replay.datablock_1.mapName;
            MapDisplayName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName, GetMapMode(replay.datablock_1.gameplayID));
            TankIcon = GetTankIcon(replay.datablock_1.playerVehicle);
        }

        private object GetMapMode(string gameplayID)
        {
            if ("ctf".Equals(gameplayID))
            {
                return "Стандартный бой";
            }
            if ("domination".Equals(gameplayID))
            {
                return "Встречный бой";
            }
            return gameplayID;
        }

        private TankIcon GetTankIcon(string playerVehicle)
        {
            string replace = playerVehicle.Replace("-", "_").Replace(" ", "_").Replace(".", "_").ToLower();
            if (WotApiClient.IconsDictionary.ContainsKey(replace))
            {
                return WotApiClient.IconsDictionary[replace];
            }
            return TankIcon.Empty;
        }

        public string MapDisplayName

        {
            get { return _mapDisplayName; }
            set { _mapDisplayName = value; }
        }

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }
    }
}
