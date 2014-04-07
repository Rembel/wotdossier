using System;
using WotDossier.Common;
using WotDossier.Domain.Player;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'PlayerStatistic'.
    /// </summary>
    [Serializable]
    public class PlayerStatisticEntity : StatisticEntity
    {
        #region Property names

        public static readonly string PropRatingIntegratedValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingIntegratedValue);
        public static readonly string PropRatingIntegratedPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingIntegratedPlace);
        public static readonly string PropRatingBattleAvgPerformanceValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgPerformanceValue);
        public static readonly string PropRatingBattleAvgPerformancePlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgPerformancePlace);
        public static readonly string PropRatingBattleAvgXpValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgXpValue);
        public static readonly string PropRatingBattleAvgXpPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleAvgXpPlace);
        public static readonly string PropRatingBattleWinsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleWinsValue);
        public static readonly string PropRatingBattleWinsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattleWinsPlace);
        public static readonly string PropRatingBattlesValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattlesValue);
        public static readonly string PropRatingBattlesPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingBattlesPlace);
        public static readonly string PropRatingCapturedPointsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingCapturedPointsValue);
        public static readonly string PropRatingCapturedPointsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingCapturedPointsPlace);
        public static readonly string PropRatingDamageDealtValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDamageDealtValue);
        public static readonly string PropRatingDamageDealtPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDamageDealtPlace);
        public static readonly string PropRatingDroppedPointsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDroppedPointsValue);
        public static readonly string PropRatingDroppedPointsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingDroppedPointsPlace);
        public static readonly string PropRatingFragsValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingFragsValue);
        public static readonly string PropRatingFragsPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingFragsPlace);
        public static readonly string PropRatingSpottedValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingSpottedValue);
        public static readonly string PropRatingSpottedPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingSpottedPlace);
        public static readonly string PropRatingXpValue = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingXpValue);
        public static readonly string PropRatingXpPlace = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.RatingXpPlace);
		public static readonly string PropAchievementsId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

        /// <summary>
        /// Gets/Sets the field "Rating_IntegratedValue".
        /// </summary>
        public virtual int RatingIntegratedValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_IntegratedPlace".
        /// </summary>
        public virtual int RatingIntegratedPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgPerformanceValue".
        /// </summary>
        public virtual double RatingBattleAvgPerformanceValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgPerformancePlace".
        /// </summary>
        public virtual int RatingBattleAvgPerformancePlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgXpValue".
        /// </summary>
        public virtual double RatingBattleAvgXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgXpPlace".
        /// </summary>
        public virtual int RatingBattleAvgXpPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleWinsValue".
        /// </summary>
        public virtual int RatingBattleWinsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleWinsPlace".
        /// </summary>
        public virtual int RatingBattleWinsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattlesValue".
        /// </summary>
        public virtual int RatingBattlesValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattlesPlace".
        /// </summary>
        public virtual int RatingBattlesPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_CapturedPointsValue".
        /// </summary>
        public virtual int RatingCapturedPointsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_CapturedPointsPlace".
        /// </summary>
        public virtual int RatingCapturedPointsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DamageDealtValue".
        /// </summary>
        public virtual int RatingDamageDealtValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DamageDealtPlace".
        /// </summary>
        public virtual int RatingDamageDealtPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DroppedPointsValue".
        /// </summary>
        public virtual int RatingDroppedPointsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_DroppedPointsPlace".
        /// </summary>
        public virtual int RatingDroppedPointsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_FragsValue".
        /// </summary>
        public virtual int RatingFragsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_FragsPlace".
        /// </summary>
        public virtual int RatingFragsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_SpottedValue".
        /// </summary>
        public virtual int RatingSpottedValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_SpottedPlace".
        /// </summary>
        public virtual int RatingSpottedPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_XpValue".
        /// </summary>
        public virtual int RatingXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_XpPlace".
        /// </summary>
        public virtual int RatingXpPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingHitsPercentsValue".
        /// </summary>
        public virtual double RatingHitsPercentsValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingHitsPercentsPlace".
        /// </summary>
        public virtual int RatingHitsPercentsPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingMaxXpValue".
        /// </summary>
        public virtual int RatingMaxXpValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "RatingMaxXpPlace".
        /// </summary>
        public virtual int RatingMaxXpPlace { get; set; }

        /// <summary>
        /// Gets/Sets the field "AchievementsId".
        /// </summary>
        public virtual int? AchievementsId { get; set; }

        /// <summary>
        /// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
        /// </summary>
        public virtual PlayerAchievementsEntity AchievementsIdObject { get; set; }

        /// <summary>
        /// Updates the ratings.
        /// </summary>
        /// <param name="ratings">The ratings.</param>
        public override void UpdateRatings(Ratings ratings)
        {
            #region Ratings init

            //GR-->
            //Global Rating
            //RatingIntegratedValue = ratings.Integrated_rating.Value;
            //RatingIntegratedPlace = ratings.Integrated_rating.Rank ?? 0;
            //W/B-->
            //Victories/Battles
            //wins_ratio 	������� ����� wins_ratio.rank 	numeric 	
            RatingBattleAvgPerformanceValue = ratings.wins_ratio.Value ?? 0;
            RatingBattleAvgPerformancePlace = ratings.wins_ratio.Rank ?? 0;
            //E/B-->
            //Average Experience per BattleJson
            //xp_avg 	������� ���� �� ��� xp_avg.rank 	numeric 	
            RatingBattleAvgXpValue = ratings.xp_avg.Value ?? 0;
            RatingBattleAvgXpPlace = ratings.xp_avg.Rank ?? 0;
            //WIN-->
            //Victories
            //RatingBattleWinsValue = ratings.Battle_wins.Value;
            //RatingBattleWinsPlace = ratings.Battle_wins.Rank ?? 0;
            //GPL-->
            //Battles Participated
            //battles_count 	���������� ���������� ��� battles_count.rank 	numeric 	
            RatingBattlesValue = (int) (ratings.battles_count.Value ?? 0);
            RatingBattlesPlace = ratings.battles_count.Rank ?? 0;
            //CPT-->
            //Capture Points
            //RatingCapturedPointsValue = ratings.Ctf_points.Value;
            //RatingCapturedPointsPlace = ratings.Ctf_points.Rank ?? 0;
            //DMG-->
            //Damage Caused
            //damage_dealt 	����� ��������� ���� damage_dealt.rank 	numeric 	
            RatingDamageDealtValue = (int) (ratings.damage_dealt.Value ?? 0);
            RatingDamageDealtPlace = ratings.damage_dealt.Rank ?? 0;
            //DPT-->
            //Defense Points
            //RatingDroppedPointsValue = ratings.Dropped_ctf_points.Value;
            //RatingDroppedPointsPlace = ratings.Dropped_ctf_points.Rank ?? 0;
            //FRG-->
            //Targets Destroyed
            //frags_count 	���������� ������������ ������� frags_count.rank 	numeric 	
            RatingFragsValue = (int) (ratings.frags_count.Value ?? 0);
            RatingFragsPlace = ratings.frags_count.Rank ?? 0;
            //SPT-->
            //Targets Detected
            //spotted_count 	���������� ������������ ������� spotted_count.rank 	numeric 	
            RatingSpottedValue = (int) (ratings.spotted_count.Value ?? 0);
            RatingSpottedPlace = ratings.spotted_count.Rank ?? 0;
            //EXP-->
            //Total Experience
            //xp_amount 	����� ���� xp_amount.rank 	numeric 	
            RatingXpValue = (int) (ratings.xp_amount.Value ?? 0);
            RatingXpPlace = ratings.xp_amount.Rank ?? 0;

            RatingMaxXpValue = (int) (ratings.xp_max.Value ?? 0);
            RatingMaxXpPlace = ratings.xp_max.Rank ?? 0;

            RatingHitsPercentsValue = ratings.hits_ratio.Value ?? 0;
            RatingHitsPercentsPlace = ratings.hits_ratio.Rank ?? 0;

            /*
            damage_avg 	������� ��������� ���� �� ��� damage_avg.rank 	numeric 	
            frags_avg 	������� ���������� ������������ ������� �� ��� frags_avg.rank 	numeric 	
            hits_ratio 	������� ��������� hits_ratio.rank 	numeric 	
            spotted_avg 	������� ���������� ������������ ������� �� ��� spotted_avg.rank 	numeric 	
            survived_ratio 	������� ������������ survived_ratio.rank 	numeric 	
            xp_max 	������������ ���� �� ��� xp_max.rank 	numeric 	
             */

            #endregion
        }
    }
}

