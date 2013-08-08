using System;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    public abstract class StatisticViewModelBase : IRating, IRowBattleAwards, IRowEpicAwards, IRowSpecialAwards, IRowMedals, IRowSeries
    {
        private DateTime _updated;
        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }

        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int Spotted { get; set; }

        public int DamageDealt { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }

        public double Tier { get; set; }

        #region Percents

        public double HitsPercents { get; set; }

        public double WinsPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Wins/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }
        public double LossesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Losses/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }
        public double SurvivedBattlesPercent
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return SurvivedBattles/(double) BattlesCount*100.0;
                }
                return 0;
            }
        }

        #endregion

        #region Average values

        public double AvgXp
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Xp/(double) BattlesCount;
                }
                return 0;
            }
        }

        public double AvgFrags
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Frags/(double) BattlesCount;
                }
                return 0;
            }
        }

        public double AvgSpotted
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return Spotted / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDamageDealt
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DamageDealt / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return CapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePoints
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return DroppedCapturePoints / (double)BattlesCount;
                }
                return 0;
            }
        }

        #endregion

        #region Unofficial ratings
        
        public double WN6Rating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.CalcWN6(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgDroppedCapturePoints, WinsPercent);
                }
                return 0;
            }
        }

        public double EffRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.CalcER(AvgDamageDealt, Tier, AvgFrags, AvgSpotted, AvgCapturePoints, AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double KievArmorRating
        {
            get
            {
                if (BattlesCount > 0)
                {
                    return RatingHelper.CalcKievArmorRating(BattlesCount, AvgXp, AvgDamageDealt, WinsPercent/100.0,
                                                            AvgFrags, AvgSpotted, AvgCapturePoints,
                                                            AvgDroppedCapturePoints);
                }
                return 0;
            }
        }

        public double XEFF
        {
            get { return RatingHelper.XEFF(EffRating); }
        }

        public double XWN
        {
            get { return RatingHelper.XWN(WN6Rating); }
        }

        #endregion

        #region Achievments

        #region [ ITankRowBattleAwards ]

        public int BattleHero { get; set; }

        public int TopGun { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int CoolHeaded { get; set; }

        public int LuckyDevil { get; set; }

        public int Spartan { get; set; }

        public int Jager { get; set; }

        #endregion

        #region [ ITankRowEpic ]

        public int Boelter { get; set; }

        public int RadleyWalters { get; set; }

        public int LafayettePool { get; set; }

        public int Orlik { get; set; }

        public int Oskin { get; set; }

        public int Lehvaslaiho { get; set; }

        public int Nikolas { get; set; }

        public int Halonen { get; set; }

        public int Burda { get; set; }

        public int Pascucci { get; set; }

        public int Dumitru { get; set; }

        public int TamadaYoshio { get; set; }

        public int Billotte { get; set; }

        public int BrunoPietro { get; set; }

        public int Tarczay { get; set; }

        public int Kolobanov { get; set; }

        public int Fadin { get; set; }

        public int HeroesOfRaseiniai { get; set; }

        public int DeLanglade { get; set; }

        #endregion

        #region [ ITankRowSpecialAwards ]

        public int Kamikaze { get; set; }

        public int Raider { get; set; }

        public int Bombardier { get; set; }

        public int Reaper { get; set; }

        public int Sharpshooter { get; set; }

        public int Invincible { get; set; }

        public int Survivor { get; set; }

        public int MouseTrap { get; set; }

        public int Hunter { get; set; }

        public int Sinai { get; set; }

        public int PattonValley { get; set; }

        public int Ranger { get; set; }

        #endregion

        #region [ ITankRowMedals]

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

        #endregion

        #region [ ITankRowSeries ]

        public int ReaperLongest { get; set; }

        public int ReaperProgress { get; set; }

        public int SharpshooterLongest { get; set; }

        public int SharpshooterProgress { get; set; }

        public int MasterGunnerLongest { get; set; }

        public int MasterGunnerProgress { get; set; }

        public int InvincibleLongest { get; set; }

        public int InvincibleProgress { get; set; }

        public int SurvivorLongest { get; set; }

        public int SurvivorProgress { get; set; }

        #endregion

        #endregion

        
        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        public int BattlesPerDay { get; set; }
    }
}