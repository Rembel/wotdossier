using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using WotDossier.Applications.ViewModel.Replay;
using WotDossier.Common.Collections;
using WotDossier.Dal;
using WotDossier.Domain;
using WotDossier.Domain.Replay;
using WotDossier.Framework.Forms.Commands;

namespace WotDossier.Applications.ViewModel.Filter
{
    public class ReplaysFilterViewModel : INotifyPropertyChanged
    {
        #region FILTERS

        private bool _level1Selected = true;
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
            get { return _level1Selected; }
            set
            {
                _level1Selected = value;
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
        public DelegateCommand AllCommand { get; set; }

        private List<ListItem<BattleType>> _battleTypes = new List<ListItem<BattleType>>
            {
                new ListItem<BattleType>(BattleType.Unknown, Resources.Resources.TankFilterPanel_All), 
                new ListItem<BattleType>(BattleType.Regular, Resources.Resources.BattleType_Regular), 
                new ListItem<BattleType>(BattleType.Historical,Resources.Resources.BattleType_Historical), 
                new ListItem<BattleType>(BattleType.CyberSport,Resources.Resources.BattleType_CyberSport), 
                new ListItem<BattleType>(BattleType.ClanWar, Resources.Resources.BattleType_ClanWar), 
                new ListItem<BattleType>(BattleType.CompanyWar,Resources.Resources.BattleType_CompanyWar), 
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

        /// <summary>
        /// Filters the replays list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replays">The replays.</param>
        /// <returns></returns>
        public List<ReplayFile> Filter(List<ReplayFile> replays)
        {
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
                                    x.Tank != null &&
                                   (x.Tank.Tier == 1 && Level1Selected
                                    || x.Tank.Tier == 2 && Level2Selected
                                    || x.Tank.Tier == 3 && Level3Selected
                                    || x.Tank.Tier == 4 && Level4Selected
                                    || x.Tank.Tier == 5 && Level5Selected
                                    || x.Tank.Tier == 6 && Level6Selected
                                    || x.Tank.Tier == 7 && Level7Selected
                                    || x.Tank.Tier == 8 && Level8Selected
                                    || x.Tank.Tier == 9 && Level9Selected
                                    || x.Tank.Tier == 10 && Level10Selected)
                                   &&
                                       (x.Tank.Type == (int)TankType.LT && LTSelected
                                        || x.Tank.Type == (int)TankType.MT && MTSelected
                                        || x.Tank.Type == (int)TankType.HT && HTSelected
                                        || x.Tank.Type == (int)TankType.TD && TDSelected
                                        || x.Tank.Type == (int)TankType.SPG && SPGSelected)
                                   &&
                                    (SelectedFolder != null && x.FolderId == SelectedFolder.Id)
                                    &&
                                   (x.CountryId == Country.Ussr && USSRSelected
                                    || x.CountryId == Country.Germany && GermanySelected
                                    || x.CountryId == Country.China && ChinaSelected
                                    || x.CountryId == Country.France && FranceSelected
                                    || x.CountryId == Country.Usa && USSelected
                                    || x.CountryId == Country.Japan && JPSelected
                                    || x.CountryId == Country.Uk && UKSelected)
                                    &&
                                    (x.IsWinner == SelectedBattleResult || SelectedBattleResult == BattleStatus.Unknown)
                                   && (x.Tank.Premium == 1 || !IsPremium)
                                   && (SelectedMap == null || x.MapId == SelectedMap.Id || SelectedMap.Id == 0)
                                   && FieldFilter(x)
                                   && MembersFilter(x.TeamMembers, members)
                                   && (BattleType == BattleType.Unknown || x.BattleType == BattleType)
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
            AllCommand = new DelegateCommand(OnAll);
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