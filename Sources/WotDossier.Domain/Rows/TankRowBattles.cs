namespace WotDossier.Domain.Rows
{
    public class TankRowBattles : TankRowBase
    {
        private int _battles;
        private int _won;
        private double _wonPercent;
        private int _lost;
        private double _lostPercent;
        private int _draws;
        private double _drawsPercent;
        private int _survived;
        private double _survivedPercent;
        private int _survivedAndWon;
        private double _survivedAndWonPercent;

        public TankRowBattles(Tank tank) : base(tank)
        {
            _battles = tank.Tankdata.battlesCount;
            _won = tank.Tankdata.wins;
            _wonPercent = _won / (double)_battles * 100.0;
            _lost = tank.Tankdata.losses;
            _lostPercent = _lost / (double)_battles * 100.0;
            _draws = _battles - _won - _lost;
            _drawsPercent = _draws / (double)_battles * 100.0;
            _survived = tank.Tankdata.survivedBattles;
            _survivedPercent = _survived / (double)_battles * 100.0;
            _survivedAndWon = tank.Tankdata.winAndSurvived;
            _survivedAndWonPercent = _survivedAndWon / (double)_battles * 100.0;
        }

        public int Battles
        {
            get { return _battles; }
            set { _battles = value; }
        }

        public int Won
        {
            get { return _won; }
            set { _won = value; }
        }

        public double WonPercent
        {
            get { return _wonPercent; }
            set { _wonPercent = value; }
        }

        public int Lost
        {
            get { return _lost; }
            set { _lost = value; }
        }

        public double LostPercent
        {
            get { return _lostPercent; }
            set { _lostPercent = value; }
        }

        public int Draws
        {
            get { return _draws; }
            set { _draws = value; }
        }

        public double DrawsPercent
        {
            get { return _drawsPercent; }
            set { _drawsPercent = value; }
        }

        public int Survived
        {
            get { return _survived; }
            set { _survived = value; }
        }

        public double SurvivedPercent
        {
            get { return _survivedPercent; }
            set { _survivedPercent = value; }
        }

        public int SurvivedAndWon
        {
            get { return _survivedAndWon; }
            set { _survivedAndWon = value; }
        }

        public double SurvivedAndWonPercent
        {
            get { return _survivedAndWonPercent; }
            set { _survivedAndWonPercent = value; }
        }
    }
}
