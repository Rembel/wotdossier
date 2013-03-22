using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;
using WotDossier.Domain.Entities;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    public class PlayerStatisticViewModel : INotifyPropertyChanged
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

        #region Fields

        private readonly IEnumerable<PlayerStatisticViewModel> _list;
        private DateTime _previousDate;
        private PlayerStatisticViewModel _prevPlayerStatistic;
        private double _effRating;
        private double _wn6Rating;
        private double _kievArmorRating;
        private int _battlesPerDay;

        #endregion

        #region Common

        public string Name { get; set; }

        /// <summary>
        /// Player account created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public int BattlesCount { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int SurvivedBattles { get; set; }

        public int Xp { get; set; }

        public int BattleAvgXp { get; set; }

        public int MaxXp { get; set; }

        public int Frags { get; set; }

        public int Spotted { get; set; }

        public int HitsPercents { get; set; }

        public int DamageDealt { get; set; }

        public int CapturePoints { get; set; }

        public int DroppedCapturePoints { get; set; }

        #endregion

        #region Percents

        public double WinsPercent { get; set; }

        public double LossesPercent { get; set; }

        public double SurvivedBattlesPercent { get; set; }

        #endregion

        #region Common delta

        public int BattlesCountDelta
        {
            get { return BattlesCount - _prevPlayerStatistic.BattlesCount; }
        }

        public int WinsDelta
        {
            get { return Wins - _prevPlayerStatistic.Wins; }
        }

        public double WinsPercentDelta
        {
            get { return WinsPercent - _prevPlayerStatistic.WinsPercent; }
        }

        public int LossesDelta
        {
            get { return Losses - _prevPlayerStatistic.Losses; }
        }

        public double LossesPercentDelta
        {
            get { return LossesPercent - _prevPlayerStatistic.LossesPercent; }
        }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - _prevPlayerStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - _prevPlayerStatistic.SurvivedBattlesPercent; }
        }

        public int XpDelta
        {
            get { return Xp - _prevPlayerStatistic.Xp; }
        }

        public int BattleAvgXpDelta
        {
            get { return BattleAvgXp - _prevPlayerStatistic.BattleAvgXp; }
        }

        public int MaxXpDelta
        {
            get { return MaxXp - _prevPlayerStatistic.MaxXp; }
        }

        public int FragsDelta
        {
            get { return Frags - _prevPlayerStatistic.Frags; }
        }

        public int SpottedDelta
        {
            get { return Spotted - _prevPlayerStatistic.Spotted; }
        }

        public int HitsPercentsDelta
        {
            get { return HitsPercents - _prevPlayerStatistic.HitsPercents; }
        }

        public int DamageDealtDelta
        {
            get { return DamageDealt - _prevPlayerStatistic.DamageDealt; }
        }

        public int CapturePointsDelta
        {
            get { return CapturePoints - _prevPlayerStatistic.CapturePoints; }
        }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - _prevPlayerStatistic.DroppedCapturePoints; }
        }

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
            get { return Rating_IntegratedValue - _prevPlayerStatistic.Rating_IntegratedValue; }
        }

        public int Rating_IntegratedPlaceDelta
        {
            get { return Rating_IntegratedPlace - _prevPlayerStatistic.Rating_IntegratedPlace; }
        }

        public int Rating_BattleAvgPerformanceValueDelta
        {
            get { return Rating_BattleAvgPerformanceValue - _prevPlayerStatistic.Rating_BattleAvgPerformanceValue; }
        }

        public int Rating_BattleAvgPerformancePlaceDelta
        {
            get { return Rating_BattleAvgPerformancePlace - _prevPlayerStatistic.Rating_BattleAvgPerformancePlace; }
        }

        public int Rating_BattleAvgXpValueDelta
        {
            get { return Rating_BattleAvgXpValue - _prevPlayerStatistic.Rating_BattleAvgXpValue; }
        }

        public int Rating_BattleAvgXpPlaceDelta
        {
            get { return Rating_BattleAvgXpPlace - _prevPlayerStatistic.Rating_BattleAvgXpPlace; }
        }

        public int Rating_BattleWinsValueDelta
        {
            get { return Rating_BattleWinsValue - _prevPlayerStatistic.Rating_BattleWinsValue; }
        }

        public int Rating_BattleWinsPlaceDelta
        {
            get { return Rating_BattleWinsPlace - _prevPlayerStatistic.Rating_BattleWinsPlace; }
        }

        public int Rating_BattlesValueDelta
        {
            get { return Rating_BattlesValue - _prevPlayerStatistic.Rating_BattlesValue; }
        }

        public int Rating_BattlesPlaceDelta
        {
            get { return Rating_BattlesPlace - _prevPlayerStatistic.Rating_BattlesPlace; }
        }

        public int Rating_CapturedPointsValueDelta
        {
            get { return Rating_CapturedPointsValue - _prevPlayerStatistic.Rating_CapturedPointsValue; }
        }

        public int Rating_CapturedPointsPlaceDelta
        {
            get { return Rating_CapturedPointsPlace - _prevPlayerStatistic.Rating_CapturedPointsPlace; }
        }

        public int Rating_DamageDealtValueDelta
        {
            get { return Rating_DamageDealtValue - _prevPlayerStatistic.Rating_DamageDealtValue; }
        }

        public int Rating_DamageDealtPlaceDelta
        {
            get { return Rating_DamageDealtPlace - _prevPlayerStatistic.Rating_DamageDealtPlace; }
        }

        public int Rating_DroppedPointsValueDelta
        {
            get { return Rating_DroppedPointsValue - _prevPlayerStatistic.Rating_DroppedPointsValue; }
        }

        public int Rating_DroppedPointsPlaceDelta
        {
            get { return Rating_DroppedPointsPlace - _prevPlayerStatistic.Rating_DroppedPointsPlace; }
        }

        public int Rating_FragsValueDelta
        {
            get { return Rating_FragsValue - _prevPlayerStatistic.Rating_FragsValue; }
        }

        public int Rating_FragsPlaceDelta
        {
            get { return Rating_FragsPlace - _prevPlayerStatistic.Rating_FragsPlace; }
        }

        public int Rating_SpottedValueDelta
        {
            get { return Rating_SpottedValue - _prevPlayerStatistic.Rating_SpottedValue; }
        }

        public int Rating_SpottedPlaceDelta
        {
            get { return Rating_SpottedPlace - _prevPlayerStatistic.Rating_SpottedPlace; }
        }

        public int Rating_XpValueDelta
        {
            get { return Rating_XpValue - _prevPlayerStatistic.Rating_XpValue; }
        }

        public int Rating_XpPlaceDelta
        {
            get { return Rating_XpPlace - _prevPlayerStatistic.Rating_XpPlace; }
        }

        #endregion

        public PlayerStatisticClanViewModel Clan { get; set; }

        public void SetPreviousDate(DateTime date)
        {
            _previousDate = date;
            _prevPlayerStatistic = _list.OrderBy(x => x.Updated).FirstOrDefault(x => x.Updated <= date) ?? this;
            OnPropertyChanged(PropBattlesCountDelta);
            OnPropertyChanged(PropWinsDelta);
            OnPropertyChanged(PropLossesDelta);
            OnPropertyChanged(PropSurvivedBattlesDelta);
            OnPropertyChanged(PropXpDelta);
            OnPropertyChanged(PropBattleAvgXpDelta);
            OnPropertyChanged(PropMaxXpDelta);
            OnPropertyChanged(PropFragsDelta);
            OnPropertyChanged(PropSpottedDelta);
            OnPropertyChanged(PropHitsPercentsDelta);
            OnPropertyChanged(PropDamageDealtDelta);
            OnPropertyChanged(PropCapturePointsDelta);
            OnPropertyChanged(PropDroppedCapturePointsDelta);
        }

        public PlayerStatisticViewModel(PlayerStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public PlayerStatisticViewModel(PlayerStatisticEntity stat, IEnumerable<PlayerStatisticViewModel> list)
        {
            _list = list;

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

            double battles = BattlesCount;
            double winrate = WinsPercent;
            double averageDamage = DamageDealt / battles;
            double avgFrags = Frags / battles;
            double avgSpot = Spotted / battles;
            double avgCap = CapturePoints/ battles;
            double avgDef = DroppedCapturePoints/ battles;

            _effRating = RatingHelper.CalcER(averageDamage, stat.AvgLevel, avgFrags, avgSpot, avgCap, avgDef);
            _wn6Rating = RatingHelper.CalcWN6(averageDamage, stat.AvgLevel, avgFrags, avgSpot, avgDef, winrate);
            _kievArmorRating = RatingHelper.CalcKievArmorRating(battles, Rating_BattleAvgXpValue, averageDamage, winrate / 100.0, avgFrags, avgSpot, avgCap, avgDef);

            _prevPlayerStatistic = _list.Where(x => x.Updated <= Updated).OrderByDescending(x => x.Updated).FirstOrDefault() ?? this;
            _previousDate = Updated;
        }

        public double WN6Rating
        {
            get { return _wn6Rating; }
        }

        public double EffRating
        {
            get { return _effRating; }
        }

        public double KievArmorRating
        {
            get { return _kievArmorRating; }
        }

        public int BattlesPerDay
        {
            get { return _battlesPerDay; }
            set { _battlesPerDay = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
