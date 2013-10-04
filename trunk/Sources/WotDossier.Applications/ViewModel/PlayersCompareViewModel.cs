using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Framework.Applications;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (PlayersCompareViewModel))]
    public class PlayersCompareViewModel : ViewModel<IPlayersCompareView>
    {
        private static readonly ILog _log = LogManager.GetLogger("PlayersCompareViewModel");
        private CompareStatisticViewModelBase<PlayerStatisticViewModel> _compareStatistic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public PlayersCompareViewModel([Import(typeof(IPlayersCompareView))]IPlayersCompareView view)
            : base(view)
        {
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }

        public CompareStatisticViewModelBase<PlayerStatisticViewModel> CompareStatistic
        {
            get { return _compareStatistic; }
            set
            {
                _compareStatistic = value;
                RaisePropertyChanged("CompareStatistic");
            }
        }

        public void Init(PlayerStat stat1, PlayerStat stat2)
        {
            CompareStatistic = new CompareStatisticViewModelBase<PlayerStatisticViewModel>(GetPlayerViewModel(stat1), GetPlayerViewModel(stat2));
        }

        private StatisticViewModelBase GetPlayerViewModel(PlayerStat stat)
        {
            PlayerStatisticEntity entity = new PlayerStatisticEntity();
            entity.Update(new PlayerStatAdapter(stat));
            PlayerStatisticViewModel statistic = new PlayerStatisticViewModel(entity);
            statistic.Name = stat.data.name;
            if (stat.data.clan != null && stat.data.clan.clan != null)
            {
                statistic.Clan = new PlayerStatisticClanViewModel(stat.data.clan);
            }
            return statistic;
        }
    }
}
