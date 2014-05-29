using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="DbVersionEntity"/>.
    /// </summary>
    public class DbVersionMapping : ClassMapBase<DbVersionEntity>
    {
        public DbVersionMapping()
        {
			Map(v => v.SchemaVersion);
			Map(v => v.Applied);
        }
    }
}
