using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'DbVersion'.
	/// </summary>
	[Serializable]
	public class DbVersionEntity : EntityBase
	{	
		#region Property names
		
		public static readonly string PropSchemaVersion = TypeHelper<DbVersionEntity>.PropertyName(v => v.SchemaVersion);
		public static readonly string PropApplied = TypeHelper<DbVersionEntity>.PropertyName(v => v.Applied);
		
		#endregion

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

