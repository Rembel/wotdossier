﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications.ViewModel
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
        private bool _isPremium;
        private bool _isFavorite;
        private ReplayFolder _selectedFolder;
        private KeyValue<int, string> _selectedMap;
        private List<KeyValue<int, string>> _maps;
        private string _field;
        private int? _startValue;
        private int? _endValue;

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

        public List<KeyValue<int, string>> Maps
        {
            get { return _maps; }
            set
            {
                _maps = value;
                OnPropertyChanged("Maps");
            }
        }

        public KeyValue<int, string> SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                _selectedMap = value;
                OnPropertyChanged("SelectedMap");
            }
        }

        #endregion

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

        public List<T> Filter<T>(List<T> replays) where T : ReplayFile
        {
            return replays.Where(x =>
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
                                   (x.CountryId == (int)Country.USSR && USSRSelected
                                    || x.CountryId == (int)Country.Germany && GermanySelected
                                    || x.CountryId == (int)Country.China && ChinaSelected
                                    || x.CountryId == (int)Country.France && FranceSelected
                                    || x.CountryId == (int)Country.US && USSelected
                                    || x.CountryId == (int)Country.UK && UKSelected)
                                   && (x.Tank.Premium == 1 || !IsPremium)
                                   && (SelectedMap == null || x.MapId == SelectedMap.Key || SelectedMap.Key == 0)
                                   && FieldFilter(x)
                ).ToList();
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
            List<KeyValue<int, string>> list = WotApiClient.Instance.Maps.Values.OrderByDescending(x => x.mapid).Select(x => new KeyValue<int, string>(x.mapid, x.localizedmapname)).ToList();
            list.Insert(0, new KeyValue<int, string>(0, ""));
            Maps = list;

            FilterFields = new List<KeyValue<string, string>>
            {
                new KeyValue<string, string>(ReplayFile.PropCredits,Resources.Resources.Column_Replay_Credits), 
                new KeyValue<string, string>(ReplayFile.PropDamageDealt,Resources.Resources.Column_Replay_DamageDealt), 
                new KeyValue<string, string>(ReplayFile.PropXp,Resources.Resources.Column_Replay_XP), 
                new KeyValue<string, string>(ReplayFile.PropKilled,Resources.Resources.Column_Replay_Frags), 
                new KeyValue<string, string>(ReplayFile.PropDamaged,Resources.Resources.Column_Replay_Damaged), 
            };
        }

        public List<KeyValue<string, string>> FilterFields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}