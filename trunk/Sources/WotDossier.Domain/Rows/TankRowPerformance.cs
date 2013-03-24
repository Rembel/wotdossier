using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowPerformance : TankRowBase
    {
        private int _shots;
        private int _hits;
        private double _hitRatio;
        private int _capturePoints;
        private int _defencePoints;
        private int _tanksSpotted;

        public TankRowPerformance(TankJson tank)
            : base(tank)
        {
            _shots = tank.Tankdata.shots;
            _hits = tank.Tankdata.hits;
            _hitRatio = _hits/(double) _shots*100.0;
            _capturePoints = tank.Tankdata.capturePoints;
            _defencePoints = tank.Tankdata.droppedCapturePoints;
            _tanksSpotted = tank.Tankdata.spotted;
        }

        public int Shots
        {
            get { return _shots; }
            set { _shots = value; }
        }

        public int Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }

        public double HitRatio
        {
            get { return _hitRatio; }
            set { _hitRatio = value; }
        }

        public int CapturePoints
        {
            get { return _capturePoints; }
            set { _capturePoints = value; }
        }

        public int DefencePoints
        {
            get { return _defencePoints; }
            set { _defencePoints = value; }
        }

        public int TanksSpotted
        {
            get { return _tanksSpotted; }
            set { _tanksSpotted = value; }
        }
    }
}
