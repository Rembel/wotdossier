using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.Model;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain.Player;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ClanViewModel))]
    public class ClanViewModel : ViewModel<IClanView>
    {
        private static readonly ILog _log = LogManager.GetLogger("ClanViewModel");

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ClanViewModel([Import(typeof(IClanView))]IClanView view)
            : base(view)
        {
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClickCommand);
        }

        private void OnRowDoubleClickCommand(object item)
        {
            ClanMemberModel member = item as ClanMemberModel;
            if (member != null)
            {
                PlayerStat playerStat = WotApiClient.Instance.LoadPlayerStat(SettingsReader.Get(), member.Id);
                if (playerStat != null)
                {
                    PlayerServerStatisticViewModel viewModel = CompositionContainerFactory.Instance.Container.GetExport<PlayerServerStatisticViewModel>().Value;
                    viewModel.Init(playerStat);
                    viewModel.Show();
                }
            }
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }

        public void Init(ClanData clan)
        {
            Clan = new ClanModel(clan);
        }

        public ClanModel Clan { get; set; }
    }
}
