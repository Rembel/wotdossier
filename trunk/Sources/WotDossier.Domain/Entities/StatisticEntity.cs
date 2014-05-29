using System;
using WotDossier.Domain.Server;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Base object representation for tables of type 'Statistic'.
    /// </summary>
    [Serializable]
    public abstract class StatisticEntity : EntityBase
    {
        /// <summary>
        /// Gets/Sets the field "PlayerId".
        /// </summary>
        public virtual int PlayerId { get; set; }

        /// <summary>
        /// Gets/Sets the field "Updated".
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Gets/Sets the field "BattlesCount".
        /// </summary>
        public virtual int BattlesCount { get; set; }

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
        /// Gets/Sets the <see cref="PlayerEntity"/> object.
        /// </summary>
        public virtual PlayerEntity PlayerIdObject { get; set; }

        public virtual void UpdateRatings(Ratings ratings)
        {
            
        }
    }
}