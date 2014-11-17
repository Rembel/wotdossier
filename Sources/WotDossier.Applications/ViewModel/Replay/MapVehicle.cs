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
            Health = teamMember.Health;
            DamageReceived = teamMember.DamageReceived;
            TeamMate = teamMember.TeamMate;

            CurrentHealth = Health + DamageReceived;
        }

        public string Tank { get; set; }

        public TankIcon TankIcon { get; set; }

        public long Id { get; set; }
        public long AccountDBID { get; set; }
        public string FullName { get; set; }
        
        public int Team { get; set; }

        public int DamageReceived { get; set; }
        public int Health { get; set; }
        public bool TeamMate { get; set; }

        public TankDescription TankDescription
        {
            get { return _tankDescription; }
            set { _tankDescription = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}