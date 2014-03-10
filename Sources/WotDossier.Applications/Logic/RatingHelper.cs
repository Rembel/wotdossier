using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic
{
    /// <summary>
    /// http://wot-news.com/main/post/02172013/1/Izmenenija-v-kalakuljatore
    /// http://armor.kiev.ua/wot/rating/
    /// http://www.koreanrandom.com/forum/topic/1643-per-vehicle-efficiency-%D1%8D%D1%84%D1%84%D0%B5%D0%BA%D1%82%D0%B8%D0%B2%D0%BD%D0%BE%D1%81%D1%82%D1%8C-%D0%BF%D0%BE-%D1%82%D0%B0%D0%BD%D0%BA%D1%83-e-teff/
    /// </summary>
    public static class RatingHelper
    {
        /*
*             bc - количество боёв
mid - средний уровень танков (с учётом количества боёв) = SUM<L=1..10>(L * (количество боёв на технике уровня L) / (общее кол-во боев))
dmg = damageDealt / bc (нанесённый урон делится на количество боёв, т.е. средний дамаг за бой)
des = frags / bc (среднее количество убитых за бой)
det = spotted / bc (среднее количество обнаруженных за бой)
cap = capture_points / bc (среднее количество очков захвата за бой)
def = dropped_capture_points / bc (среднее количество очков защиты за бой)
*/

        #region KievArmorRating

        private const double K_AvgWonBattles = 2.0;
        private const double K_AvgFrags = 0.9;
        private const double K_AvgSpotted = 0.5;
        private const double K_AvgCapPoints = 0.5;
        private const double K_AvgDefPoints = 0.5;
        private const double khp = 1;

        #endregion

        /// <summary>
        /// Calcs the Wn6.
        /// wn6 = (1240-1040/(MIN(TIER,6))^0.164)*FRAGS
        /// +DAMAGE*530/(184*e^(0.24*TIER)+130)
        /// +SPOT*125
        /// +MIN(DEF,2.2)*100
        /// +((185/(0.17+e^((WINRATE-35)*-0.134)))-500)*0.45
        /// +(6-MIN(TIER,6))*-60
        /// </summary>
        /// <param name="avgDamage">The avg damage.</param>
        /// <param name="tier">The tier.</param>
        /// <param name="avgFrags">The avg frags.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <param name="winrate">The winrate.</param>
        /// <returns></returns>
        public static double CalcWN6(double avgDamage, double tier, double avgFrags, double avgSpot, double avgDef, double winrate)
        {
            return (1240 - 1040 / Math.Pow((Math.Min(tier, 6)), 0.164)) * avgFrags + avgDamage * 530 / (184 * Math.Pow(Math.E, (0.24 * tier)) + 130)
                   + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Pow(Math.E, ((winrate - 35) * -0.134)))) - 500) * 0.45 + (6 - Math.Min(tier, 6)) * -60;
        }

        /// <summary>
        /// Calcs the Wn7.
        /// WN7 formula:
        /// (1240-1040/(MIN(TIER,6))^0.164)*FRAGS
        /// +DAMAGE*530/(184*e^(0.24*TIER)+130)
        /// +SPOT*125*MIN(TIER, 3)/3
        /// +MIN(DEF,2.2)*100
        /// +((185/(0.17+e^((WINRATE-35)*-0.134)))-500)*0.45
        /// -[(5 - MIN(TIER,5))*125] / [1 + e^( ( TIER - (GAMESPLAYED/220)^(3/TIER) )*1.5 )]
        /// </summary>
        /// <param name="battles">The battles.</param>
        /// <param name="avgDamage">The avg damage.</param>
        /// <param name="tier">The tier.</param>
        /// <param name="avgFrags">The avg frags.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <param name="winrate">The winrate.</param>
        /// <returns></returns>
        public static double CalcWN7(double battles, double avgDamage, double tier, double avgFrags, double avgSpot, double avgDef, double winrate)
        {
            return (1240 - 1040 / Math.Pow((Math.Min(tier, 6)), 0.164)) * avgFrags + avgDamage * 530 / (184 * Math.Exp(0.24 * tier) + 130)
                   + avgSpot * 125 + Math.Min(avgDef, 2.2) * 100 + ((185 / (0.17 + Math.Exp((winrate - 35) * -0.134))) - 500) * 0.45
                   - ((5 - Math.Min(tier, 5)) * 125) / (1 + Math.Exp((tier - Math.Pow(battles / 220.0, 3 / tier)) * 1.5));
        }

        /// <summary>
        /// http://blog.noobmeter.com/2013/10/wn8-rating-alpha-testing.html
        /// </summary>
        /// <param name="avgDmg">The avg DMG.</param>
        /// <param name="expDmg">The exp DMG.</param>
        /// <param name="avgFrag">The avg frag.</param>
        /// <param name="expFrag">The exp frag.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="expSpot">The exp spot.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <param name="expDef">The exp def.</param>
        /// <param name="avgWinRate">The avg win rate.</param>
        /// <param name="expWinRate">The exp win rate.</param>
        /// <returns></returns>
        public static double CalcWN8(double avgDmg, double expDmg, double avgFrag, double expFrag, double avgSpot, double expSpot, double avgDef, double expDef, double avgWinRate, double expWinRate)
        {
            double rDamage = avgDmg / expDmg;
            double rSpot = avgSpot / expSpot;
            double rFrag = avgFrag / expFrag;
            double rDef = avgDef / expDef;
            double rWin = avgWinRate / expWinRate;

            return CalcWn8(rWin, rDamage, rFrag, rSpot, rDef);
        }

        private static double CalcWn8(double rWin, double rDamage, double rFrag, double rSpot, double rDef)
        {
            double rWiNc = Math.Max(0, (rWin - 0.71)/(1 - 0.71));
            double rDamagEc = Math.Max(0, (rDamage - 0.22)/(1 - 0.22));
            double rFraGc = Math.Min(rDamagEc + 0.2, Math.Max(0, (rFrag - 0.12)/(1 - 0.12)));
            double rSpoTc = Math.Min(rDamagEc + 0.1, Math.Max(0, (rSpot - 0.38)/(1 - 0.38)));
            double rDeFc = Math.Min(rDamagEc + 0.1, Math.Max(0, (rDef - 0.10)/(1 - 0.10)));

            return 980*rDamagEc + 210*rDamagEc*rFraGc + 155*rFraGc*rSpoTc + 75*rDeFc*rFraGc + 145*Math.Min(1.8, rWiNc);
        }

        /// <summary>
        /// Calcs the ER.
        /// РЭ = DAMAGE * (10 / (TIER + 2)) * (0.21 + 3*TIER / 100)
        /// FRAGS * 250 +
        /// SPOT * 150 +
        /// (log(CAP + 1,1.732))*150 + 
        /// DEF * 150;
        /// </summary>
        /// <param name="avgDamage">The avg damage.</param>
        /// <param name="tier">The tier.</param>
        /// <param name="avgFrags">The avg frags.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="avgCap">The avg cap.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <returns></returns>
        public static double CalcER(double avgDamage, double tier, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            return avgDamage * (10.0 / (tier + 2.0)) * (0.23 + 2.0 * tier / 100.0) + avgFrags * 250.0 + avgSpot * 150.0 + (Math.Log(avgCap + 1, 1.732)) * 150.0 + avgDef * 150.0;
        }

        /// <summary>
        /// http://forum.worldoftanks.ru/index.php?/topic/691284-%D0%BD%D1%83%D0%B1%D0%BE-%D1%80%D0%B5%D0%B9%D1%82%D0%B8%D0%BD%D0%B3-%D0%BF%D0%BE-%D0%B2%D0%B5%D1%80%D1%81%D0%B8%D0%B8-wot-noobsru/
        /// </summary>
        /// <param name="avgDamage">The avg damage.</param>
        /// <param name="tier">The tier.</param>
        /// <param name="avgFrags">The avg frags.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="avgCap">The avg cap.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <returns></returns>
        public static double CalcNR(double avgDamage, double tier, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            double kDamage = (avgDamage * 10 * (0.15 + 2 * (tier / 100))) / tier;
            double kFrags = avgFrags * (0.35 - 2 * (tier / 100)) * 1000;
            double kSpotted = avgSpot * 0.2 * 1000;
            double kCap = avgCap * 0.15 * 1000;
            double kDef = avgDef * 0.15 * 1000;
            return (kDamage + kFrags + kSpotted + kCap + kDef) / 10;
        }

        /// <summary>
        /// Calcs the kiev armor rating.
        /// http://armor.kiev.ua/wot/info/
        /// </summary>
        /// <param name="battles">The battles.</param>
        /// <param name="avgXP">The avg XP.</param>
        /// <param name="avgDamage">The avg damage.</param>
        /// <param name="avgWonBattles">The avg won battles.</param>
        /// <param name="avgFrags">The avg frags.</param>
        /// <param name="avgSpot">The avg spot.</param>
        /// <param name="avgCap">The avg cap.</param>
        /// <param name="avgDef">The avg def.</param>
        /// <returns></returns>
        public static double CalcKievArmorRating(double battles, double avgXP, double avgDamage, double avgWonBattles, double avgFrags, double avgSpot, double avgCap, double avgDef)
        {
            double log10 = Math.Log(battles) / 10;
            double d = (avgWonBattles * K_AvgWonBattles) + (avgFrags * K_AvgFrags) + (avgSpot * K_AvgSpotted) + (avgCap * K_AvgCapPoints) + (avgDef * K_AvgDefPoints);
            return log10 * (avgXP * khp + avgDamage * d);
        }

        /// <summary>
        /// Calc XWN6
        /// </summary>
        /// <param name="wn6">The WN6.</param>
        /// <returns></returns>
        public static double XWN6(double wn6)
        {
            return wn6 > 2200 ? 100 : Math.Max(Math.Min(wn6 * (wn6 * (wn6 * (-0.00000000001268 * wn6 + 0.00000005147) - 0.00006418) + 0.07576) - 7.25, 100), 0);
        }

        /// <summary>
        /// Calc XWN8
        /// </summary>
        /// <param name="wn8">The WN8.</param>
        /// <returns></returns>
        public static double XWN8(double wn8)
        {
            return wn8 > 3250 ? 100 : Math.Max(Math.Min(wn8 * (wn8 *(wn8 * (wn8 * (wn8 * (0.00000000000000000007119*wn8 + 0.0000000000000002334) - 0.000000000006963) + 0.00000002845) - 0.00004558) + 0.06565) - 0.18, 100), 0);
        }

        /// <summary>
        /// Calc XEFF
        /// </summary>
        /// <param name="eff">The eff.</param>
        /// <returns></returns>
        public static double XEFF(double eff)
        {
            return eff < 400 ? 0 : Math.Max(Math.Min(eff * (eff * (eff * (eff * (eff * (0.000000000000000045254 * eff - 0.00000000000033131) + 0.00000000094164) - 0.0000013227) + 0.00095664) - 0.2598) + 13.23, 100), 0);
        }

        /// <summary>
        /// Calc Performance Rating
        /// http://tanks.noobmeter.com/tankList
        /// http://blog.noobmeter.com/2013/07/noobmeter-performance-rating-algorithm.html
        /// </summary>
        /// <param name="battles">The battles.</param>
        /// <param name="wins">The wins.</param>
        /// <param name="expectedDamage">The expected damage.</param>
        /// <param name="playerDamage">The player damage.</param>
        /// <param name="avgTier">The avg tier.</param>
        /// <returns></returns>
        public static double PerformanceRating(double battles, double wins, double expectedDamage, double playerDamage, double avgTier, bool applyPenalties = true)
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
            double expectedDamageAdjustment = 0.975; // introduced on 25-Sep-2013
            double damageRatio = playerDamage / (expectedDamage * expectedDamageAdjustment);
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
            double performanceRating = beforePenalties;

            if (beforePenalties > clearedFromPenalties1 && applyPenalties)
            {
                //Here is the penalties logic (applied twice for each of the two sets of penalty parameters):
                double subjectToPenalties = beforePenalties - clearedFromPenalties1;
                double lowTierPenalty = Math.Max(0, 1 - (avgTier/expectedMinAvgTier1));
                double lowBattlePenalty = Math.Max(0, 1 - (battles/expectedMinBattles1));
                double whichPenalty = Math.Max(lowTierPenalty, lowBattlePenalty);
                double totalPenalty = Math.Min(Math.Pow(whichPenalty, 0.5), 1);
                double afterPenalties = subjectToPenalties*(1 - totalPenalty);
                performanceRating = (clearedFromPenalties1 + afterPenalties);

                beforePenalties = performanceRating;
            }

            if (beforePenalties > clearedFromPenalties1 && applyPenalties)
            {
                double subjectToPenalties = beforePenalties - clearedFromPenalties2;
                double lowTierPenalty = Math.Max(0, 1 - (avgTier / expectedMinAvgTier2));
                double lowBattlePenalty = Math.Max(0, 1 - (battles / expectedMinBattles2));
                double whichPenalty = Math.Max(lowTierPenalty, lowBattlePenalty);
                double totalPenalty = Math.Min(Math.Pow(whichPenalty, 0.5), 1);
                double afterPenalties = subjectToPenalties * (1 - totalPenalty);
                performanceRating = (clearedFromPenalties2 + afterPenalties);
            }

            // with "seal-clubbing" penalties applied
            return performanceRating;
        }

        /// <summary>
        /// Calc Rating WG
        /// </summary>
        /// <param name="battles">The battles.</param>
        /// <param name="battles88">The battles88.</param>
        /// <param name="wins">The wins.</param>
        /// <param name="survive">The survive.</param>
        /// <param name="dmg">The avg DMG.</param>
        /// <param name="avgXp88">The avg XP88.</param>
        /// <param name="avgXpRadio88">The avg radio XP88.</param>
        /// <param name="avgXpTrack88">The avg track XP88.</param>
        /// <remarks>http://forum.worldoftanks.ru/index.php?/topic/995453-%D0%BE%D0%B1%D0%BD%D0%BE%D0%B2%D0%BB%D1%91%D0%BD%D0%BD%D1%8B%D0%B9-%D1%8D%D0%BA%D1%80%D0%B0%D0%BD-%D0%B4%D0%BE%D1%81%D1%82%D0%B8%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F-%D0%B8-%D1%84%D0%BE%D1%80%D0%BC%D1%83%D0%BB%D0%B0-%D1%80%D0%B0%D1%81%D1%87%D1%91/</remarks>
        /// <returns></returns>
        public static double RatingWG(double battles, double battles88, double wins, double survive, double dmg, double avgXp88, double avgXpRadio88, double avgXpTrack88)
        {
            return 540*Math.Pow(battles, 0.37)*
                   Math.Tanh(0.00163*Math.Pow(battles, -0.37)*
                             (3500/(1 + Math.Exp(16 - 31*wins)) + 1400/(1 + Math.Exp(8 - 27*survive))
                              + 3700*Asinh(0.0006*dmg) + Math.Tanh(0.002*battles88)*(3900*Asinh(0.0015*avgXp88) +
                              1.4*avgXpRadio88 + 1.1*avgXpTrack88)));
        }

        public static double GetPerformanceRating(List<TankJson> tanks)
        {
            double expDamage = tanks.Select(x => x.A15x15.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();
            int battlesCount = tanks.Sum(x => x.A15x15.battlesCount);
            int wins = tanks.Sum(x => x.A15x15.wins);
            int playerDamage = tanks.Sum(x => x.A15x15.damageDealt);
            double avgTier = tanks.Sum(x => x.A15x15.battlesCount * x.Description.Tier) / (double)battlesCount;
            return PerformanceRating(battlesCount, wins, expDamage, playerDamage, avgTier);
        }

        public static double GetWN8Rating(List<TankJson> tanks)
        {
            double battles = tanks.Sum(x => x.A15x15.battlesCount);

            double damage = tanks.Sum(x => x.A15x15.damageDealt) / battles;
            double spotted = tanks.Sum(x => x.A15x15.spotted) / battles;
            double def = tanks.Sum(x => x.A15x15.droppedCapturePoints) / battles;
            double winRate = tanks.Sum(x => x.A15x15.wins) / battles;
            double frags = tanks.Sum(x => x.A15x15.frags) / battles;

            double expDamage = tanks.Sum(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDamage) / battles;
            double expSpotted = tanks.Sum(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalSpotted) / battles;
            double expDef = tanks.Sum(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalDefence) / battles;
            double expWinRate = tanks.Sum(x => (x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalWinRate) / 100.0) / battles;
            double expFrags = tanks.Sum(x => x.A15x15.battlesCount * x.Description.Expectancy.Wn8NominalFrags) / battles;
            return CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        public static double GetPerformanceRating7x7(List<TankJson> tanks)
        {
            double expDamage = tanks.Select(x => x.A7x7.battlesCount * x.Description.Expectancy.PRNominalDamage).Sum();
            int battlesCount = tanks.Sum(x => x.A7x7.battlesCount);
            int wins = tanks.Sum(x => x.A7x7.wins);
            int playerDamage = tanks.Sum(x => x.A7x7.damageDealt);
            double avgTier = tanks.Sum(x => x.A7x7.battlesCount * x.Description.Tier) / (double)battlesCount;
            return PerformanceRating(battlesCount, wins, expDamage, playerDamage, avgTier);
        }

        public static double GetWN8Rating7x7(List<TankJson> tanks)
        {
            double battles = tanks.Sum(x => x.A7x7.battlesCount);

            double damage = tanks.Sum(x => x.A7x7.damageDealt) / battles;
            double spotted = tanks.Sum(x => x.A7x7.spotted) / battles;
            double def = tanks.Sum(x => x.A7x7.droppedCapturePoints) / battles;
            double winRate = tanks.Sum(x => x.A7x7.wins) / battles;
            double frags = tanks.Sum(x => x.A7x7.frags) / battles;

            double expDamage = tanks.Sum(x => x.A7x7.battlesCount * x.Description.Expectancy.Wn8NominalDamage) / battles;
            double expSpotted = tanks.Sum(x => x.A7x7.battlesCount * x.Description.Expectancy.Wn8NominalSpotted) / battles;
            double expDef = tanks.Sum(x => x.A7x7.battlesCount * x.Description.Expectancy.Wn8NominalDefence) / battles;
            double expWinRate = tanks.Sum(x => (x.A7x7.battlesCount * x.Description.Expectancy.Wn8NominalWinRate) / 100.0) / battles;
            double expFrags = tanks.Sum(x => x.A7x7.battlesCount * x.Description.Expectancy.Wn8NominalFrags) / battles;
            return CalcWN8(damage, expDamage, frags, expFrags, spotted, expSpotted, def, expDef, winRate, expWinRate);
        }

        public static double GetRBR7x7(List<TankJson> tanks)
        {
            double battlesCount = tanks.Sum(x => x.A7x7.battlesCount);
            double winBattles = tanks.Sum(x => x.A7x7.wins);
            double surviveBattles = tanks.Sum(x => x.A7x7.survivedBattles);
            double damage = tanks.Sum(x => x.A7x7.damageDealt);
            double battlesCount88 = tanks.Sum(x => x.A7x7.battlesCount - x.A7x7.battlesCountBefore8_8);
            battlesCount88 = battlesCount88 != 0 ? battlesCount88 : 1;
            int xp88 = tanks.Sum(x => x.A7x7.originalXP);
            int xpRadio88 = tanks.Sum(x => x.A7x7.damageAssistedRadio);
            int xpTrack88 = tanks.Sum(x => x.A7x7.damageAssistedTrack);
            double avgXp88 = xp88 / battlesCount88;
            double avgXpRadio88 = xpRadio88 / battlesCount88;
            double avgXpTrack88 = xpTrack88 / battlesCount88;
            double wins = winBattles / battlesCount;
            double survive = surviveBattles / battlesCount;
            double avgDamageDealt = damage / battlesCount;

            return RatingWG(battlesCount, battlesCount88, wins, survive, avgDamageDealt, avgXp88, avgXpRadio88, avgXpTrack88);
        }

        public static double GetRBR(List<TankJson> tanks)
        {
            double battlesCount = tanks.Sum(x => x.A15x15.battlesCount);
            double winBattles = tanks.Sum(x => x.A15x15.wins);
            double surviveBattles = tanks.Sum(x => x.A15x15.survivedBattles);
            double damage = tanks.Sum(x => x.A15x15.damageDealt);
            double battlesCount88 = tanks.Sum(x => x.A15x15.battlesCount - x.A15x15.battlesCountBefore8_8);
            battlesCount88 = battlesCount88 != 0 ? battlesCount88 : 1;
            int xp88 = tanks.Sum(x => x.A15x15.originalXP);
            int xpRadio88 = tanks.Sum(x => x.A15x15.damageAssistedRadio);
            int xpTrack88 = tanks.Sum(x => x.A15x15.damageAssistedTrack);
            double avgXp88 = xp88 / battlesCount88;
            double avgXpRadio88 = xpRadio88 / battlesCount88;
            double avgXpTrack88 = xpTrack88 / battlesCount88;
            double wins = winBattles / battlesCount;
            double survive = surviveBattles / battlesCount;
            double avgDamageDealt = damage / battlesCount;

            return RatingWG(battlesCount, battlesCount88, wins, survive, avgDamageDealt, avgXp88, avgXpRadio88, avgXpTrack88);
        }

        private static double Asinh(double x)
        {
            //asinh(x) = log(x + sqrt(x^2 + 1))
            return Math.Log(x + Math.Sqrt(Math.Pow(x, 2) + 1));
        }
    }
}