using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using WotDossier.Applications.Events;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Replay;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;
using Vehicle = WotDossier.Domain.Replay.Vehicle;

namespace WotDossier.Applications.ViewModel.Filter
{
    public class ReplaysFilterViewModel : INotifyPropertyChanged
    {
        public static readonly string PropTanks = TypeHelper<ReplaysFilterViewModel>.PropertyName(v => v.Tanks);
        public static readonly string PropSelectedTank = TypeHelper<ReplaysFilterViewModel>.PropertyName(v => v.SelectedTank);
        public static readonly string PropIsPremium = TypeHelper<ReplaysFilterViewModel>.PropertyName(v => v.IsPremium);

        #region FILTERS

        private bool _level1Selected = true;
        private bool _level2Selected = true;
        private bool _level3Selected = true;
        private bool _level4Selected = true;
        private bool _level5Selected = true;
        private bool _level6Selected = true;
        private bool _level7Selected = true;
        private bool _level8Selected = true;
        private bool _level9Selected = true;
        private bool _level10Selected = true;
        private bool _tankTypeLtSelected = true;
        private bool _tankTypeMtSelected = true;
        private bool _tankTypeHtSelected = true;
        private bool _tankTypeTdSelected = true;
        private bool _tankTypeSpgSelected = true;
        private bool _nationUssrSelected = true;
        private bool _nationGermanySelected = true;
        private bool _nationUsSelected = true;
        private bool _nationChinaSelected = true;
        private bool _nationFranceSelected = true;
        private bool _nationUkSelected = true;
        private bool _nationJpSelected = true;
        private bool _isPremium;
        private bool _isFavorite;
        private ReplayFolder _selectedFolder;
        private ListItem<int> _selectedMap;
        private List<ListItem<int>> _maps;
        private string _field;
        private int? _startValue;
        private int? _endValue;
        private string _member;
        private BattleStatus _selectedBattleResult = BattleStatus.Unknown;
        
        #region levels

        public bool Level10Selected
        {
            get { return _level10Selected; }
            set
            {
                _level10Selected = value;
                OnPropertyChanged("Level10Selected");
            }
        }

        public bool Level9Selected
        {
            get { return _level9Selected; }
            set
            {
                _level9Selected = value;
                OnPropertyChanged("Level9Selected");
            }
        }

        public bool Level8Selected
        {
            get { return _level8Selected; }
            set
            {
                _level8Selected = value;
                OnPropertyChanged("Level8Selected");
            }
        }

        public bool Level7Selected
        {
            get { return _level7Selected; }
            set
            {
                _level7Selected = value;
                OnPropertyChanged("Level7Selected");
            }
        }

        public bool Level6Selected
        {
            get { return _level6Selected; }
            set
            {
                _level6Selected = value;
                OnPropertyChanged("Level6Selected");
            }
        }

        public bool Level5Selected
        {
            get { return _level5Selected; }
            set
            {
                _level5Selected = value;
                OnPropertyChanged("Level5Selected");
            }
        }

        public bool Level4Selected
        {
            get { return _level4Selected; }
            set
            {
                _level4Selected = value;
                OnPropertyChanged("Level4Selected");
            }
        }

        public bool Level3Selected
        {
            get { return _level3Selected; }
            set
            {
                _level3Selected = value;
                OnPropertyChanged("Level3Selected");
            }
        }

        public bool Level2Selected
        {
            get { return _level2Selected; }
            set
            {
                _level2Selected = value;
                OnPropertyChanged("Level2Selected");
            }
        }

        public bool Level1Selected
        {
            get { return _level1Selected; }
            set
            {
                _level1Selected = value;
                OnPropertyChanged("Level1Selected");
            }
        }

        #endregion

        #region types

        public bool TankTypeSPGSelected
        {
            get { return _tankTypeSpgSelected; }
            set
            {
                _tankTypeSpgSelected = value;
                OnPropertyChanged("TankTypeSPGSelected");
            }
        }

        public bool TankTypeTDSelected
        {
            get { return _tankTypeTdSelected; }
            set
            {
                _tankTypeTdSelected = value;
                OnPropertyChanged("TankTypeTDSelected");
            }
        }

        public bool TankTypeHTSelected
        {
            get { return _tankTypeHtSelected; }
            set
            {
                _tankTypeHtSelected = value;
                OnPropertyChanged("TankTypeHTSelected");
            }
        }

        public bool TankTypeMTSelected
        {
            get { return _tankTypeMtSelected; }
            set
            {
                _tankTypeMtSelected = value;
                OnPropertyChanged("TankTypeMTSelected");
            }
        }

        public bool TankTypeLTSelected
        {
            get { return _tankTypeLtSelected; }
            set
            {
                _tankTypeLtSelected = value;
                OnPropertyChanged("TankTypeLTSelected");
            }
        }

        #endregion

        #region countries

        public bool NationUSSRSelected
        {
            get { return _nationUssrSelected; }
            set
            {
                _nationUssrSelected = value;
                OnPropertyChanged("NationUSSRSelected");
            }
        }

        public bool NationGermanySelected
        {
            get { return _nationGermanySelected; }
            set
            {
                _nationGermanySelected = value;
                OnPropertyChanged("NationGermanySelected");
            }
        }

        public bool NationUSSelected
        {
            get { return _nationUsSelected; }
            set
            {
                _nationUsSelected = value;
                OnPropertyChanged("NationUSSelected");
            }
        }

        public bool NationChinaSelected
        {
            get { return _nationChinaSelected; }
            set
            {
                _nationChinaSelected = value;
                OnPropertyChanged("NationChinaSelected");
            }
        }

        public bool NationFranceSelected
        {
            get { return _nationFranceSelected; }
            set
            {
                _nationFranceSelected = value;
                OnPropertyChanged("NationFranceSelected");
            }
        }

        public bool NationUKSelected
        {
            get { return _nationUkSelected; }
            set
            {
                _nationUkSelected = value;
                OnPropertyChanged("NationUKSelected");
            }
        }

        public bool NationJPSelected
        {
            get { return _nationJpSelected; }
            set
            {
                _nationJpSelected = value;
                OnPropertyChanged("NationJPSelected");
            }
        }

        #endregion

        public bool IsPremium
        {
            get { return _isPremium; }
            set
            {
                _isPremium = value;
                OnPropertyChanged(PropIsPremium);
            }
        }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        public ReplayFolder SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                _selectedFolder = value;
                OnPropertyChanged("SelectedFolder");
            }
        }

        public List<ListItem<int>> Maps
        {
            get { return _maps; }
            set
            {
                _maps = value;
                OnPropertyChanged("Maps");
            }
        }

        public ListItem<int> SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                _selectedMap = value;
                OnPropertyChanged("SelectedMap");
            }
        }

        public int? StartValue
        {
            get { return _startValue; }
            set
            {
                _startValue = value;
                OnPropertyChanged("StartValue");
            }
        }

        public int? EndValue
        {
            get { return _endValue; }
            set
            {
                _endValue = value;
                OnPropertyChanged("EndValue");
            }
        }

        public string Field
        {
            get { return _field; }
            set
            {
                _field = value;
                OnPropertyChanged("Field");
            }
        }

        public string Member
        {
            get { return _member; }
            set
            {
                _member = value;
                OnPropertyChanged("Member");
            }
        }

        #endregion

        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand AllCommand { get; set; }

        private List<CheckListItem<Version>> _versions;

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        public List<CheckListItem<Version>> Versions
        {
            get { return _versions; }
            set { _versions = value; }
        }

        //NOTE: single version selection mode. use Dictionaries.VersionAll as default value 
        private Version _selectedVersion;
        /// <summary>
        /// Gets or sets the selected version.
        /// </summary>
        public Version SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                OnPropertyChanged("SelectedVersion");
            }
        }

        private List<ListItem<DeathReason>> _deathReasons = new List<ListItem<DeathReason>>
            {
                new ListItem<DeathReason>(DeathReason.Unknown, string.Empty), 
                new ListItem<DeathReason>(DeathReason.Alive, Resources.Resources.DeathReason_Alive), 
                new ListItem<DeathReason>(DeathReason.Dead, Resources.Resources.DeathReason_Dead), 
                new ListItem<DeathReason>(DeathReason.DestroyedByShot, " - " + Resources.Resources.DeathReason_DestroyedByShot), 
                new ListItem<DeathReason>(DeathReason.DestroyedByFire, " - " + Resources.Resources.DeathReason_DestroyedByFire), 
                new ListItem<DeathReason>(DeathReason.DestroyedByRamming, " - " + Resources.Resources.DeathReason_DestroyedByRamming), 
                new ListItem<DeathReason>(DeathReason.VehicleDrowned, " - " + Resources.Resources.DeathReason_VehicleDrowned), 
                new ListItem<DeathReason>(DeathReason.DestroyedByDeathZone, " - " + Resources.Resources.DeathReason_DestroyedByDeathZone), 
                new ListItem<DeathReason>(DeathReason.Crashed, " - " + Resources.Resources.DeathReason_Crashed), 
                new ListItem<DeathReason>(DeathReason.CrewDead, " - " + Resources.Resources.DeathReason_CrewDead), 
            };

        public List<ListItem<DeathReason>> DeathReasons
        {
            get { return _deathReasons; }
            set { _deathReasons = value; }
        }

        private DeathReason _deathReason = DeathReason.Unknown;
        public DeathReason DeathReason
        {
            get { return _deathReason; }
            set
            {
                _deathReason = value;
                OnPropertyChanged("DeathReason");
            }
        }

        private List<ListItem<Platoon>> _platoonFilter = new List<ListItem<Platoon>>
            {
                new ListItem<Platoon>(Platoon.Unknown, string.Empty), 
                new ListItem<Platoon>(Platoon.Solo, Resources.Resources.Platoon_Solo), 
                new ListItem<Platoon>(Platoon.Platoon, Resources.Resources.Platoon_Platoon)
            };

        public List<ListItem<Platoon>> PlatoonFilter
        {
            get { return _platoonFilter; }
            set { _platoonFilter = value; }
        }

        private Platoon _selectedPlatoonFilter = Platoon.Unknown;
        public Platoon SelectedPlatoonFilter
        {
            get { return _selectedPlatoonFilter; }
            set
            {
                _selectedPlatoonFilter = value;
                OnPropertyChanged("DeathReason");
            }
        }

        private List<ListItem<FinishReason>> _finishReasons = new List<ListItem<FinishReason>>
        {
            new ListItem<FinishReason>(FinishReason.Unknown, string.Empty), 
            new ListItem<FinishReason>(FinishReason.BaseCapture, Resources.Resources.FinishReason_BaseCapture),
            new ListItem<FinishReason>(FinishReason.Extermination, Resources.Resources.FinishReason_Extermination),
            new ListItem<FinishReason>(FinishReason.Timeout, Resources.Resources.FinishReason_Timeout),
            new ListItem<FinishReason>(FinishReason.Technical, Resources.Resources.FinishReason_Technical),
        };

        public List<ListItem<FinishReason>> FinishReasons
        {
            get { return _finishReasons; }
            set { _finishReasons = value; }
        }

        private FinishReason _finishReason;
        public FinishReason FinishReason
        {
            get { return _finishReason; }
            set
            {
                _finishReason = value;
                OnPropertyChanged("FinishReason");
            }
        }

        private readonly List<ListItem<BattleType>> _battleTypes = new List<ListItem<BattleType>>
            {
                new ListItem<BattleType>(BattleType.Unknown, Resources.Resources.TankFilterPanel_All), 
                new ListItem<BattleType>(BattleType.Regular, Resources.Resources.BattleType_Regular), 
                new ListItem<BattleType>(BattleType.ctf, " - " + Resources.Resources.BattleType_ctf), 
                new ListItem<BattleType>(BattleType.domination, " - " + Resources.Resources.BattleType_domination), 
                new ListItem<BattleType>(BattleType.assault, " - " + Resources.Resources.BattleType_assault), 
                new ListItem<BattleType>(BattleType.nations, " - " + Resources.Resources.BattleType_nations), 
                new ListItem<BattleType>(BattleType.Historical,Resources.Resources.BattleType_Historical), 
                new ListItem<BattleType>(BattleType.CyberSport,Resources.Resources.BattleType_CyberSport), 
                new ListItem<BattleType>(BattleType.ClanWar, Resources.Resources.BattleType_ClanWar), 
                new ListItem<BattleType>(BattleType.CompanyWar, Resources.Resources.BattleType_CompanyWar), 
                new ListItem<BattleType>(BattleType.Training, Resources.Resources.BattleType_Training), 
                new ListItem<BattleType>(BattleType.Sorties,Resources.Resources.BattleType_Sorties), 
                new ListItem<BattleType>(BattleType.Event,Resources.Resources.BattleType_Event), 
            };

        /// <summary>
        /// Gets the battle types.
        /// </summary>
        /// <value>
        /// The battle types.
        /// </value>
        public List<ListItem<BattleType>> BattleTypes
        {
            get { return _battleTypes; }
        }

        private BattleType _battleType;
        /// <summary>
        /// Gets or sets the type of the battle.
        /// </summary>
        /// <value>
        /// The type of the battle.
        /// </value>
        public BattleType BattleType
        {
            get { return _battleType; }
            set
            {
                _battleType = value;
                OnPropertyChanged("BattleType");
            }
        }

        private bool _resp1;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ReplaysFilterViewModel" /> is resp1.
        /// </summary>
        /// <value>
        ///   <c>true</c> if resp1; otherwise, <c>false</c>.
        /// </value>
        public bool Resp1
        {
            get { return _resp1; }
            set
            {
                _resp1 = value;
                OnPropertyChanged("Resp1");
            }
        }

        private bool _resp2;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ReplaysFilterViewModel" /> is resp2.
        /// </summary>
        /// <value>
        ///   <c>true</c> if resp2; otherwise, <c>false</c>.
        /// </value>
        public bool Resp2
        {
            get { return _resp2; }
            set
            {
                _resp2 = value;
                OnPropertyChanged("Resp2");
            }
        }

        private bool _allResps = true;
        /// <summary>
        /// Gets or sets a value indicating whether [all resps].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all resps]; otherwise, <c>false</c>.
        /// </value>
        public bool AllResps
        {
            get { return !(Resp1||Resp2) || _allResps; }
            set
            {
                _allResps = value;
                OnPropertyChanged("AllResps");
            }
        }

        private DateTime? _startDate;
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime? _endDate;
        private ListItem<int> _selectedTank;
        private CheckListItem<Version> _allVersionsListItem;

        private void OnSelectVersion(CheckListItem<Version> item, bool isChecked)
        {
            OnPropertyChanged("SelectedVersion");
        }

        private void OnSelectAllVersions(CheckListItem<Version> item, bool isChecked)
        {
            foreach (var listItem in Versions)
            {
                if (listItem.Id != _allVersionsListItem.Id)
                {
                    listItem.GroupCheck = isChecked;
                }
            }
            OnSelectVersion(item, isChecked);
        }

        private readonly IEnumerable<CheckListItem<Version>> _baseVersionsListItems;
        
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public List<ListItem<int>> Tanks
        {
            get { return GetTanks(); }
        }

        private List<ListItem<int>> GetTanks()
        {
            return Dictionaries.Instance.Tanks.Values
                .Where(TankFilter)
                .OrderBy(x => x.Title)
                .Select(x => new ListItem<int>(x.UniqueId(), x.Title))
                .ToList();
        }

        private bool TankFilter(TankDescription tank)
        {
            return (Level1Selected && tank.Tier == 1
                    || Level2Selected && tank.Tier == 2
                    || Level3Selected && tank.Tier == 3
                    || Level4Selected && tank.Tier == 4
                    || Level5Selected && tank.Tier == 5
                    || Level6Selected && tank.Tier == 6
                    || Level7Selected && tank.Tier == 7
                    || Level8Selected && tank.Tier == 8
                    || Level9Selected && tank.Tier == 9
                    || Level10Selected && tank.Tier == 10)
                   &&
                   (TankTypeLTSelected && tank.Type == (int) TankType.LT
                    || TankTypeMTSelected && tank.Type == (int) TankType.MT
                    || TankTypeHTSelected && tank.Type == (int) TankType.HT
                    || TankTypeTDSelected && tank.Type == (int) TankType.TD
                    || TankTypeSPGSelected && tank.Type == (int) TankType.SPG)
                   &&
                   (NationUSSRSelected && tank.CountryId == (int) Country.Ussr
                    || NationGermanySelected && tank.CountryId == (int) Country.Germany
                    || NationChinaSelected && tank.CountryId == (int) Country.China
                    || NationFranceSelected && tank.CountryId == (int) Country.France
                    || NationUSSelected && tank.CountryId == (int) Country.Usa
                    || NationJPSelected && tank.CountryId == (int) Country.Japan
                    || NationUKSelected && tank.CountryId == (int) Country.Uk)
                   && (tank.Premium == 1 || !IsPremium);
        }

        public ListItem<int> SelectedTank
        {
            get { return _selectedTank; }
            set
            {
                _selectedTank = value;
                OnPropertyChanged("SelectedTank");
            }
        }

        /// <summary>
        /// Filters the specified replays.
        /// </summary>
        /// <param name="replays">The replays.</param>
        /// <returns></returns>
        public List<ReplayFile> Filter(IEnumerable<ReplayFile> replays)
        {
            return Filter(replays, false);
        }

        /// <summary>
        /// Filters the replays list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replays">The replays.</param>
        /// <param name="applySettingsFilters"></param>
        /// <returns></returns>
        public List<ReplayFile> Filter(IEnumerable<ReplayFile> replays, bool applySettingsFilters)
        {
            AppSettings settings = SettingsReader.Get();

            if (replays == null)
            {
                return new List<ReplayFile>();
            }

            string [] members = null;

            if (!string.IsNullOrEmpty(Member))
            {
                members = Member.Split(',');
            }

            var versions = Versions.Where(x => x.Checked).Select(x => x.Id).ToList();
            var medals = Medals.Where(x => x is MedalCheckListItem && ((MedalCheckListItem)x).Checked).Select(x => x.Id).ToList();

            List<ReplayFile> result = replays.ToList().Where(x =>
                //show all unknown tanks
                 (x.Tank != null && TankIcon.Empty.Equals(x.Tank.Icon) && (SelectedFolder == null || x.FolderId == SelectedFolder.Id)) 
                || 
                //or apply filter
                 x.Tank != null
                &&
                 (SelectedTank == null || x.Tank.UniqueId() == SelectedTank.Id)
                &&
                 VersionFilter(versions, x)
                &&
                 TankFilter(x.Tank)
                &&
                 MedalFilter(medals, x)
                && 
                 (SelectedFolder == null || x.FolderId == SelectedFolder.Id)
                && 
                 (x.IsWinner == SelectedBattleResult || SelectedBattleResult == BattleStatus.Unknown)
                && 
                 (SelectedMap == null || x.MapId == SelectedMap.Id || SelectedMap.Id == 0)
                && 
                 FieldFilter(x)
                && 
                 MembersFilter(x.TeamMembers, members)
                && 
                 (BattleType == BattleType.Unknown || x.BattleType == BattleType || CheckRegularBattle(x, BattleType))
                &&
                (AllResps
                 || Resp1 && x.Team == 1
                 || Resp2 && x.Team == 2)
                &&
                 (StartDate == null || x.PlayTime.Date >= StartDate)
                &&
                 (EndDate == null || x.PlayTime.Date <= EndDate)
                &&
                 (!applySettingsFilters ||
                    ((settings.PlayerId == 0 || x.PlayerId == settings.PlayerId || x.PlayerName == settings.PlayerName)
                        && (settings.UseIncompleteReplaysResultsForCharts || (x.IsWinner != BattleStatus.Incomplete && x.IsWinner != BattleStatus.Unknown)))
                 )
                && (DeathReason == DeathReason.Unknown || x.DeathReason == DeathReason || DeathReason == DeathReason.Dead && (x.DeathReason != DeathReason.Alive && x.DeathReason != DeathReason.Unknown ))
                && (SelectedPlatoonFilter == Platoon.Unknown || (SelectedPlatoonFilter == Platoon.Platoon && x.IsPlatoon) || (SelectedPlatoonFilter == Platoon.Solo && !x.IsPlatoon))
                ).ToList();

            return result;
        }

        private bool MedalFilter(List<int> medals, ReplayFile replayFile)
        {
            if (medals.Any())
            {
                var replayMedals = replayFile.Medals.Select(x => x.Id);
                var replayAchievements = replayFile.Achievements.Select(x => x.Id);
                return medals.Intersect(replayMedals).Any() || medals.Intersect(replayAchievements).Any();
            }
            return true;
        }

        private bool VersionFilter(List<Version> versions, ReplayFile replayFile)
        {
            if (SelectedVersion != null)
            {
                return (SelectedVersion == Dictionaries.VersionAll ||
                        (SelectedVersion == Dictionaries.VersionTest &&
                         replayFile.ClientVersion > Dictionaries.VersionRelease) ||
                        SelectedVersion == replayFile.ClientVersion);
            }

            if (versions.Contains(Dictionaries.VersionAll) || versions.Contains(replayFile.ClientVersion))
            {
                return true;
            }

            if (versions.Contains(Dictionaries.VersionTest))
            {
                return replayFile.ClientVersion > Dictionaries.VersionRelease;
            }

            return false;
        }

        private bool CheckRegularBattle(ReplayFile replay, BattleType battleType)
        {
            if (battleType == BattleType.ctf)
            {
                return replay.Gameplay == Gameplay.ctf && replay.BattleType == BattleType.Regular;
            }
            if (battleType == BattleType.domination)
            {
                return replay.Gameplay == Gameplay.domination && replay.BattleType == BattleType.Regular;
            }
            if (battleType == BattleType.assault)
            {
                return replay.Gameplay == Gameplay.assault && replay.BattleType == BattleType.Regular;
            }
            if (battleType == BattleType.nations)
            {
                return replay.Gameplay == Gameplay.nations && replay.BattleType == BattleType.Regular;
            }
            return false;
        }

        private bool MembersFilter(List<Vehicle> vehicles, string[] members)
        {
            if (members == null)
            {
                return true;
            }

            bool result = true;

            foreach (var member in members)
            {
                result &= vehicles.FirstOrDefault(m => m.name.StartsWith(member, StringComparison.InvariantCultureIgnoreCase)) != null;
            }

            return result;
        }

        private bool FieldFilter<T>(T x) where T : ReplayFile
        {
            if (!string.IsNullOrEmpty(Field))
            {
                BinaryExpression greaterThan = Expression.GreaterThan(Expression.PropertyOrField(Expression.Constant(x), Field), Expression.Constant(StartValue ?? 0));
                BinaryExpression lessThan = Expression.LessThan(Expression.PropertyOrField(Expression.Constant(x), Field), Expression.Constant(EndValue ?? 0));

                BinaryExpression expression;

                if (EndValue != null && StartValue != null)
                {
                    expression = Expression.And(greaterThan, lessThan);
                }
                else if (EndValue != null)
                {
                    expression = lessThan;
                }
                else if (StartValue != null)
                {
                    expression = greaterThan;
                }
                else
                {
                    return true;
                }

                return Expression.Lambda<Func<T, bool>>(expression, Expression.Parameter(typeof(T))).Compile()(x);
            }
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReplaysFilterViewModel()
        {
            List<ListItem<int>> list = Dictionaries.Instance.Maps.Values.OrderByDescending(x => x.MapId).Select(x => new ListItem<int>(x.MapId, x.LocalizedMapName)).ToList();
            list.Insert(0, new ListItem<int>(0, ""));
            Maps = list;

            FilterFields = new List<ListItem<string>>
            {
                new ListItem<string>(ReplayFile.PropCredits,Resources.Resources.Column_Replay_Credits), 
                new ListItem<string>(ReplayFile.PropDamageDealt,Resources.Resources.Column_Replay_DamageDealt), 
                new ListItem<string>(ReplayFile.PropXp,Resources.Resources.Column_Replay_XP), 
                new ListItem<string>(ReplayFile.PropKilled,Resources.Resources.Column_Replay_Frags), 
                new ListItem<string>(ReplayFile.PropDamaged,Resources.Resources.Column_Replay_Damaged), 
            };

            BattleResults = new List<ListItem<BattleStatus>>
            {
                new ListItem<BattleStatus>(BattleStatus.Unknown, Resources.Resources.TankFilterPanel_All), 
                new ListItem<BattleStatus>(BattleStatus.Victory, Resources.Resources.BattleStatus_Victory), 
                new ListItem<BattleStatus>(BattleStatus.Defeat,Resources.Resources.BattleStatus_Defeat), 
                new ListItem<BattleStatus>(BattleStatus.Draw,Resources.Resources.BattleStatus_Draw), 
            };

            _allVersionsListItem = new CheckListItem<Version>(Dictionaries.VersionAll, Resources.Resources.TankFilterPanel_All, true, OnSelectAllVersions);
            _baseVersionsListItems = Dictionaries.Instance.Versions.Select(x => new CheckListItem<Version>(x, x.ToString(3), true, OnSelectVersion, _allVersionsListItem));

            _versions = new List<CheckListItem<Version>>();
            
            _versions.AddRange(_baseVersionsListItems);

            _versions.Insert(0, _allVersionsListItem);
            _versions.Add(new CheckListItem<Version>(Dictionaries.VersionTest, "Test 0.9.x", true, OnSelectVersion, _allVersionsListItem));

            Medals = GetMedals();

            ClearCommand = new DelegateCommand(OnClear);
            RefreshCommand = new DelegateCommand(OnRefresh);
            AllCommand = new DelegateCommand(OnAll);
        }

        private List<ListItem<int>> GetMedals()
        {
            var medals = Dictionaries.Instance.Medals.Values.Where(x => x.Group.Filter);

            MedalGroup currentGroup = null;

            List<ListItem<int>> resultList = new List<ListItem<int>>();

            foreach (var medal in medals)
            {
                if (!medal.Group.Equals(currentGroup))
                {
                    resultList.Add(new ListItem<int>(0, medal.Group.Name));
                    currentGroup = medal.Group;
                }
                resultList.Add(new MedalCheckListItem(medal, (checkListItem, b) => { if (FilterChanged != null) FilterChanged(); }));
            }

            return resultList;
        }

        public List<ListItem<int>> Medals { get; set; }

        private void OnRefresh()
        {
            EventAggregatorFactory.EventAggregator.GetEvent<ReplayManagerRefreshEvent>().Publish(EventArgs.Empty);
        }

        private void OnAll()
        {
            StartValue = EndValue = null;
            Field = null;
            SelectedMap = null;
            Level10Selected =
                Level9Selected =
                    Level8Selected =
                        Level7Selected =
                            Level6Selected =
                                Level5Selected = Level4Selected = Level3Selected = Level2Selected = Level1Selected = true;
            TankTypeTDSelected = TankTypeMTSelected = TankTypeLTSelected = TankTypeHTSelected = TankTypeSPGSelected = true;
            NationUSSRSelected =
                NationUKSelected = NationUSSelected = NationGermanySelected = NationJPSelected = NationChinaSelected = NationFranceSelected = true;

            SelectedBattleResult = BattleStatus.Unknown;
            //NOTE: single version selection mode. use Dictionaries.VersionAll as default value 
            //SelectedVersion = Dictionaries.VersionAll;
            _allVersionsListItem.Checked = true;
            BattleType = BattleType.Unknown;
            SelectedTank = null;
            Member = string.Empty;
        }

        private void OnClear()
        {
            StartValue = EndValue = null;
            Field = null;
            SelectedMap = null;
            Level10Selected =
                Level9Selected =
                    Level8Selected =
                        Level7Selected =
                            Level6Selected =
                                Level5Selected = Level4Selected = Level3Selected = Level2Selected = Level1Selected = false;
            TankTypeTDSelected = TankTypeMTSelected = TankTypeLTSelected = TankTypeHTSelected = TankTypeSPGSelected = false;
            NationUSSRSelected =
                NationUKSelected = NationUSSelected = NationGermanySelected = NationJPSelected = NationChinaSelected = NationFranceSelected = false;

            SelectedBattleResult = BattleStatus.Unknown;
            //NOTE: single version selection mode. use Dictionaries.VersionAll as default value 
            //SelectedVersion = Dictionaries.VersionAll;
            _allVersionsListItem.Checked = true;
            SelectedTank = null;
            BattleType = BattleType.Unknown;
            Member = string.Empty;
        }

        public List<ListItem<string>> FilterFields { get; set; }

        public List<ListItem<BattleStatus>> BattleResults { get; set; }

        public BattleStatus SelectedBattleResult
        {
            get { return _selectedBattleResult; }
            set
            {
                _selectedBattleResult = value;
                OnPropertyChanged("SelectedBattleResult");
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when filter changed.
        /// </summary>
        public event Action FilterChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName.Contains("Level") || propertyName.Contains("TankType") || propertyName.Contains("Nation") || propertyName.Equals(PropIsPremium))
            {
                RaisePropertyChanged(PropTanks);
            }

            RaisePropertyChanged(propertyName);

            if (FilterChanged != null) FilterChanged();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}