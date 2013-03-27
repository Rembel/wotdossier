using System;
using System.Collections.Generic;
using WotDossier.Common;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerStatisticViewModel : PeriodStatisticViewModel<PlayerStatisticViewModel>
    {
        #region Constants

        public static readonly string PropBattlesCountDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.BattlesCountDelta);
        public static readonly string PropWinsDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.WinsDelta);
        public static readonly string PropWinsPercentDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.WinsPercentDelta);
        public static readonly string PropLossesDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.LossesDelta);
        public static readonly string PropLossesPercentDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.LossesPercentDelta);
        public static readonly string PropSurvivedBattlesDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.SurvivedBattlesPercentDelta);
        public static readonly string PropSurvivedBattlesPercentDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.SurvivedBattlesDelta);
        public static readonly string PropXpDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.XpDelta);
        public static readonly string PropBattleAvgXpDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.BattleAvgXpDelta);
        public static readonly string PropMaxXpDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.MaxXpDelta);
        public static readonly string PropFragsDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.FragsDelta);
        public static readonly string PropSpottedDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.SpottedDelta);
        public static readonly string PropHitsPercentsDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.HitsPercentsDelta);
        public static readonly string PropDamageDealtDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.DamageDealtDelta);
        public static readonly string PropCapturePointsDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.CapturePointsDelta);
        public static readonly string PropDroppedCapturePointsDelta = TypeHelper<PlayerStatisticViewModel>.PropertyName(v => v.DroppedCapturePointsDelta);

        #endregion

        #region Common

        public string Name { get; set; }

        /// <summary>
        /// Player account created
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
        public int Rating_BattleAvgPerformanceValue { get; set; }

        public int Rating_BattleAvgPerformancePlace { get; set; }

        //E/B-->
        //Average Experience per Battle
        public int Rating_BattleAvgXpValue { get; set; }

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

        #endregion

        #region Rating delta

        public int Rating_IntegratedValueDelta
        {
            get { return Rating_IntegratedValue - PrevPlayerStatistic.Rating_IntegratedValue; }
        }

        public int Rating_IntegratedPlaceDelta
        {
            get { return Rating_IntegratedPlace - PrevPlayerStatistic.Rating_IntegratedPlace; }
        }

        public int Rating_BattleAvgPerformanceValueDelta
        {
            get { return Rating_BattleAvgPerformanceValue - PrevPlayerStatistic.Rating_BattleAvgPerformanceValue; }
        }

        public int Rating_BattleAvgPerformancePlaceDelta
        {
            get { return Rating_BattleAvgPerformancePlace - PrevPlayerStatistic.Rating_BattleAvgPerformancePlace; }
        }

        public int Rating_BattleAvgXpValueDelta
        {
            get { return Rating_BattleAvgXpValue - PrevPlayerStatistic.Rating_BattleAvgXpValue; }
        }

        public int Rating_BattleAvgXpPlaceDelta
        {
            get { return Rating_BattleAvgXpPlace - PrevPlayerStatistic.Rating_BattleAvgXpPlace; }
        }

        public int Rating_BattleWinsValueDelta
        {
            get { return Rating_BattleWinsValue - PrevPlayerStatistic.Rating_BattleWinsValue; }
        }

        public int Rating_BattleWinsPlaceDelta
        {
            get { return Rating_BattleWinsPlace - PrevPlayerStatistic.Rating_BattleWinsPlace; }
        }

        public int Rating_BattlesValueDelta
        {
            get { return Rating_BattlesValue - PrevPlayerStatistic.Rating_BattlesValue; }
        }

        public int Rating_BattlesPlaceDelta
        {
            get { return Rating_BattlesPlace - PrevPlayerStatistic.Rating_BattlesPlace; }
        }

        public int Rating_CapturedPointsValueDelta
        {
            get { return Rating_CapturedPointsValue - PrevPlayerStatistic.Rating_CapturedPointsValue; }
        }

        public int Rating_CapturedPointsPlaceDelta
        {
            get { return Rating_CapturedPointsPlace - PrevPlayerStatistic.Rating_CapturedPointsPlace; }
        }

        public int Rating_DamageDealtValueDelta
        {
            get { return Rating_DamageDealtValue - PrevPlayerStatistic.Rating_DamageDealtValue; }
        }

        public int Rating_DamageDealtPlaceDelta
        {
            get { return Rating_DamageDealtPlace - PrevPlayerStatistic.Rating_DamageDealtPlace; }
        }

        public int Rating_DroppedPointsValueDelta
        {
            get { return Rating_DroppedPointsValue - PrevPlayerStatistic.Rating_DroppedPointsValue; }
        }

        public int Rating_DroppedPointsPlaceDelta
        {
            get { return Rating_DroppedPointsPlace - PrevPlayerStatistic.Rating_DroppedPointsPlace; }
        }

        public int Rating_FragsValueDelta
        {
            get { return Rating_FragsValue - PrevPlayerStatistic.Rating_FragsValue; }
        }

        public int Rating_FragsPlaceDelta
        {
            get { return Rating_FragsPlace - PrevPlayerStatistic.Rating_FragsPlace; }
        }

        public int Rating_SpottedValueDelta
        {
            get { return Rating_SpottedValue - PrevPlayerStatistic.Rating_SpottedValue; }
        }

        public int Rating_SpottedPlaceDelta
        {
            get { return Rating_SpottedPlace - PrevPlayerStatistic.Rating_SpottedPlace; }
        }

        public int Rating_XpValueDelta
        {
            get { return Rating_XpValue - PrevPlayerStatistic.Rating_XpValue; }
        }

        public int Rating_XpPlaceDelta
        {
            get { return Rating_XpPlace - PrevPlayerStatistic.Rating_XpPlace; }
        }

        #endregion

        public PlayerStatisticClanViewModel Clan { get; set; }

        public PlayerStatisticViewModel(PlayerStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public PlayerStatisticViewModel(PlayerStatisticEntity stat, IEnumerable<PlayerStatisticViewModel> list)
            : base(stat.Updated, list)
        {
            #region Common init

            BattlesCount = stat.BattlesCount;
            Wins = stat.Wins;
            WinsPercent = Wins/(double) BattlesCount*100.0;
            Losses = stat.Losses;
            LossesPercent = Losses/(double) BattlesCount*100.0;
            SurvivedBattles = stat.SurvivedBattles;
            SurvivedBattlesPercent = SurvivedBattles/(double) BattlesCount*100.0;
            Xp = stat.Xp;
            BattleAvgXp = stat.BattleAvgXp;
            MaxXp = stat.MaxXp;
            Frags = stat.Frags;
            Spotted = stat.Spotted;
            HitsPercents = stat.HitsPercents;
            DamageDealt = stat.DamageDealt;
            CapturePoints = stat.CapturePoints;
            DroppedCapturePoints = stat.DroppedCapturePoints;
            //Created = stat.PlayerIdObject.Creaded);
            Updated = stat.Updated;

            double battles = BattlesCount;
            double winrate = WinsPercent;
            double averageDamage = DamageDealt / battles;
            double avgFrags = Frags / battles;
            double avgSpot = Spotted / battles;
            double avgCap = CapturePoints / battles;
            double avgDef = DroppedCapturePoints / battles;

            EffRating = RatingHelper.CalcER(averageDamage, stat.AvgLevel, avgFrags, avgSpot, avgCap, avgDef);
            WN6Rating = RatingHelper.CalcWN6(averageDamage, stat.AvgLevel, avgFrags, avgSpot, avgDef, winrate);
            KievArmorRating = RatingHelper.CalcKievArmorRating(battles, stat.BattleAvgXp, averageDamage, winrate / 100.0, avgFrags, avgSpot, avgCap, avgDef);
            
            #endregion

            #region Ratings init

            //GR-->
            //Global Rating
            Rating_IntegratedValue = stat.RatingIntegratedValue;
            Rating_IntegratedPlace = stat.RatingIntegratedPlace;
            //W/B-->
            //Victories/Battles
            Rating_BattleAvgPerformanceValue = stat.RatingBattleAvgPerformanceValue;
            Rating_BattleAvgPerformancePlace = stat.RatingBattleAvgPerformancePlace;
            //E/B-->
            //Average Experience per Battle
            Rating_BattleAvgXpValue = stat.RatingBattleAvgXpValue;
            Rating_BattleAvgXpPlace = stat.RatingBattleAvgXpPlace;
            //WIN-->
            //Victories
            Rating_BattleWinsValue = stat.RatingBattleWinsValue;
            Rating_BattleWinsPlace = stat.RatingBattleWinsPlace;
            //GPL-->
            //Battles Participated
            Rating_BattlesValue = stat.RatingBattlesValue;
            Rating_BattlesPlace = stat.RatingBattlesPlace;
            //CPT-->
            //Capture Points
            Rating_CapturedPointsValue = stat.RatingCapturedPointsValue;
            Rating_CapturedPointsPlace = stat.RatingCapturedPointsPlace;
            //DMG-->
            //Damage Caused
            Rating_DamageDealtValue = stat.RatingDamageDealtValue;
            Rating_DamageDealtPlace = stat.RatingDamageDealtPlace;
            //DPT-->
            //Defense Points
            Rating_DroppedPointsValue = stat.RatingDroppedPointsValue;
            Rating_DroppedPointsPlace = stat.RatingDroppedPointsPlace;
            //FRG-->
            //Targets Destroyed
            Rating_FragsValue = stat.RatingFragsValue;
            Rating_FragsPlace = stat.RatingFragsPlace;
            //SPT-->
            //Targets Detected
            Rating_SpottedValue = stat.RatingSpottedValue;
            Rating_SpottedPlace = stat.RatingSpottedPlace;
            //EXP-->
            //Total Experience
            Rating_XpValue = stat.RatingXpValue;
            Rating_XpPlace = stat.RatingXpPlace;

            #endregion
        }
    }
}
