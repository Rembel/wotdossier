using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using WotDossier.Applications.Events;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Framework.EventAggregator;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel.Filter
{
    public class ReplaysFilterViewModel : INotifyPropertyChanged
    {
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
        private bool _ltSelected = true;
        private bool _mtSelected = true;
        private bool _htSelected = true;
        private bool _tdSelected = true;
        private bool _spgSelected = true;
        private bool _ussrSelected = true;
        private bool _germanySelected = true;
        private bool _usSelected = true;
        private bool _chinaSelected = true;
        private bool _franceSelected = true;
        private bool _ukSelected = true;
        private bool _jpSelected = true;
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
        private static readonly Version VersionAll = new Version("0.0.0.0");
        private static readonly Version VersionRelease = new Version("0.9.3.0");
        private static readonly Version VersionTest = new Version("100.0.0.0");

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

        public bool SPGSelected
        {
            get { return _spgSelected; }
            set
            {
                _spgSelected = value;
                OnPropertyChanged("SPGSelected");
            }
        }

        public bool TDSelected
        {
            get { return _tdSelected; }
            set
            {
                _tdSelected = value;
                OnPropertyChanged("TDSelected");
            }
        }

        public bool HTSelected
        {
            get { return _htSelected; }
            set
            {
                _htSelected = value;
                OnPropertyChanged("HTSelected");
            }
        }

        public bool MTSelected
        {
            get { return _mtSelected; }
            set
            {
                _mtSelected = value;
                OnPropertyChanged("MTSelected");
            }
        }

        public bool LTSelected
        {
            get { return _ltSelected; }
            set
            {
                _ltSelected = value;
                OnPropertyChanged("LTSelected");
            }
        }

        #endregion

        #region countries

        public bool USSRSelected
        {
            get { return _ussrSelected; }
            set
            {
                _ussrSelected = value;
                OnPropertyChanged("USSRSelected");
            }
        }

        public bool GermanySelected
        {
            get { return _germanySelected; }
            set
            {
                _germanySelected = value;
                OnPropertyChanged("GermanySelected");
            }
        }

        public bool USSelected
        {
            get { return _usSelected; }
            set
            {
                _usSelected = value;
                OnPropertyChanged("USSelected");
            }
        }

        public bool ChinaSelected
        {
            get { return _chinaSelected; }
            set
            {
                _chinaSelected = value;
                OnPropertyChanged("ChinaSelected");
            }
        }

        public bool FranceSelected
        {
            get { return _franceSelected; }
            set
            {
                _franceSelected = value;
                OnPropertyChanged("FranceSelected");
            }
        }

        public bool UKSelected
        {
            get { return _ukSelected; }
            set
            {
                _ukSelected = value;
                OnPropertyChanged("UKSelected");
            }
        }

        public bool JPSelected
        {
            get { return _jpSelected; }
            set
            {
                _jpSelected = value;
                OnPropertyChanged("JPSelected");
            }
        }

        #endregion

        public bool IsPremium
        {
            get { return _isPremium; }
            set
            {
                _isPremium = value;
                OnPropertyChanged("IsPremium");
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

        private List<ListItem<Version>> _versions = new List<ListItem<Version>>
            {
                new ListItem<Version>(VersionAll, Resources.Resources.TankFilterPanel_All), 
                new ListItem<Version>(VersionRelease, "0.9.3"),
                new ListItem<Version>(new Version("0.9.2.0"), "0.9.2"),
                new ListItem<Version>(new Version("0.9.1.0"), "0.9.1"),
                new ListItem<Version>(new Version("0.9.0.0"), "0.9.0"),
                new ListItem<Version>(new Version("0.8.11.0"), "0.8.11"), 
                new ListItem<Version>(new Version("0.8.10.0"), "0.8.10"), 
                new ListItem<Version>(new Version("0.8.9.0"), "0.8.9"), 
                new ListItem<Version>(new Version("0.8.8.0"), "0.8.8"), 
                new ListItem<Version>(new Version("0.8.7.0"), "0.8.7"), 
                new ListItem<Version>(new Version("0.8.6.0"), "0.8.6"), 
                new ListItem<Version>(new Version("0.8.5.0"), "0.8.5"), 
                new ListItem<Version>(new Version("0.8.4.0"), "0.8.4"), 
                new ListItem<Version>(new Version("0.8.3.0"), "0.8.3"), 
                new ListItem<Version>(new Version("0.8.2.0"), "0.8.2"), 
                new ListItem<Version>(new Version("0.8.1.0"), "0.8.1"), 
                new ListItem<Version>(VersionTest, "Test 0.9.x"), 
            };

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        public List<ListItem<Version>> Versions
        {
            get { return _versions; }
            set { _versions = value; }
        }

        private Version _selectedVersion = VersionAll;
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

        private readonly List<ListItem<BattleType>> _battleTypes = new List<ListItem<BattleType>>
            {
                new ListItem<BattleType>(BattleType.Unknown, Resources.Resources.TankFilterPanel_All), 
                new ListItem<BattleType>(BattleType.Regular, Resources.Resources.BattleType_Regular), 
                new ListItem<BattleType>(BattleType.Historical,Resources.Resources.BattleType_Historical), 
                new ListItem<BattleType>(BattleType.CyberSport,Resources.Resources.BattleType_CyberSport), 
                new ListItem<BattleType>(BattleType.ClanWar, Resources.Resources.BattleType_ClanWar), 
                new ListItem<BattleType>(BattleType.CompanyWar, Resources.Resources.BattleType_CompanyWar), 
                new ListItem<BattleType>(BattleType.Training, Resources.Resources.BattleType_Training), 
                new ListItem<BattleType>(BattleType.Sorties,Resources.Resources.BattleType_Sorties), 
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
        private bool _resp2;
        private bool _allResps = true;
        private DateTime? _startDate;
        private DateTime? _endDate = DateTime.Now;
        
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

        /// <summary>
        /// Gets or sets a value indicating whether [all resps].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all resps]; otherwise, <c>false</c>.
        /// </value>
        public bool AllResps
        {
            get { return _allResps; }
            set
            {
                _allResps = value;
                OnPropertyChanged("AllResps");
            }
        }

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

            List<ReplayFile> result = replays.Where(x =>
                x.Tank != null
                &&
                (SelectedVersion == VersionAll || (SelectedVersion == VersionTest && x.ClientVersion > VersionRelease) || SelectedVersion == x.ClientVersion)
                &&
                (Level1Selected && x.Tank.Tier == 1
                 || Level2Selected && x.Tank.Tier == 2
                 || Level3Selected && x.Tank.Tier == 3
                 || Level4Selected && x.Tank.Tier == 4
                 || Level5Selected && x.Tank.Tier == 5
                 || Level6Selected && x.Tank.Tier == 6
                 || Level7Selected && x.Tank.Tier == 7
                 || Level8Selected && x.Tank.Tier == 8
                 || Level9Selected && x.Tank.Tier == 9
                 || Level10Selected && x.Tank.Tier == 10)
                &&
                (LTSelected && x.Tank.Type == (int) TankType.LT
                 || MTSelected && x.Tank.Type == (int) TankType.MT
                 || HTSelected && x.Tank.Type == (int) TankType.HT
                 || TDSelected && x.Tank.Type == (int) TankType.TD
                 || SPGSelected && x.Tank.Type == (int) TankType.SPG)
                &&
                (SelectedFolder == null || x.FolderId == SelectedFolder.Id)
                &&
                (USSRSelected && x.CountryId == Country.Ussr
                 || GermanySelected && x.CountryId == Country.Germany
                 || ChinaSelected && x.CountryId == Country.China
                 || FranceSelected && x.CountryId == Country.France
                 || USSelected && x.CountryId == Country.Usa
                 || JPSelected && x.CountryId == Country.Japan
                 || UKSelected && x.CountryId == Country.Uk)
                &&
                (x.IsWinner == SelectedBattleResult || SelectedBattleResult == BattleStatus.Unknown)
                && (x.Tank.Premium == 1 || !IsPremium)
                && (SelectedMap == null || x.MapId == SelectedMap.Id || SelectedMap.Id == 0)
                && FieldFilter(x)
                && MembersFilter(x.TeamMembers, members)
                && (BattleType == BattleType.Unknown || x.BattleType == BattleType)
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
                        && (settings.UseIncompleteReplaysResultsForCharts || x.IsWinner != BattleStatus.Incomplete))
                )
                ).ToList();

            //var footerList = PrepareToReturn(result, SelectedFolder != null ? SelectedFolder.Id : Guid.NewGuid());

            return result;
        }

        //private static List<ReplayFile> PrepareToReturn(List<ReplayFile> result, Guid folderId)
        //{
        //    List<ReplayFile> footerList = new FooterList<ReplayFile>(result);

        //    footerList.Insert(0, new TotalReplayFile(result, folderId));

        //    return footerList;
        //}

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
            List<ListItem<int>> list = Dictionaries.Instance.Maps.Values.OrderByDescending(x => x.mapid).Select(x => new ListItem<int>(x.mapid, x.localizedmapname)).ToList();
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

            ClearCommand = new DelegateCommand(OnClear);
            RefreshCommand = new DelegateCommand(OnRefresh);
            AllCommand = new DelegateCommand(OnAll);
        }

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
            TDSelected = MTSelected = LTSelected = HTSelected = SPGSelected = true;
            USSRSelected =
                UKSelected = USSelected = GermanySelected = JPSelected = ChinaSelected = FranceSelected = true;

            SelectedBattleResult = BattleStatus.Unknown;
            SelectedVersion = VersionAll;
            BattleType = BattleType.Unknown;
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
            TDSelected = MTSelected = LTSelected = HTSelected = SPGSelected = false;
            USSRSelected =
                UKSelected = USSelected = GermanySelected = JPSelected = ChinaSelected = FranceSelected = false;

            SelectedBattleResult = BattleStatus.Unknown;
            SelectedVersion = VersionAll;
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
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}