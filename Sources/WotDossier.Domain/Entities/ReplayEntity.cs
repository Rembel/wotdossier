namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Replay'.
	/// </summary>
	public class ReplayEntity : EntityBase
	{	
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

        /// <summary>
        /// Gets or sets the raw.
        /// </summary>
		public virtual byte[] Raw	{get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
	    public virtual string Comment { get; set; }
	}
}

