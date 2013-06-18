using System.Collections.Generic;
using WotDossier.Dal;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using System.Linq;

namespace WotDossier.Applications.ViewModel
{
    public class TeamMember
    {
        public TeamMember(KeyValuePair<long, Player> player, KeyValuePair<long, VehicleResult> vehicleResult, KeyValuePair<long, Vehicle> vehicle)
        {
            Id = vehicle.Key;
            string[] strings = vehicle.Value.vehicleType.Split(':');
            string tankCountryCode = strings[0];
            string tankIcon = strings[1];
            TankInfo tank = WotApiClient.Instance.TanksDictionary.Values.FirstOrDefault(x => x.countryCode.Equals(tankCountryCode) && x.icon_orig.Equals(tankIcon));
            Tank = tank != null ? tank.title : tankIcon;
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
            BattleMedals = vehicleResult.Value.achievements;
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

        public List<int> BattleMedals { get; set; }

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
        public double tdamageDealt { get; set; }
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

        public int Squad { get; set; }
    }
}