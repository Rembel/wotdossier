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
            Map(v => v.Name, "Name");
			Map(v => v.Creaded, "Creaded");
			Map(v => v.PlayerId, "PlayerId");
		

			HasMany(v => v.PlayerStatisticEntities).KeyColumn(Column<PlayerStatisticEntity>(v => v.PlayerId));
        }
    }
}
