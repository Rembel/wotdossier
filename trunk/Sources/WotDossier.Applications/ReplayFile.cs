using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using WotDossier.Applications.ViewModel;
using WotDossier.Dal;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications
{
    public class ReplayFile:INotifyPropertyChanged
    {
        private string _link;
        //20121201_1636_ussr-IS_42_north_america
        private const string REPLAY_DATETIME_FORMAT = @"(\d+_\d+)";
        private const string MAPNAME_FILENAME = @"(\d+_[a-zA-Z_]+)(\.wotreplay)";
        private const string TANKNAME_FORMAT = @"([a-zA-Z]+)-(.+)";

        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }
        public string ClientVersion { get; set; }
        public string Tank { get; set; }
        public int CountryId { get; set; }
        public DateTime PlayTime { get; set; }
        public int Damaged { get; set; }
        public int Killed { get; set; }
        public long PlayerId { get; set; }
        public long ReplayId { get; set; }
        public int Xp { get; set; }
        public BattleStatus IsWinner { get; set; }
        public int DamageReceived { get; set; }
        public int DamageDealt { get; set; }
        public int Credits { get; set; }
        public FileInfo FileInfo { get; set; }
        
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
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReplayFile(FileInfo replayFileInfo) : this(replayFileInfo, null)
        {
        }

        public ReplayFile(FileInfo replayFileInfo, Replay replay)
        {
            if (replay != null)
            {
                MapName = replay.datablock_1.mapDisplayName;
                MapNameId = replay.datablock_1.mapName;
                if (WotApiClient.Instance.Maps.ContainsKey(replay.datablock_1.mapName))
                {
                    MapId = WotApiClient.Instance.Maps[replay.datablock_1.mapName].mapid;
                }
                ClientVersion = replay.datablock_1.clientVersionFromExe;

                Regex tankNameRegexp = new Regex(TANKNAME_FORMAT);
                Match tankNameMatch = tankNameRegexp.Match(replay.datablock_1.playerVehicle);
                CountryId = WotApiHelper.GetCountryId(tankNameMatch.Groups[1].Value);
                Tank = tankNameMatch.Groups[2].Value;

                PlayTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));
                ReplayId = Int64.Parse(PlayTime.ToString("yyyyMMddHHmm"));

                FileInfo = replayFileInfo;
                Credits = replay.CommandResult.Damage.credits;
                DamageDealt = replay.CommandResult.Damage.damageDealt;
                DamageReceived = replay.CommandResult.Damage.damageReceived;
                IsWinner = (BattleStatus) replay.CommandResult.Damage.isWinner;
                Xp = replay.CommandResult.Damage.xp;
                Killed = replay.CommandResult.Damage.killed.Count;
                Damaged = replay.CommandResult.Damage.damaged.Count;
                PlayerId = replay.datablock_1.playerID;
            }
            else
            {
                FileInfo = replayFileInfo;
                string fileName = replayFileInfo.Name;

                Regex dateTimeRegexp = new Regex(REPLAY_DATETIME_FORMAT);
                Match dateTimeMatch = dateTimeRegexp.Match(fileName);
                CultureInfo provider = CultureInfo.InvariantCulture;
                PlayTime = DateTime.ParseExact(dateTimeMatch.Groups[1].Value, "yyyyMMdd_HHmm", provider);

                Regex mapNameRegexp = new Regex(MAPNAME_FILENAME);
                Match mapNameMatch = mapNameRegexp.Match(fileName);
                MapName = mapNameMatch.Groups[1].Value;

                Regex tankNameRegexp = new Regex(TANKNAME_FORMAT);
                string tankName = fileName.Replace(dateTimeMatch.Groups[1].Value, "").Replace(mapNameMatch.Groups[0].Value, "");
                tankName = tankName.Substring(1, tankName.Length - 2);
                Match tankNameMatch = tankNameRegexp.Match(tankName);
                CountryId = WotApiHelper.GetCountryId(tankNameMatch.Groups[1].Value);
                Tank = tankNameMatch.Groups[2].Value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
