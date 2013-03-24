using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowDamage : TankRowBase, ITankRowDamage
    {
        public int DamageDealt
        {
            get { return _damageDealt; }
            set { _damageDealt = value; }
        }

        public int DamageTaken
        {
            get { return _damageTaken; }
            set { _damageTaken = value; }
        }

        public double DamageRatio
        {
            get { return _damageRatio; }
            set { _damageRatio = value; }
        }

        public int AverageDamageDealt
        {
            get { return _averageDamageDealt; }
            set { _averageDamageDealt = value; }
        }

        public int DamagePerHit
        {
            get { return _damagePerHit; }
            set { _damagePerHit = value; }
        }

        private int _damageDealt;
        private int _damageTaken;
        private double _damageRatio;
        private int _averageDamageDealt;
        private int _damagePerHit;

        public TankRowDamage(TankJson tank)
            : base(tank)
        {
            _damageDealt = tank.Tankdata.damageDealt;
            _damageTaken = tank.Tankdata.damageReceived;
            _damageRatio = DamageDealt/ (double)DamageTaken;
            _averageDamageDealt = DamageDealt/ tank.Tankdata.battlesCount;
            _damagePerHit = DamageDealt / tank.Tankdata.hits;
        }
    }
}
