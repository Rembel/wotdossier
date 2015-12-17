using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Common.Logging;
using WotDossier.Applications.ViewModel.Replay.Viewer;
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
    public abstract class ReplayFile : INotifyPropertyChanged, IReplayMap
    {
        protected static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static readonly string PropDamageDealt = TypeHelper<ReplayFile>.PropertyName(v => v.DamageDealt);
        public static readonly string PropDamaged = TypeHelper<ReplayFile>.PropertyName(v => v.Damaged);
        public static readonly string PropCredits = TypeHelper<ReplayFile>.PropertyName(v => v.Credits);
        public static readonly string PropKilled = TypeHelper<ReplayFile>.PropertyName(v => v.Killed);
        public static readonly string PropXp = TypeHelper<ReplayFile>.PropertyName(v => v.Xp);

        #region Stat props

        public List<Medal> Achievements { get; set; }

        public int AchievementsCount { get; set; }
        public TimeSpan BattleTime { get; set; }
        public BattleType BattleType { get; set; }
        public string BattleTypeString { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid FolderId { get; set; }

        public string FinishReasonString { get; set; }

        public Gameplay Gameplay { get; set; }
        public Version ClientVersion { get; set; }
        public string Comment { get; set; }
        public Country CountryId { get; set; }
        public int Credits { get; set; }
        public int CreditsEarned { get; set; }
        public int DamageAssisted { get; set; }
        public int DamageDealt { get; set; }
        public int DamageReceived { get; set; }
        public int Damaged { get; set; }

        private DeathReason _deathReason = DeathReason.Unknown;

        public DeathReason DeathReason
        {
            get { return _deathReason; }
            set { _deathReason = value; }
        }

        public int DamageBlockedByArmor { get; set; }
        public int DamageAssistedTrack { get; set; }

        public int DamageAssistedRadio { get; set; }
        public string DeathReasonString { get; set; }
        public FinishReason FinishReason { get; set; }
        public TankIcon Icon { get; set; }
        public bool IsAlive { get; set; }
        public bool IsPlatoon { get; set; }
        private BattleStatus _isWinner = BattleStatus.Unknown;
        public BattleStatus IsWinner
        {
            get { return _isWinner; }
            set { _isWinner = value; }
        }

        public string IsWinnerString { get; set; }

        public int Killed { get; set; }
        public TimeSpan LifeTime { get; set; }
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

        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }

        /// <summary>
        /// Gets or sets the mark of mastery.
        /// </summary>
        public int MarkOfMastery { get; set; }

        public List<Medal> Medals { get; set; }

        public int MedalsCount { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        public int OriginalXp { get; set; }

        /// <summary>
        /// Gets the phisical path.
        /// </summary>
        public string PhisicalPath { get; set; }

        public long PlayerId { get; set; }
        public string PlayerName { get; set; }
        public DateTime PlayTime { get; set; }
        public int PotentialDamageReceived { get; set; }
        public long ReplayId { get; set; }
        public int Spotted { get; set; }

        public TankDescription Tank { get; set; }
        public string TankName { get; set; }
        public int Team { get; set; }
        public List<Vehicle> TeamMembers { get; set; }

        public int Xp { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayFile" /> class.
        /// </summary>
        protected ReplayFile()
        {
            Medals = new List<Medal>();
            Achievements = new List<Medal>();
        }

        private MapGrid _mapGrid;
        public MapGrid MapGrid
        {
            get
            {
                if (_mapGrid == null)
                {
                    InitMap();
                }
                return _mapGrid;
            }
        }

        public void InitMap()
        {
            if (Team != 0)
            {
                var replay = ReplayData();

                var map = Dictionaries.Instance.Maps[replay.datablock_1.mapName];

                _mapGrid = new MapGrid(new MapElementContext(map, replay.datablock_1.gameplayID, Team, 300, 300));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayFile" /> class.
        /// </summary>
        /// <param name="replay">The replay.</param>
        /// <param name="folderId">The folder id.</param>
        protected ReplayFile(Domain.Replay.Replay replay, Guid folderId) : this()
        {
            FolderId = folderId;

            if (replay != null)
            {
                PlayTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));

                ClientVersion = ReplayFileHelper.ResolveVersion(replay.datablock_1.Version, PlayTime);

                TankDescription description = Dictionaries.Instance.GetReplayTankDescription(replay.datablock_1.playerVehicle, ClientVersion); 

                Icon = description.Icon;

                CountryId = (Country) description.CountryId;

                Tank = description;

                TankName = Tank.Title;

                ReplayId = Int64.Parse(PlayTime.ToString("yyyyMMddHHmm"));

                PlayerId = replay.datablock_1.playerID;

                PlayerName = replay.datablock_1.playerName;

                TeamMembers = replay.datablock_1.vehicles.Values.ToList();


                MapName = replay.datablock_1.mapDisplayName;
                MapNameId = replay.datablock_1.mapName;

                if (Dictionaries.Instance.Maps.ContainsKey(replay.datablock_1.mapName))
                {
                    MapId = Dictionaries.Instance.Maps[replay.datablock_1.mapName].MapId;
                }
                else
                {
                    Log.WarnFormat("Unknown map: {0}", replay.datablock_1.mapName);
                }

                BattleType = (BattleType) replay.datablock_1.battleType;
                Gameplay = (Gameplay) Enum.Parse(typeof (Gameplay), replay.datablock_1.gameplayID);
                Team = TeamMembers.First(x => x.name == replay.datablock_1.playerName).team;

                if (replay.datablock_battle_result != null)
                {
                    int autoRepairCost = replay.datablock_battle_result.personal.autoRepairCost ?? 0;
                    int autoLoadCost = ReplayFileHelper.GetAutoLoadCost(replay);
                    int autoEquipCost = ReplayFileHelper.GetAutoEquipCost(replay);

                    Credits = replay.datablock_battle_result.personal.credits;
                    CreditsEarned = Credits - autoRepairCost - autoLoadCost - autoEquipCost;
                    DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                    DamageReceived = replay.datablock_battle_result.personal.damageReceived;
                    IsWinner = GetBattleStatus(replay);
                    Xp = replay.datablock_battle_result.personal.xp;
                    OriginalXp = replay.datablock_battle_result.personal.originalXP;
                    Killed = replay.datablock_battle_result.personal.kills;
                    Damaged = replay.datablock_battle_result.personal.damaged;
                    Spotted = replay.datablock_battle_result.personal.spotted;
                    DamageAssisted = replay.datablock_battle_result.personal.damageAssisted;
                    DamageAssistedRadio = replay.datablock_battle_result.personal.damageAssistedRadio;
                    DamageAssistedTrack = replay.datablock_battle_result.personal.damageAssistedTrack;
                    PotentialDamageReceived = replay.datablock_battle_result.personal.potentialDamageReceived;
                    DamageBlockedByArmor = replay.datablock_battle_result.personal.damageBlockedByArmor;
                    MarkOfMastery = replay.datablock_battle_result.personal.markOfMastery;
                    BattleTime = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                    LifeTime = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                    IsAlive = replay.datablock_battle_result.personal.deathReason == -1 ||
                              replay.datablock_battle_result.personal.killerID == 0;
                    Medals = Dictionaries.Instance.GetMedals(replay.datablock_battle_result.personal.achievements);
                    Achievements =
                        Dictionaries.Instance.GetAchievMedals(replay.datablock_battle_result.personal.dossierPopUps)
                            .Except(Medals)
                            .ToList();
                    MedalsCount = Medals.Count;
                    AchievementsCount = Achievements.Count;
                    IsPlatoon = ResolvePlatoonFlag(replay);

                    BattleType = (BattleType) replay.datablock_battle_result.common.bonusType;
                    DeathReason = ResolveDeathReason(replay);
                    FinishReason = (FinishReason) replay.datablock_battle_result.common.finishReason;
                }

                IsWinnerString = Resources.Resources.ResourceManager.GetEnumResource((Enum) IsWinner);
                BattleTypeString = Resources.Resources.ResourceManager.GetEnumResource((Enum) BattleType);
                DeathReasonString = Resources.Resources.ResourceManager.GetEnumResource((Enum) DeathReason);
                FinishReasonString = Resources.Resources.ResourceManager.GetEnumResource((Enum) FinishReason);
            }
        }

        #endregion

        #region Help functions

        private DeathReason ResolveDeathReason(Domain.Replay.Replay replay)
        {
            var deathReason = (DeathReason) replay.datablock_battle_result.personal.deathReason;
            if (deathReason == DeathReason.DestroyedByShot && replay.datablock_battle_result.personal.damageReceived < Tank.Health)
            {
                return DeathReason.CrewDead;
            }
            return deathReason;
        }

        private bool ResolvePlatoonFlag(Domain.Replay.Replay replay)
        {
            if (PlayerId != 0)
            {
                var player = replay.datablock_battle_result.players[PlayerId];
                return player.platoonID > 0;
            }
            return replay.datablock_battle_result.players.Values.Any(x => x.name == PlayerName && x.platoonID > 0);
        }

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

        #endregion

        #region File Operations

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Moves replay to the specified folder.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        public abstract void Move(ReplayFolder targetFolder);

        /// <summary>
        /// Gets Replay data.
        /// </summary>
        /// <param name="readAdvancedData"></param>
        /// <returns></returns>
        public abstract Domain.Replay.Replay ReplayData(bool readAdvancedData = false);

        #endregion

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
