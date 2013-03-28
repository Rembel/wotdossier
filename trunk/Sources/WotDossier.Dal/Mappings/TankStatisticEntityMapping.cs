using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TankStatisticEntity"/>.
    /// </summary>
    public class TankStatisticMapping : ClassMapBase<TankStatisticEntity>
    {
        public TankStatisticMapping()
        {
			Map(v => v.Updated, "Updated");
            Map(v => v.Raw, "Raw").CustomSqlType("BinaryBlob");
			Map(v=>v.TankId, "TankId").ReadOnly();
            Map(v => v.Version, "Version");
		
			References(v => v.TankIdObject).Column(Column(v => v.TankId)).Insert();
        }
    }
}
