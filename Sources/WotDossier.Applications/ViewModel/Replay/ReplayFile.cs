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
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ReplayFile : INotifyPropertyChanged
    {
        protected static readonly ILog _log = LogManager.GetCurrentClassLogger();

        public static readonly string PropDamageDealt = TypeHelper<ReplayFile>.PropertyName(v => v.DamageDealt);
        public static readonly string PropDamaged = TypeHelper<ReplayFile>.PropertyName(v => v.Damaged);
        public static readonly string PropCredits = TypeHelper<ReplayFile>.PropertyName(v => v.Credits);
        public static readonly string PropKilled = TypeHelper<ReplayFile>.PropertyName(v => v.Killed);
        public static readonly string PropXp = TypeHelper<ReplayFile>.PropertyName(v => v.Xp);

        #region result fields

        public TimeSpan BattleTime { get; set; }
        public BattleType BattleType { get; set; }
        public Version ClientVersion { get; set; }
        public string Comment { get; set; }
        public Country CountryId { get; set; }
        public TimeSpan LifeTime { get; set; }
        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }

        /// <summary>
        /// Gets or sets the mark of mastery.
        /// </summary>
        public int MarkOfMastery { get; set; }

        public string TankName { get; set; }
        public DateTime PlayTime { get; set; }
        public int Damaged { get; set; }
        public int Killed { get; set; }
        public long PlayerId { get; set; }
        public string PlayerName { get; set; }
        public long ReplayId { get; set; }
        public int Xp { get; set; }
        public BattleStatus IsWinner { get; set; }
        public int DamageReceived { get; set; }
        public int DamageDealt { get; set; }
        public int Credits { get; set; }
        public int Team { get; set; }
        
        public TankDescription Tank { get; set; }
        public TankIcon Icon { get; set; }
        public List<Vehicle> TeamMembers { get; set; }

        private string _link;
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged("Link");
            }
        }

        /// <summary>
        /// Gets or sets the phisical file.
        /// </summary>
        /// <value>
        /// The phisical file.
        /// </value>
        public FileInfo PhisicalFile { get; set; }
        
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid FolderId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ReplayFile" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public abstract bool Exists { get; }

        /// <summary>
        /// Gets the phisical path.
        /// </summary>
        /// <value>
        /// The phisical path.
        /// </value>
        public abstract string PhisicalPath { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public abstract string Name { get; }

        public bool IsAlive { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayFile" /> class.
        /// </summary>
        protected ReplayFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayFile" /> class.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        protected ReplayFile(Domain.Replay.Replay replay, Guid folderId)
        {
            FolderId = folderId;

            if (replay != null)
            {
                MapName = replay.datablock_1.mapDisplayName;
                MapNameId = replay.datablock_1.mapName;

                if (Dictionaries.Instance.Maps.ContainsKey(replay.datablock_1.mapName))
                {
                    MapId = Dictionaries.Instance.Maps[replay.datablock_1.mapName].mapid;
                }
                else
                {
                    _log.WarnFormat("Unknown map: {0}", replay.datablock_1.mapName);
                }

                IsWinner = BattleStatus.Unknown;

                Icon = Dictionaries.Instance.GetTankIcon(replay.datablock_1.playerVehicle);

                CountryId = Icon.CountryId;


                if (Dictionaries.Instance.IconTanks.ContainsKey(Icon))
                {
                    Tank = Dictionaries.Instance.IconTanks[Icon];
                }

                TankName = Tank != null ? Tank.Title : replay.datablock_1.playerVehicle;

                PlayTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));

                ClientVersion = ResolveVersion(replay.datablock_1.Version, PlayTime);

                ReplayId = Int64.Parse(PlayTime.ToString("yyyyMMddHHmm"));

                PlayerId = replay.datablock_1.playerID;

                PlayerName = replay.datablock_1.playerName;

                BattleType = (BattleType) replay.datablock_1.battleType;
    
                if (replay.datablock_battle_result != null)
                {
                    Credits = replay.datablock_battle_result.personal.credits;
                    DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                    DamageReceived = replay.datablock_battle_result.personal.damageReceived;
                    IsWinner = GetBattleStatus(replay);
                    Xp = replay.datablock_battle_result.personal.xp;
                    Killed = replay.datablock_battle_result.personal.kills;
                    Damaged = replay.datablock_battle_result.personal.damaged;
                    MarkOfMastery = replay.datablock_battle_result.personal.markOfMastery;
                    BattleTime = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                    LifeTime = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                    IsAlive = replay.datablock_battle_result.personal.deathReason == -1 || replay.datablock_battle_result.personal.killerID == 0;
                    //Medals = MedalHelper.GetMedals(replay.datablock_battle_result.achieveIndices);
                }

                TeamMembers = replay.datablock_1.vehicles.Values.ToList();
                Team = TeamMembers.First(x => x.name == replay.datablock_1.playerName).team;
            }
        }

        private Version ResolveVersion(Version version, DateTime playTime)
        {
            if (version == new Version(0,0))
            {
                if (playTime.Date >= new DateTime(2013, 4, 18))
                {
                    return new Version("0.8.5.0");
                }

                if (playTime.Date >= new DateTime(2013, 2, 28))
                {
                    return new Version("0.8.4.0");
                }

                if (playTime.Date >= new DateTime(2013, 1, 16))
                {
                    return new Version("0.8.3.0");
                }

                if (playTime.Date >= new DateTime(2012, 12, 8))
                {
                    return new Version("0.8.2.0");
                }

                if (playTime.Date >= new DateTime(2012, 10, 25))
                {
                    return new Version("0.8.1.0");
                }
            }
            return version;
        }

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public abstract void Move(ReplayFolder targetFolder);

        /// <summary>
        /// Plays replay.
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public abstract Domain.Replay.Replay ReplayData(bool readAdvancedData = false);

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Gets the battle status.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <returns></returns>
        private BattleStatus GetBattleStatus(Domain.Replay.Replay replay)
        {
            if (replay.datablock_battle_result.common.winnerTeam == 0)
            {
                return BattleStatus.Draw;
            }

            if (replay.datablock_battle_result.common.winnerTeam == replay.datablock_battle_result.personal.team)
            {
                return BattleStatus.Victory;
            }

            return BattleStatus.Defeat;
        }

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
