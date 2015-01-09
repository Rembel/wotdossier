using System.ComponentModel;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class MapVehicle : INotifyPropertyChanged
    {
        private int _currentHealth;
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                HealthPercent = (int) (_currentHealth*100.0/Health);
                IsAlive = _currentHealth != 0;
                OnPropertyChanged("CurrentHealth");
            }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                _isAlive = value;
                OnPropertyChanged("IsAlive");
            }
        }

        public string Tank { get; set; }

        public TankIcon TankIcon { get; set; }

        public long Id { get; set; }

        public long AccountDBID { get; set; }

        public string FullName { get; set; }
        
        public int Team { get; set; }

        public int DamageReceived { get; set; }

        public int EndHealth { get; set; }

        public int Health { get; set; }

        private int _healthPercent = 100;
        public int HealthPercent
        {
            get { return _healthPercent; }
            set
            {
                _healthPercent = value;
                OnPropertyChanged("HealthPercent");
            }
        }

        public bool TeamMate { get; set; }

        private TankDescription _tankDescription;
        public TankDescription TankDescription
        {
            get { return _tankDescription; }
            set { _tankDescription = value; }
        }

        public float Clock { get; set; }

        private bool _seen;
        public bool Seen
        {
            get { return _seen; }
            set
            {
                _seen = value;
                OnPropertyChanged("Seen");
            }
        }

        private bool _visible;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }

        private double _y;
        private double _orientation;
        private int _kills;
        private bool _isAlive = true;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");

            }
        }

        public bool Recorder { get; set; }

        public double Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                OnPropertyChanged("Orientation");
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public MapVehicle(TeamMember teamMember)
        {
            Id = teamMember.Id;
            AccountDBID = teamMember.AccountDBID;

            TankDescription = teamMember.TankDescription;

            TankIcon = TankDescription.Icon;

            Tank = teamMember.Tank;
            FullName = teamMember.FullName;
            Team = teamMember.Team;
            EndHealth = teamMember.Health;
            DamageReceived = teamMember.DamageReceived;
            TeamMate = teamMember.TeamMate;
            
            Health = EndHealth + DamageReceived;
            CurrentHealth = Health;
            Squad = teamMember.Squad;
        }

        public int Squad { get; set; }

        public int Kills
        {
            get { return _kills; }
            set
            {
                _kills = value;
                OnPropertyChanged("Kills");
            }
        }

        public void Show()
        {
            if (!Visible)
            {
                Visible = true;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}