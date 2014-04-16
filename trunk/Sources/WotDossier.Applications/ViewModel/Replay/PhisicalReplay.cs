using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class PhisicalReplay : ReplayFile
    {
        public PhisicalReplay(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId) : base(replayFileInfo, replay, folderId)
        {
        }
    }
}
