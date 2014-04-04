using System;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'HistoricalBattlesStatisticEntity'.
    /// </summary>
    [Serializable]
    public class HistoricalBattlesStatisticEntity : StatisticEntity
    {
        #region Property names

        public static readonly string PropUpdated = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Updated);
        public static readonly string PropWins = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Wins);
        public static readonly string PropLosses = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Losses);
        public static readonly string PropSurvivedBattles = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.SurvivedBattles);
        public static readonly string PropXp = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Xp);
        public static readonly string PropBattleAvgXp = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.BattleAvgXp);
        public static readonly string PropMaxXp = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.MaxXp);
        public static readonly string PropFrags = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Frags);
        public static readonly string PropSpotted = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.Spotted);
        public static readonly string PropHitsPercents = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.HitsPercents);
        public static readonly string PropDamageDealt = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.DamageDealt);
        public static readonly string PropCapturePoints = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.CapturePoints);
        public static readonly string PropDroppedCapturePoints = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.DroppedCapturePoints);
        public static readonly string PropBattlesCount = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.BattlesCount);
        public static readonly string PropAvgLevel = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.AvgLevel);
        public static readonly string PropPlayerId = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.PlayerId);
		public static readonly string PropAchievementsId = TypeHelper<HistoricalBattlesStatisticEntity>.PropertyName(v => v.AchievementsId);

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricalBattlesStatisticEntity"/> class.
        /// </summary>
        public HistoricalBattlesStatisticEntity()
        {
            AchievementsIdObject = new HistoricalBattlesAchievementsEntity();
        }

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
        /// Gets/Sets the AvgLevel object.
        /// </summary>
        public virtual double AvgLevel { get; set; }

		/// <summary>
		/// Gets/Sets the field "AchievementsId".
		/// </summary>
		public virtual int? AchievementsId { get; set; }
		
		/// <summary>
        /// Gets or sets the RBR.
        /// </summary>
        public virtual double RBR { get; set; }

        /// <summary>
        /// Gets or sets the W n8 rating.
        /// </summary>
        public virtual double WN8Rating { get; set; }

        /// <summary>
        /// Gets or sets the performance rating.
        /// </summary>
        public virtual double PerformanceRating { get; set; }

        /// <summary>
        /// Gets/Sets the <see cref="PlayerAchievementsEntity"/> object.
        /// </summary>
        public virtual HistoricalBattlesAchievementsEntity AchievementsIdObject { get; set; }
    }
}

