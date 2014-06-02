using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Server;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : ViewModel<ISettingsView>
    {
        private readonly DossierRepository _dossierRepository;
        private readonly AppSettings _appSettings;
        private List<string> _servers;
        private List<ListItem<string>> _languages = new List<ListItem<string>>
        {
            new ListItem<string>("ru-RU", Resources.Resources.Language_Russian),
            new ListItem<string>("en-US", Resources.Resources.Language_English),
        };
        private bool _nameChanged;
        public DelegateCommand SaveCommand { get; set; }

        public AppSettings AppSettings
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

        public string PlayerName
        {
            get { return AppSettings.PlayerName; }
            set
            {
                AppSettings.PlayerName = value;
                _nameChanged = true;
            }
        }

        public bool CheckForUpdates
        {
            get { return AppSettings.CheckForUpdates; }
            set { AppSettings.CheckForUpdates = value; }
        }

        public bool AutoLoadStatistic
        {
            get { return _appSettings.AutoLoadStatistic; }
            set { _appSettings.AutoLoadStatistic = value; }
        }

        public string ReplaysFolderPath
        {
            get { return AppSettings.ReplaysFolderPath; }
            set
            {
                AppSettings.ReplaysFolderPath = value;
                RaisePropertyChanged("ReplaysFolderPath");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dossierRepository">The dossier repository.</param>
        [ImportingConstructor]
        public SettingsViewModel([Import(typeof(ISettingsView))]ISettingsView view, [Import]DossierRepository dossierRepository)
            : base(view)
        {
            _dossierRepository = dossierRepository;
            SaveCommand = new DelegateCommand(OnSave);
            _appSettings = SettingsReader.Get();
            Servers = Dictionaries.Instance.GameServers.Keys.ToList();
        }

        private void OnSave()
        {
            if (_nameChanged)
            {
                if (string.IsNullOrEmpty(_appSettings.Server))
                {
                    MessageBox.Show(Resources.Resources.Msg_ServerNotSelected, Resources.Resources.WindowCaption_Warning, MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

                PlayerSearchJson player = WotApiClient.Instance.SearchPlayer(_appSettings.PlayerName, _appSettings);

                Player playerStat = null;

                if (player != null)
                {
                    _appSettings.PlayerId = player.id;
                    
                    playerStat = WotApiClient.Instance.LoadPlayerStat(player.id, _appSettings, false);

                    if (playerStat != null)
                    {
                        double createdAt = playerStat.dataField.created_at;
                        _dossierRepository.GetOrCreatePlayer(player.nickname, player.id, Utils.UnixDateToDateTime((long) createdAt));
                    }
                }

                if (playerStat == null)
                {
                    _appSettings.PlayerId = 0;
                    MessageBox.Show(string.Format(Resources.Resources.Msg_GetPlayerData, _appSettings.PlayerName), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            SettingsReader.Save(_appSettings);
            ViewTyped.Close();
        }

        public virtual void Show()
        {
            ViewTyped.Show();
        }
    }
}
