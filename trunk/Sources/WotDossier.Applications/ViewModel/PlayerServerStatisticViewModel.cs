using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.Model;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Player;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (PlayerServerStatisticViewModel))]
    public class PlayerServerStatisticViewModel : ViewModel<IPlayerServerStatisticView>
    {
        private static readonly string PropClan = TypeHelper.GetPropertyName<PlayerServerStatisticViewModel>(x => x.Clan);

        public DelegateCommand<object> OpenClanCommand { get; set; }

        private PlayerStatisticViewModel _playerStatistic;
        /// <summary>
        /// Gets or sets the player statistic.
        /// </summary>
        /// <value>
        /// The player statistic.
        /// </value>
        public PlayerStatisticViewModel PlayerStatistic
        {
            get { return _playerStatistic; }
            set
            {
                _playerStatistic = value;
                RaisePropertyChanged("PlayerStatistic");
            }
        }

        private ClanModel _clan;
        private List<TankStatisticRowViewModel> _tanks;

        /// <summary>
        /// Gets or sets the clan.
        /// </summary>
        /// <value>
        /// The clan.
        /// </value>
        public ClanModel Clan
        {
            get { return _clan; }
            set
            {
                _clan = value;
                RaisePropertyChanged(PropClan);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public PlayerServerStatisticViewModel([Import(typeof(IPlayerServerStatisticView))]IPlayerServerStatisticView view)
            : base(view)
        {
            OpenClanCommand = new DelegateCommand<object>(OnOpenClanCommand);
        }

        private void OnOpenClanCommand(object param)
        {
            ClanData clan;
            using (new WaitCursor())
            {
                clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), Clan.Id);
            }
            if (clan != null)
            {
                ClanViewModel viewModel = CompositionContainerFactory.Instance.GetExport<ClanViewModel>();
                viewModel.Init(clan);
                viewModel.Show();
            }
            else
            {
                MessageBox.Show(Resources.Resources.Msg_CantGetClanDataFromServer, Resources.Resources.WindowCaption_Information,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }

        public void Init(PlayerStat playerStat)
        {
            PlayerStatisticEntity entity = new PlayerStatisticEntity();
            PlayerStatAdapter statAdapter = new PlayerStatAdapter(playerStat);
            statAdapter.Update(entity);
            PlayerStatisticViewModel statistic = new RandomPlayerStatisticViewModel(entity);
            statistic.Name = playerStat.dataField.nickname;
            statistic.AccountId = playerStat.dataField.account_id;
            statistic.BattlesPerDay = statistic.BattlesCount / (DateTime.Now - statAdapter.Created).Days;
            statistic.Created = Utils.UnixDateToDateTime( (long) playerStat.dataField.created_at);
            if (playerStat.dataField.clan != null && playerStat.dataField.clan != null && playerStat.dataField.clanData != null)
            {
                Clan = new ClanModel(playerStat.dataField.clanData, playerStat.dataField.clan.role, playerStat.dataField.clan.since);
            }

            PlayerStatistic = statistic;

            Tanks = playerStat.dataField.vehicles.Select(x => new TankStatisticRowViewModel(TankJsonV2Converter.Convert(x))).OrderByDescending(x => x.Tier).ToList();
        }

        public List<TankStatisticRowViewModel> Tanks
        {
            get { return _tanks; }
            set
            {
                _tanks = value;
                RaisePropertyChanged("Tanks");
            }
        }
    }
}
