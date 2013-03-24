using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Rows
{
    public class TankRowRatings : TankRowBase, ITankRowRatings
    {
        private int _battles;
        private double _winrate;
        private int _averageDamage;
        private double _killDeathRatio;
        private int _newEffRating;
        private int _wn6;
        private int _damageRatingRev1;
        private int _kievArmorRating;
        private int _markOfMastery;

        public int Battles
        {
            get { return _battles; }
            set { _battles = value; }
        }

        public double Winrate
        {
            get { return _winrate; }
            set { _winrate = value; }
        }

        public int AverageDamage
        {
            get { return _averageDamage; }
            set { _averageDamage = value; }
        }

        public double KillDeathRatio
        {
            get { return _killDeathRatio; }
            set { _killDeathRatio = value; }
        }

        public int NewEffRating
        {
            get { return _newEffRating; }
            set { _newEffRating = value; }
        }

        public int WN6
        {
            get { return _wn6; }
            set { _wn6 = value; }
        }

        public int DamageRatingRev1
        {
            get { return _damageRatingRev1; }
            set { _damageRatingRev1 = value; }
        }

        public int KievArmorRating
        {
            get { return _kievArmorRating; }
            set { _kievArmorRating = value; }
        }

        public int MarkOfMastery
        {
            get { return _markOfMastery; }
            set { _markOfMastery = value; }
        }

        public TankRowRatings(TankJson tank)
            : base(tank)
        {
            _battles = tank.Tankdata.battlesCount;
            _winrate = tank.Tankdata.wins/(double)tank.Tankdata.battlesCount*100.0;
            _averageDamage = tank.Tankdata.damageDealt/tank.Tankdata.battlesCount;
            _killDeathRatio = tank.Tankdata.frags/(double) (_battles - tank.Tankdata.survivedBattles);
            double avgFrags = tank.Tankdata.frags / (double)_battles;
            double avgSpot = tank.Tankdata.spotted/(double)_battles;
            double avgCap = tank.Tankdata.capturePoints/(double)_battles;
            double avgDef = tank.Tankdata.droppedCapturePoints / (double)_battles;
            double avgXP = tank.Tankdata.xp / (double)_battles;

            double value = RatingHelper.CalcER(_averageDamage, Tier, avgFrags, avgSpot, avgCap, avgDef);

            _newEffRating = (int)value;
            value = RatingHelper.CalcWN6(_averageDamage, Tier, avgFrags, avgSpot, avgDef, _winrate);
            _wn6 = (int)value;
            _damageRatingRev1 = (int)(tank.Tankdata.damageDealt / (double)tank.Tankdata.damageReceived * 100);
            value = RatingHelper.CalcKievArmorRating(_battles, avgXP, _averageDamage, _winrate / 100, avgFrags, avgSpot, avgCap, avgDef);
            _kievArmorRating = (int) value;
            _markOfMastery = tank.Special.markOfMastery;
        }

        
    }
}
