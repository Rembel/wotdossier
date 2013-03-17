using System;

namespace WotDossier.Common
{
    /// <summary>
    /// http://wot-news.com/main/post/02172013/1/Izmenenija-v-kalakuljatore
    /// http://armor.kiev.ua/wot/rating/
    /// http://www.koreanrandom.com/forum/topic/1643-per-vehicle-efficiency-%D1%8D%D1%84%D1%84%D0%B5%D0%BA%D1%82%D0%B8%D0%B2%D0%BD%D0%BE%D1%81%D1%82%D1%8C-%D0%BF%D0%BE-%D1%82%D0%B0%D0%BD%D0%BA%D1%83-e-teff/
    /// </summary>
    public static class RatingHelper
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

        public static double CalcWN6(double avgDamage, double tier, double avgFrags, double avgSpot, double avgDef, double winrate)
        {
            return (1240 - 1040 / Math.Pow((Math.Min(tier, 6)), 0.164)) * avgFrags + avgDamage * 530 / (184 * Math.Pow(Math.E, (0.24 * tier)) + 130)
                   + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Pow(Math.E, ((winrate - 35) * -0.134)))) - 500) * 0.45 + (6 - Math.Min(tier, 6)) * -60;
        }

        public static double CalcER(double avgDamage, double tier, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            return avgDamage * (10.0 / (tier + 2.0)) * (0.23 + 2.0 * tier / 100.0) + avgFrags * 250.0 + avgSpot * 150.0 + (Math.Log(avgCap + 1, 1.732)) * 150.0 + avgDef * 150.0;
        }
    }
}