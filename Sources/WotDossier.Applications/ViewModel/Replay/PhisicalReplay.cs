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
        /// Initializes a new instance of the <see cref="PhisicalReplay" /> class.
        /// </summary>
        /// <param name="replayFileInfo">The replay file info.</param>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        public PhisicalReplay(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId) : base(replay, folderId)
        {
            PhisicalPath = replayFileInfo.FullName;
            Name = replayFileInfo.Name;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public override void Move(ReplayFolder targetFolder)
        {
            if (!string.IsNullOrEmpty(PhisicalPath) && File.Exists(PhisicalPath))
            {
                string destPath = Path.Combine(targetFolder.Path, Name);
                if (!File.Exists(destPath))
                {
                    File.Move(PhisicalPath, destPath);
                }
                else
                {
                    File.Delete(PhisicalPath);
                }

                PhisicalPath = destPath;
            }
        }

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            if (!string.IsNullOrEmpty(PhisicalPath) && File.Exists(PhisicalPath))
            {
                return ReplayFileHelper.ParseReplay(new FileInfo(PhisicalPath), ClientVersion, readAdvancedData);
            }
            return null;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            if (!string.IsNullOrEmpty(PhisicalPath) && File.Exists(PhisicalPath))
            {
                NativeMethods.DeleteFileOperation(PhisicalPath);
            }
        }

        public override string ToString()
        {
            return PhisicalPath;
        }
    }
}
