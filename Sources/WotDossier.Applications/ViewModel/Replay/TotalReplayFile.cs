using System;
using System.Collections.Generic;
using System.Linq;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public class TotalReplayFile
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
                OriginalXp = (int)replayFiles.Average(x => x.OriginalXp);
                Killed = replayFiles.Average(x => x.Killed).ToString("N2");
                Damaged = replayFiles.Average(x => x.Damaged).ToString("N2");
                Spotted = replayFiles.Average(x => x.Spotted).ToString("N1");
                DamageAssisted = replayFiles.Average(x => x.DamageAssisted).ToString("N0");
                BattleTime = new TimeSpan(0, 0, (int)replayFiles.Average(x => x.BattleTime.TotalSeconds));
                LifeTime = new TimeSpan(0, 0, (int)replayFiles.Average(x => x.LifeTime.TotalSeconds));
                CreditsEarned = (int)replayFiles.Average(x => x.CreditsEarned);
                IsWinnerString = (replayFiles.Count(x => x.IsWinner == BattleStatus.Victory)/(double)replayFiles.Count()).ToString("P");
                DeathReasonString = (replayFiles.Count(x => x.DeathReason == Replay.DeathReason.Alive) / (double)replayFiles.Count()).ToString("P");
                Team = (replayFiles.Count(x => x.Team == 1) / (double)replayFiles.Count()).ToString("P");
                PlayTime = string.Format("{0:dd.MM.yy} - {1:dd.MM.yy}", result.Min(x => x.PlayTime), result.Max(x => x.PlayTime));
                ClientVersion = string.Format("{0} - {1}", replayFiles.Min(x => x.ClientVersion).ToString(3), replayFiles.Max(x => x.ClientVersion).ToString(3));
                IsPlatoonString = (replayFiles.Count(x => x.IsPlatoon) / (double)replayFiles.Count()).ToString("P");
            }
        }

        public string IsPlatoonString { get; set; }

        public string Team { get; set; }

        public string DeathReasonString { get; set; }

        public string IsWinnerString { get; set; }

        public string ClientVersion { get; set; }

        public int CreditsEarned { get; set; }

        public TimeSpan LifeTime { get; set; }

        public TimeSpan BattleTime { get; set; }

        public string PlayTime { get; set; }

        public string Damaged { get; set; }
        public string Spotted { get; set; }
        public string DamageAssisted { get; set; }

        public string Killed { get; set; }

        public int Xp { get; set; }
        public int OriginalXp { get; set; }

        public int DamageReceived { get; set; }

        public int DamageDealt { get; set; }

        public int Credits { get; set; }

        public Guid FolderId { get; set; }
    }
}
