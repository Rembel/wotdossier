using WotDossier.Domain.Replay;

namespace WotDossier.Domain.Interfaces
{
    public interface IMapDescription
    {
        Gameplay Gameplay { get; set; }
        string MapName { get; set; }
        int MapId { get; set; }
        string MapNameId { get; set; }
        int Team { get; set; }
    }
}