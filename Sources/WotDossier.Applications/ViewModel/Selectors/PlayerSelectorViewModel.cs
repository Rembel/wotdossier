using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Dal;

namespace WotDossier.Applications.ViewModel.Selectors
{
    public class PlayerSelectorViewModel : Framework.Foundation.Model
    {
        private readonly DossierRepository _repository;
        private readonly Action _onSelectionChanged;
        private List<PlayerListItem> _players;

        public List<PlayerListItem> Players
        {
            get { return _players; }
            set { _players = value; }
        }

        private int _player;
        public int Player
        {
            get { return _player; }
            set
            {
                _player = value;
                var appSettings = SettingsReader.Get();
                appSettings.PlayerId = value;
                PlayerListItem listItem = Players.First(x => x.Id == value);
                appSettings.PlayerName = listItem.Value;
                appSettings.Server = listItem.Server;
                SettingsReader.Save(appSettings);
                _onSelectionChanged();
                RaisePropertyChanged("Player");
            }
        }

        public PlayerSelectorViewModel(DossierRepository repository, Action onSelectionChanged)
        {
            _repository = repository;
            _onSelectionChanged = onSelectionChanged;
            InitPlayers();
        }

        public void InitPlayers()
        {
            Players = _repository.GetPlayers().Select(x => new PlayerListItem(x.AccountId, x.Name, x.Server)).ToList();

            var appSettings = SettingsReader.Get();
            
            if (appSettings.PlayerId > 0 && Players.FirstOrDefault(x => x.Id == appSettings.PlayerId) == null)
            {
                Players.Add(new PlayerListItem(appSettings.PlayerId, appSettings.PlayerName, appSettings.Server));
            }
            
            _player = appSettings.PlayerId;

            RaisePropertyChanged("Players");
            RaisePropertyChanged("Player");
        }
    }

    public class PlayerListItem : ListItem<int>
    {
        public string Server { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerListItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="server"></param>
        public PlayerListItem(int id, string value, string server) : base(id, value)
        {
            Server = server;
        }
    }
}