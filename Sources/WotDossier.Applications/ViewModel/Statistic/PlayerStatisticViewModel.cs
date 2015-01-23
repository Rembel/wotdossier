using System;
using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Applications.Model;
using WotDossier.Dal;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract class PlayerStatisticViewModel : StatisticViewModelBase
    {
        private ClanModel _clan;

        #region Common

        protected PlayerStatisticViewModel()
        {
        }

        public string Name { get; set; }

        public long AccountId { get; set; }

        /// <summary>
        ///     Player account created
        /// </summary>
        public DateTime Created { get; set; }

        #endregion

        #region Rating

        //GR-->
        //Global Rating
        public int Rating_IntegratedValue { get; set; }

        public int Rating_IntegratedPlace { get; set; }

        //W/B-->
        //Victories/Battles
        public double Rating_BattleAvgPerformanceValue { get; set; }

        public int Rating_BattleAvgPerformancePlace { get; set; }

        //E/B-->
        //Average Experience per Battle
        public double Rating_BattleAvgXpValue { get; set; }

        public int Rating_BattleAvgXpPlace { get; set; }

        //WIN-->
        //Victories
        public int Rating_BattleWinsValue { get; set; }

        public int Rating_BattleWinsPlace { get; set; }

        //GPL-->
        //Battles Participated
        public int Rating_BattlesValue { get; set; }

        public int Rating_BattlesPlace { get; set; }

        //CPT-->
        //Capture Points
        public int Rating_CapturedPointsValue { get; set; }

        public int Rating_CapturedPointsPlace { get; set; }

        //DMG-->
        //Damage Caused
        public int Rating_DamageDealtValue { get; set; }

        public int Rating_DamageDealtPlace { get; set; }

        //DPT-->
        //Defense Points
        public int Rating_DroppedPointsValue { get; set; }

        public int Rating_DroppedPointsPlace { get; set; }

        //FRG-->
        //Targets Destroyed
        public int Rating_FragsValue { get; set; }

        public int Rating_FragsPlace { get; set; }

        //SPT-->
        //Targets Detected
        public int Rating_SpottedValue { get; set; }

        public int Rating_SpottedPlace { get; set; }

        //EXP-->
        //Total Experience
        public int Rating_XpValue { get; set; }

        public int Rating_XpPlace { get; set; }

        //HR-->
        //Hits Percent
        public int Rating_HitsPercentsPlace { get; set; }
        public double Rating_HitsPercentsValue { get; set; }

        //MXP-->
        //Max Experience
        public int Rating_MaxXpPlace { get; set; }
        public int Rating_MaxXpValue { get; set; }

        #endregion

        private PlayerStatisticViewModel TypedPrevStatistic
        {
            get { return (PlayerStatisticViewModel)PrevStatisticSlice; }
        }

        #region Rating delta

        public int Rating_IntegratedValueDelta
        {
            get { return Rating_IntegratedValue - TypedPrevStatistic.Rating_IntegratedValue; }
        }

        public int Rating_IntegratedPlaceDelta
        {
            get { return Rating_IntegratedPlace - TypedPrevStatistic.Rating_IntegratedPlace; }
        }

        public double Rating_BattleAvgPerformanceValueDelta
        {
            get { return Rating_BattleAvgPerformanceValue - TypedPrevStatistic.Rating_BattleAvgPerformanceValue; }
        }

        public int Rating_BattleAvgPerformancePlaceDelta
        {
            get { return Rating_BattleAvgPerformancePlace - TypedPrevStatistic.Rating_BattleAvgPerformancePlace; }
        }

        public double Rating_BattleAvgXpValueDelta
        {
            get { return Rating_BattleAvgXpValue - TypedPrevStatistic.Rating_BattleAvgXpValue; }
        }

        public int Rating_BattleAvgXpPlaceDelta
        {
            get { return Rating_BattleAvgXpPlace - TypedPrevStatistic.Rating_BattleAvgXpPlace; }
        }

        public int Rating_BattleWinsValueDelta
        {
            get { return Rating_BattleWinsValue - TypedPrevStatistic.Rating_BattleWinsValue; }
        }

        public int Rating_BattleWinsPlaceDelta
        {
            get { return Rating_BattleWinsPlace - TypedPrevStatistic.Rating_BattleWinsPlace; }
        }

        public int Rating_BattlesValueDelta
        {
            get { return Rating_BattlesValue - TypedPrevStatistic.Rating_BattlesValue; }
        }

        public int Rating_BattlesPlaceDelta
        {
            get { return Rating_BattlesPlace - TypedPrevStatistic.Rating_BattlesPlace; }
        }

        public int Rating_CapturedPointsValueDelta
        {
            get { return Rating_CapturedPointsValue - TypedPrevStatistic.Rating_CapturedPointsValue; }
        }

        public int Rating_CapturedPointsPlaceDelta
        {
            get { return Rating_CapturedPointsPlace - TypedPrevStatistic.Rating_CapturedPointsPlace; }
        }

        public int Rating_DamageDealtValueDelta
        {
            get { return Rating_DamageDealtValue - TypedPrevStatistic.Rating_DamageDealtValue; }
        }

        public int Rating_DamageDealtPlaceDelta
        {
            get { return Rating_DamageDealtPlace - TypedPrevStatistic.Rating_DamageDealtPlace; }
        }

        public int Rating_DroppedPointsValueDelta
        {
            get { return Rating_DroppedPointsValue - TypedPrevStatistic.Rating_DroppedPointsValue; }
        }

        public int Rating_DroppedPointsPlaceDelta
        {
            get { return Rating_DroppedPointsPlace - TypedPrevStatistic.Rating_DroppedPointsPlace; }
        }

        public int Rating_FragsValueDelta
        {
            get { return Rating_FragsValue - TypedPrevStatistic.Rating_FragsValue; }
        }

        public int Rating_FragsPlaceDelta
        {
            get { return Rating_FragsPlace - TypedPrevStatistic.Rating_FragsPlace; }
        }

        public int Rating_SpottedValueDelta
        {
            get { return Rating_SpottedValue - TypedPrevStatistic.Rating_SpottedValue; }
        }

        public int Rating_SpottedPlaceDelta
        {
            get { return Rating_SpottedPlace - TypedPrevStatistic.Rating_SpottedPlace; }
        }

        public int Rating_XpValueDelta
        {
            get { return Rating_XpValue - TypedPrevStatistic.Rating_XpValue; }
        }

        public int Rating_XpPlaceDelta
        {
            get { return Rating_XpPlace - TypedPrevStatistic.Rating_XpPlace; }
        }

        public int Rating_MaxXpValueDelta
        {
            get { return Rating_MaxXpValue - TypedPrevStatistic.Rating_MaxXpValue; }
        }

        public int Rating_MaxXpPlaceDelta
        {
            get { return Rating_MaxXpPlace - TypedPrevStatistic.Rating_MaxXpPlace; }
        }

        public double Rating_HitsPercentsValueDelta
        {
            get { return Rating_HitsPercentsValue - TypedPrevStatistic.Rating_HitsPercentsValue; }
        }

        public int Rating_HitsPercentsPlaceDelta
        {
            get { return Rating_HitsPercentsPlace - TypedPrevStatistic.Rating_HitsPercentsPlace; }
        }

        #endregion

        public string PerformanceRatingLink
        {
            get { return string.Format(RatingHelper.NOOBMETER_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, Name); }
        }

        public string KievArmorRatingLink
        {
            get { return string.Format(RatingHelper.ARNORKIEV_STATISTIC_LINK_FORMAT, Name); }
        }

        public string EffRatingLink
        {
            get { return string.Format(RatingHelper.WOTNEWS_STATISTIC_LINK_FORMAT, Name); }
        }

        public string NameLink
        {
            get { return string.Format(RatingHelper.WG_STATISTIC_LINK_FORMAT, SettingsReader.Get().Server, AccountId, Name); }
        }

        public ClanModel Clan
        {
            get { return _clan; }
            set
            {
                _clan = value;
                OnPropertyChanged("Clan");
            }
        }

        protected PlayerStatisticViewModel(StatisticEntity stat, List<PlayerStatisticViewModel> list) : base(stat.Updated, list)
        {
            BattlesCount = stat.BattlesCount;
            Wins = stat.Wins;
            Losses = stat.Losses;
            SurvivedBattles = stat.SurvivedBattles;
            Xp = stat.Xp;
            MaxXp = stat.MaxXp;
            Frags = stat.Frags;
            //TODO: field MaxFrags
            MaxFrags = stat.MaxFrags;
            Spotted = stat.Spotted;
            HitsPercents = stat.HitsPercents;
            DamageDealt = stat.DamageDealt;
            DamageTaken = stat.DamageTaken;
            MaxDamage = stat.MaxDamage;
            CapturePoints = stat.CapturePoints;
            DroppedCapturePoints = stat.DroppedCapturePoints;
            //Created = stat.PlayerIdObject.Creaded);
            Updated = stat.Updated;
            Tier = stat.AvgLevel;
            MarkOfMastery = stat.MarkOfMastery;

            RBR = stat.RBR;
            PerformanceRating = stat.PerformanceRating;
            WN8Rating = stat.WN8Rating;
        }
    }
}