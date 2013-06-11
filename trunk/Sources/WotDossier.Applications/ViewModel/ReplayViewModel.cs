﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework.Applications;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        private Replay _replay;
        private List<object> _combatEffects = new List<object> {1, 2, 3, 4, 5, 6};
        private List<TeamMember> _firstTeam;
        private List<TeamMember> _secondTeam;
        private string _mapName;
        private string _mapDisplayName;
        private TankIcon _tankIcon;

        public Replay Replay
        {
            get { return _replay; }
            set { _replay = value; }
        }

        public List<object> CombatEffects
        {
            get { return _combatEffects; }
            set { _combatEffects = value; }
        }

        public List<TeamMember> FirstTeam
        {
            get { return _firstTeam; }
            set { _firstTeam = value; }
        }

        public List<TeamMember> SecondTeam
        {
            get { return _secondTeam; }
            set { _secondTeam = value; }
        }

        public TankIcon TankIcon
        {
            get { return _tankIcon; }
            set { _tankIcon = value; }
        }

        public string Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel&lt;TView&gt;"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        [ImportingConstructor]
        public ReplayViewModel([Import(typeof(IReplayView))]IReplayView view)
            : base(view)
        {
        }

        public void Show()
        {
            ViewTyped.Show();
        }

        public void Init(Replay replay)
        {
            Replay = replay;
            MapName = replay.datablock_1.mapName;
            MapDisplayName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName, GetMapMode(replay.datablock_1.gameplayID));
            TankIcon = WotApiClient.Instance.GetTankIcon(replay.datablock_1.playerVehicle);
            Date = replay.datablock_1.dateTime;

            if (replay.datablock_battle_result != null)
            {
                List<KeyValuePair<long, Player>> players = replay.datablock_battle_result.players.ToList();
                List<KeyValuePair<long, VehicleResult>> vehicleResults = replay.datablock_battle_result.vehicles.ToList();
                List<KeyValuePair<long, Vehicle>> vehicles = replay.datablock_1.vehicles.ToList();
                IEnumerable<TeamMember> teamMembers = players.Join(vehicleResults, p => p.Key, vr => vr.Value.accountDBID, Tuple.Create).Join(vehicles, p_vr => p_vr.Item2.Key, v => v.Key, (p_vr, v) => new TeamMember(p_vr.Item1, p_vr.Item2, v)).ToList();

                int myTeamId = replay.datablock_battle_result.players[replay.datablock_1.playerID].team;

                FirstTeam = teamMembers.Where(x => x.team == myTeamId).OrderByDescending(x => x.xp).ToList();
                SecondTeam = teamMembers.Where(x => x.team != myTeamId).OrderByDescending(x => x.xp).ToList();
            }
        }

        private object GetMapMode(string gameplayId)
        {
            if ("ctf".Equals(gameplayId))
            {
                return "Стандартный бой";
            }
            if ("domination".Equals(gameplayId))
            {
                return "Встречный бой";
            }
            return "Штурм";
        }

        public string MapDisplayName

        {
            get { return _mapDisplayName; }
            set { _mapDisplayName = value; }
        }

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }
    }

    public class TeamMember
    {
        public TeamMember(KeyValuePair<long, Player> player, KeyValuePair<long, VehicleResult> vehicleResult, KeyValuePair<long, Vehicle> vehicle)
        {
            Id = vehicle.Key;
            Tank = vehicle.Value.vehicleType.Split(':')[1];
            TankIcon = WotApiClient.Instance.GetTankIcon(vehicle.Value.vehicleType);
            clanAbbrev = vehicle.Value.clanAbbrev;
            name = vehicle.Value.name;
            FullName = string.Format("{0} {1}", name, clanAbbrev);
            vehicleType = vehicle.Value.vehicleType;
            team = vehicle.Value.team;
            isTeamKiller = vehicle.Value.isTeamKiller;
            isAlive = vehicle.Value.isAlive;

            clanDBID = player.Value.clanDBID;
            prebattleID = player.Value.prebattleID;

            accountDBID = vehicleResult.Value.accountDBID;
            achievements = vehicleResult.Value.achievements;
            capturePoints = vehicleResult.Value.capturePoints;
            credits = vehicleResult.Value.credits;
            damageAssisted = vehicleResult.Value.damageAssisted;
            damageDealt = vehicleResult.Value.damageDealt;
            damageReceived = vehicleResult.Value.damageReceived;
            damaged = vehicleResult.Value.damaged;
            droppedCapturePoints = vehicleResult.Value.droppedCapturePoints;
            freeXP = vehicleResult.Value.freeXP;
            gold = vehicleResult.Value.gold;
            he_hits = vehicleResult.Value.he_hits;
            health = vehicleResult.Value.health;
            hits = vehicleResult.Value.hits;
            isTeamKiller = vehicleResult.Value.isTeamKiller;
            killerID = vehicleResult.Value.killerID;
            kills = vehicleResult.Value.kills;
            lifeTime = vehicleResult.Value.lifeTime;
            mileage = vehicleResult.Value.mileage;
            pierced = vehicleResult.Value.pierced;
            potentialDamageReceived = vehicleResult.Value.potentialDamageReceived;
            repair = vehicleResult.Value.repair;
            shots = vehicleResult.Value.shots;
            shotsReceived = vehicleResult.Value.shotsReceived;
            spotted = vehicleResult.Value.spotted;
            tdamageDealt = vehicleResult.Value.tdamageDealt;
            //team = vehicleResult.Value;
            thits = vehicleResult.Value.thits;
            tkills = vehicleResult.Value.tkills;
            typeCompDescr = vehicleResult.Value.typeCompDescr;
            xp = vehicleResult.Value.xp;
        }

        public string Tank { get; set; }

        public TankIcon TankIcon { get; set; }

        public long Id { get; set; }
        public string FullName { get; set; }
        
        public string clanAbbrev { get; set; }
        public long clanDBID { get; set; }
        public string name { get; set; }
        public long prebattleID { get; set; }
        public int team { get; set; }

        public long accountDBID { get; set; }
        public List<int> achievements { get; set; }
        public int capturePoints { get; set; }
        public int credits { get; set; }
        public int damageAssisted { get; set; }
        public int damageDealt { get; set; }
        public int damageReceived { get; set; }
        public int damaged { get; set; }
        public int droppedCapturePoints { get; set; }
        public int freeXP { get; set; }
        public int gold { get; set; }
        public int he_hits { get; set; }
        public int health { get; set; }
        public int hits { get; set; }
        public bool isTeamKiller { get; set; }
        public int killerID { get; set; }
        public int kills { get; set; }
        public int lifeTime { get; set; }
        public int mileage { get; set; }
        public int pierced { get; set; }
        public int potentialDamageReceived { get; set; }
        public int repair { get; set; }
        public int shots { get; set; }
        public int shotsReceived { get; set; }
        public int spotted { get; set; }
        public int tdamageDealt { get; set; }
        //public int team { get; set; }
        public int thits { get; set; }
        public int tkills { get; set; }
        public int typeCompDescr { get; set; }
        public int xp { get; set; }

        //public string clanAbbrev { get; set; }
        //        "events": {}, 
        public bool isAlive { get; set; }
        //public bool isTeamKiller { get; set; }
        //public string name { get; set; }
        //public int team { get; set; }
        public string vehicleType { get; set; }
    }
}
