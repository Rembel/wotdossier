using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using WotDossier.Applications.View;
using WotDossier.Dal;
using WotDossier.Domain;
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
        #region Fields and Properties

        private Replay _replay;
        private List<CombatTarget> _combatEffects;
        private List<TeamMember> _firstTeam;
        private List<TeamMember> _secondTeam;
        private string _mapName;
        private string _mapDisplayName;
        private TankIcon _tankIcon;
        private TeamMember _alienTeamMember;
        private TeamMember _ourTeamMember;

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

        public List<Medal> BattleMedals { get; set; }

        public List<Medal> AchievMedals { get; set; }

        public BattleStatus Status { get; set; }

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

        public string XpTitle { get; set; }

        public int TotalXp { get; set; }

        public int Xp { get; set; }

        public int PremiumTotalXp { get; set; }
        public int PremiumXp { get; set; }
        
        public int TotalPremiumXp { get; set; }

        public int Credits { get; set; }

        public int PremiumCredits { get; set; }

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

        public TeamMember AlienTeamMember
        {
            get { return _alienTeamMember; }
            set
            {
                _alienTeamMember = value;
                RaisePropertyChanged("AlienTeamMember");
            }
        }

        public TeamMember OurTeamMember
        {
            get { return _ourTeamMember; }
            set
            {
                _ourTeamMember = value;
                RaisePropertyChanged("OurTeamMember");
            }
        }

        #endregion

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
                List<TeamMember> teamMembers = players.Join(vehicleResults, p => p.Key, vr => vr.Value.accountDBID, Tuple.Create).Join(vehicles, pVr => pVr.Item2.Key, v => v.Key, (pVr, v) => new TeamMember(pVr.Item1, pVr.Item2, v)).ToList();
                
                long playerId = replay.datablock_battle_result.personal.accountDBID;
                int myTeamId = replay.datablock_battle_result.players[playerId].team;

                FirstTeam = teamMembers.Where(x => x.Team == myTeamId).OrderByDescending(x => x.Xp).ToList();
                SecondTeam = teamMembers.Where(x => x.Team != myTeamId).OrderByDescending(x => x.Xp).ToList();
                TeamMember replayUser = teamMembers.First(x => x.AccountDBID == playerId);

                List<long> squads1 = FirstTeam.Where(x => x.PrebattleId > 0).OrderBy(x => x.PrebattleId).Select(x => x.PrebattleId).Distinct().ToList();
                List<long> squads2 = SecondTeam.Where(x => x.PrebattleId > 0).OrderBy(x => x.PrebattleId).Select(x => x.PrebattleId).Distinct().ToList();

                FirstTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads1.IndexOf(tm.PrebattleId) + 1; });
                SecondTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads2.IndexOf(tm.PrebattleId) + 1; });

                CombatEffects = replay.datablock_battle_result.personal.details.Select(x => new CombatTarget(x, teamMembers.First(tm => tm.Id == x.Key))).ToList();

                Tank = replayUser.Tank;
                FullName = replayUser.FullName;

                double premiumFactor = replay.datablock_battle_result.personal.premiumCreditsFactor10 / (double)10;
                PremiumCredits = replay.datablock_battle_result.personal.credits;
                PremiumTotalXp = replay.datablock_battle_result.personal.xp;
                PremiumXp = (int)Math.Round(replayUser.Xp * premiumFactor, 0);
                Credits = (int) Math.Round((PremiumCredits / premiumFactor), 0);
                ActionCredits = Credits - replayUser.Credits;
                XpFactor = replay.datablock_battle_result.personal.dailyXPFactor10/10;
                TotalXp = replayUser.Xp * XpFactor;
                Xp = replayUser.Xp;
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
                Mileage = string.Format(Resources.Resources.Traveled_Format, replay.datablock_battle_result.personal.mileage/(double)1000);

                StartTime = replayFile.PlayTime.ToShortTimeString();
                TimeSpan battleLength = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                BattleTime = battleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                BattleMedals = replayUser.BattleMedals;
                AchievMedals = MedalHelper.GetAchievMedals(replay.datablock_battle_result.personal.dossierPopUps).Except(BattleMedals).ToList();

                TimeSpan userbattleLength = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                UserBattleTime = userbattleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                if (replay.datablock_battle_result.common.winnerTeam == replayUser.Team)
                {
                    Status = BattleStatus.Win;
                }
                else if (replay.datablock_battle_result.common.winnerTeam < 1)
                {
                    Status = BattleStatus.Draw;
                }
                else
                {
                    Status = BattleStatus.Loose;
                }

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

        private string GetXpTitle(int dailyXpFactor)
        {
            if (dailyXpFactor > 1)
            {
                return string.Format(Resources.Resources.Label_Replay_XpFactorFormat, dailyXpFactor);
            }
            return Resources.Resources.Label_Experience;
        }

        private object GetMapMode(string gameplayId)
        {
            if ("ctf".Equals(gameplayId))
            {
                return Resources.Resources.Label_Replay_MapMode_Standart;
            }
            if ("domination".Equals(gameplayId))
            {
                return Resources.Resources.Label_Replay_MapMode_domination;
            }
            return Resources.Resources.Label_Replay_MapMode_Assault;
        }
    }
}
