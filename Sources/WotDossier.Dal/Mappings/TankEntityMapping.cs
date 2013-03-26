using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TankEntity"/>.
    /// </summary>
    public class TankMapping : ClassMapBase<TankEntity>
    {
        public TankMapping()
        {
			Map(v => v.TankId, "TankId");
			Map(v => v.Name, "Name");
			Map(v => v.Tier, "Tier");
			Map(v => v.CountryId, "CountryId");
			Map(v => v.CountryCode, "CountryCode");
			Map(v => v.Icon, "Icon");
			Map(v => v.TankType, "TankType");
			Map(v => v.IsPremium, "IsPremium");
			Map(v=>v.PlayerId, "PlayerId").Insert();
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();

			HasMany(v => v.TankStatisticEntities).KeyColumn(Column<TankStatisticEntity>(v => v.TankId)).Cascade.All().Inverse();
        }
    }
}
