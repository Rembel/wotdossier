using System;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
    /// <summary>
	/// Base Object representation for tables like 'TankStatistic'.
	/// </summary>
	[DataContract]
	public class TankStatisticEntityBase : EntityBase, IRevised
    {	
		/// <summary>
		/// Gets/Sets the field "Updated".
		/// </summary>
		[DataMember]
		public virtual DateTime Updated	{get; set; }

        /// <summary>
        /// Gets/Sets the field "Raw".
        /// </summary>
        [DataMember]
        public virtual Byte[] Raw	{get; set; }

        /// <summary>
        /// Gets/Sets the field "TankId".
        /// </summary>
        [DataMember]
        public virtual int TankId { get; set; }

        [DataMember]
        public virtual Guid TankUId { get; set; }

        /// <summary>
        /// Gets or sets the battles count.
        /// </summary>
        [DataMember]
        public virtual int BattlesCount { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="TankEntity"/> object.
		/// </summary>
		public virtual TankEntity TankIdObject { get; set; }
	}
}

