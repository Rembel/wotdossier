using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="ReplayEntity"/>.
    /// </summary>
    public class ReplayMapping : ClassMapBase<ReplayEntity>
    {
        public ReplayMapping()
        {
			Map(v => v.ReplayId, "ReplayId");
			Map(v => v.PlayerId, "PlayerId");
			Map(v => v.Link, "Link");
		

        }
    }
}
