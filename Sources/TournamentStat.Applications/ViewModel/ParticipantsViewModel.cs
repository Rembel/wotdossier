using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TournamentStat.Applications.Annotations;
using WotDossier.Framework;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class ParticipantsViewModel : INotifyPropertyChanged
    {
        private List<TournamentPlayer> _players;

        public List<TournamentPlayer> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public ICommand RowDoubleClickCommand { get; set; }

        public ParticipantsViewModel(List<TournamentPlayer> players)
        {
            Players = players;
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
        }

        private void OnRowDoubleClick(object obj)
        {
            var row = ((TournamentPlayer)obj);
            var viewModel = CompositionContainerFactory.Instance.GetExport<PlayerDataViewModel>();
            viewModel.Player = row;
            viewModel.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}