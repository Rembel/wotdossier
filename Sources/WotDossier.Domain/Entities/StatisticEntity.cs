using System;

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
        /// Gets/Sets the <see cref="PlayerEntity"/> object.
        /// </summary>
        public virtual PlayerEntity PlayerIdObject { get; set; }
    }
}