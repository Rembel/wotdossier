using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Tank'.
	/// </summary>
	[Serializable]
    [DataContract]
	public class TankEntity : EntityBase, IRevised
	{	
		/// <summary>
		/// Gets/Sets the field "TankId".
		/// </summary>
		[DataMember]
        public virtual int TankId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
        [DataMember]
        public virtual string Name	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Tier".
		/// </summary>
        [DataMember]
        public virtual int Tier	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "CountryId".
		/// </summary>
        [DataMember]
        public virtual int CountryId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Icon".
		/// </summary>
        [DataMember]
        public virtual string Icon	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "TankType".
		/// </summary>
        [DataMember]
        public virtual int TankType	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "IsPremium".
		/// </summary>
        [DataMember]
        public virtual Boolean IsPremium	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
        [DataMember]
        public virtual int PlayerId { get; set; }

        [DataMember]
        public virtual Guid PlayerUId { get; set; }

        /// <summary>
        /// Gets/Sets the field "IsFavorite".
        /// </summary>
        [DataMember]
        public virtual bool IsFavorite { get; set; }
		
		/// <summary>
		/// Gets/Sets the <see cref="PlayerEntity"/> object.
		/// </summary>
		public virtual PlayerEntity PlayerIdObject { get; set; }

		#region Collections
		
		private IList<TankRandomBattlesStatisticEntity> _tankStatisticEntities;
		/// <summary>
		/// Gets/Sets the <see cref="TankStatisticEntity"/> collection.
		/// </summary>
        public virtual IList<TankRandomBattlesStatisticEntity> TankStatisticEntities
        {
            get
            {
                return _tankStatisticEntities ?? (_tankStatisticEntities = new List<TankRandomBattlesStatisticEntity>());
            }
            set { _tankStatisticEntities = value; }
        }
		
		#endregion Collections
		
	}
}

