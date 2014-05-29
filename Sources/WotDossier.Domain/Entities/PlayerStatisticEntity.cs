using System;
using WotDossier.Domain.Server;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'PlayerStatistic'.
    /// </summary>
    [Serializable]
    public class PlayerStatisticEntity : StatisticEntity
    {
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
        public virtual double RatingWinsRatioValue { get; set; }

        /// <summary>
        /// Gets/Sets the field "Rating_BattleAvgPerformancePlace".
        /// </summary>
        public virtual int RatingWinsRatioPlace { get; set; }

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
            RatingIntegratedValue = (int) (ratings.global_rating.Value ?? 0);
            RatingIntegratedPlace = ratings.global_rating.Rank ?? 0;
            //W/B-->
            //Victories/Battles
            //wins_ratio 	Процент побед wins_ratio.rank 	numeric 	
            RatingWinsRatioValue = ratings.wins_ratio.Value ?? 0;
            RatingWinsRatioPlace = ratings.wins_ratio.Rank ?? 0;
            //E/B-->
            //Average Experience per BattleJson
            //xp_avg 	Средний опыт за бой xp_avg.rank 	numeric 	
            RatingBattleAvgXpValue = ratings.xp_avg.Value ?? 0;
            RatingBattleAvgXpPlace = ratings.xp_avg.Rank ?? 0;
            //WIN-->
            //Victories
            //RatingBattleWinsValue = ratings.Battle_wins.Value;
            //RatingBattleWinsPlace = ratings.Battle_wins.Rank ?? 0;
            //GPL-->
            //Battles Participated
            //battles_count 	Количество проведённых боёв battles_count.rank 	numeric 	
            RatingBattlesValue = (int) (ratings.battles_count.Value ?? 0);
            RatingBattlesPlace = ratings.battles_count.Rank ?? 0;
            //CPT-->
            //Capture Points
            //RatingCapturedPointsValue = ratings.Ctf_points.Value;
            //RatingCapturedPointsPlace = ratings.Ctf_points.Rank ?? 0;
            //DMG-->
            //Damage Caused
            //damage_dealt 	Общий нанесённый урон damage_dealt.rank 	numeric 	
            RatingDamageDealtValue = (int) (ratings.damage_dealt.Value ?? 0);
            RatingDamageDealtPlace = ratings.damage_dealt.Rank ?? 0;
            //DPT-->
            //Defense Points
            //RatingDroppedPointsValue = ratings.Dropped_ctf_points.Value;
            //RatingDroppedPointsPlace = ratings.Dropped_ctf_points.Rank ?? 0;
            //FRG-->
            //Targets Destroyed
            //frags_count 	Количество уничтоженной техники frags_count.rank 	numeric 	
            RatingFragsValue = (int) (ratings.frags_count.Value ?? 0);
            RatingFragsPlace = ratings.frags_count.Rank ?? 0;
            //SPT-->
            //Targets Detected
            //spotted_count 	Количество обнаруженной техники spotted_count.rank 	numeric 	
            RatingSpottedValue = (int) (ratings.spotted_count.Value ?? 0);
            RatingSpottedPlace = ratings.spotted_count.Rank ?? 0;
            //EXP-->
            //Total Experience
            //xp_amount 	Общий опыт xp_amount.rank 	numeric 	
            RatingXpValue = (int) (ratings.xp_amount.Value ?? 0);
            RatingXpPlace = ratings.xp_amount.Rank ?? 0;

            RatingMaxXpValue = (int) (ratings.xp_max.Value ?? 0);
            RatingMaxXpPlace = ratings.xp_max.Rank ?? 0;

            RatingHitsPercentsValue = ratings.hits_ratio.Value ?? 0;
            RatingHitsPercentsPlace = ratings.hits_ratio.Rank ?? 0;

            /*
            damage_avg 	Средний нанесённый урон за бой damage_avg.rank 	numeric 	
            frags_avg 	Среднее количество уничтоженной техники за бой frags_avg.rank 	numeric 	
            hits_ratio 	Процент попаданий hits_ratio.rank 	numeric 	
            spotted_avg 	Среднее количество обнаруженной техники за бой spotted_avg.rank 	numeric 	
            survived_ratio 	Процент выживаемости survived_ratio.rank 	numeric 	
            xp_max 	Максимальный опыт за бой xp_max.rank 	numeric 	
             */

            #endregion
        }
    }
}

