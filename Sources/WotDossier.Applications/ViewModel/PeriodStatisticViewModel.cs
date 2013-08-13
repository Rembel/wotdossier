using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.ViewModel
{
    public abstract class PeriodStatisticViewModel<T> : StatisticViewModelBase, INotifyPropertyChanged where T : StatisticViewModelBase
    {
        #region Constants

        public static readonly string PropBattlesCountDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BattlesCountDelta);
        public static readonly string PropWinsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WinsDelta);
        public static readonly string PropWinsPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WinsPercentDelta);
        public static readonly string PropLossesDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LossesDelta);
        public static readonly string PropLossesPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LossesPercentDelta);
        public static readonly string PropSurvivedBattlesDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivedBattlesPercentDelta);
        public static readonly string PropSurvivedBattlesPercentDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivedBattlesDelta);
        public static readonly string PropXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.XpDelta);
        public static readonly string PropMaxXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.MaxXpDelta);
        public static readonly string PropFragsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.FragsDelta);
        public static readonly string PropSpottedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SpottedDelta);
        public static readonly string PropHitsPercentsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HitsPercentsDelta);
        public static readonly string PropDamageDealtDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DamageDealtDelta);
        public static readonly string PropCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.CapturePointsDelta);
        public static readonly string PropDroppedCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DroppedCapturePointsDelta);

        public static readonly string PropEffRatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.EffRatingDelta);
        public static readonly string PropKievArmorRatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KievArmorRatingDelta);
        public static readonly string PropWN6RatingDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WN6RatingDelta);

        public static readonly string PropAvgCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgCapturePointsDelta);
        public static readonly string PropAvgDamageDealtDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDamageDealtDelta);
        public static readonly string PropAvgDroppedCapturePointsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDroppedCapturePointsDelta);
        public static readonly string PropAvgFragsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgFragsDelta);
        public static readonly string PropAvgSpottedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgSpottedDelta);
        public static readonly string PropAvgXpDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgXpDelta);

        public static readonly string PropWinsPercentForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WinsPercentForPeriod);
        public static readonly string PropLossesPercentForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LossesPercentForPeriod);
        public static readonly string PropSurvivedBattlesPercentForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivedBattlesPercentForPeriod);


        public static readonly string PropAvgCapturePointsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgCapturePointsForPeriod);
        public static readonly string PropAvgDamageDealtForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDamageDealtForPeriod);
        public static readonly string PropAvgDroppedCapturePointsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgDroppedCapturePointsForPeriod);
        public static readonly string PropAvgFragsForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgFragsForPeriod);
        public static readonly string PropAvgSpottedForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgSpottedForPeriod);
        public static readonly string PropAvgXpForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.AvgXpForPeriod);

        public static readonly string PropWN6RatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WN6RatingForPeriod);
        public static readonly string PropEffRatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.EffRatingForPeriod);
        public static readonly string PropKievArmorRatingForPeriod = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KievArmorRatingForPeriod);

        public static readonly string PropPreviousDate = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.PreviousDate);

        public static readonly string PropWarriorDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.WarriorDelta);
        public static readonly string PropSniperDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SniperDelta);
        public static readonly string PropInvaderDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.InvaderDelta);
        public static readonly string PropDefenderDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DefenderDelta);
        public static readonly string PropSteelWallDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SteelWallDelta);
        public static readonly string PropConfederateDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.ConfederateDelta);
        public static readonly string PropScoutDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.ScoutDelta);
        public static readonly string PropPatrolDutyDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.PatrolDutyDelta);
        public static readonly string PropHeroesOfRassenayDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HeroesOfRassenayDelta);
        public static readonly string PropLafayettePoolDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LafayettePoolDelta);
        public static readonly string PropRadleyWaltersDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.RadleyWaltersDelta);
        public static readonly string PropCrucialContributionDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.CrucialContributionDelta);
        public static readonly string PropBrothersInArmsDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BrothersInArmsDelta);
        public static readonly string PropKolobanovDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KolobanovDelta);
        public static readonly string PropNikolasDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.NikolasDelta);
        public static readonly string PropOrlikDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.OrlikDelta);
        public static readonly string PropOskinDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.OskinDelta);
        public static readonly string PropHalonenDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HalonenDelta);
        public static readonly string PropLehvaslaihoDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LehvaslaihoDelta);
        public static readonly string PropDeLangladeDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DeLangladeDelta);
        public static readonly string PropBurdaDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BurdaDelta);
        public static readonly string PropDumitruDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.DumitruDelta);
        public static readonly string PropPascucciDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.PascucciDelta);
        public static readonly string PropTamadaYoshioDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.TamadaYoshioDelta);
        public static readonly string PropBoelterDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BoelterDelta);
        public static readonly string PropFadinDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.FadinDelta);
        public static readonly string PropTarczayDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.TarczayDelta);
        public static readonly string PropBrunoPietroDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BrunoPietroDelta);
        public static readonly string PropBillotteDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BillotteDelta);
        public static readonly string PropSurvivorDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SurvivorDelta);
        public static readonly string PropKamikazeDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.KamikazeDelta);
        public static readonly string PropInvincibleDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.InvincibleDelta);
        public static readonly string PropRaiderDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.RaiderDelta);
        public static readonly string PropBombardierDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.BombardierDelta);
        public static readonly string PropReaperDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.ReaperDelta);
        public static readonly string PropMouseTrapDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.MouseTrapDelta);
        public static readonly string PropPattonValleyDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.PattonValleyDelta);
        public static readonly string PropHunterDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.HunterDelta);
        public static readonly string PropSinaiDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SinaiDelta);
        public static readonly string PropMasterGunnerLongestDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.MasterGunnerLongestDelta);
        public static readonly string PropSharpshooterLongestDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SharpshooterLongestDelta);
        public static readonly string PropRangerDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.RangerDelta);
        public static readonly string PropCoolHeadedDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.CoolHeadedDelta);
        public static readonly string PropSpartanDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.SpartanDelta);
        public static readonly string PropLuckyDevilDelta = TypeHelper<PeriodStatisticViewModel<T>>.PropertyName(v => v.LuckyDevilDelta);

        #endregion

        #region Fields

        private readonly IEnumerable<T> _list;

        #endregion

        #region Common delta

        public int BattlesCountDelta
        {
            get { return BattlesCount - PrevStatistic.BattlesCount; }
        }

        public int WinsDelta
        {
            get { return Wins - PrevStatistic.Wins; }
        }

        public double WinsPercentDelta
        {
            get { return WinsPercent - PrevStatistic.WinsPercent; }
        }

        public int LossesDelta
        {
            get { return Losses - PrevStatistic.Losses; }
        }

        public double LossesPercentDelta
        {
            get { return LossesPercent - PrevStatistic.LossesPercent; }
        }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - PrevStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - PrevStatistic.SurvivedBattlesPercent; }
        }

        public int XpDelta
        {
            get { return Xp - PrevStatistic.Xp; }
        }

        public int MaxXpDelta
        {
            get { return MaxXp - PrevStatistic.MaxXp; }
        }

        public int FragsDelta
        {
            get { return Frags - PrevStatistic.Frags; }
        }

        public int SpottedDelta
        {
            get { return Spotted - PrevStatistic.Spotted; }
        }

        public double HitsPercentsDelta
        {
            get { return HitsPercents - PrevStatistic.HitsPercents; }
        }

        public int DamageDealtDelta
        {
            get { return DamageDealt - PrevStatistic.DamageDealt; }
        }

        public int CapturePointsDelta
        {
            get { return CapturePoints - PrevStatistic.CapturePoints; }
        }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - PrevStatistic.DroppedCapturePoints; }
        }

        public double WN6RatingDelta
        {
            get { return WN6Rating - PrevStatistic.WN6Rating; }
        }

        public double EffRatingDelta
        {
            get { return EffRating - PrevStatistic.EffRating; }
        }

        public double KievArmorRatingDelta
        {
            get { return KievArmorRating - PrevStatistic.KievArmorRating; }
        }

        public double XvmRatingDelta
        {
            get { return XEFF - PrevStatistic.XEFF; }
        }

        #endregion

        #region Average values

        public double AvgXpDelta
        {
            get
            {
                return AvgXp - PrevStatistic.AvgXp;
            }
        }

        public double AvgFragsDelta
        {
            get
            {
                return AvgFrags - PrevStatistic.AvgFrags;
            }
        }

        public double AvgSpottedDelta
        {
            get
            {
                return AvgSpotted - PrevStatistic.AvgSpotted;
            }
        }

        public double AvgDamageDealtDelta
        {
            get
            {
                return AvgDamageDealt - PrevStatistic.AvgDamageDealt;
            }
        }

        public double AvgCapturePointsDelta
        {
            get
            {
                return AvgCapturePoints - PrevStatistic.AvgCapturePoints;
            }
        }

        public double AvgDroppedCapturePointsDelta
        {
            get
            {
                return AvgDroppedCapturePoints - PrevStatistic.AvgDroppedCapturePoints;
            }
        }

        #endregion

        public T PrevStatistic { get; protected set; }

        public DateTime PreviousDate
        {
            get { return PrevStatistic.Updated; }
        }

        #region Statistic For Period

        public double WinsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return WinsDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double LossesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return LossesDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double SurvivedBattlesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SurvivedBattlesDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double HitsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    //TODO
                    return 0;
                }
                return 0;
            }
        }

        public double TierForInterval
        {
            get
            {
                return (Tier * BattlesCount - PrevStatistic.Tier * PrevStatistic.BattlesCount) / BattlesCountDelta;
            }
        }

        public double EffRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcER(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                               AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double WN6RatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcWN6(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                                AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
                }
                return 0;
            }
        }

        public double KievArmorRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.CalcKievArmorRating(BattlesCountDelta, AvgXpForPeriod, AvgDamageDealtForPeriod,
                                                            WinsPercentForPeriod / 100.0,
                                                            AvgFragsForPeriod, AvgSpottedForPeriod,
                                                            AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double AvgXpForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return XpDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgFragsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return FragsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgSpottedForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SpottedDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDamageDealtForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DamageDealtDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return CapturePointsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DroppedCapturePointsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        #endregion

        #region Achievments

        #region [ ITankRowBattleAwards ]

        public int BattleHeroDelta
        {
            get { return BattleHero - PrevStatistic.BattleHero; }
        }

        public int WarriorDelta
        {
            get { return Warrior - PrevStatistic.Warrior; }
        }

        public int InvaderDelta
        {
            get { return Invader - PrevStatistic.Invader; }
        }

        public int SniperDelta
        {
            get { return Sniper - PrevStatistic.Sniper; }
        }

        public int DefenderDelta
        {
            get { return Defender - PrevStatistic.Defender; }
        }

        public int SteelWallDelta
        {
            get { return SteelWall - PrevStatistic.SteelWall; }
        }

        public int ConfederateDelta
        {
            get { return Confederate - PrevStatistic.Confederate; }
        }

        public int ScoutDelta
        {
            get { return Scout - PrevStatistic.Scout; }
        }

        public int PatrolDutyDelta
        {
            get { return PatrolDuty - PrevStatistic.PatrolDuty; }
        }

        public int BrothersInArmsDelta
        {
            get { return BrothersInArms - PrevStatistic.BrothersInArms; }
        }

        public int CrucialContributionDelta
        {
            get { return CrucialContribution - PrevStatistic.CrucialContribution; }
        }

        public int CoolHeadedDelta
        {
            get { return CoolHeaded - PrevStatistic.CoolHeaded; }
        }

        public int LuckyDevilDelta
        {
            get { return LuckyDevil - PrevStatistic.LuckyDevil; }
        }

        public int SpartanDelta
        {
            get { return Spartan - PrevStatistic.Spartan; }
        }

        #endregion

        #region [ ITankRowEpic ]

        public int BoelterDelta
        {
            get { return Boelter - PrevStatistic.Boelter; }
        }

        public int RadleyWaltersDelta
        {
            get { return RadleyWalters - PrevStatistic.RadleyWalters; }
        }

        public int LafayettePoolDelta
        {
            get { return LafayettePool - PrevStatistic.LafayettePool; }
        }

        public int OrlikDelta
        {
            get { return Orlik - PrevStatistic.Orlik; }
        }

        public int OskinDelta
        {
            get { return Oskin - PrevStatistic.Oskin; }
        }

        public int LehvaslaihoDelta
        {
            get { return Lehvaslaiho - PrevStatistic.Lehvaslaiho; }
        }

        public int NikolasDelta
        {
            get { return Nikolas - PrevStatistic.Nikolas; }
        }

        public int HalonenDelta
        {
            get { return Halonen - PrevStatistic.Halonen; }
        }

        public int BurdaDelta
        {
            get { return Burda - PrevStatistic.Burda; }
        }

        public int PascucciDelta
        {
            get { return Pascucci - PrevStatistic.Pascucci; }
        }

        public int DumitruDelta
        {
            get { return Dumitru - PrevStatistic.Dumitru; }
        }

        public int TamadaYoshioDelta
        {
            get { return TamadaYoshio - PrevStatistic.TamadaYoshio; }
        }

        public int BillotteDelta
        {
            get { return Billotte - PrevStatistic.Billotte; }
        }

        public int BrunoPietroDelta
        {
            get { return BrunoPietro - PrevStatistic.BrunoPietro; }
        }

        public int TarczayDelta
        {
            get { return Tarczay - PrevStatistic.Tarczay; }
        }

        public int KolobanovDelta
        {
            get { return Kolobanov - PrevStatistic.Kolobanov; }
        }

        public int FadinDelta
        {
            get { return Fadin - PrevStatistic.Fadin; }
        }

        public int HeroesOfRassenayDelta
        {
            get { return HeroesOfRassenay - PrevStatistic.HeroesOfRassenay; }
        }

        public int DeLangladeDelta
        {
            get { return DeLanglade - PrevStatistic.DeLanglade; }
        }

        #endregion

        #region [ ITankRowSpecialAwards ]

        public int KamikazeDelta
        {
            get { return Kamikaze - PrevStatistic.Kamikaze; }
        }

        public int RaiderDelta
        {
            get { return Raider - PrevStatistic.Raider; }
        }

        public int BombardierDelta
        {
            get { return Bombardier - PrevStatistic.Bombardier; }
        }

        public int ReaperDelta
        {
            get { return Reaper - PrevStatistic.Reaper; }
        }

        public int SharpshooterDelta
        {
            get { return Sharpshooter - PrevStatistic.Sharpshooter; }
        }

        public int InvincibleDelta
        {
            get { return Invincible - PrevStatistic.Invincible; }
        }

        public int SurvivorDelta
        {
            get { return Survivor - PrevStatistic.Survivor; }
        }

        public int MouseTrapDelta
        {
            get { return MouseTrap - PrevStatistic.MouseTrap; }
        }

        public int HunterDelta
        {
            get { return Hunter - PrevStatistic.Hunter; }
        }

        public int SinaiDelta
        {
            get { return Sinai - PrevStatistic.Sinai; }
        }

        public int PattonValleyDelta
        {
            get { return PattonValley - PrevStatistic.PattonValley; }
        }

        public int RangerDelta
        {
            get { return Ranger - PrevStatistic.Ranger; }
        }

        #endregion

        #region [ ITankRowMedals]

        public int KayDelta
        {
            get { return Kay - PrevStatistic.Kay; }
        }

        public int CariusDelta
        {
            get { return Carius - PrevStatistic.Carius; }
        }

        public int KnispelDelta
        {
            get { return Knispel - PrevStatistic.Knispel; }
        }

        public int PoppelDelta
        {
            get { return Poppel - PrevStatistic.Poppel; }
        }

        public int AbramsDelta
        {
            get { return Abrams - PrevStatistic.Abrams; }
        }

        public int LeclerkDelta
        {
            get { return Leclerk - PrevStatistic.Leclerk; }
        }

        public int LavrinenkoDelta
        {
            get { return Lavrinenko - PrevStatistic.Lavrinenko; }
        }

        public int EkinsDelta
        {
            get { return Ekins - PrevStatistic.Ekins; }
        }

        #endregion

        #region [ ITankRowSeries ]

        public int ReaperLongestDelta
        {
            get { return ReaperLongest - PrevStatistic.ReaperLongest; }
        }

        public int ReaperProgressDelta
        {
            get { return ReaperProgress - PrevStatistic.ReaperProgress; }
        }

        public int SharpshooterLongestDelta
        {
            get { return SharpshooterLongest - PrevStatistic.SharpshooterLongest; }
        }

        public int SharpshooterProgressDelta
        {
            get { return SharpshooterProgress - PrevStatistic.SharpshooterProgress; }
        }

        public int MasterGunnerLongestDelta
        {
            get { return MasterGunnerLongest - PrevStatistic.MasterGunnerLongest; }
        }

        public int MasterGunnerProgressDelta
        {
            get { return MasterGunnerProgress - PrevStatistic.MasterGunnerProgress; }
        }

        public int InvincibleLongestDelta
        {
            get { return InvincibleLongest - PrevStatistic.InvincibleLongest; }
        }

        public int InvincibleProgressDelta
        {
            get { return InvincibleProgress - PrevStatistic.InvincibleProgress; }
        }

        public int SurvivorLongestDelta
        {
            get { return SurvivorLongest - PrevStatistic.SurvivorLongest; }
        }

        public int SurvivorProgressDelta
        {
            get { return SurvivorProgress - PrevStatistic.SurvivorProgress; }
        }

        #endregion

        #endregion

        protected PeriodStatisticViewModel(DateTime updated, List<T> list)
        {
            _list = list;
            Updated = updated;

            AppSettings appSettings = SettingsReader.Get();
            T prevPlayerStatistic = GetPrevStatistic(appSettings.Period, appSettings.PrevDate);
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);

            if (_list.Any())
            {
                EventAggregatorFactory.EventAggregator.GetEvent<StatisticPeriodChangedEvent>().Subscribe(OnStatisticPeriodChanged);
            }
        }

        private void OnStatisticPeriodChanged(StatisticPeriodChangedEvent eventArgs)
        {
            StatisticPeriod statisticPeriod = eventArgs.StatisticPeriod;
            DateTime? prevDateTime = eventArgs.PrevDateTime;

            T prevStatistic = GetPrevStatistic(statisticPeriod, prevDateTime);
            SetPreviousStatistic(prevStatistic);
        }

        private T GetPrevStatistic(StatisticPeriod statisticPeriod, DateTime? prevDateTime)
        {
            switch (statisticPeriod)
            {
                case StatisticPeriod.Recent:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= Updated);

                case StatisticPeriod.LastWeek:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= DateTime.Now.AddDays(-7));

                case StatisticPeriod.AllObservationPeriod:
                    return _list.OrderBy(x => x.Updated).FirstOrDefault();

                case StatisticPeriod.Custom:
                    return _list.OrderByDescending(x => x.Updated).FirstOrDefault(x => x.Updated <= prevDateTime);
            }
            return null;
        }

        protected virtual void SetPreviousStatistic(T prevPlayerStatistic)
        {
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);

            OnPropertyChanged(PropBattlesCountDelta);
            OnPropertyChanged(PropWinsDelta);
            OnPropertyChanged(PropLossesDelta);
            OnPropertyChanged(PropSurvivedBattlesDelta);
            OnPropertyChanged(PropXpDelta);
            OnPropertyChanged(PropMaxXpDelta);
            OnPropertyChanged(PropFragsDelta);
            OnPropertyChanged(PropSpottedDelta);
            OnPropertyChanged(PropHitsPercentsDelta);
            OnPropertyChanged(PropDamageDealtDelta);
            OnPropertyChanged(PropCapturePointsDelta);
            OnPropertyChanged(PropDroppedCapturePointsDelta);
            OnPropertyChanged(PropWinsPercentDelta);
            OnPropertyChanged(PropLossesPercentDelta);
            OnPropertyChanged(PropSurvivedBattlesPercentDelta);

            OnPropertyChanged(PropAvgCapturePointsDelta);
            OnPropertyChanged(PropAvgDamageDealtDelta);
            OnPropertyChanged(PropAvgDroppedCapturePointsDelta);
            OnPropertyChanged(PropAvgFragsDelta);
            OnPropertyChanged(PropAvgSpottedDelta);
            OnPropertyChanged(PropAvgXpDelta);

            OnPropertyChanged(PropEffRatingDelta);
            OnPropertyChanged(PropKievArmorRatingDelta);
            OnPropertyChanged(PropWN6RatingDelta);

            OnPropertyChanged(PropWinsPercentForPeriod);
            OnPropertyChanged(PropLossesPercentForPeriod);
            OnPropertyChanged(PropSurvivedBattlesPercentForPeriod);

            OnPropertyChanged(PropAvgCapturePointsForPeriod);
            OnPropertyChanged(PropAvgDamageDealtForPeriod);
            OnPropertyChanged(PropAvgDroppedCapturePointsForPeriod);
            OnPropertyChanged(PropAvgFragsForPeriod);
            OnPropertyChanged(PropAvgSpottedForPeriod);
            OnPropertyChanged(PropAvgXpForPeriod);

            OnPropertyChanged(PropWN6RatingForPeriod);
            OnPropertyChanged(PropEffRatingForPeriod);
            OnPropertyChanged(PropKievArmorRatingForPeriod);

            OnPropertyChanged(PropPreviousDate);

            #region achievements

            OnPropertyChanged(PropWarrior);
            OnPropertyChanged(PropSniper);
            OnPropertyChanged(PropInvader);
            OnPropertyChanged(PropDefender);
            OnPropertyChanged(PropSteelWall);
            OnPropertyChanged(PropConfederate);
            OnPropertyChanged(PropScout);
            OnPropertyChanged(PropPatrolDuty);

            OnPropertyChanged(PropHeroesOfRassenay);
            OnPropertyChanged(PropLafayettePool);
            OnPropertyChanged(PropRadleyWalters);
            OnPropertyChanged(PropCrucialContribution);
            OnPropertyChanged(PropBrothersInArms);
            OnPropertyChanged(PropKolobanov);
            OnPropertyChanged(PropNikolas);
            OnPropertyChanged(PropOrlik);
            OnPropertyChanged(PropOskin);
            OnPropertyChanged(PropHalonen);
            OnPropertyChanged(PropLehvaslaiho);
            OnPropertyChanged(PropDeLanglade);
            OnPropertyChanged(PropBurda);
            OnPropertyChanged(PropDumitru);
            OnPropertyChanged(PropPascucci);
            OnPropertyChanged(PropTamadaYoshio);
            OnPropertyChanged(PropBoelter);
            OnPropertyChanged(PropFadin);
            OnPropertyChanged(PropTarczay);
            OnPropertyChanged(PropBrunoPietro);
            OnPropertyChanged(PropBillotte);

            OnPropertyChanged(PropSurvivor);
            OnPropertyChanged(PropKamikaze);
            OnPropertyChanged(PropInvincible);
            OnPropertyChanged(PropRaider);
            OnPropertyChanged(PropBombardier);
            OnPropertyChanged(PropReaper);
            OnPropertyChanged(PropMouseTrap);
            OnPropertyChanged(PropPattonValley);
            OnPropertyChanged(PropHunter);
            OnPropertyChanged(PropSinai);
            OnPropertyChanged(PropMasterGunnerLongest);
            OnPropertyChanged(PropSharpshooterLongest);

            OnPropertyChanged(PropRanger);
            OnPropertyChanged(PropCoolHeaded);
            OnPropertyChanged(PropSpartan);
            OnPropertyChanged(PropLuckyDevil);

            //OnPropertyChanged(PropKay);
            //OnPropertyChanged(PropCarius);
            //OnPropertyChanged(PropKnispel);
            //OnPropertyChanged(PropPoppel);
            //OnPropertyChanged(PropAbrams);
            //OnPropertyChanged(PropLeclerk);
            //OnPropertyChanged(PropLavrinenko);
            //OnPropertyChanged(PropEkins);

            OnPropertyChanged(PropWarriorDelta);
            OnPropertyChanged(PropSniperDelta);
            OnPropertyChanged(PropInvaderDelta);
            OnPropertyChanged(PropDefenderDelta);
            OnPropertyChanged(PropSteelWallDelta);
            OnPropertyChanged(PropConfederateDelta);
            OnPropertyChanged(PropScoutDelta);
            OnPropertyChanged(PropPatrolDutyDelta);

            OnPropertyChanged(PropHeroesOfRassenayDelta);
            OnPropertyChanged(PropLafayettePoolDelta);
            OnPropertyChanged(PropRadleyWaltersDelta);
            OnPropertyChanged(PropCrucialContributionDelta);
            OnPropertyChanged(PropBrothersInArmsDelta);
            OnPropertyChanged(PropKolobanovDelta);
            OnPropertyChanged(PropNikolasDelta);
            OnPropertyChanged(PropOrlikDelta);
            OnPropertyChanged(PropOskinDelta);
            OnPropertyChanged(PropHalonenDelta);
            OnPropertyChanged(PropLehvaslaihoDelta);
            OnPropertyChanged(PropDeLangladeDelta);
            OnPropertyChanged(PropBurdaDelta);
            OnPropertyChanged(PropDumitruDelta);
            OnPropertyChanged(PropPascucciDelta);
            OnPropertyChanged(PropTamadaYoshioDelta);
            OnPropertyChanged(PropBoelterDelta);
            OnPropertyChanged(PropFadinDelta);
            OnPropertyChanged(PropTarczayDelta);
            OnPropertyChanged(PropBrunoPietroDelta);
            OnPropertyChanged(PropBillotteDelta);

            OnPropertyChanged(PropSurvivorDelta);
            OnPropertyChanged(PropKamikazeDelta);
            OnPropertyChanged(PropInvincibleDelta);
            OnPropertyChanged(PropRaiderDelta);
            OnPropertyChanged(PropBombardierDelta);
            OnPropertyChanged(PropReaperDelta);
            OnPropertyChanged(PropMouseTrapDelta);
            OnPropertyChanged(PropPattonValleyDelta);
            OnPropertyChanged(PropHunterDelta);
            OnPropertyChanged(PropSinaiDelta);
            OnPropertyChanged(PropMasterGunnerLongestDelta);
            OnPropertyChanged(PropSharpshooterLongestDelta);

            OnPropertyChanged(PropRangerDelta);
            OnPropertyChanged(PropCoolHeadedDelta);
            OnPropertyChanged(PropSpartanDelta);
            OnPropertyChanged(PropLuckyDevilDelta);

            #endregion

        }

        public List<T> GetAll()
        {
            List<StatisticViewModelBase> list = new List<StatisticViewModelBase>();
            list.AddRange(_list);
            list.Add(this);
            return list.Cast<T>().ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StatisticPeriodChangedEvent : BaseEvent<StatisticPeriodChangedEvent>
    {
        public StatisticPeriod StatisticPeriod { get; set; }

        public DateTime? PrevDateTime { get; set; }

        public StatisticPeriodChangedEvent()
        {
        }

        public StatisticPeriodChangedEvent(StatisticPeriod statisticPeriod, DateTime? prevDate)
        {
            StatisticPeriod = statisticPeriod;
            PrevDateTime = prevDate;
        }
    }
}