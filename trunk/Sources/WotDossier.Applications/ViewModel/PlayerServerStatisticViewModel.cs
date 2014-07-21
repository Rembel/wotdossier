using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using WotDossier.Applications.Logic.Adapter;
using WotDossier.Applications.Model;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Server;
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
        private List<ITankStatisticRow> _tanks;

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

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }

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
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
        }

        private void OnRowDoubleClick(object rowData)
        {
            ITankStatisticRow tankStatisticRowViewModel = rowData as ITankStatisticRow;

            //NRE if row of type TotalTankStatisticRowViewModel
            if (tankStatisticRowViewModel != null && !(tankStatisticRowViewModel is TotalTankStatisticRowViewModel))
            {
                var export = CompositionContainerFactory.Instance.GetExport<TankStatisticViewModel>();
                if (export != null)
                {
                    TankStatisticViewModel viewModel = export;
                    viewModel.TankStatistic = tankStatisticRowViewModel;
                    viewModel.Show();
                }
            }
        }

        private void OnOpenClanCommand(object param)
        {
            ClanData clan;
            using (new WaitCursor())
            {
                clan = WotApiClient.Instance.LoadClan(Clan.Id, SettingsReader.Get());
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

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public void Show()
        {
            ViewTyped.ShowDialog();
        }

        public void Init(Player player)
        {
            PlayerStatisticEntity entity = new PlayerStatisticEntity();
            RandomBattlesStatAdapter statAdapter = new RandomBattlesStatAdapter(player);
            statAdapter.Update(entity);
            PlayerStatisticViewModel statistic = new RandomPlayerStatisticViewModel(entity);
            statistic.Name = player.dataField.nickname;
            statistic.AccountId = player.dataField.account_id;
            statistic.BattlesPerDay = statistic.BattlesCount / (DateTime.Now - statAdapter.Created).Days;
            statistic.Created = Utils.UnixDateToDateTime( (long) player.dataField.created_at);
            if (player.dataField.clan_id != null)
            {
                AppSettings settings = SettingsReader.Get();
                ClanMemberInfo clanMember = WotApiClient.Instance.GetClanMemberInfo(player.dataField.account_id, settings);
                if (clanMember != null)
                {
                    Clan = new ClanModel(clanMember);
                }
            }

            Tanks = statAdapter.Tanks;

            PlayerStatistic = statistic;
        }

        public List<ITankStatisticRow> Tanks
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
