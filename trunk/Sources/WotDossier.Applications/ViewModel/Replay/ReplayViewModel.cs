using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json.Linq;
using WotDossier.Applications.View;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel.Replay
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        #region Fields and Properties

        public string Title { get; set; }

        public Domain.Replay.Replay Replay { get; set; }

        public List<CombatTarget> CombatEffects { get; set; }

        public List<TeamMember> FirstTeam { get; set; }

        public List<TeamMember> SecondTeam { get; set; }

        public TankIcon TankIcon { get; set; }

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

        public int PotentialDamageReceived { get; set; }

        public int DamageBlockedByArmor { get; set; }

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

        public string MapDisplayName { get; set; }

        public string MapName { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }

        private List<DeviceDescription> _devices;
        public List<DeviceDescription> Devices
        {
            get { return _devices; }
            set { _devices = value; }
        }

        private List<ConsumableDescription> _consumables = new List<ConsumableDescription>();
        public List<ConsumableDescription> Consumables
        {
            get { return _consumables; }
            set { _consumables = value; }
        }

        private TeamMember _alienTeamMember;
        public TeamMember AlienTeamMember
        {
            get { return _alienTeamMember; }
            set
            {
                _alienTeamMember = value;
                RaisePropertyChanged("AlienTeamMember");
            }
        }
        
        private TeamMember _ourTeamMember;
        public TeamMember OurTeamMember
        {
            get { return _ourTeamMember; }
            set
            {
                _ourTeamMember = value;
                RaisePropertyChanged("OurTeamMember");
            }
        }

        public DelegateCommand HideTeamMemberResultsCommand { get; set; }
        public DelegateCommand<IList<object>> CopyCommand { get; set; }
        public DelegateCommand<TeamMember> CopyPlayerNameCommand { get; set; }
        public DelegateCommand<TeamMember> OpenPlayerCommand { get; set; }

        public TeamMember ReplayUser { get; set; }

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
            HideTeamMemberResultsCommand = new DelegateCommand(OnHideTeamMemberResultsCommand);
            CopyCommand = new DelegateCommand<IList<object>>(OnCopyCommand);
            CopyPlayerNameCommand = new DelegateCommand<TeamMember>(OnCopyPlayerNameCommand);
            OpenPlayerCommand = new DelegateCommand<TeamMember>(OnOpenPlayerCommand);
        }

        private void OnCopyPlayerNameCommand(TeamMember player)
        {
            if (player != null)
            {
                Clipboard.SetText(player.Name);
            }
        }

        private void OnOpenPlayerCommand(TeamMember member)
        {
            Domain.Server.Player player;
            using (new WaitCursor())
            {
                player = WotApiClient.Instance.LoadPlayerStat((int) member.AccountDBID, SettingsReader.Get(), PlayerStatLoadOptions.LoadVehicles | PlayerStatLoadOptions.LoadAchievments);
            }
            if (player != null)
            {
                PlayerServerStatisticViewModel viewModel = CompositionContainerFactory.Instance.GetExport<PlayerServerStatisticViewModel>();
                viewModel.Init(player);
                viewModel.Show();
            }
            else
            {
                MessageBox.Show(string.Format(Resources.Resources.Msg_GetPlayerData, member.Name), Resources.Resources.WindowCaption_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnCopyCommand(IList<object> rows)
        {
            if (rows != null)
            {
                IEnumerable<ChatMessage> chatMessages = rows.Cast<ChatMessage>();

                StringBuilder builder = new StringBuilder();

                foreach (ChatMessage message in chatMessages)
                {
                    builder.AppendFormat("{0}: {1}", message.Player, message.Text);
                    builder.AppendLine();
                }

                Clipboard.SetText(builder.ToString());
            }
        }

        private void OnHideTeamMemberResultsCommand()
        {
            OurTeamMember = null;
            AlienTeamMember = null;
        }

        public void Show()
        {
            ViewTyped.Show();
        }

        public bool Init(Domain.Replay.Replay replay, ReplayFile replayFile)
        {
            Replay = replay;
            
            if (replay.datablock_battle_result != null)
            {
                MapName = replay.datablock_1.mapName;
                MapDisplayName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName, GetMapMode(replay.datablock_1.gameplayID, (BattleType)replay.datablock_1.battleType));
                TankIcon = Dictionaries.Instance.GetTankIcon(replay.datablock_1.playerVehicle);
                Date = replay.datablock_1.dateTime;

                long playerId = replay.datablock_battle_result.personal.accountDBID;
                int myTeamId = replay.datablock_battle_result.players[playerId].team;

                List<TeamMember> teamMembers = GetTeamMembers(replay, myTeamId);

                FirstTeam = teamMembers.Where(x => x.Team == myTeamId).OrderByDescending(x => x.Xp).ToList();
                SecondTeam = teamMembers.Where(x => x.Team != myTeamId).OrderByDescending(x => x.Xp).ToList();
                ReplayUser = teamMembers.First(x => x.AccountDBID == playerId);

                List<long> squads1 = FirstTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();
                List<long> squads2 = SecondTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();

                FirstTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads1.IndexOf(tm.platoonID) + 1; });
                SecondTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads2.IndexOf(tm.platoonID) + 1; });

                CombatEffects = GetCombatTargets(replay, teamMembers);

                Tank = ReplayUser.Tank;
                FullName = ReplayUser.FullName;

                double premiumFactor = replay.datablock_battle_result.personal.premiumCreditsFactor10 / (double)10;
                PremiumCredits = replay.datablock_battle_result.personal.credits;
                PremiumTotalXp = replay.datablock_battle_result.personal.xp;
                PremiumXp = (int)Math.Round(ReplayUser.Xp * premiumFactor, 0);
                Credits = (int) Math.Round((PremiumCredits / premiumFactor), 0);
                ActionCredits = Credits - ReplayUser.Credits;
                XpFactor = replay.datablock_battle_result.personal.dailyXPFactor10/10;
                TotalXp = ReplayUser.Xp * XpFactor;
                Xp = ReplayUser.Xp;
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
                PotentialDamageReceived = replay.datablock_battle_result.personal.potentialDamageReceived;
                DamageBlockedByArmor = replay.datablock_battle_result.personal.damageBlockedByArmor;
                Mileage = string.Format(Resources.Resources.Traveled_Format, replay.datablock_battle_result.personal.mileage/(double)1000);

                StartTime = replayFile.PlayTime.ToShortTimeString();
                TimeSpan battleLength = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                BattleTime = battleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                List<Medal> medals = GetMedals(replay);

                BattleMedals = medals.Where(x => x.Type == 0).ToList();
                AchievMedals = medals.Where(x => x.Type == 1).ToList();

                TimeSpan userbattleLength = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                UserBattleTime = userbattleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                //calc levels by squad
                List<LevelRange> membersLevels = new List<LevelRange>();
                membersLevels.AddRange(teamMembers.Where(x => x.Squad == 0).Select(x => x.LevelRange));
                IEnumerable<IGrouping<int, TeamMember>> squads = teamMembers.Where(x => x.Squad > 0).GroupBy(x => x.Squad);
                membersLevels.AddRange(squads.Select(x => x.Select(s => s.LevelRange).OrderByDescending(o => o.Max).First()));

                int level = Dictionaries.Instance.GetBattleLevel(membersLevels);

                Title = string.Format(Resources.Resources.WindowTitleFormat_Replay, Tank, MapDisplayName, level > 0 ? level.ToString(CultureInfo.InvariantCulture) : "n/a");

                Status = GetBattleStatus(replay);

                _devices = new List<DeviceDescription>();

                if (replay.datablock_advanced != null)
                {
                    if (replay.datablock_advanced.roster != null && replay.datablock_advanced.roster.ContainsKey(replay.datablock_1.playerName))
                    {
                        var info = replay.datablock_advanced.roster[replay.datablock_1.playerName];

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_0))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_0]);
                        }

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_1))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_1]);
                        }

                        if (Dictionaries.Instance.DeviceDescriptions.ContainsKey(info.vehicle.module_2))
                        {
                            _devices.Add(Dictionaries.Instance.DeviceDescriptions[info.vehicle.module_2]);
                        }
                    }
                    
                    foreach (Slot slot in replay.datablock_advanced.Slots)
                    {
                        if (Dictionaries.Instance.ConsumableDescriptions.ContainsKey((int) slot.item.id) &&
                            slot.item.type_id == SlotType.Equipment)
                        {
                            Consumables.Add(Dictionaries.Instance.ConsumableDescriptions[(int) slot.item.id]);
                        }
                    }

                    ChatMessages = replay.datablock_advanced.Messages;
                }

                return true;
            }
            return false;
        }

        private List<Medal> GetMedals(Domain.Replay.Replay replay)
        {
            return ReplayUser.BattleMedals.Union(Dictionaries.Instance.GetAchievMedals(replay.datablock_battle_result.personal.dossierPopUps))
                .Union(Dictionaries.Instance.GetAchievMedals(new List<List<JValue>> { new List<JValue> { new JValue(7900 + replay.datablock_battle_result.personal.markOfMastery), new JValue(0) } })).ToList();
        }

        private List<CombatTarget> GetCombatTargets(Domain.Replay.Replay replay, List<TeamMember> teamMembers)
        {
            return replay.datablock_battle_result.personal.details
                .Where(x => x.Key != ReplayUser.Id)
                .Select(x => new CombatTarget(x, teamMembers.First(tm => tm.Id == x.Key), replay.datablock_1.clientVersionFromExe))
                .OrderBy(x => x.TeamMember.FullName)
                .ToList();
        }

        private static List<TeamMember> GetTeamMembers(Domain.Replay.Replay replay, int myTeamId)
        {
            List<KeyValuePair<long, Player>> players = replay.datablock_battle_result.players.ToList();
            List<KeyValuePair<long, VehicleResult>> vehicleResults = replay.datablock_battle_result.vehicles.ToList();
            List<KeyValuePair<long, Vehicle>> vehicles = replay.datablock_1.vehicles.ToList();
            List<TeamMember> teamMembers =
                players.Join(vehicleResults, p => p.Key, vr => vr.Value.accountDBID, Tuple.Create)
                    .Join(vehicles, pVr => pVr.Item2.Key, v => v.Key,
                        (pVr, v) => new TeamMember(pVr.Item1, pVr.Item2, v, myTeamId, replay.datablock_1.regionCode))
                    .ToList();
            return teamMembers;
        }

        private BattleStatus GetBattleStatus(Domain.Replay.Replay replay)
        {
            if(replay.datablock_battle_result.common.winnerTeam == 0)
            {
                return BattleStatus.Draw;
            }

            if (replay.datablock_battle_result.common.winnerTeam == ReplayUser.Team)
            {
                return BattleStatus.Victory;
            }

            return BattleStatus.Defeat;
        }

        private static int GetAutoEquipCost(Domain.Replay.Replay replay)
        {
            if (replay.datablock_battle_result.personal.autoEquipCost != null)
            {
                return replay.datablock_battle_result.personal.autoEquipCost.Sum();
            }
            return 0;
        }

        private static int GetAutoLoadCost(Domain.Replay.Replay replay)
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

        private object GetMapMode(string gameplayId, BattleType battleType)
        {
            List<BattleType> list = new List<BattleType>
            {
                BattleType.Unknown,
                BattleType.Regular,
                BattleType.CompanyWar,
                BattleType.ClanWar
            };

            if (list.Contains(battleType))
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
            return Resources.Resources.ResourceManager.GetEnumResource(battleType);
        }
    }
}
