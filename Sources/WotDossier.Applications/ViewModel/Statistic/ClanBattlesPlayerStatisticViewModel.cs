using System.Collections.Generic;
using WotDossier.Applications.Logic;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class ClanBattlesPlayerStatisticViewModel : PlayerStatisticViewModel, IClanBattlesAchievements
    {
        public ClanBattlesPlayerStatisticViewModel(PlayerStatisticEntity stat) : this(stat, new List<StatisticSlice>())
        {
        }

        public ClanBattlesPlayerStatisticViewModel(PlayerStatisticEntity stat, List<StatisticSlice> list)
            : base(stat, list)
        {
            #region Ratings init

            //PR-->
            //Private Rating
            Rating_IntegratedValue = stat.RatingIntegratedValue;
            Rating_IntegratedPlace = stat.RatingIntegratedPlace;
            //W/B-->
            //Victories/Battles
            Rating_BattleAvgPerformanceValue = stat.RatingWinsRatioValue;
            Rating_BattleAvgPerformancePlace = stat.RatingWinsRatioPlace;
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
            //MXP-->
            //Max Experience
            Rating_MaxXpValue = stat.RatingMaxXpValue;
            Rating_MaxXpPlace = stat.RatingMaxXpPlace;
            //HR-->
            //Hits Percent
            Rating_HitsPercentsValue = stat.RatingHitsPercentsValue;
            Rating_HitsPercentsPlace = stat.RatingHitsPercentsPlace;

            #endregion

            #region Achievements

            if (stat.AchievementsIdObject != null)
            {
                Mapper.Map<IClanBattlesAchievements>(stat.AchievementsIdObject, this);
            }

            #endregion
        }
    }
}