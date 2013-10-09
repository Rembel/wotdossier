using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Common.Logging;
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

        private static readonly string PropFullName = TypeHelper.GetPropertyName<ClanViewModel>(x => x.FullName);

        private string _fullName;

        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                RaisePropertyChanged(PropFullName);
            }
        }

        private static readonly string PropCreated = TypeHelper.GetPropertyName<ClanViewModel>(x => x.Created);

        private DateTime _created;

        /// <summary>
        /// 
        /// </summary>
        public DateTime Created
        {
            get { return _created; }
            set
            {
                _created = value;
                RaisePropertyChanged(PropCreated);
            }
        }

        private static readonly string PropMotto = TypeHelper.GetPropertyName<ClanViewModel>(x => x.Motto);

        private string _motto;

        /// <summary>
        /// 
        /// </summary>
        public string Motto
        {
            get { return _motto; }
            set
            {
                _motto = value;
                RaisePropertyChanged(PropMotto);
            }
        }

        private static readonly string PropMembers = TypeHelper.GetPropertyName<ClanViewModel>(x => x.Members);

        private List<ClanMemberViewModel> _members;

        /// <summary>
        /// 
        /// </summary>
        public List<ClanMemberViewModel> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                RaisePropertyChanged(PropMembers);
            }
        }

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
            ClanMemberViewModel member = item as ClanMemberViewModel;
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
            FullName = string.Format("[{0}] {1}", clan.abbreviation, clan.name);
            Created = Utils.UnixDateToDateTime(clan.created_at);
            Motto = clan.motto;
            Members = clan.members.Values.Select(x => new ClanMemberViewModel(x)).OrderBy(x => x.Name).ToList();
            Clan = clan;
        }

        public ClanData Clan { get; set; }
    }
}
