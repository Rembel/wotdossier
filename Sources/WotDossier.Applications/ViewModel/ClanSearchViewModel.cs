using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Common.Logging;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Player;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ClanSearchViewModel))]
    public class ClanSearchViewModel : ViewModel<ISearchView>
    {
        private static readonly ILog _log = LogManager.GetLogger("ClanSearchViewModel");
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
                return Resources.Resources.WindowCaption_SearchClan;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ClanSearchViewModel([Import(typeof(ISearchView))]ISearchView view)
            : base(view)
        {
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            SearchCommand = new DelegateCommand(OnSearch);
        }

        private void OnSearch()
        {
            List<ClanSearchJson> clans = WotApiClient.Instance.SearchClan(SettingsReader.Get(), SearchText, 10);
            if (clans != null)
            {
                List = clans.Select(x => new SearchResultRowViewModel {Id = x.clan_id, Name = x.name}).ToList();
            }
        }

        private void OnRowDoubleClick(object item)
        {
            SearchResultRowViewModel row = item as SearchResultRowViewModel;
            if (row != null)
            {
                ClanData clan = WotApiClient.Instance.LoadClan(SettingsReader.Get(), row.Id);
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
        }

        public void Show()
        {
            ViewTyped.ShowDialog();
        }
    }
}
