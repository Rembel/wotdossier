using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class ReplayMapViewModel : IReplayMap
    {
        public Gameplay Gameplay { get; set; }
        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }
        public int Team { get; set; }
    }
}