using System;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Common;

namespace WotDossier.Applications.ViewModel
{
    public abstract class StatisticViewModelBase : IRating, IRowBattleAwards, IRowEpicAwards, IRowSpecialAwards, IRowMedals, IRowSeries
    {
        public static readonly string PropWarrior = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.TopGun);
        public static readonly string PropSniper = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Sniper);
        public static readonly string PropInvader = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Invader);
        public static readonly string PropDefender = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Defender);
        public static readonly string PropSteelWall = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.SteelWall);
        public static readonly string PropConfederate = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Confederate);
        public static readonly string PropScout = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Scout);
        public static readonly string PropPatrolDuty = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.PatrolDuty);
        public static readonly string PropHeroesOfRassenay = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.HeroesOfRassenay);
        public static readonly string PropLafayettePool = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.LafayettePool);
        public static readonly string PropRadleyWalters = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.RadleyWalters);
        public static readonly string PropCrucialContribution = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.CrucialContribution);
        public static readonly string PropBrothersInArms = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.BrothersInArms);
        public static readonly string PropKolobanov = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Kolobanov);
        public static readonly string PropNikolas = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Nikolas);
        public static readonly string PropOrlik = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Orlik);
        public static readonly string PropOskin = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Oskin);
        public static readonly string PropHalonen = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Halonen);
        public static readonly string PropLehvaslaiho = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Lehvaslaiho);
        public static readonly string PropDeLanglade = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.DeLanglade);
        public static readonly string PropBurda = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Burda);
        public static readonly string PropDumitru = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Dumitru);
        public static readonly string PropPascucci = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Pascucci);
        public static readonly string PropTamadaYoshio = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.TamadaYoshio);
        public static readonly string PropBoelter = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Boelter);
        public static readonly string PropFadin = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Fadin);
        public static readonly string PropTarczay = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Tarczay);
        public static readonly string PropBrunoPietro = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.BrunoPietro);
        public static readonly string PropBillotte = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Billotte);
        public static readonly string PropSurvivor = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Survivor);
        public static readonly string PropKamikaze = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Kamikaze);
        public static readonly string PropInvincible = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Invincible);
        public static readonly string PropRaider = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Raider);
        public static readonly string PropBombardier = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Bombardier);
        public static readonly string PropReaper = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Reaper);
        public static readonly string PropMouseTrap = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.MouseTrap);
        public static readonly string PropPattonValley = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.PattonValley);
        public static readonly string PropHunter = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Hunter);
        public static readonly string PropSinai = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Sinai);
        public static readonly string PropMasterGunnerLongest = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.MasterGunnerLongest);
        public static readonly string PropSharpshooterLongest = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.SharpshooterLongest);
        public static readonly string PropRanger = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Ranger);
        public static readonly string PropCoolHeaded = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.CoolHeaded);
        public static readonly string PropSpartan = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.Spartan);
        public static readonly string PropLuckyDevil = TypeHelper<StatisticViewModelBase>.PropertyName(v => v.LuckyDevil);

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

        public int HeroesOfRassenay { get; set; }

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