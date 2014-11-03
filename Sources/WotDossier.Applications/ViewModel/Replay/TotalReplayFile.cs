using System;
using System.Collections.Generic;
using System.Linq;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public class TotalReplayFile : ReplayFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TotalReplayFile" /> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="folderId">The folder id.</param>
        public TotalReplayFile(List<ReplayFile> result, Guid folderId)
        {
            FolderId = folderId;

            var replayFiles = result.Where(x => x.IsWinner != BattleStatus.Unknown && x.IsWinner != BattleStatus.Incomplete).ToList();

            if (replayFiles.Any())
            {
                Credits = (int)replayFiles.Average(x => x.Credits);
                DamageDealt = (int)replayFiles.Average(x => x.DamageDealt);
                DamageReceived = (int)replayFiles.Average(x => x.DamageReceived);
                Xp = (int)replayFiles.Average(x => x.Xp);
                Killed = (int)replayFiles.Average(x => x.Killed);
                Damaged = (int) replayFiles.Average(x => x.Damaged);
                PlayTime = result.Max(x => x.PlayTime);
                BattleTime = new TimeSpan(0, 0, (int)replayFiles.Average(x => x.BattleTime.TotalSeconds));
                LifeTime = new TimeSpan(0, 0, (int)replayFiles.Average(x => x.LifeTime.TotalSeconds));
                CreditsEarned = (int)replayFiles.Average(x => x.CreditsEarned);
                ClientVersion = replayFiles.Max(x => x.ClientVersion);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ReplayFile" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public override bool Exists
        {
            get { return false; }
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
        /// <exception cref="System.NotImplementedException"></exception>
        public override Domain.Replay.Replay ReplayData(bool readAdvancedData = false)
        {
            return null;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
        }
    }
}
