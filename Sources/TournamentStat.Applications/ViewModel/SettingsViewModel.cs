using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TournamentStat.Applications.View;
using WotDossier.Applications;
using WotDossier.Applications.ViewModel;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private readonly TournamentStatSettings _appSettings;
        private List<string> _servers;
        private List<ListItem<string>> _languages = new List<ListItem<string>>
        {
            new ListItem<string>("ru-RU", WotDossier.Resources.Resources.Language_Russian),
            new ListItem<string>("en-US", WotDossier.Resources.Resources.Language_English),
        };
        public DelegateCommand SaveCommand { get; set; }
        public ICommand AddNominationCommand { get; set; }
        public ICommand EditNominationCommand { get; set; }
        public ICommand DeleteNominationCommand { get; set; }

        public TournamentStatSettings AppSettings
        {
            get { return _appSettings; }
        }

        public List<string> Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }

        public List<ListItem<string>> Languages
        {
            get { return _languages; }
            set { _languages = value; }
        }

        private List<ListItem<DossierTheme>> _themes = new List<ListItem<DossierTheme>>
        {
            new ListItem<DossierTheme>(DossierTheme.Black, WotDossier.Resources.Resources.DossierTheme_Black), 
            new ListItem<DossierTheme>(DossierTheme.Silver, WotDossier.Resources.Resources.DossierTheme_Silver)
        };

        public List<ListItem<DossierTheme>> Themes
        {
            get { return _themes; }
            set { _themes = value; }
        }

        public DossierTheme Theme
        {
            get { return AppSettings.Theme; }
            set
            {
                AppSettings.Theme = value;
                RaisePropertyChanged(nameof(Theme));
            }
        }

        public string TournamentName
        {
            get { return _appSettings.TournamentName; }
            set
            {
                _appSettings.TournamentName = value;
                RaisePropertyChanged(nameof(TournamentName));
            }
        }

        public ObservableCollection<TournamentNomination> TournamentNominations
        {
            get { return new ObservableCollection<TournamentNomination>(_appSettings.TournamentNominations); }
        }

        public TournamentNomination SelectedNomination { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public SettingsViewModel(ISettingsView view)
            : base(view)
        {
            SaveCommand = new DelegateCommand(OnSave);
            AddNominationCommand = new DelegateCommand(OnAddNomination);
            EditNominationCommand = new DelegateCommand<object>(OnEditNomination, CanEditNomination);
            DeleteNominationCommand = new DelegateCommand<object>(OnDeleteNomination, CanDeleteNomination);


            _appSettings = SettingsReader.Get<TournamentStatSettings>();
            Servers = Dictionaries.Instance.GameServers.Keys.ToList();

            if (_appSettings.TournamentNominations == null)
            {
                _appSettings.TournamentNominations = new List<TournamentNomination>();
            }
        }

        private void OnEditNomination(object item)
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<TournamentNominationViewModel>();
            viewModel.TournamentNomination = (TournamentNomination) item;
            if (viewModel.Show() == true)
            {
                RaisePropertyChanged(nameof(TournamentNominations));
            }
        }

        private void OnDeleteNomination(object item)
        {
            _appSettings.TournamentNominations.Remove((TournamentNomination)item);
            RaisePropertyChanged(nameof(TournamentNominations));
        }

        private void OnAddNomination()
        {
            var viewModel = CompositionContainerFactory.Instance.GetExport<TournamentNominationViewModel>();
            if (viewModel.Show() == true)
            {
                _appSettings.TournamentNominations.Add(viewModel.TournamentNomination);
                RaisePropertyChanged(nameof(TournamentNominations));
            }
        }

        private bool CanEditNomination(object o)
        {
            return o != null;
        }

        private bool CanDeleteNomination(object o)
        {
            return o != null;
        }

        private void OnSave()
        {
            ThemesManager.ApplyTheme(_appSettings.Theme);

            SettingsReader.Save(_appSettings);
            ViewTyped.DialogResult = true;
            ViewTyped.Close();
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }
    }
}
