using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Player'.
	/// </summary>
	[Serializable]
	public class PlayerEntity : EntityBase
	{	
		#region Property names
		
		public static readonly string PropName = TypeHelper<PlayerEntity>.PropertyName(v => v.Name);
		public static readonly string PropCreaded = TypeHelper<PlayerEntity>.PropertyName(v => v.Creaded);
		public static readonly string PropPlayerId = TypeHelper<PlayerEntity>.PropertyName(v => v.PlayerId);
		
		#endregion

		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
		public virtual string Name	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Creaded".
		/// </summary>
		public virtual DateTime Creaded	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		public virtual int PlayerId	{get; set; }
		
		#region Collections
		
		private IList<PlayerStatisticEntity> _playerStatisticEntities;
		/// <summary>
		/// Gets/Sets the <see cref="PlayerStatisticEntity"/> collection.
		/// </summary>
        public virtual IList<PlayerStatisticEntity> PlayerStatisticEntities
        {
            get
            {
                return _playerStatisticEntities ?? (_playerStatisticEntities = new List<PlayerStatisticEntity>());
            }
            set { _playerStatisticEntities = value; }
        }
		
		private IList<TankEntity> _tankEntities;
		/// <summary>
		/// Gets/Sets the <see cref="TankEntity"/> collection.
		/// </summary>
        public virtual IList<TankEntity> TankEntities
        {
            get
            {
                return _tankEntities ?? (_tankEntities = new List<TankEntity>());
            }
            set { _tankEntities = value; }
        }
		
		#endregion Collections
		
	}
}

