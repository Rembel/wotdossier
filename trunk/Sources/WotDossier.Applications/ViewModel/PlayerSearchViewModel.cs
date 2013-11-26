﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
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
    [Export(typeof (PlayerSearchViewModel))]
    public class PlayerSearchViewModel : ViewModel<ISearchView>
    {
        private static readonly ILog _log = LogManager.GetLogger("PlayerSearchViewModel");
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
            Mouse.SetCursor(Cursors.Wait);
            List<PlayerSearchJson> player = WotApiClient.Instance.SearchPlayer(SettingsReader.Get(), SearchText, 10);
            if (player != null)
            {
                List = player.Select(x => new SearchResultRowViewModel { Id = x.id, Name = x.nickname }).ToList();
            }
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void OnRowDoubleClick(object item)
        {
            SearchResultRowViewModel row = item as SearchResultRowViewModel;
            if (row != null)
            {
                Mouse.SetCursor(Cursors.Wait);
                PlayerStat playerStat = WotApiClient.Instance.LoadPlayerStat(SettingsReader.Get(), row.Id);
                Mouse.SetCursor(Cursors.Arrow);
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
    }
}