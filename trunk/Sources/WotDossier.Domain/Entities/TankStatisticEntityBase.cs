using System;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
	/// Base Object representation for tables like 'TankStatistic'.
	/// </summary>
	public class TankStatisticEntityBase : EntityBase
	{	
		#region Property names
		
		public static readonly string PropUpdated = TypeHelper<TankStatisticEntity>.PropertyName(v => v.Updated);
		public static readonly string PropRaw = TypeHelper<TankStatisticEntity>.PropertyName(v => v.Raw);
		public static readonly string PropTankId = TypeHelper<TankStatisticEntity>.PropertyName(v => v.TankId);
		
		#endregion

		/// <summary>
		/// Gets/Sets the field "Updated".
		/// </summary>
		public virtual DateTime Updated	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Raw".
		/// </summary>
		public virtual Byte[] Raw	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "TankId".
		/// </summary>
		public virtual int TankId { get; set; }

        /// <summary>
        /// Gets or sets the battles count.
        /// </summary>
        public virtual int BattlesCount { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="TankEntity"/> object.
		/// </summary>
		public virtual TankEntity TankIdObject { get; set; }
	}
}

