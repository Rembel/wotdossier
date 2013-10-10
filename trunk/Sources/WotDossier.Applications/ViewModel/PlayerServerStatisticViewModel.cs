﻿using System.ComponentModel.Composition;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.Model;
using WotDossier.Applications.View;
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
        private static readonly ILog _log = LogManager.GetLogger("PlayerServerStatisticViewModel");

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
            ClanData clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), Clan.Id);
            if (clan != null)
            {
                ClanViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<ClanViewModel>().Value;
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
            entity.Update(new PlayerStatAdapter(playerStat));
            PlayerStatisticViewModel statistic = new PlayerStatisticViewModel(entity);
            statistic.Name = playerStat.dataField.nickname;
            if (playerStat.dataField.clan != null && playerStat.dataField.clan != null)
            {
                Clan = new ClanModel(playerStat.dataField.clanData, playerStat.dataField.clan.role, playerStat.dataField.clan.since);
            }

            PlayerStatistic = statistic;
        }
    }
}
