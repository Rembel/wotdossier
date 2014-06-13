using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (PlayersCompareViewModel))]
    public class PlayersCompareViewModel : ViewModel<IPlayersCompareView>
    {
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
            using (new WaitCursor())
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(SecondName))
                {
                    PlayerSearchJson first = WotApiClient.Instance.SearchPlayer(FirstName, SettingsReader.Get());
                    PlayerSearchJson second = WotApiClient.Instance.SearchPlayer(SecondName, SettingsReader.Get());

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

                    Player stat1 = WotApiClient.Instance.LoadPlayerStat(first.account_id, SettingsReader.Get(), true);
                    Player stat2 = WotApiClient.Instance.LoadPlayerStat(second.account_id, SettingsReader.Get(), true);

                    if (stat1 != null && stat2 != null)
                    {
                        CompareStatistic = new CompareStatisticViewModelBase<PlayerStatisticViewModel>(GetPlayerViewModel(stat1), GetPlayerViewModel(stat2));
                    }
                }
            }
        }

        private StatisticViewModelBase GetPlayerViewModel(Player stat)
        {
            PlayerStatisticEntity entity = new PlayerStatisticEntity();
            new PlayerStatAdapter(stat).Update(entity);
            PlayerStatisticViewModel statistic = new RandomPlayerStatisticViewModel(entity);
            statistic.Name = stat.dataField.nickname;
            statistic.AccountId = stat.dataField.account_id;
            if (stat.dataField.clan != null && stat.dataField.clan != null)
            {
                //statistic.Clan = new PlayerStatisticClanViewModel(stat.dataField.clan);
            }
            return statistic;
        }
    }
}
