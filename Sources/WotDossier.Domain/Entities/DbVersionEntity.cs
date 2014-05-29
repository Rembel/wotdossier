using System;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'DbVersion'.
	/// </summary>
	[Serializable]
	public class DbVersionEntity : EntityBase
	{	
		/// <summary>
		/// Gets/Sets the field "SchemaVersion".
		/// </summary>
		public virtual string SchemaVersion	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Applied".
		/// </summary>
		public virtual DateTime Applied	{get; set; }
		
		
	}
}

