using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Interfaces
{
    public interface ITankDescription
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        TankDescription Description { get; set; }
    }
}