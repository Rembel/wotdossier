using System;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'TeamBattlesStatisticEntity'.
    /// </summary>
    [Serializable]
    public class TeamBattlesStatisticEntity : StatisticEntity
    {
        #region Property names

        public static readonly string PropUpdated = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Updated);
        public static readonly string PropWins = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Wins);
        public static readonly string PropLosses = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Losses);
        public static readonly string PropSurvivedBattles = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.SurvivedBattles);
        public static readonly string PropXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Xp);
        public static readonly string PropBattleAvgXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.BattleAvgXp);
        public static readonly string PropMaxXp = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.MaxXp);
        public static readonly string PropFrags = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Frags);
        public static readonly string PropSpotted = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.Spotted);
        public static readonly string PropHitsPercents = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.HitsPercents);
        public static readonly string PropDamageDealt = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.DamageDealt);
        public static readonly string PropCapturePoints = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.CapturePoints);
        public static readonly string PropDroppedCapturePoints = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.DroppedCapturePoints);
        public static readonly string PropBattlesCount = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.BattlesCount);
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
        public static readonly string PropAvgLevel = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.AvgLevel);
        public static readonly string PropPlayerId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.PlayerId);
		public static readonly string PropAchievementsId = TypeHelper<PlayerStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

        /// <summary>
        /// Gets/Sets the field "Updated".
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Gets/Sets the field "Wins".
        /// </summary>
        public virtual int Wins { get; set; }

        /// <summary>
        /// Gets/Sets the field "Losses".
        /// </summary>
        public virtual int Losses { get; set; }

        /// <summary>
        /// Gets/Sets the field "SurvivedBattles".
        /// </summary>
        public virtual int SurvivedBattles { get; set; }

        /// <summary>
        /// Gets/Sets the field "Xp".
        /// </summary>
        public virtual int Xp { get; set; }

        /// <summary>
        /// Gets/Sets the field "BattleAvgXp".
        /// </summary>
        public virtual double BattleAvgXp { get; set; }

        /// <summary>
        /// Gets/Sets the field "MaxXp".
        /// </summary>
        public virtual int MaxXp { get; set; }

        /// <summary>
        /// Gets/Sets the field "Frags".
        /// </summary>
        public virtual int Frags { get; set; }

        /// <summary>
        /// Gets/Sets the field "Spotted".
        /// </summary>
        public virtual int Spotted { get; set; }

        /// <summary>
        /// Gets/Sets the field "HitsPercents".
        /// </summary>
        public virtual double HitsPercents { get; set; }

        /// <summary>
        /// Gets/Sets the field "DamageDealt".
        /// </summary>
        public virtual int DamageDealt { get; set; }

        /// <summary>
        /// Gets/Sets the field "DamageTaken".
        /// </summary>
        public virtual int DamageTaken { get; set; }

        /// <summary>
        /// Gets/Sets the field "CapturePoints".
        /// </summary>
        public virtual int CapturePoints { get; set; }

        /// <summary>
        /// Gets/Sets the field "DroppedCapturePoints".
        /// </summary>
        public virtual int DroppedCapturePoints { get; set; }

        /// <summary>
        /// Gets/Sets the field "BattlesCount".
        /// </summary>
        public virtual int BattlesCount { get; set; }

        /// <summary>
        /// Gets/Sets the AvgLevel object.
        /// </summary>
        public virtual double AvgLevel { get; set; }

				/// <summary>
		/// Gets/Sets the field "AchievementsId".
		/// </summary>
		public virtual int? AchievementsId { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
		/// </summary>
		public virtual TeamBattlesAchievementsEntity AchievementsIdObject { get; set; }

        public virtual void Update(TeamBattlesStatAdapter statAdapter)
        {
            #region CommonJson init

            BattlesCount = statAdapter.Battles_count;
            Wins = statAdapter.Wins;
            Losses = statAdapter.Losses;
            SurvivedBattles = statAdapter.Survived_battles;
            Xp = statAdapter.Xp;
            BattleAvgXp = statAdapter.Battle_avg_xp;
            MaxXp = statAdapter.Max_xp;
            Frags = statAdapter.Frags;
            Spotted = statAdapter.Spotted;
            HitsPercents = statAdapter.Hits_percents;
            DamageDealt = statAdapter.Damage_dealt;
            DamageTaken = statAdapter.Damage_taken;
            CapturePoints = statAdapter.Capture_points;
            DroppedCapturePoints = statAdapter.Dropped_capture_points;
            Updated = statAdapter.Updated;
            AvgLevel = statAdapter.AvgLevel;

            if (AchievementsIdObject == null)
            {
                AchievementsIdObject = new TeamBattlesAchievementsEntity();
            }

            AchievementsIdObject.WolfAmongSheep = statAdapter.WolfAmongSheep;
            AchievementsIdObject.WolfAmongSheepMedal = statAdapter.WolfAmongSheepMedal;
            AchievementsIdObject.GeniusForWar = statAdapter.GeniusForWar;
            AchievementsIdObject.GeniusForWarMedal = statAdapter.GeniusForWarMedal;
            AchievementsIdObject.KingOfTheHill = statAdapter.KingOfTheHill;
            AchievementsIdObject.TacticalBreakthroughSeries = statAdapter.TacticalBreakthroughSeries;
            AchievementsIdObject.MaxTacticalBreakthroughSeries = statAdapter.MaxTacticalBreakthroughSeries;
            AchievementsIdObject.ArmoredFist = statAdapter.ArmoredFist;
            AchievementsIdObject.TacticalBreakthrough = statAdapter.TacticalBreakthrough;

            #endregion

        }
    }
}

