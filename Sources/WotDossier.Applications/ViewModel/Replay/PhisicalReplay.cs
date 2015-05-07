using System;
using System.IO;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public class PhisicalReplay : ReplayFile
    {
        /// <summary>
        /// Gets the phisical path.
        /// </summary>
        /// <value>
        /// The phisical path.
        /// </value>
        public override string PhisicalPath
        {
            get { return PhisicalFile != null ? PhisicalFile.FullName : null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhisicalReplay" /> class.
        /// </summary>
        /// <param name="replayFileInfo">The replay file info.</param>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public PhisicalReplay(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            PhisicalFile = replayFileInfo;
            Name = replayFileInfo.Name;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public override void Move(ReplayFolder targetFolder)
        {
            if (PhisicalFile != null)
            {
                string destFileName = Path.Combine(targetFolder.Path, PhisicalFile.Name);
                if (!File.Exists(destFileName))
                {
                    PhisicalFile.MoveTo(destFileName);
                }
                else
                {
                    PhisicalFile.Delete();
                }
            }
        }

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            if (PhisicalFile != null)
            {
                return ReplayFileHelper.ParseReplay(PhisicalFile, ClientVersion, readAdvancedData);
            }
            return null;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            NativeMethods.DeleteFileOperation(PhisicalFile.FullName);
            //PhisicalFile.Delete();
        }

        public override string ToString()
        {
            return PhisicalPath;
        }
    }
}
