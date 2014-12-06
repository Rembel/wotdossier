using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using Common.Logging;
using Newtonsoft.Json.Linq;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel.Replay.Viewer;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Tank;
using WotDossier.Framework;
using WotDossier.Framework.Applications;
using WotDossier.Framework.Forms.Commands;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel.Replay
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof (ReplayViewModel))]
    public class ReplayViewModel : ViewModel<IReplayView>
    {
        private static readonly ILog _log = LogManager.GetCurrentClassLogger();

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

        public int DamageAssistedTrack { get; set; }
        
        public int DamageAssistedRadio { get; set; }

        public int Spotted { get; set; }

        public int Killed { get; set; }

        public int Damaged { get; set; }

        public string TDamage { get; set; }

        public int ShotsReceived { get; set; }

        public int DamageDealt { get; set; }

        public int Pierced { get; set; }

        public int Hits { get; set; }

        public int Crits { get; set; }

        public int Shots { get; set; }

        public int CreditsContributionOut { get; set; }
        public int PremiumCreditsContributionOut { get; set; }

        public int CreditsContributionIn { get; set; }
        public int PremiumCreditsContributionIn { get; set; }

        public int BaseTotalCredits { get; set; }

        public int PremiumTotalCredits { get; set; }

        public int AutoEquipCost { get; set; }

        public int AutoLoadCost { get; set; }

        public int AutoRepairCost { get; set; }

        public int ActionCredits { get; set; }
        public int ActionXp { get; set; }

        public string XpTitle { get; set; }

        public int BaseTotalXp { get; set; }
        public int TotalXp { get; set; }
        public int TotalCredits { get; set; }

        public int Xp { get; set; }

        public int PremiumTotalXp { get; set; }
        public int PremiumXp { get; set; }
        
        public int Credits { get; set; }

        public int PremiumCredits { get; set; }

        public FinishReason FinishReason { get; set; }

        public DeathReason DeathReason { get; set; }

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

        private List<Slot> _shells = new List<Slot>();
        public List<Slot> Shells
        {
            get { return _shells; }
            set { _shells = value; }
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
        
        private ReplayViewer _replayViewer;
        
        public IMapDescription MapDescription { get; private set; }

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

        private List<TeamMember> _teamMembers;
        private ProgressControlViewModel _simulationWorker;

        public List<TeamMember> TeamMembers
        {
            get { return _teamMembers; }
            set
            {
                _teamMembers = value;
                RaisePropertyChanged("TeamMembers");
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
            PlayCommand = new DelegateCommand(OnPlayCommand);
            SetSpeedCommand = new DelegateCommand<int>(OnSetSpeedCommand);

            ViewTyped.Closing += OnClosing;
        }

        private void OnSetSpeedCommand(int speed)
        {
            if (ReplayViewer != null)
            {
                ReplayViewer.SetSpeed(speed);
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (_simulationWorker != null)
            {
                _simulationWorker.Cancel();
            }
        }

        public DelegateCommand PlayCommand { get; set; }
        public DelegateCommand<int> SetSpeedCommand { get; set; }

        private void OnPlayCommand()
        {
            _simulationWorker = new ProgressControlViewModel();

            ReplayViewer = new ReplayViewer(Replay, TeamMembers.Select(x => new MapVehicle(x)).ToList());

            _simulationWorker.Execute(Resources.Resources.ProgressTitle_Loading_replays, (bw, we) => ReplayViewer.Replay());
        }

        
        public ReplayViewer ReplayViewer
        {
            get { return _replayViewer; }
            set
            {
                _replayViewer = value;
                RaisePropertyChanged("ReplayViewer");
            }
        }

        private void OnCopyPlayerNameCommand(TeamMember player)
        {
            if (player != null)
            {
                try
                {
                    Clipboard.SetText(player.Name);
                }
                catch (Exception e)
                {
                    _log.Error("Clipboard error", e);
                }
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

        public bool Init(Domain.Replay.Replay replay)
        {
            Replay = replay;
            
            if (replay.datablock_battle_result != null)
            {
                MapDescription = GetMapDescription(replay);

                var tankDescription = Dictionaries.Instance.GetTankDescription(replay.datablock_battle_result.personal.typeCompDescr);

                Tank = tankDescription.Title;
                TankIcon = tankDescription.Icon;
                
                Date = replay.datablock_1.dateTime;

                TeamMembers = GetTeamMembers(replay, MapDescription.Team);

                DateTime playTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU"));

                Version clientVersion = ReplayFileHelper.ResolveVersion(replay.datablock_1.Version, playTime);

                long playerId = replay.datablock_battle_result.personal.accountDBID;
                FirstTeam = TeamMembers.Where(x => x.Team == MapDescription.Team).OrderByDescending(x => x.Xp).ToList();
                SecondTeam = TeamMembers.Where(x => x.Team != MapDescription.Team).OrderByDescending(x => x.Xp).ToList();
                ReplayUser = TeamMembers.First(x => x.AccountDBID == playerId);

                List<long> squads1 = FirstTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();
                List<long> squads2 = SecondTeam.Where(x => x.platoonID > 0).OrderBy(x => x.platoonID).Select(x => x.platoonID).Distinct().ToList();

                FirstTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads1.IndexOf(tm.platoonID) + 1; });
                SecondTeam.ForEach(delegate(TeamMember tm) { tm.Squad = squads2.IndexOf(tm.platoonID) + 1; });

                CombatEffects = GetCombatTargets(replay, TeamMembers);

                FullName = ReplayUser.FullName;

                double premiumFactor = replay.datablock_battle_result.personal.premiumCreditsFactor10 / (double)10;
                XpFactor = replay.datablock_battle_result.personal.dailyXPFactor10 / 10;

                int creditsPenalty = replay.datablock_battle_result.personal.creditsPenalty;
                int premiumCreditsPenalty = (int)Math.Round(creditsPenalty * premiumFactor, 0);

                TotalCredits = replay.datablock_battle_result.personal.credits;
                TotalXp = replay.datablock_battle_result.personal.xp;

                int premiumCredits;
                
                if (replay.datablock_battle_result.personal.isPremium)
                {
                    premiumCredits = replay.datablock_battle_result.personal.credits;
                    PremiumCreditsContributionIn = replay.datablock_battle_result.personal.creditsContributionIn;
                    PremiumCreditsContributionOut = premiumCreditsPenalty;
                }
                else
                {
                    premiumCredits = (int)(replay.datablock_battle_result.personal.credits * premiumFactor);
                }

                Xp = replay.datablock_battle_result.vehicles[ReplayUser.Id].xp;

                PremiumCredits = premiumCredits;
                PremiumXp = (int)(Xp * premiumFactor);

                CreditsContributionOut = (int)Math.Round((PremiumCreditsContributionOut / premiumFactor), 0);
                CreditsContributionIn = (int)Math.Round((PremiumCreditsContributionIn / premiumFactor), 0);

                Credits = (int)Math.Round((PremiumCredits / premiumFactor), 0);
                
                ActionCredits = replay.datablock_battle_result.personal.eventCredits;
                ActionXp = replay.datablock_battle_result.personal.eventXP;

                XpPenalty = replay.datablock_battle_result.personal.xpPenalty;
                XpTitle = GetXpTitle(XpFactor);

                PremiumTotalXp = PremiumXp * XpFactor + ActionXp;
                BaseTotalXp = Xp * XpFactor + ActionXp;

                AutoRepairCost = replay.datablock_battle_result.personal.autoRepairCost ?? 0;
                AutoLoadCost = ReplayFileHelper.GetAutoLoadCost(replay);
                AutoEquipCost = ReplayFileHelper.GetAutoEquipCost(replay);
                
                PremiumTotalCredits = PremiumCredits - AutoRepairCost - AutoLoadCost - AutoEquipCost;
                BaseTotalCredits = Credits - AutoRepairCost - AutoLoadCost - AutoEquipCost;

                Shots = replay.datablock_battle_result.personal.shots;
                Hits = replay.datablock_battle_result.personal.hits;
                Crits = CombatEffects.Sum(x => x.Crits);
                HEHits = replay.datablock_battle_result.personal.he_hits;
                Pierced = replay.datablock_battle_result.personal.pierced;
                DamageDealt = replay.datablock_battle_result.personal.damageDealt;
                ShotsReceived = replay.datablock_battle_result.personal.shotsReceived;
                TDamage = string.Format("{0}/{1}", replay.datablock_battle_result.personal.tkills, replay.datablock_battle_result.personal.tdamageDealt);
                Damaged = replay.datablock_battle_result.personal.damaged;
                Killed = replay.datablock_battle_result.personal.kills;
                Spotted = replay.datablock_battle_result.personal.spotted;
                DamageAssisted = replay.datablock_battle_result.personal.damageAssisted;
                DamageAssistedTrack = replay.datablock_battle_result.personal.damageAssistedTrack;
                DamageAssistedRadio = replay.datablock_battle_result.personal.damageAssistedRadio == 0
                    ? replay.datablock_battle_result.personal.damageAssisted - replay.datablock_battle_result.personal.damageAssistedTrack 
                    : replay.datablock_battle_result.personal.damageAssistedRadio;
                CapturePoints = replay.datablock_battle_result.personal.capturePoints;
                DroppedCapturePoints = replay.datablock_battle_result.personal.droppedCapturePoints;
                PotentialDamageReceived = replay.datablock_battle_result.personal.potentialDamageReceived;
                DamageBlockedByArmor = replay.datablock_battle_result.personal.damageBlockedByArmor;
                Mileage = string.Format(Resources.Resources.Traveled_Format, replay.datablock_battle_result.personal.mileage/(double)1000);

                StartTime = DateTime.Parse(replay.datablock_1.dateTime, CultureInfo.GetCultureInfo("ru-RU")).ToShortTimeString();
                TimeSpan battleLength = new TimeSpan(0, 0, (int) replay.datablock_battle_result.common.duration);
                BattleTime = battleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                List<Medal> medals = GetMedals(replay);

                BattleMedals = medals.Where(x => x.Type == 0).ToList();
                AchievMedals = medals.Where(x => x.Type == 1).ToList();

                TimeSpan userbattleLength = new TimeSpan(0, 0, replay.datablock_battle_result.personal.lifeTime);
                UserBattleTime = userbattleLength.ToString(Resources.Resources.ExtendedTimeFormat);

                //calc levels by squad
                List<LevelRange> membersLevels = new List<LevelRange>();
                membersLevels.AddRange(TeamMembers.Where(x => x.Squad == 0).Select(x => x.LevelRange));
                IEnumerable<IGrouping<int, TeamMember>> squads = TeamMembers.Where(x => x.Squad > 0).GroupBy(x => x.Squad);
                membersLevels.AddRange(squads.Select(x => x.Select(s => s.LevelRange).OrderByDescending(o => o.Max).First()));

                int level = Dictionaries.Instance.GetBattleLevel(membersLevels);

                Status = GetBattleStatus(replay);

                DeathReason = (DeathReason)replay.datablock_battle_result.personal.deathReason;

                FinishReason = (FinishReason)replay.datablock_battle_result.common.finishReason;

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
                        if (Dictionaries.Instance.ConsumableDescriptions.ContainsKey(slot.Item.Id) &&
                            slot.Item.TypeId == SlotType.Equipment)
                        {
                            Consumables.Add(Dictionaries.Instance.ConsumableDescriptions[slot.Item.Id]);
                        }
                        if (Dictionaries.Instance.Shells[TankIcon.CountryId].ContainsKey(slot.Item.Id) &&
                            slot.Item.TypeId == SlotType.Shell)
                        {
                            slot.Description = Dictionaries.Instance.Shells[(Country)TankIcon.CountryId][slot.Item.Id];
                            Shells.Add(slot);
                        }
                    }

                    if (replay.datablock_advanced.more != null)
                    {
                        level = replay.datablock_advanced.more.battleLevel;
                    }

                    ChatMessages = replay.datablock_advanced.Messages;
                }

                Title = string.Format(Resources.Resources.WindowTitleFormat_Replay, Tank, MapDescription.MapName, level > 0 ? level.ToString(CultureInfo.InvariantCulture) : "n/a", clientVersion.ToString(3));

                return true;
            }
            return false;
        }

        private Map GetMapDescription(Domain.Replay.Replay replay)
        {
            Map mapDescription = new Map();

            mapDescription.MapNameId = replay.datablock_1.mapName;
            mapDescription.Gameplay = (Gameplay) Enum.Parse(typeof (Gameplay), replay.datablock_1.gameplayID);
            mapDescription.MapName = string.Format("{0} - {1}", replay.datablock_1.mapDisplayName,
                GetMapMode(mapDescription.Gameplay, (BattleType)replay.datablock_1.battleType));

            if (Dictionaries.Instance.Maps.ContainsKey(replay.datablock_1.mapName))
            {
                mapDescription.MapId = Dictionaries.Instance.Maps[replay.datablock_1.mapName].MapId;
            }
            else
            {
                _log.WarnFormat("Unknown map: {0}", replay.datablock_1.mapName);
            }

            long playerId = replay.datablock_battle_result.personal.accountDBID;

            mapDescription.Team = replay.datablock_battle_result.players[playerId].team;
            return mapDescription;
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
                .Select(x => new CombatTarget(x, teamMembers.First(tm => tm.Id == x.Key), replay.datablock_1.Version))
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

        private string GetXpTitle(int dailyXpFactor)
        {
            if (dailyXpFactor > 1)
            {
                return string.Format(Resources.Resources.Label_Replay_XpFactorFormat, dailyXpFactor);
            }
            return Resources.Resources.Label_Experience;
        }

        private object GetMapMode(Gameplay gameplayId, BattleType battleType)
        {
            List<BattleType> list = new List<BattleType>
            {
                BattleType.Unknown,
                BattleType.Regular,
                BattleType.CompanyWar,
                BattleType.ClanWar,
                BattleType.Event
            };

            if (list.Contains(battleType))
            {
                if (gameplayId == Gameplay.ctf)
                {
                    return Resources.Resources.BattleType_ctf;
                }
                if (gameplayId == Gameplay.domination)
                {
                    return Resources.Resources.BattleType_domination;
                }
                if (gameplayId == Gameplay.nations)
                {
                    return Resources.Resources.BattleType_nations;
                }
                return Resources.Resources.BattleType_assault;
            }
            return Resources.Resources.ResourceManager.GetEnumResource(battleType);
        }
    }
}
