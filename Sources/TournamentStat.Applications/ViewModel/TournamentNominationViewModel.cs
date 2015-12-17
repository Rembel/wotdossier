using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TournamentStat.Applications.Properties;
using TournamentStat.Applications.View;
using WotDossier.Applications.ViewModel;
using WotDossier.Dal;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace TournamentStat.Applications.ViewModel
{
    public class TournamentNominationViewModel : ViewModel<ITournamentNominationView>
    {
        private TournamentNomination _tournamentNomination;

        public DelegateCommand SaveCommand { get; set; }

        public TournamentNomination TournamentNomination
        {
            get
            {
                if (_tournamentNomination == null)
                {
                    _tournamentNomination = new TournamentNomination {TournamentTanks = new List<TournamentTank>()};
                }
                return _tournamentNomination;
            }
            set
            {
                _tournamentNomination = value;

                var list = _tournamentNomination.TournamentTanks.Select(x => x.TankUniqueId).ToList();

                TournamentTanks =
                    Dictionaries.Instance.Tanks.Where(x => x.Value.Active)
                        .Select(x => new TournamentTank(x.Value, list.Contains(x.Key))).ToList();
            }
        }

        private List<TournamentTank> _tournamentTanks;

        public string Nomination
        {
            get { return TournamentNomination.Nomination; }
            set
            {
                TournamentNomination.Nomination = value;
                RaisePropertyChanged(nameof(Nomination));
            }
        }

        public List<TournamentTank> TournamentTanks
        {
            get
            {
                if (_tournamentTanks == null)
                {
                    _tournamentTanks = Dictionaries.Instance.Tanks.Where(x => x.Value.Active)
                        .Select(x => new TournamentTank(x.Value)).ToList();
                }

                return TankFilter.Filter(_tournamentTanks).OrderByDescending(x => x.IsSelected).ThenBy(n => n.Tank).ToList();
            }
            set { _tournamentTanks = value; }
        }

        private List<ListItem<TournamentCriterion>> _criterions = new List<ListItem<TournamentCriterion>>
            {
                new ListItem<TournamentCriterion>(TournamentCriterion.Damage, Resources.TournamentCriterion_Damage),
                new ListItem<TournamentCriterion>(TournamentCriterion.DamageWithArmor, Resources.TournamentCriterion_DamageWithArmor),
                new ListItem<TournamentCriterion>(TournamentCriterion.DamageWithAssist, Resources.TournamentCriterion_DamageWithAssist),
                new ListItem<TournamentCriterion>(TournamentCriterion.Frags, Resources.TournamentCriterion_Frags),
                new ListItem<TournamentCriterion>(TournamentCriterion.WinPercent, Resources.TournamentCriterion_WinPercent),
            };


        public List<ListItem<TournamentCriterion>> Criterions
        {
            get { return _criterions; }
            set { _criterions = value; }
        }

        public TournamentCriterion Criterion
        {
            get { return _tournamentNomination.Criterion; }
            set { _tournamentNomination.Criterion = value; }
        }

        public TournamentTankFilterViewModel TankFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;" /> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public TournamentNominationViewModel(ITournamentNominationView view)
            : base(view)
        {
            SaveCommand = new DelegateCommand(OnSave);
            
            TankFilter = new TournamentTankFilterViewModel();

            TankFilter.PropertyChanged += TankFilterOnPropertyChanged;
        }

        private void TankFilterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(TournamentTanks));
        }

        private void OnSave()
        {
            _tournamentNomination.TournamentTanks = TournamentTanks.Where(x => x.IsSelected).ToList();

            ViewTyped.DialogResult = true;
            ViewTyped.Close();
        }

        public virtual bool? Show()
        {
            return ViewTyped.ShowDialog();
        }
    }
}
