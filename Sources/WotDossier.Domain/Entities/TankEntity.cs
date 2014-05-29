using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Tank'.
	/// </summary>
	[Serializable]
	public class TankEntity : EntityBase
	{	
		/// <summary>
		/// Gets/Sets the field "TankId".
		/// </summary>
		public virtual int TankId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
		public virtual string Name	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Tier".
		/// </summary>
		public virtual int Tier	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "CountryId".
		/// </summary>
		public virtual int CountryId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Icon".
		/// </summary>
		public virtual string Icon	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "TankType".
		/// </summary>
		public virtual int TankType	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "IsPremium".
		/// </summary>
		public virtual Boolean IsPremium	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		public virtual int PlayerId { get; set; }

        /// <summary>
        /// Gets/Sets the field "IsFavorite".
        /// </summary>
        public virtual bool IsFavorite { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="PlayerEntity"/> object.
		/// </summary>
		public virtual PlayerEntity PlayerIdObject { get; set; }

		#region Collections
		
		private IList<TankStatisticEntity> _tankStatisticEntities;
		/// <summary>
		/// Gets/Sets the <see cref="TankStatisticEntity"/> collection.
		/// </summary>
        public virtual IList<TankStatisticEntity> TankStatisticEntities
        {
            get
            {
                return _tankStatisticEntities ?? (_tankStatisticEntities = new List<TankStatisticEntity>());
            }
            set { _tankStatisticEntities = value; }
        }
		
		#endregion Collections
		
	}
}

