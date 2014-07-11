using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Server;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (PlayerSearchViewModel))]
    public class PlayerSearchViewModel : ViewModel<ISearchView>
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();
        private List<SearchResultRowViewModel> _list;

        public DelegateCommand<object> RowDoubleClickCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }

        public List<SearchResultRowViewModel> List
        {
            get { return _list; }
            set
            {
                _list = value;
                RaisePropertyChanged("List");
            }
        }

        public string SearchText { get; set; }

        public string Title
        {
            get
            {
                return Resources.Resources.WindowCaption_SearchPlayer;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public PlayerSearchViewModel([Import(typeof(ISearchView))]ISearchView view)
            : base(view)
        {
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            SearchCommand = new DelegateCommand(OnSearch);
        }

        private void OnSearch()
        {
            using (new WaitCursor())
            {
                List<PlayerSearchJson> player = WotApiClient.Instance.SearchPlayer(SearchText, 10, SettingsReader.Get());
                if (player != null)
                {
                    List = player.Select(x => new SearchResultRowViewModel {Id = x.account_id, Name = x.nickname}).ToList();
                }
            }
        }

        private void OnRowDoubleClick(object item)
        {
            SearchResultRowViewModel row = item as SearchResultRowViewModel;
            if (row != null)
            {
                Player player;
                using (new WaitCursor())
                {
                    player = WotApiClient.Instance.LoadPlayerStat(row.Id, SettingsReader.Get(), PlayerStatLoadOptions.LoadVehicles | PlayerStatLoadOptions.LoadAchievments);
                }
                if (player != null)
                {
                    PlayerServerStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<PlayerServerStatisticViewModel>();
                    viewModel.Init(player);
                    viewModel.Show();
                }
                else
                {
                    MessageBox.Show(string.Format(Resources.Resources.Msg_GetPlayerData, row.Name), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
