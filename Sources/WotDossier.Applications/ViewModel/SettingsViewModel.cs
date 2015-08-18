using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Server;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Controls.AutoCompleteTextBox;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : ViewModel<ISettingsView>, ISuggestionProvider
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
        public DelegateCommand SelectCacheFolderCommand { get; set; }

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

        public bool UseIncompleteReplaysResultsForCharts
        {
            get { return _appSettings.UseIncompleteReplaysResultsForCharts; }
            set { _appSettings.UseIncompleteReplaysResultsForCharts = value; }
        }

        private List<ListItem<DossierTheme>> _themes = new List<ListItem<DossierTheme>>
        {
            new ListItem<DossierTheme>(DossierTheme.Black, Resources.Resources.DossierTheme_Black), 
            new ListItem<DossierTheme>(DossierTheme.Silver, Resources.Resources.DossierTheme_Silver)
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
                RaisePropertyChanged("Theme");
            }
        }

        public bool ShowExtendedReplaysData
        {
            get { return AppSettings.ShowExtendedReplaysData; }
            set
            {
                AppSettings.ShowExtendedReplaysData = value;
                RaisePropertyChanged("ShowExtendedReplaysData");
            }
        }

        public string CacheFolderPath
        {
            get { return AppSettings.DossierCachePath; }
            set
            {
                AppSettings.DossierCachePath = value;
                RaisePropertyChanged("CacheFolderPath");
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
            SelectCacheFolderCommand = new DelegateCommand(OnSelectCacheFolder);
        }

        private void OnSelectCacheFolder()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            bool? showDialog = dialog.ShowDialog();
            if (showDialog == true)
            {
                CacheFolderPath = dialog.SelectedPath;
            }
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
                    _appSettings.PlayerId = player.account_id;

                    playerStat = WotApiClient.Instance.LoadPlayerStat(player.account_id, _appSettings, PlayerStatLoadOptions.LoadCommon, new[] { "account_id", "nickname", "created_at" });

                    if (playerStat != null)
                    {
                        double createdAt = playerStat.dataField.created_at;
                        _dossierRepository.GetOrCreatePlayer(player.nickname, player.account_id, Utils.UnixDateToDateTime((long)createdAt), _appSettings.Server);
                    }
                }

                if (playerStat == null)
                {
                    _appSettings.PlayerId = 0;
                    MessageBox.Show(string.Format(Resources.Resources.Msg_GetPlayerData, _appSettings.PlayerName), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            ThemesManager.ApplyTheme(_appSettings.Theme);

            SettingsReader.Save(_appSettings);
            ViewTyped.DialogResult = true;
            ViewTyped.Close();
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }


        /// <summary>
        /// Gets the suggestions.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IEnumerable GetSuggestions(string filter)
        {
            var dossierCacheFolder = CacheFolderPath;
            if (Directory.Exists(dossierCacheFolder))
            {
                IEnumerable<FileInfo> files =
                    Directory.GetFiles(dossierCacheFolder, "*.dat").Select(x => new FileInfo(x));
                IEnumerable<string> suggestions =
                    files.Select(CacheFileHelper.GetPlayerName)
                        .Distinct()
                        .Where(x => x.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(filter)).OrderBy(x => x);
                return suggestions;
            }
            return new string[0];
        }
    }
}
