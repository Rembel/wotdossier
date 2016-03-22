using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TournamentStat.Applications.Annotations;
using WotDossier.Applications.ViewModel;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Framework;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class TournamentStatViewModel : INotifyPropertyChanged
    {
        private List<ITankStatisticRow> _series;
        private TournamentTankResultsViewModel _tournamentTankResults;

        public List<ITankStatisticRow> Series
        {
            get { return _series; }
            set
            {
                if (Equals(value, _series)) return;
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }

        public ICommand RowDoubleClickCommand { get; set; }

        public TournamentTankResultsViewModel TournamentTankResults
        {
            get { return _tournamentTankResults; }
            set
            {
                if (Equals(value, _tournamentTankResults)) return;
                _tournamentTankResults = value;
                OnPropertyChanged(nameof(TournamentTankResults));
            }
        }

        public ICommand DeleteSeriesCommand { get; set; }
        public ICommand AddSeriesDataCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TournamentStatViewModel()
        {
            RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);
            DeleteSeriesCommand = new DelegateCommand<object>(OnDelete);
            AddSeriesDataCommand = new DelegateCommand<object>(OnAddSeriesData);
        }

        private void OnAddSeriesData(object obj)
        {
            var row = ((TankStatisticRowViewModelBase)obj);
            var viewModel = CompositionContainerFactory.Instance.GetExport<SeriesDataViewModel>();
            viewModel.Series = row;
            viewModel.Show();
            OnPropertyChanged(nameof(Series));
        }

        private void OnDelete(object obj)
        {
            var row = ((ITankStatisticRow)obj);
            var dossierRepository = CompositionContainerFactory.Instance.GetExport<DossierRepository>();
            dossierRepository.DeletePlayerData(row.PlayerId);
        }

        private void OnRowDoubleClick(object rowData)
        {
            ITankStatisticRow tankStatisticRowViewModel = rowData as ITankStatisticRow;

            //NRE if row of type TotalTankStatisticRowViewModel
            if (tankStatisticRowViewModel != null && !(tankStatisticRowViewModel is TotalTankStatisticRowViewModel))
            {
                TankStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<TankStatisticViewModel>();
                if (viewModel != null)
                {
                    viewModel.TankStatistic = tankStatisticRowViewModel;
                    viewModel.Show();
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
