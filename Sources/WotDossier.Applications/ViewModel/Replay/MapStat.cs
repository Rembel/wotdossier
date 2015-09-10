using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public class MapStat : INotifyPropertyChanged
    {
        protected static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static readonly string PropDamageDealt = TypeHelper<ReplayFile>.PropertyName(v => v.DamageDealt);
        public static readonly string PropDamaged = TypeHelper<ReplayFile>.PropertyName(v => v.Damaged);
        public static readonly string PropCredits = TypeHelper<ReplayFile>.PropertyName(v => v.Credits);
        public static readonly string PropKilled = TypeHelper<ReplayFile>.PropertyName(v => v.Killed);
        public static readonly string PropXp = TypeHelper<ReplayFile>.PropertyName(v => v.Xp);

        #region result fields

        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }

        public int Damaged { get; set; }
        public int Spotted { get; set; }
        public int DamageAssisted { get; set; }
        public int Killed { get; set; }
        public int Xp { get; set; }

        public int DamageReceived { get; set; }
        public int DamageDealt { get; set; }
        public int Credits { get; set; }
        public int CreditsEarned { get; set; }
        public int Team { get; set; }
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapStat" /> class.
        /// </summary>
        /// <param name="replays">The replays.</param>
        public MapStat(List<ReplayFile> replays, string mapName)
        {
            MapNameId = mapName;

            if (Dictionaries.Instance.Maps.ContainsKey(mapName))
            {
                var map = Dictionaries.Instance.Maps[mapName];
                MapId = map.MapId;
                MapName = map.LocalizedMapName;
            }
            
            Credits = (int) replays.Average(x => x.Credits);
            CreditsEarned = (int) replays.Average(x => x.CreditsEarned);
            DamageDealt = (int) replays.Average(x => x.DamageDealt);
            DamageReceived = (int) replays.Average(x => x.DamageReceived);
            WinsPercent = (replays.Count(x => x.IsWinner == BattleStatus.Victory)/(double) replays.Count())* 100;
            Xp = (int) replays.Average(x => x.Xp);
            Killed = (int) replays.Average(x => x.Killed);
            Damaged = (int) replays.Average(x => x.Damaged);
            Spotted = (int) replays.Average(x => x.Spotted);
            DamageAssisted = (int) replays.Average(x => x.DamageAssisted);
            ClientVersion = string.Format("{0} - {1}", replays.Min(x => x.ClientVersion).ToString(3),
                replays.Max(x => x.ClientVersion).ToString(3));
            ReplaysCount = replays.Count();
            ValidReplaysCount = replays.Count(x => x.IsWinner != BattleStatus.Unknown);
            SurvivedPercent = (replays.Count(x => x.IsAlive == true) / (double)replays.Count()) * 100;
            SurvivedAndWonPercent = (replays.Count(x => x.IsWinner == BattleStatus.Victory && x.IsAlive == true) / (double)replays.Count()) * 100;
            PlatoonPercent = (replays.Count(x => x.IsPlatoon == true) / (double)replays.Count()) * 100;
        }

        public double PlatoonPercent { get; set; }

        public double SurvivedAndWonPercent { get; set; }

        public double SurvivedPercent { get; set; }

        public int ValidReplaysCount { get; set; }

        public int ReplaysCount { get; set; }

        public string ClientVersion { get; set; }

        public double WinsPercent { get; set; }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
