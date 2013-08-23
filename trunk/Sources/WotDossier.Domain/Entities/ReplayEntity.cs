using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Replay'.
	/// </summary>
	[Serializable]
	public class ReplayEntity : EntityBase
	{	
		#region Property names
		
		public static readonly string PropReplayId = TypeHelper<ReplayEntity>.PropertyName(v => v.ReplayId);
		public static readonly string PropPlayerId = TypeHelper<ReplayEntity>.PropertyName(v => v.PlayerId);
		public static readonly string PropLink = TypeHelper<ReplayEntity>.PropertyName(v => v.Link);
		
		#endregion

		/// <summary>
		/// Gets/Sets the field "ReplayId".
		/// </summary>
		public virtual long ReplayId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		public virtual long PlayerId	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Link".
		/// </summary>
		public virtual string Link	{get; set; }
		
		
	}
}

