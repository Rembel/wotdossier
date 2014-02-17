using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel.Replay
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
        public int Team { get; set; }
        
        public FileInfo FileInfo { get; set; }
        public TankDescription Tank { get; set; }
        public TankIcon Icon { get; set; }
        public List<Medal> Medals { get; set; }
        public List<Vehicle> TeamMembers { get; set; }

        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged("Link");
            }
        }

        public ReplayFile(FileInfo replayFileInfo, Domain.Replay.Replay replay, Guid folderId)
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
                ClientVersion = replay.datablock_1.clientVersionFromExe;

                Regex tankNameRegexp = new Regex(TANKNAME_FORMAT);
                Match tankNameMatch = tankNameRegexp.Match(replay.datablock_1.playerVehicle);
                CountryId = WotApiHelper.GetCountryId(tankNameMatch.Groups[1].Value);
                TankName = tankNameMatch.Groups[2].Value;
                Tank =
                    Dictionaries.Instance.Tanks.Values.FirstOrDefault(
                        x => x.Icon.IconOrig.ToLower() == TankName.ToLower());

                PlayTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));
                ReplayId = Int64.Parse(PlayTime.ToString("yyyyMMddHHmm"));

                FileInfo = replayFileInfo;
                PlayerId = replay.datablock_1.playerID;
                Icon = Dictionaries.Instance.GetTankIcon(replay.datablock_1.playerVehicle);

                if (replay.datablock_1.Version < WotApiClient.JsonFormatedResultsMinVersion)
                {
                    if (replay.datablock_battle_result_plain != null)
                    {
                        Credits = replay.datablock_battle_result_plain.credits;
                        DamageDealt = replay.datablock_battle_result_plain.damageDealt;
                        DamageReceived = replay.datablock_battle_result_plain.damageReceived;
                        IsWinner = (BattleStatus) replay.datablock_battle_result_plain.isWinner;
                        Xp = replay.datablock_battle_result_plain.xp;
                        Killed = replay.datablock_battle_result_plain.killed.Count;
                        Damaged = replay.datablock_battle_result_plain.damaged.Count;
                    }
                }
                else
                {
                    if (replay.datablock_battle_result != null)
                    {
                        Credits = replay.datablock_battle_result.personal.credits;
                        DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                        DamageReceived = replay.datablock_battle_result.personal.damageReceived;
                        IsWinner = GetBattleStatus(replay);
                        Xp = replay.datablock_battle_result.personal.xp;
                        Killed = replay.datablock_battle_result.personal.kills;
                        Damaged = replay.datablock_battle_result.personal.damaged;
                        //Medals = MedalHelper.GetMedals(replay.datablock_battle_result.achieveIndices);
                    }
                }

                TeamMembers = replay.datablock_1.vehicles.Values.ToList();
                Team = TeamMembers.First(x => x.name == replay.datablock_1.playerName).team;
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
