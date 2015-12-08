using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentStat.Applications.Annotations;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Applications.ViewModel.Statistic;

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
                    var nominationTanks = SelectedNomination.TournamentTanks.Select(x => x.TankUniqueId).ToList();
                    var tankStatisticRows = _statisticRows.Where(x => nominationTanks.Contains(x.TankUniqueId));
                    if (SelectedNomination.Criterion == TournamentCriterion.Damage)
                    {
                        return
                            tankStatisticRows.OrderByDescending(
                                x => ((StatisticViewModelBase) x).AvgDamageDealtForPeriod).ToList();
                    }
                    if (SelectedNomination.Criterion == TournamentCriterion.DamageWithAssist)
                    {
                        return
                            tankStatisticRows.OrderByDescending(
                                x =>
                                    ((TankStatisticRowViewModelBase) x).AvgDamageAssistedForPeriod +
                                    ((TankStatisticRowViewModelBase) x).AvgDamageDealtForPeriod).ToList();
                    }
                    if (SelectedNomination.Criterion == TournamentCriterion.WinPercent)
                    {
                        return
                            tankStatisticRows.OrderByDescending(
                                x => ((TankStatisticRowViewModelBase) x).WinsPercentForPeriod).ToList();
                    }
                    if (SelectedNomination.Criterion == TournamentCriterion.Frags)
                    {
                        return
                            tankStatisticRows.OrderByDescending(
                                x => ((TankStatisticRowViewModelBase) x).AvgFragsForPeriod).ToList();
                    }
                    if (SelectedNomination.Criterion == TournamentCriterion.DamageWithArmor)
                    {
                        return
                            tankStatisticRows.OrderByDescending(
                                x =>
                                    ((TankStatisticRowViewModelBase) x).AvgDamageDealtForPeriod +
                                    ((TankStatisticRowViewModelBase) x).AvgPotentialDamageReceivedForPeriod).ToList();
                    }

                    return _statisticRows;
                }
                return null;
            }
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