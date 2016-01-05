using System;
using System.Runtime.Serialization;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'DbVersion'.
	/// </summary>
    [DataContract]
    public class DbVersionEntity : EntityBase
	{	
		/// <summary>
		/// Gets/Sets the field "SchemaVersion".
		/// </summary>
		[DataMember]
		public virtual string SchemaVersion	{get; set; }

        /// <summary>
        /// Gets/Sets the field "Applied".
        /// </summary>
        [DataMember]
        public virtual DateTime Applied	{get; set; }
	}
}

