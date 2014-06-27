using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WotDossier.Dal;

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
            
            Credits = (int) result.Average(x => x.Credits);
            DamageDealt = (int)result.Average(x => x.DamageDealt);
            DamageReceived = (int)result.Average(x => x.DamageReceived);
            Xp = (int)result.Average(x => x.Xp);
            Killed = (int)result.Average(x => x.Killed);
            Damaged = (int)result.Average(x => x.Damaged);
            BattleTime = new TimeSpan(0, 0, (int)result.Average(x => x.BattleTime.TotalSeconds));
            LifeTime = new TimeSpan(0, 0, (int)result.Average(x => x.LifeTime.TotalSeconds));
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
        /// Plays replay.
        /// </summary>
        public override void Play()
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
