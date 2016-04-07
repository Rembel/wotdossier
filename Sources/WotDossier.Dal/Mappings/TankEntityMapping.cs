using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="TankEntity"/>.
    /// </summary>
    public class TankMapping : ClassMapBase<TankEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TankMapping"/> class.
        /// </summary>
        public TankMapping()
        {
			Map(v => v.UId);
			Map(v => v.TankId);
			Map(v => v.Name);
			Map(v => v.Tier);
			Map(v => v.CountryId);
			Map(v => v.Icon);
			Map(v => v.TankType);
			Map(v => v.IsPremium);
            Map(v => v.IsFavorite);
            Map(v => v.PlayerId).Insert();
            Map(v => v.PlayerUId);
            Map(v => v.Rev);
            Map(v => v.UniqueId);
		
			References(v => v.PlayerIdObject).Column(Column(v => v.PlayerId)).ReadOnly();

			HasMany(v => v.TankStatisticEntities).KeyColumn(Column<TankRandomBattlesStatisticEntity>(v => v.TankId)).Cascade.All().Inverse();
        }
    }
}
