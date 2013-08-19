using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Domain;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class TankFilterViewModel : INotifyPropertyChanged
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

        #endregion

        public List<T> Filter<T>(List<T> tankFrags) where T : ITankFilterable
        {
            return tankFrags.Where(x =>
                                   (x.Tier == 1 && Level1Selected
                                    || x.Tier == 2 && Level2Selected
                                    || x.Tier == 3 && Level3Selected
                                    || x.Tier == 4 && Level4Selected
                                    || x.Tier == 5 && Level5Selected
                                    || x.Tier == 6 && Level6Selected
                                    || x.Tier == 7 && Level7Selected
                                    || x.Tier == 8 && Level8Selected
                                    || x.Tier == 9 && Level9Selected
                                    || x.Tier == 10 && Level10Selected)
                                   &&
                                   (x.Type == (int)TankType.LT && LTSelected
                                    || x.Type == (int)TankType.MT && MTSelected
                                    || x.Type == (int)TankType.HT && HTSelected
                                    || x.Type == (int)TankType.TD && TDSelected
                                    || x.Type == (int)TankType.SPG && SPGSelected)
                                   &&
                                   (x.CountryId == (int)Country.USSR && USSRSelected
                                    || x.CountryId == (int)Country.Germany && GermanySelected
                                    || x.CountryId == (int)Country.China && ChinaSelected
                                    || x.CountryId == (int)Country.France && FranceSelected
                                    || x.CountryId == (int)Country.US && USSelected
                                    || x.CountryId == (int)Country.UK && UKSelected)
                ).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}