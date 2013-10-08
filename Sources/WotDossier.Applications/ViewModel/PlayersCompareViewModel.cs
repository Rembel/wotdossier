using System.ComponentModel.Composition;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (PlayersCompareViewModel))]
    public class PlayersCompareViewModel : ViewModel<IPlayersCompareView>
    {
        private static readonly ILog _log = LogManager.GetLogger("PlayersCompareViewModel");
        private CompareStatisticViewModelBase<PlayerStatisticViewModel> _compareStatistic;
        private string _firstName;
        private string _secondName;

        public CompareStatisticViewModelBase<PlayerStatisticViewModel> CompareStatistic
        {
            get { return _compareStatistic; }
            set
            {
                _compareStatistic = value;
                RaisePropertyChanged("CompareStatistic");
            }
        }

        public DelegateCommand CompareCommand { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        public string SecondName
        {
            get { return _secondName; }
            set
            {
                _secondName = value;
                RaisePropertyChanged("SecondName");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public PlayersCompareViewModel([Import(typeof(IPlayersCompareView))]IPlayersCompareView view)
            : base(view)
        {
            CompareCommand = new DelegateCommand(OnCompare);
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }

        private void OnCompare()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(SecondName))
            {
                PlayerSearchJson first = WotApiClient.Instance.SearchPlayer(SettingsReader.Get(), FirstName);
                PlayerSearchJson second = WotApiClient.Instance.SearchPlayer(SettingsReader.Get(), SecondName);

                if (first == null)
                {
                    MessageBox.Show(string.Format(Resources.Resources.Msg_CantFindPlayerData, FirstName), Resources.Resources.WindowCaption_Warning, MessageBoxButton.OK);
                    return;
                }

                if (second == null)
                {
                    MessageBox.Show(string.Format(Resources.Resources.Msg_CantFindPlayerData, SecondName), Resources.Resources.WindowCaption_Warning, MessageBoxButton.OK);
                    return;
                }

                PlayerStat stat1 = WotApiClient.Instance.LoadPlayerStat(SettingsReader.Get(), first.id);
                PlayerStat stat2 = WotApiClient.Instance.LoadPlayerStat(SettingsReader.Get(), second.id);

                CompareStatistic = new CompareStatisticViewModelBase<PlayerStatisticViewModel>(GetPlayerViewModel(stat1), GetPlayerViewModel(stat2));
            }
        }

        private StatisticViewModelBase GetPlayerViewModel(PlayerStat stat)
        {
            PlayerStatisticEntity entity = new PlayerStatisticEntity();
            entity.Update(new PlayerStatAdapter(stat));
            PlayerStatisticViewModel statistic = new PlayerStatisticViewModel(entity);
            statistic.Name = stat.dataField.nickname;
            if (stat.dataField.clan != null && stat.dataField.clan != null)
            {
                //statistic.Clan = new PlayerStatisticClanViewModel(stat.dataField.clan);
            }
            return statistic;
        }
    }
}
