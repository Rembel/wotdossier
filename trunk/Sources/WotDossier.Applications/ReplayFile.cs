using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WotDossier.Applications.ViewModel;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications
{
    public class ReplayFile : INotifyPropertyChanged
    {
        public static readonly string PropDamageDealt = TypeHelper<ReplayFile>.PropertyName(v => v.DamageDealt);
        public static readonly string PropDamaged = TypeHelper<ReplayFile>.PropertyName(v => v.Damaged);
        public static readonly string PropCredits = TypeHelper<ReplayFile>.PropertyName(v => v.Credits);
        public static readonly string PropKilled = TypeHelper<ReplayFile>.PropertyName(v => v.Killed);
        public static readonly string PropXp = TypeHelper<ReplayFile>.PropertyName(v => v.Xp);

        public Guid FolderId { get; set; }
        private string _link;
        //20121201_1636_ussr-IS_42_north_america
        private const string REPLAY_DATETIME_FORMAT = @"(\d+_\d+)";
        private const string MAPNAME_FILENAME = @"(\d+_[a-zA-Z_]+)(\.wotreplay)";
        private const string TANKNAME_FORMAT = @"([a-zA-Z]+)-(.+)";

        public string MapName { get; set; }
        public int MapId { get; set; }
        public string MapNameId { get; set; }
        public string ClientVersion { get; set; }
        public string TankName { get; set; }
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
        public TankInfo Tank { get; set; }
        public TankIcon Icon { get; set; }
        public List<Medal> Medals { get; set; }

        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged("Link");
            }
        }

        public ReplayFile(FileInfo replayFileInfo, Replay replay, Guid folderId)
        {
            FolderId = folderId;

            if (replay == null)
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
                string tankName =
                    fileName.Replace(dateTimeMatch.Groups[1].Value, "").Replace(mapNameMatch.Groups[0].Value, "");
                tankName = tankName.Substring(1, tankName.Length - 2);
                Match tankNameMatch = tankNameRegexp.Match(tankName);
                CountryId = WotApiHelper.GetCountryId(tankNameMatch.Groups[1].Value);
                TankName = tankNameMatch.Groups[2].Value;
            }
            else
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
                TankName = tankNameMatch.Groups[2].Value;
                Tank = WotApiClient.Instance.TanksDictionary.Values.FirstOrDefault(x => x.icon_orig.ToLower() == TankName.ToLower());

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
                Medals = MedalHelper.GetMedals(replay.CommandResult.Damage.achieveIndices);
                Icon = WotApiClient.Instance.GetTankIcon(replay.datablock_1.playerVehicle);
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
