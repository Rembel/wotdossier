using System;

namespace WotDossier.Domain.Rows
{
    /*
     РЭ = DAMAGE * (10 / (TIER + 2)) * (0.21 + 3*TIER / 100)
FRAGS * 250 +
SPOT * 150 +
(log(CAP + 1,1.732))*150 + 
DEF * 150;
     *             bc - количество боёв
mid - средний уровень танков (с учётом количества боёв) = SUM<L=1..10>(L * (количество боёв на технике уровня L) / (общее кол-во боев))
dmg = damageDealt / bc (нанесённый урон делится на количество боёв, т.е. средний дамаг за бой)
des = frags / bc (среднее количество убитых за бой)
det = spotted / bc (среднее количество обнаруженных за бой)
cap = capture_points / bc (среднее количество очков захвата за бой)
def = dropped_capture_points / bc (среднее количество очков защиты за бой)
     * 
     * 
     * wn6 = (1240-1040/(MIN(TIER,6))^0.164)*FRAGS
+DAMAGE*530/(184*e^(0.24*TIER)+130)
+SPOT*125
+MIN(DEF,2.2)*100
+((185/(0.17+e^((WINRATE-35)*-0.134)))-500)*0.45
+(6-MIN(TIER,6))*-60
     */

    /// <summary>
    /// http://wot-news.com/main/post/02172013/1/Izmenenija-v-kalakuljatore
    /// http://armor.kiev.ua/wot/rating/
    /// </summary>
    public class TankRowRatings : TankRowBase
    {
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

        private int _battles;
        private double _winrate;
        private int _averageDamage;
        private double _killDeathRatio;
        private int _newEffRating;
        private int _wn6;
        private int _damageRatingRev1;
        private int _kievArmorRating;
        private int _markOfMastery;

        public TankRowRatings(Tank tank)
        {
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            _battles = tank.Tankdata.battlesCount;
            _winrate = tank.Tankdata.wins/(double)tank.Tankdata.battlesCount*100.0;
            _averageDamage = tank.Tankdata.damageDealt/tank.Tankdata.battlesCount;
            _killDeathRatio = tank.Tankdata.frags/(double) (_battles - tank.Tankdata.survivedBattles);
            double avgFrags = tank.Tankdata.frags / (double)_battles;
            double avgSpot = tank.Tankdata.spotted/(double)_battles;
            double avgCap = tank.Tankdata.capturePoints/(double)_battles;
            double avgDef = tank.Tankdata.droppedCapturePoints / (double)_battles;

            double value = _averageDamage * (10.0 / (Tier + 2.0)) * (0.23 + 2.0 * Tier / 100.0) + avgFrags * 250.0 + avgSpot * 150.0 + (Math.Log(avgCap + 1, 1.732)) * 150.0 + avgDef * 150.0;

            _newEffRating = (int)value;
            value = (1240 - 1040 / Math.Pow((Math.Min(Tier, 6)), 0.164)) * avgFrags + _averageDamage * 530 / (184 * Math.Pow(Math.E, (0.24 * Tier)) + 130)
                + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Pow(Math.E, ((_winrate - 35) * -0.134)))) - 500) * 0.45 + (6 - Math.Min(Tier, 6)) * -60;
            _wn6 = (int)value;
            _damageRatingRev1 = (int)(tank.Tankdata.damageDealt / (double)tank.Tankdata.damageReceived * 100);
            _kievArmorRating = 0;
            _markOfMastery = tank.Special.markOfMastery;
        }
    }
}
