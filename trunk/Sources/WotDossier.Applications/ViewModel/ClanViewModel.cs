using System.ComponentModel.Composition;
using Common.Logging;
using WotDossier.Applications.Model;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Server;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ClanViewModel))]
    public class ClanViewModel : ViewModel<IClanView>
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

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
                Player player;
                using (new WaitCursor())
                {
                    player = WotApiClient.Instance.LoadPlayerStat(member.Id, SettingsReader.Get());
                }
                if (player != null)
                {
                    PlayerServerStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<PlayerServerStatisticViewModel>();
                    viewModel.Init(player);
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
