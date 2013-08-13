using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WotDossier.Common;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.ViewModel
{
    public abstract class PeriodStatisticViewModel<T> : StatisticViewModelBase, INotifyPropertyChanged where T : StatisticViewModelBase
    {
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

        protected T PrevStatistic { get; set; }

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

        public void SetPreviousStatistic(T prevPlayerStatistic)
        {
            PrevStatistic = (T)((object)prevPlayerStatistic ?? this);

            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                OnPropertyChanged(propertyInfo.Name);
            }
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