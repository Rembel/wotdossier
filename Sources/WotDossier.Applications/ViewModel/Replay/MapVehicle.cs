using System.ComponentModel;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class MapVehicle : INotifyPropertyChanged
    {
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
        private TankDescription _tankDescription;
        private int _currentHealth;
        private bool _seen;
        private bool _visible;
        private int _healthPercent = 100;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");

            }
        }

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                HealthPercent = (int) (_currentHealth*100.0/Health);
                OnPropertyChanged("CurrentHealth");
            }
        }

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

        public TankDescription TankDescription
        {
            get { return _tankDescription; }
            set { _tankDescription = value; }
        }

        public float Clock { get; set; }

        public bool Seen
        {
            get { return _seen; }
            set
            {
                _seen = value;
                OnPropertyChanged("Seen");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Show()
        {
            if (!Visible)
            {
                Visible = true;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }
    }
}