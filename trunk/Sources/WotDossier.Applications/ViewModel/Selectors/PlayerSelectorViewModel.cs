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
        private List<ListItem<int>> _players;

        public List<ListItem<int>> Players
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
                appSettings.PlayerName = Players.First(x => x.Id == value).Value;
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
            Players = _repository.GetPlayers().Select(x => new ListItem<int>(x.PlayerId, x.Name)).ToList();
            _player = SettingsReader.Get().PlayerId;
        }
    }
}