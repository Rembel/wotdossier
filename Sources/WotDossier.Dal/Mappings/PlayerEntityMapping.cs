using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
	/// <summary>
    /// Represents map class for <see cref="PlayerEntity"/>.
    /// </summary>
    public class PlayerMapping : ClassMapBase<PlayerEntity>
    {
        public PlayerMapping()
        {
			Map(v => v.Name);
			Map(v => v.Creaded);
			Map(v => v.PlayerId);
			Map(v => v.Server);
		

            //HasMany(v => v.PlayerStatisticEntities).KeyColumn(Column<PlayerStatisticEntity>(v => v.PlayerId));
            //HasMany(v => v.TankEntities).KeyColumn(Column<TankEntity>(v => v.PlayerId));
        }
    }
}
