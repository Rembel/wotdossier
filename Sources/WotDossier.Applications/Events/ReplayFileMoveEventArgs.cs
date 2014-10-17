using System.Collections;
using WotDossier.Applications.ViewModel.Replay;

namespace WotDossier.Applications.Events
{
    public class ReplayFileMoveEventArgs
    {
        public IList ReplayFiles { get; set; }
        public ReplayFolder TargetFolder { get; set; }
    }
}