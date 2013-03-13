using System;
using System.Collections.Generic;
using System.ComponentModel;
using WotDossier.Common;
using WotDossier.Domain.Player;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    public class CommonPlayerStatistic : INotifyPropertyChanged
    {
        public static readonly string PropBattlesCountDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.BattlesCountDelta);
        public static readonly string PropWinsDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.WinsDelta);
        public static readonly string PropWinsPercentDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.WinsPercentDelta);
        public static readonly string PropLossesDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.LossesDelta);
        public static readonly string PropLossesPercentDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.LossesPercentDelta);
        public static readonly string PropSurvivedBattlesDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.SurvivedBattlesPercentDelta);
        public static readonly string PropSurvivedBattlesPercentDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.SurvivedBattlesDelta);
        public static readonly string PropXpDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.XpDelta);
        public static readonly string PropBattleAvgXpDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.BattleAvgXpDelta);
        public static readonly string PropMaxXpDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.MaxXpDelta);
        public static readonly string PropFragsDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.FragsDelta);
        public static readonly string PropSpottedDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.SpottedDelta);
        public static readonly string PropHitsPercentsDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.HitsPercentsDelta);
        public static readonly string PropDamageDealtDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.DamageDealtDelta);
        public static readonly string PropCapturePointsDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.CapturePointsDelta);
        public static readonly string PropDroppedCapturePointsDelta = TypeHelper<CommonPlayerStatistic>.PropertyName(v => v.DroppedCapturePointsDelta);

        private readonly List<CommonPlayerStatistic> _list;
        private DateTime _previousDate;
        private CommonPlayerStatistic _prevPlayerStatistic;

        #region Common

        public int BattlesCount { get; set; }

        public int BattlesCountDelta
        {
            get { return BattlesCount - _prevPlayerStatistic.BattlesCount; }
        }

        public int Wins { get; set; }

        public int WinsDelta
        {
            get { return Wins - _prevPlayerStatistic.Wins; }
        }

        public double WinsPercent { get; set; }

        public double WinsPercentDelta
        {
            get { return WinsPercent - _prevPlayerStatistic.WinsPercent; }
        }

        public int Losses { get; set; }

        public int LossesDelta
        {
            get { return Losses - _prevPlayerStatistic.Losses; }
        }

        public double LossesPercent { get; set; }

        public double LossesPercentDelta
        {
            get { return LossesPercent - _prevPlayerStatistic.LossesPercent; }
        }

        public int SurvivedBattles { get; set; }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - _prevPlayerStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercent { get; set; }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - _prevPlayerStatistic.SurvivedBattlesPercent; }
        }

        public int Xp { get; set; }

        public int XpDelta
        {
            get { return Xp - _prevPlayerStatistic.Xp; }
        }

        public int BattleAvgXp { get; set; }

        public int BattleAvgXpDelta
        {
            get { return BattleAvgXp - _prevPlayerStatistic.BattleAvgXp; }
        }

        public int MaxXp { get; set; }

        public int MaxXpDelta
        {
            get { return MaxXp - _prevPlayerStatistic.MaxXp; }
        }

        public int Frags { get; set; }

        public int FragsDelta
        {
            get { return Frags - _prevPlayerStatistic.Frags; }
        }

        public int Spotted { get; set; }

        public int SpottedDelta
        {
            get { return Spotted - _prevPlayerStatistic.Spotted; }
        }

        public int HitsPercents { get; set; }

        public int HitsPercentsDelta
        {
            get { return HitsPercents - _prevPlayerStatistic.HitsPercents; }
        }

        public int DamageDealt { get; set; }

        public int DamageDealtDelta
        {
            get { return DamageDealt - _prevPlayerStatistic.DamageDealt; }
        }

        public int CapturePoints { get; set; }

        public int CapturePointsDelta
        {
            get { return CapturePoints - _prevPlayerStatistic.CapturePoints; }
        }

        public int DroppedCapturePoints { get; set; }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - _prevPlayerStatistic.DroppedCapturePoints; }
        }

        #endregion

        #region Rating

        //GR-->
        //Global Rating
        public int Rating_IntegratedValue { get; set; }
        public int Rating_IntegratedValueDelta
        {
            get { return Rating_IntegratedValue - _prevPlayerStatistic.Rating_IntegratedValue; }
        }

        public int Rating_IntegratedPlace { get; set; }
        public int Rating_IntegratedPlaceDelta
        {
            get { return Rating_IntegratedPlace - _prevPlayerStatistic.Rating_IntegratedPlace; }
        }

        //W/B-->
        //Victories/Battles
        public int Rating_BattleAvgPerformanceValue { get; set; }
        public int Rating_BattleAvgPerformanceValueDelta
        {
            get { return Rating_BattleAvgPerformanceValue - _prevPlayerStatistic.Rating_BattleAvgPerformanceValue; }
        }

        public int Rating_BattleAvgPerformancePlace { get; set; }
        public int Rating_BattleAvgPerformancePlaceDelta
        {
            get { return Rating_BattleAvgPerformancePlace - _prevPlayerStatistic.Rating_BattleAvgPerformancePlace; }
        }

        //E/B-->
        //Average Experience per Battle
        public int Rating_BattleAvgXpValue { get; set; }
        public int Rating_BattleAvgXpValueDelta
        {
            get { return Rating_BattleAvgXpValue - _prevPlayerStatistic.Rating_BattleAvgXpValue; }
        }

        public int Rating_BattleAvgXpPlace { get; set; }
        public int Rating_BattleAvgXpPlaceDelta
        {
            get { return Rating_BattleAvgXpPlace - _prevPlayerStatistic.Rating_BattleAvgXpPlace; }
        }

        //WIN-->
        //Victories
        public int Rating_BattleWinsValue { get; set; }
        public int Rating_BattleWinsValueDelta
        {
            get { return Rating_BattleWinsValue - _prevPlayerStatistic.Rating_BattleWinsValue; }
        }

        public int Rating_BattleWinsPlace { get; set; }
        public int Rating_BattleWinsPlaceDelta
        {
            get { return Rating_BattleWinsPlace - _prevPlayerStatistic.Rating_BattleWinsPlace; }
        }

        //GPL-->
        //Battles Participated
        public int Rating_BattlesValue { get; set; }
        public int Rating_BattlesValueDelta
        {
            get { return Rating_BattlesValue - _prevPlayerStatistic.Rating_BattlesValue; }
        }

        public int Rating_BattlesPlace { get; set; }
        public int Rating_BattlesPlaceDelta
        {
            get { return Rating_BattlesPlace - _prevPlayerStatistic.Rating_BattlesPlace; }
        }

        //CPT-->
        //Capture Points
        public int Rating_CapturedPointsValue { get; set; }
        public int Rating_CapturedPointsValueDelta
        {
            get { return Rating_CapturedPointsValue - _prevPlayerStatistic.Rating_CapturedPointsValue; }
        }

        public int Rating_CapturedPointsPlace { get; set; }
        public int Rating_CapturedPointsPlaceDelta
        {
            get { return Rating_CapturedPointsPlace - _prevPlayerStatistic.Rating_CapturedPointsPlace; }
        }

        //DMG-->
        //Damage Caused
        public int Rating_DamageDealtValue { get; set; }
        public int Rating_DamageDealtValueDelta
        {
            get { return Rating_DamageDealtValue - _prevPlayerStatistic.Rating_DamageDealtValue; }
        }

        public int Rating_DamageDealtPlace { get; set; }
        public int Rating_DamageDealtPlaceDelta
        {
            get { return Rating_DamageDealtPlace - _prevPlayerStatistic.Rating_DamageDealtPlace; }
        }

        //DPT-->
        //Defense Points
        public int Rating_DroppedPointsValue { get; set; }
        public int Rating_DroppedPointsValueDelta
        {
            get { return Rating_DroppedPointsValue - _prevPlayerStatistic.Rating_DroppedPointsValue; }
        }

        public int Rating_DroppedPointsPlace { get; set; }
        public int Rating_DroppedPointsPlaceDelta
        {
            get { return Rating_DroppedPointsPlace - _prevPlayerStatistic.Rating_DroppedPointsPlace; }
        }

        //FRG-->
        //Targets Destroyed
        public int Rating_FragsValue { get; set; }
        public int Rating_FragsValueDelta
        {
            get { return Rating_FragsValue - _prevPlayerStatistic.Rating_FragsValue; }
        }

        public int Rating_FragsPlace { get; set; }
        public int Rating_FragsPlaceDelta
        {
            get { return Rating_FragsPlace - _prevPlayerStatistic.Rating_FragsPlace; }
        }

        //SPT-->
        //Targets Detected
        public int Rating_SpottedValue { get; set; }
        public int Rating_SpottedValueDelta
        {
            get { return Rating_SpottedValue - _prevPlayerStatistic.Rating_SpottedValue; }
        }

        public int Rating_SpottedPlace { get; set; }
        public int Rating_SpottedPlaceDelta
        {
            get { return Rating_SpottedPlace - _prevPlayerStatistic.Rating_SpottedPlace; }
        }

        //EXP-->
        //Total Experience
        public int Rating_XpValue { get; set; }
        public int Rating_XpValueDelta
        {
            get { return Rating_XpValue - _prevPlayerStatistic.Rating_XpValue; }
        }

        public int Rating_XpPlace { get; set; }
        public int Rating_XpPlaceDelta
        {
            get { return Rating_XpPlace - _prevPlayerStatistic.Rating_XpPlace; }
        }

        #endregion


        public string Name { get; set; }
        public double Created { get; set; }
        public double Updated { get; set; }
        public DateTime Date { get; set; }
        
        public void SetPreviousDate(DateTime date)
        {
            _previousDate = date;
            _prevPlayerStatistic = _list.OrderBy(x => x.Date).FirstOrDefault(x => x.Date <= date) ?? this;
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

        public CommonPlayerStatistic(PlayerStat stat) : this(stat, new List<CommonPlayerStatistic>())
        {
        }

        public CommonPlayerStatistic(PlayerStat stat, List<CommonPlayerStatistic> list)
        {
            _list = list;

            #region Common init

            BattlesCount = stat.data.summary.Battles_count;
            Wins = stat.data.summary.Wins;
            WinsPercent = Wins/(double) BattlesCount*100.0;
            Losses = stat.data.summary.Losses;
            LossesPercent = Losses/(double) BattlesCount*100.0;
            SurvivedBattles = stat.data.summary.Survived_battles;
            SurvivedBattlesPercent = SurvivedBattles/(double) BattlesCount*100.0;
            Xp = stat.data.experience.Xp;
            BattleAvgXp = stat.data.experience.Battle_avg_xp;
            MaxXp = stat.data.experience.Max_xp;
            Frags = stat.data.battles.Frags;
            Spotted = stat.data.battles.Spotted;
            HitsPercents = stat.data.battles.Hits_percents;
            DamageDealt = stat.data.battles.Damage_dealt;
            CapturePoints = stat.data.battles.Capture_points;
            DroppedCapturePoints = stat.data.battles.Dropped_capture_points;
            Date = Utils.UnixDateToDateTime((long) stat.data.updated_at);
            Created = stat.data.created_at;
            Updated = stat.data.updated_at;
            Name = stat.data.name;

            #endregion

            #region Ratings init

            //GR-->
            //Global Rating
            Rating_IntegratedValue = stat.data.ratings.Integrated_rating.Value;
            Rating_IntegratedPlace = stat.data.ratings.Integrated_rating.Place;
            //W/B-->
            //Victories/Battles
            Rating_BattleAvgPerformanceValue = stat.data.ratings.Battle_avg_performance.Value;
            Rating_BattleAvgPerformancePlace = stat.data.ratings.Battle_avg_performance.Place;
            //E/B-->
            //Average Experience per Battle
            Rating_BattleAvgXpValue = stat.data.ratings.Battle_avg_xp.Value;
            Rating_BattleAvgXpPlace = stat.data.ratings.Battle_avg_xp.Place;
            //WIN-->
            //Victories
            Rating_BattleWinsValue = stat.data.ratings.Battle_wins.Value;
            Rating_BattleWinsPlace = stat.data.ratings.Battle_wins.Place;
            //GPL-->
            //Battles Participated
            Rating_BattlesValue = stat.data.ratings.Battles.Value;
            Rating_BattlesPlace = stat.data.ratings.Battles.Place;
            //CPT-->
            //Capture Points
            Rating_CapturedPointsValue = stat.data.ratings.Ctf_points.Value;
            Rating_CapturedPointsPlace = stat.data.ratings.Ctf_points.Place;
            //DMG-->
            //Damage Caused
            Rating_DamageDealtValue = stat.data.ratings.Damage_dealt.Value;
            Rating_DamageDealtPlace = stat.data.ratings.Damage_dealt.Place;
            //DPT-->
            //Defense Points
            Rating_DroppedPointsValue = stat.data.ratings.Dropped_ctf_points.Value;
            Rating_DroppedPointsPlace = stat.data.ratings.Dropped_ctf_points.Place;
            //FRG-->
            //Targets Destroyed
            Rating_FragsValue = stat.data.ratings.Frags.Value;
            Rating_FragsPlace = stat.data.ratings.Frags.Place;
            //SPT-->
            //Targets Detected
            Rating_SpottedValue = stat.data.ratings.Spotted.Value;
            Rating_SpottedPlace = stat.data.ratings.Spotted.Place;
            //EXP-->
            //Total Experience
            Rating_XpValue = stat.data.ratings.Xp.Value;
            Rating_XpPlace = stat.data.ratings.Xp.Place;

            #endregion

            _previousDate = Date;
            _prevPlayerStatistic = _list.OrderBy(x => x.Date).FirstOrDefault(x => x.Date <= Date) ?? this;
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
