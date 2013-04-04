using System;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'TankStatistic'.
	/// </summary>
	[Serializable]
	public class TankStatisticEntity : EntityBase
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
		/// Gets/Sets the <see cref="TankEntity"/> object.
		/// </summary>
		public virtual TankEntity TankIdObject { get; set; }


	    public virtual void Update(TankJson tank)
	    {
            Updated = tank.Common.lastBattleTimeR;
            Version = tank.Common.basedonversion;
            Raw = tank.Raw;
	    }
	}
}

