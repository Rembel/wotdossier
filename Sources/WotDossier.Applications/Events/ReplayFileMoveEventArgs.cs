using WotDossier.Applications.ViewModel.Replay;

namespace WotDossier.Applications.Events
{
    public class ReplayFileMoveEventArgs
    {
        public ReplayFile ReplayFile { get; set; }
        public ReplayFolder TargetFolder { get; set; }
    }
}