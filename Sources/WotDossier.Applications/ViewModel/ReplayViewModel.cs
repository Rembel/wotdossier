using System;
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
        private List<CombatTarget> _combatEffects;
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

        public List<CombatTarget> CombatEffects
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

        public string FullName { get; set; }

        public string Tank { get; set; }

        public List<int> BattleMedals { get; set; }

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

        public bool Init(Replay replay, ReplayFile replayFile)
        {
            Replay = replay;
            
            if (replay.datablock_battle_result != null)
            {
                MapName = replay.datablock_1.mapName;
                MapDisplayName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName, GetMapMode(replay.datablock_1.gameplayID));
                TankIcon = WotApiClient.Instance.GetTankIcon(replay.datablock_1.playerVehicle);
                Date = replay.datablock_1.dateTime;

                List<KeyValuePair<long, Player>> players = replay.datablock_battle_result.players.ToList();
                List<KeyValuePair<long, VehicleResult>> vehicleResults = replay.datablock_battle_result.vehicles.ToList();
                List<KeyValuePair<long, Vehicle>> vehicles = replay.datablock_1.vehicles.ToList();
                IEnumerable<TeamMember> teamMembers = players.Join(vehicleResults, p => p.Key, vr => vr.Value.accountDBID, Tuple.Create).Join(vehicles, pVr => pVr.Item2.Key, v => v.Key, (pVr, v) => new TeamMember(pVr.Item1, pVr.Item2, v)).ToList();

                int myTeamId = replay.datablock_battle_result.players[replay.datablock_1.playerID].team;

                FirstTeam = teamMembers.Where(x => x.team == myTeamId).OrderByDescending(x => x.xp).ToList();
                SecondTeam = teamMembers.Where(x => x.team != myTeamId).OrderByDescending(x => x.xp).ToList();

                List<long> squads1 = FirstTeam.Where(x => x.prebattleID > 0).OrderBy(x => x.prebattleID).Select(x => x.prebattleID).Distinct().ToList();
                List<long> squads2 = SecondTeam.Where(x => x.prebattleID > 0).OrderBy(x => x.prebattleID).Select(x => x.prebattleID).Distinct().ToList();

                FirstTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads1.IndexOf(tm.prebattleID) + 1; });
                SecondTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads2.IndexOf(tm.prebattleID) + 1; });

                CombatEffects = replay.datablock_battle_result.personal.details.Select(x => new CombatTarget(x, teamMembers.First(tm => tm.Id == x.Key))).ToList();

                FullName = string.Format("{0} {1}", replay.datablock_1.playerName,
                                         replay.datablock_battle_result.players[replay.datablock_1.playerID].clanAbbrev);

                TeamMember replayUser = teamMembers.First(x => x.accountDBID == replay.datablock_1.playerID);
                Tank = replayUser.Tank;

                double premiumFactor = replay.datablock_battle_result.personal.premiumCreditsFactor10 / (double)10;
                PremiumCredits = replay.datablock_battle_result.personal.credits;
                PremiumTotalXp = replay.datablock_battle_result.personal.xp;
                PremiumXp = (int)Math.Round(replayUser.xp * premiumFactor, 0);
                Credits = (int) Math.Round((PremiumCredits / premiumFactor), 0);
                ActionCredits = Credits - replayUser.credits;
                XpFactor = replay.datablock_battle_result.personal.dailyXPFactor10/10;
                TotalXp = replayUser.xp * XpFactor;
                Xp = replayUser.xp;
                XpPenalty = replay.datablock_battle_result.personal.xpPenalty;
                XpTitle = GetXpTitle(XpFactor);

                CreditsContributionOut = replay.datablock_battle_result.personal.creditsContributionOut;
                CreditsContributionIn = replay.datablock_battle_result.personal.creditsContributionIn;
                AutoRepairCost = replay.datablock_battle_result.personal.autoRepairCost ?? 0;
                AutoLoadCost = GetAutoLoadCost(replay);
                AutoEquipCost = GetAutoEquipCost(replay);
                
                PremiumTotalCredits = PremiumCredits - AutoRepairCost - AutoLoadCost - AutoEquipCost;
                TotalCredits = Credits - AutoRepairCost - AutoLoadCost - AutoEquipCost;

                Shots = replay.datablock_battle_result.personal.shots;
                Hits = replay.datablock_battle_result.personal.hits;
                HEHits = replay.datablock_battle_result.personal.he_hits;
                Pierced = replay.datablock_battle_result.personal.pierced;
                DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                ShotsReceived = replay.datablock_battle_result.personal.shotsReceived;
                TDamage = string.Format("{0}/{1}", replay.datablock_battle_result.personal.tkills, replay.datablock_battle_result.personal.tdamageDealt);
                Damaged = replay.datablock_battle_result.personal.damaged;
                Kills = replay.datablock_battle_result.personal.kills;
                Spotted = replay.datablock_battle_result.personal.spotted;
                DamageAssisted = replay.datablock_battle_result.personal.damageAssisted;
                CapturePoints = replay.datablock_battle_result.personal.capturePoints;
                DroppedCapturePoints = replay.datablock_battle_result.personal.droppedCapturePoints;
                Mileage = string.Format("{0:.00} км", replay.datablock_battle_result.personal.mileage/(double)1000);

                StartTime = replayFile.PlayTime.ToShortTimeString();
                TimeSpan battleLength = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                BattleTime = battleLength.ToString("m' м 's' с'");

                TimeSpan userbattleLength = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                UserBattleTime = userbattleLength.ToString("m' м 's' с'");

                return true;
            }
            return false;
        }

        private static int GetAutoEquipCost(Replay replay)
        {
            if (replay.datablock_battle_result.personal.autoEquipCost != null)
            {
                return replay.datablock_battle_result.personal.autoEquipCost.Sum();
            }
            return 0;
        }

        private static int GetAutoLoadCost(Replay replay)
        {
            if (replay.datablock_battle_result.personal.autoLoadCost!= null)
            {
                return replay.datablock_battle_result.personal.autoLoadCost.Sum();
            }
            return 0;
        }

        public int HEHits { get; set; }

        public int XpFactor { get; set; }

        public int XpPenalty { get; set; }

        public string UserBattleTime { get; set; }

        public string BattleTime { get; set; }

        public string StartTime { get; set; }

        public string Mileage { get; set; }

        public int DroppedCapturePoints { get; set; }

        public int CapturePoints { get; set; }

        public int DamageAssisted { get; set; }

        public int Spotted { get; set; }

        public int Kills { get; set; }

        public int Damaged { get; set; }

        public string TDamage { get; set; }

        public int ShotsReceived { get; set; }

        public int DamageDealt { get; set; }

        public int Pierced { get; set; }

        public int Hits { get; set; }

        public int Shots { get; set; }

        public int CreditsContributionOut { get; set; }

        public int CreditsContributionIn { get; set; }

        public int TotalCredits { get; set; }

        public int PremiumTotalCredits { get; set; }

        public int AutoEquipCost { get; set; }

        public int AutoLoadCost { get; set; }

        public int AutoRepairCost { get; set; }

        public int ActionCredits { get; set; }

        private string GetXpTitle(int dailyXpFactor)
        {
            if (dailyXpFactor > 1)
            {
                return string.Format("Опыт (x{0} за первую победу в день)", dailyXpFactor);
            }
            return "Опыт";
        }

        public string XpTitle { get; set; }

        public int TotalXp { get; set; }

        public int Xp { get; set; }

        public int PremiumTotalXp { get; set; }
        public int PremiumXp { get; set; }
        
        public int TotalPremiumXp { get; set; }

        public int Credits { get; set; }

        public int PremiumCredits { get; set; }

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

    public class CombatTarget
    {
        public TeamMember TeamMember { get; set; }

        public CombatTarget(KeyValuePair<long, DamagedVehicle> vehicleDamage, TeamMember teamMember)
        {
            TeamMember = teamMember;

            crits = vehicleDamage.Value.crits;
            critsTooltip = string.Format("Вы нанесли критических повреждений: {0}", crits);
            damageAssisted = vehicleDamage.Value.damageAssisted;
            damageAssistedTooltip = string.Format("По вашим разведданным союзники нанесли очков урона: {0}", damageAssisted);
            damageDealt = vehicleDamage.Value.damageDealt;
            damageDealtTooltip = string.Format("Вы нанесли урона: {0}", damageDealt);
            fire = vehicleDamage.Value.fire;
            he_hits = vehicleDamage.Value.he_hits;
            hits = vehicleDamage.Value.hits;
            killed = vehicleDamage.Value.killed;
            pierced = vehicleDamage.Value.pierced;
            spotted = vehicleDamage.Value.spotted;
            spottedTooltip = spotted > 0 ? "Вы обнаружили этот танк противника" : string.Empty;
        }

        public int crits { get; set; }
        public string critsTooltip { get; set; }
        public int damageAssisted { get; set; }
        public string damageAssistedTooltip { get; set; }
        public int damageDealt { get; set; }
        public string damageDealtTooltip { get; set; }
        public int fire { get; set; }
        public int he_hits { get; set; }
        public int hits { get; set; }
        public int killed { get; set; }
        public int pierced { get; set; }
        public int spotted { get; set; }
        public string spottedTooltip { get; set; }
    }
}
