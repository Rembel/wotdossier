using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Player'.
	/// </summary>
	[Serializable]
    [DataContract]
	public class PlayerEntity : EntityBase
	{	
		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
		[DataMember]
        public virtual string Name	{get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        [DataMember]
        public virtual string Server { get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Creaded".
		/// </summary>
        [DataMember]
        public virtual DateTime Creaded	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
        [DataMember]
        public virtual int PlayerId	{get; set; }

	    #region Collections
		
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

