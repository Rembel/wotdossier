using System;
using WotDossier.Dal;
using WotDossier.Framework;

namespace WotDossier.Applications.ViewModel.Replay
{
    public class DbReplay : ReplayFile
    {
        private readonly Domain.Replay.Replay _replay;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ReplayFile" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public override bool Exists
        {
            get { return _replay != null; }
        }

        /// <summary>
        /// Gets the phisical path.
        /// </summary>
        /// <value>
        /// The phisical path.
        /// </value>
        public override string PhisicalPath
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get { return null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbReplay" /> class.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public DbReplay(Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            _replay = replay;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public override void Move(ReplayFolder targetFolder)
        {
        }

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            return _replay;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            CompositionContainerFactory.Instance.GetExport<DossierRepository>().DeleteReplay(ReplayId);
        }
    }
}