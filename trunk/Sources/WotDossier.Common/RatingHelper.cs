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
         * 
         * 
WN7 formula:
(1240-1040/(MIN(TIER,6))^0.164)*FRAGS
+DAMAGE*530/(184*e^(0.24*TIER)+130)
+SPOT*125*MIN(TIER, 3)/3
+MIN(DEF,2.2)*100
+((185/(0.17+e^((WINRATE-35)*-0.134)))-500)*0.45
-[(5 - MIN(TIER,5))*125] / [1 + e^( ( TIER - (GAMESPLAYED/220)^(3/TIER) )*1.5 )]
*/

        #region KievArmorRating

        private const double K_AvgWonBattles = 2.0;
        private const double K_AvgFrags = 0.9;
        private const double K_AvgSpotted = 0.5;
        private const double K_AvgCapPoints = 0.5;
        private const double K_AvgDefPoints = 0.5;
        private const double khp = 1;

        #endregion
        
        public static double CalcWN6(double avgDamage, double tier, double avgFrags, double avgSpot, double avgDef, double winrate)
        {
            return (1240 - 1040 / Math.Pow((Math.Min(tier, 6)), 0.164)) * avgFrags + avgDamage * 530 / (184 * Math.Pow(Math.E, (0.24 * tier)) + 130)
                   + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Pow(Math.E, ((winrate - 35) * -0.134)))) - 500) * 0.45 + (6 - Math.Min(tier, 6)) * -60;
        }

        public static double CalcWN7(double battles, double avgDamage, double tier, double avgFrags, double avgSpot, double avgDef, double winrate)
        {
            return (1240 - 1040 / Math.Pow((Math.Min(tier, 6)), 0.164)) * avgFrags + avgDamage * 530 / (184 * Math.Exp(0.24 * tier) + 130)
                   + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Exp((winrate - 35) * -0.134))) - 500) * 0.45
                   - ((5 - Math.Min(tier, 5)) * 125) / (1 + Math.Exp((tier - Math.Pow(battles/220.0, 3/tier))*1.5));
        }
        
        public static double CalcER(double avgDamage, double tier, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            return avgDamage * (10.0 / (tier + 2.0)) * (0.23 + 2.0 * tier / 100.0) + avgFrags * 250.0 + avgSpot * 150.0 + (Math.Log(avgCap + 1, 1.732)) * 150.0 + avgDef * 150.0;
        }

        /// <summary>
        /// http://forum.worldoftanks.ru/index.php?/topic/691284-%D0%BD%D1%83%D0%B1%D0%BE-%D1%80%D0%B5%D0%B9%D1%82%D0%B8%D0%BD%D0%B3-%D0%BF%D0%BE-%D0%B2%D0%B5%D1%80%D1%81%D0%B8%D0%B8-wot-noobsru/
        /// </summary>
        /// <param name="avgDamage"></param>
        /// <param name="tier"></param>
        /// <param name="avgFrags"></param>
        /// <param name="avgSpot"></param>
        /// <param name="avgCap"></param>
        /// <param name="avgDef"></param>
        /// <returns></returns>
        public static double CalcNR(double avgDamage, double tier, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            double kDamage = (avgDamage*10*(0.15+2*(tier/100)))/tier;
            double kFrags = avgFrags * (0.35 - 2 * (tier / 100)) * 1000;
            double kSpotted = avgSpot * 0.2 * 1000;
            double kCap = avgCap * 0.15 * 1000;
            double kDef = avgDef * 0.15 * 1000;
            return (kDamage + kFrags + kSpotted + kCap + kDef)/10;
        }

        public static double CalcKievArmorRating(double battles, double avgXP, double avgDamage, double avgWonBattles, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            double log10 = Math.Log(battles) / 10;
            double d = (avgWonBattles*K_AvgWonBattles) + (avgFrags*K_AvgFrags) + (avgSpot*K_AvgSpotted) + (avgCap*K_AvgCapPoints) + (avgDef*K_AvgDefPoints);
            return log10 * (avgXP * khp + avgDamage * d);
        }

        public static double XWN(double wn6)
        {
            return wn6>2200 ? 100 : Math.Max(Math.Min( wn6*(wn6*(wn6*(-0.00000000001268*wn6 + 0.00000005147) - 0.00006418) + 0.07576) - 7.25, 100), 0);
        }

        public static double XEFF(double eff)
        {
            return eff<400 ? 0 : Math.Max(Math.Min(eff*(eff*(eff*(eff*(eff*(0.000000000000000045254*eff - 0.00000000000033131) + 0.00000000094164) - 0.0000013227) + 0.00095664) - 0.2598) + 13.23, 100), 0);
        }

        public static double PerformanceRating(double battles, double wins, double expectedDamage, double playerDamage, double avgTier)
        {
            //Win rate component
            double expectedWinrate = 0.4856;
            double winrateWeight = 500;

            double playerWinrate = wins / battles;
            double winrateRatio = playerWinrate / expectedWinrate;
            double winrateComponent = winrateRatio * winrateWeight;

            //Damage component
            //sum of all individual tank expected damages
            //double individualTankExpectedDamage = battles*tankNominalDamage;

            double damageRatio = playerDamage / expectedDamage;
            double damageWeight = 1000;
            double damageComponent = damageRatio * damageWeight;

            //
            //First penalty threshold:
            double clearedFromPenalties1 = 1500;
            double expectedMinBattles1 = 500;
            double expectedMinAvgTier1 = 6;

            //Second penalty threshold:
            double clearedFromPenalties2 = 1900;
            double expectedMinBattles2 = 2000;
            double expectedMinAvgTier2 = 7;

            //Tying it together
            double beforePenalties = winrateComponent + damageComponent;

            //Here is the penalties logic (applied twice for each of the two sets of penalty parameters):
            double subjectToPenalties = beforePenalties - clearedFromPenalties1;
            double lowTierPenalty = Math.Max(0, 1 - (avgTier / expectedMinAvgTier1));
            double lowBattlePenalty = Math.Max(0, 1 - (battles / expectedMinBattles1));
            double whichPenalty = Math.Max(lowTierPenalty, lowBattlePenalty);
            double totalPenalty = Math.Min(Math.Pow(whichPenalty, 0.5), 1);
            double afterPenalties = subjectToPenalties * (1 - totalPenalty);
            double performanceRating = (clearedFromPenalties1 + afterPenalties);

            beforePenalties = performanceRating;

            subjectToPenalties = beforePenalties - clearedFromPenalties2;
            lowTierPenalty = Math.Max(0, 1 - (avgTier / expectedMinAvgTier2));
            lowBattlePenalty = Math.Max(0, 1 - (battles / expectedMinBattles2));
            whichPenalty = Math.Max(lowTierPenalty, lowBattlePenalty);
            totalPenalty = Math.Min(Math.Pow(whichPenalty, 0.5), 1);
            afterPenalties = subjectToPenalties * (1 - totalPenalty);
            performanceRating = (clearedFromPenalties2 + afterPenalties);

            // with "seal-clubbing" penalties applied
            return performanceRating;
        }

        public static double RBR(double battles, double battles88, double wins, double survive, double hit, double dmg, double avgXp88)
        {
            return (2/(1 + Math.Exp(-battles/4500)) - 1)
                   *
                   (3000/(1 + Math.Exp((0.5 - wins)/0.03)) + 7000*Math.Max(0, survive - 0.2) + 6000*Math.Max(0, hit - 0.45) + 5*(2/(1+Math.Exp(-battles88/500)) - 1)
                   * Math.Max(0, avgXp88 - 160) + Math.Max(0, dmg - 170));
        }
    }
}