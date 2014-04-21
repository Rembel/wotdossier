using System;
using WotDossier.Dal;
using WotDossier.Framework;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class DbReplay : ReplayFile
    {
        private readonly Domain.Replay.Replay _replay;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbReplay" /> class.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public DbReplay(Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            _replay = replay;
        }

        public override bool Exists
        {
            get { return _replay != null; }
        }

        public override string PhisicalPath
        {
            get { return null; }
        }

        public override string Name
        {
            get { throw null; }
        }

        public override void Move(ReplayFolder targetFolder)
        {
        }

        public override void Play()
        {
        }

        public override Domain.Replay.Replay ReplayData()
        {
            return _replay;
        }

        public override void Delete()
        {
            CompositionContainerFactory.Instance.GetExport<DossierRepository>().DeleteReplay(ReplayId);
        }
    }
}