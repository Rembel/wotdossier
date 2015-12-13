using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentStat.Applications.Annotations;
using TournamentStat.Applications.Logic;
using WotDossier.Applications.ViewModel.Rows;

namespace TournamentStat.Applications.ViewModel
{
    public class TournamentTankResultsViewModel : INotifyPropertyChanged
    {
        private readonly List<ITankStatisticRow> _statisticRows;
        private TournamentNomination _selectedTank;
        public List<TournamentNomination> TournamentNominations { get; set; }

        public TournamentNomination SelectedNomination
        {
            get { return _selectedTank ?? TournamentNominations.FirstOrDefault(); }
            set
            {
                if (Equals(value, _selectedTank)) return;
                _selectedTank = value;
                OnPropertyChanged(nameof(TankResult));
            }
        }

        public List<ITankStatisticRow> TankResult
        {
            get
            {
                if (SelectedNomination != null)
                {
                    return NominationHelper.GetNominationResults(SelectedNomination, _statisticRows);
                }
                return null;
            }
        }

        public TournamentTankResultsViewModel()
        {
        }

        public TournamentTankResultsViewModel(List<TournamentNomination> nominations, List<ITankStatisticRow> statisticRows)
        {
            _statisticRows = statisticRows;
            TournamentNominations = nominations;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}