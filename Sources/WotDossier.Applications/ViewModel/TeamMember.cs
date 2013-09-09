using System.Collections.Generic;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    public class TeamMember
    {
        private List<int> _achievements;

        public TeamMember(KeyValuePair<long, Player> player, KeyValuePair<long, VehicleResult> vehicleResult, KeyValuePair<long, Vehicle> vehicle, int replayPlayerTeam)
        {
            Id = vehicle.Key;
            string[] strings = vehicle.Value.vehicleType.Split(':');
            string tankCountryCode = strings[0];
            string tankIcon = strings[1];
            TankInfo tank = WotApiClient.Instance.TanksDictionary.Values.FirstOrDefault(x => x.countryCode.Equals(tankCountryCode) && x.icon_orig.Equals(tankIcon));
            Tank = tank != null ? tank.title : tankIcon;
            TankIcon = WotApiClient.Instance.GetTankIcon(vehicle.Value.vehicleType);
            ClanAbbrev = vehicle.Value.clanAbbrev;
            Name = vehicle.Value.name;
            FullName = string.Format("{0}{1}", Name, GetClanAbbrev(ClanAbbrev));
            VehicleType = vehicle.Value.vehicleType;
            Team = vehicle.Value.team;
            IsTeamKiller = vehicle.Value.isTeamKiller;
            IsAlive = vehicle.Value.isAlive;

            ClanDBID = player.Value.clanDBID;
            PrebattleId = player.Value.prebattleID;

            AccountDBID = vehicleResult.Value.accountDBID;
            achievements = vehicleResult.Value.achievements;
            BattleMedals = MedalHelper.GetMedals(vehicleResult.Value.achievements);
            CapturePoints = vehicleResult.Value.capturePoints;
            Credits = vehicleResult.Value.credits;
            DamageAssisted = vehicleResult.Value.damageAssisted;
            DamageDealt = vehicleResult.Value.damageDealt;
            DamageReceived = vehicleResult.Value.damageReceived;
            Damaged = vehicleResult.Value.damaged;
            DroppedCapturePoints = vehicleResult.Value.droppedCapturePoints;
            FreeXp = vehicleResult.Value.freeXP;
            Gold = vehicleResult.Value.gold;
            HEHits = vehicleResult.Value.he_hits;
            Health = vehicleResult.Value.health;
            Hits = vehicleResult.Value.hits;
            IsTeamKiller = vehicleResult.Value.isTeamKiller;
            KillerId = vehicleResult.Value.killerID;
            Kills = vehicleResult.Value.kills;
            LifeTime = vehicleResult.Value.lifeTime;
            Mileage = vehicleResult.Value.mileage;
            Pierced = vehicleResult.Value.pierced;
            PotentialDamageReceived = vehicleResult.Value.potentialDamageReceived;
            Repair = vehicleResult.Value.repair;
            Shots = vehicleResult.Value.shots;
            ShotsReceived = vehicleResult.Value.shotsReceived;
            Spotted = vehicleResult.Value.spotted;
            TDamageDealt = vehicleResult.Value.tdamageDealt;
            //team = vehicleResult.Value;
            THits = vehicleResult.Value.thits;
            TKills = vehicleResult.Value.tkills;
            TypeCompDescr = vehicleResult.Value.typeCompDescr;
            Xp = vehicleResult.Value.xp;

            TeamMate = Team == replayPlayerTeam;
        }

        private string GetClanAbbrev(string abbrev)
        {
            if (!string.IsNullOrEmpty(abbrev))
            {
                return string.Format("[{0}]", abbrev);
            }
            return string.Empty;
        }

        public List<Medal> BattleMedals { get; set; }

        public string Tank { get; set; }

        public TankIcon TankIcon { get; set; }

        public long Id { get; set; }
        public string FullName { get; set; }
        
        public string ClanAbbrev { get; set; }
        public long ClanDBID { get; set; }
        public string Name { get; set; }
        public long PrebattleId { get; set; }
        public int Team { get; set; }

        public long AccountDBID { get; set; }
        public List<int> achievements
        {
            get { return _achievements ?? new List<int>(); }
            set { _achievements = value; }
        }

        public int CapturePoints { get; set; }
        public int Credits { get; set; }
        public int DamageAssisted { get; set; }
        public int DamageDealt { get; set; }
        public int DamageReceived { get; set; }
        public int Damaged { get; set; }
        public int DroppedCapturePoints { get; set; }
        public int FreeXp { get; set; }
        public int Gold { get; set; }
        public int HEHits { get; set; }
        public int Health { get; set; }
        public int Hits { get; set; }
        public bool IsTeamKiller { get; set; }
        public int KillerId { get; set; }
        public int Kills { get; set; }
        public int LifeTime { get; set; }
        public int Mileage { get; set; }
        public int Pierced { get; set; }
        public int PotentialDamageReceived { get; set; }
        public int Repair { get; set; }
        public int Shots { get; set; }
        public int ShotsReceived { get; set; }
        public int Spotted { get; set; }
        public double TDamageDealt { get; set; }
        //public int team { get; set; }
        public int THits { get; set; }
        public int TKills { get; set; }
        public int TypeCompDescr { get; set; }
        public int Xp { get; set; }

        //public string clanAbbrev { get; set; }
        //        "events": {}, 
        public bool IsAlive { get; set; }
        //public bool isTeamKiller { get; set; }
        //public string name { get; set; }
        //public int team { get; set; }
        public string VehicleType { get; set; }

        public int Squad { get; set; }

        public bool TeamMate { get; set; }
    }
}