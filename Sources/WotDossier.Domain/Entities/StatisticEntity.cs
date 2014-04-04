using System;
using WotDossier.Domain.Player;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    /// Object representation for table 'PlayerStatistic'.
    /// </summary>
    [Serializable]
    public class StatisticEntity : EntityBase
    {
        /// <summary>
        /// Gets/Sets the field "PlayerId".
        /// </summary>
        public virtual int PlayerId { get; set; }

        /// <summary>
        /// Gets/Sets the field "PlayerId".
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Gets/Sets the field "PlayerId".
        /// </summary>
        public virtual int BattlesCount { get; set; }

        /// <summary>
        /// Gets/Sets the <see cref="PlayerEntity"/> object.
        /// </summary>
        public virtual PlayerEntity PlayerIdObject { get; set; }

        public virtual void UpdateRatings(Ratings ratings)
        {
            
        }
    }
}