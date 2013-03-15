namespace WotDossier.Domain.Rows
{
    public class TankRowSpecialAwards : TankRowBase
    {
        private int _kamikaze;
        private int _raider;
        private int _bombardier;
        private int _reaper;
        private int _sharpshooter;
        private int _invincible;
        private int _survivor;
        private int _mouseTrap;
        private int _hunter;
        private int _sinai;
        private int _pattonValley;
        private int _ranger;

        public int Kamikaze
        {
            get { return _kamikaze; }
            set { _kamikaze = value; }
        }

        public int Raider
        {
            get { return _raider; }
            set { _raider = value; }
        }

        public int Bombardier
        {
            get { return _bombardier; }
            set { _bombardier = value; }
        }

        public int Reaper
        {
            get { return _reaper; }
            set { _reaper = value; }
        }

        public int Sharpshooter
        {
            get { return _sharpshooter; }
            set { _sharpshooter = value; }
        }

        public int Invincible
        {
            get { return _invincible; }
            set { _invincible = value; }
        }

        public int Survivor
        {
            get { return _survivor; }
            set { _survivor = value; }
        }

        public int MouseTrap
        {
            get { return _mouseTrap; }
            set { _mouseTrap = value; }
        }

        public int Hunter
        {
            get { return _hunter; }
            set { _hunter = value; }
        }

        public int Sinai
        {
            get { return _sinai; }
            set { _sinai = value; }
        }

        public int PattonValley
        {
            get { return _pattonValley; }
            set { _pattonValley = value; }
        }

        public int Ranger
        {
            get { return _ranger; }
            set { _ranger = value; }
        }

        public TankRowSpecialAwards(Tank tank)
        {
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            _kamikaze = tank.Special.kamikaze;
            _raider	 = tank.Special.raider;
            _bombardier	 = tank.Special.bombardier;
            _reaper	 = tank.Series.maxKillingSeries;
            _sharpshooter	 = tank.Series.maxSniperSeries;
            _invincible = tank.Series.maxInvincibleSeries;
            _survivor	 = tank.Series.maxDiehardSeries;
            _mouseTrap	 = tank.Special.mousebane;
            _hunter	 = tank.Special.beasthunter;
            _sinai = tank.Special.sinai;
            _pattonValley = tank.Special.pattonValley;
            _ranger = tank.Special.lumberjack;
        }
    }
}
