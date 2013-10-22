using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WotDossier.Domain;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class TankFilterViewModel : INotifyPropertyChanged
    {
        private readonly TankFilterSettings _filter;

        #region FILTERS

        public bool Level10Selected
        {

            get { return _filter.Level10Selected; }
            set
            {
                _filter.Level10Selected = value;
                OnPropertyChanged("Level10Selected");
            }
        }

        public bool Level9Selected
        {
            get { return _filter.Level9Selected; }
            set
            {
                _filter.Level9Selected = value;
                OnPropertyChanged("Level9Selected");
            }
        }

        public bool Level8Selected
        {
            get { return _filter.Level8Selected; }
            set
            {
                _filter.Level8Selected = value;
                OnPropertyChanged("Level8Selected");
            }
        }

        public bool Level7Selected
        {
            get { return _filter.Level7Selected; }
            set
            {
                _filter.Level7Selected = value;
                OnPropertyChanged("Level7Selected");
            }
        }

        public bool Level6Selected
        {
            get { return _filter.Level6Selected; }
            set
            {
                _filter.Level6Selected = value;
                OnPropertyChanged("Level6Selected");
            }
        }

        public bool Level5Selected
        {
            get { return _filter.Level5Selected; }
            set
            {
                _filter.Level5Selected = value;
                OnPropertyChanged("Level5Selected");
            }
        }

        public bool Level4Selected
        {
            get { return _filter.Level4Selected; }
            set
            {
                _filter.Level4Selected = value;
                OnPropertyChanged("Level4Selected");
            }
        }

        public bool Level3Selected
        {
            get { return _filter.Level3Selected; }
            set
            {
                _filter.Level3Selected = value;
                OnPropertyChanged("Level3Selected");
            }
        }

        public bool Level2Selected
        {
            get { return _filter.Level1Selected; }
            set
            {
                _filter.Level1Selected = value;
                OnPropertyChanged("Level2Selected");
            }
        }

        public bool Level1Selected
        {
            get { return _filter.Level1Selected; }
            set
            {
                _filter.Level1Selected = value;
                OnPropertyChanged("Level1Selected");
            }
        }

        public bool SPGSelected
        {
            get { return _filter.SPGSelected; }
            set
            {
                _filter.SPGSelected = value;
                OnPropertyChanged("SPGSelected");
            }
        }

        public bool TDSelected
        {
            get { return _filter.TDSelected; }
            set
            {
                _filter.TDSelected = value;
                OnPropertyChanged("TDSelected");
            }
        }

        public bool HTSelected
        {
            get { return _filter.HTSelected; }
            set
            {
                _filter.HTSelected = value;
                OnPropertyChanged("HTSelected");
            }
        }

        public bool MTSelected
        {
            get { return _filter.MTSelected; }
            set
            {
                _filter.MTSelected = value;
                OnPropertyChanged("MTSelected");
            }
        }

        public bool LTSelected
        {
            get { return _filter.LTSelected; }
            set
            {
                _filter.LTSelected = value;
                OnPropertyChanged("LTSelected");
            }
        }

        public bool USSRSelected
        {
            get { return _filter.USSRSelected; }
            set
            {
                _filter.USSRSelected = value;
                OnPropertyChanged("USSRSelected");
            }
        }

        public bool GermanySelected
        {
            get { return _filter.GermanySelected; }
            set
            {
                _filter.GermanySelected = value;
                OnPropertyChanged("GermanySelected");
            }
        }

        public bool USSelected
        {
            get { return _filter.USSelected; }
            set
            {
                _filter.USSelected = value;
                OnPropertyChanged("USSelected");
            }
        }

        public bool ChinaSelected
        {
            get { return _filter.ChinaSelected; }
            set
            {
                _filter.ChinaSelected = value;
                OnPropertyChanged("ChinaSelected");
            }
        }

        public bool FranceSelected
        {
            get { return _filter.FranceSelected; }
            set
            {
                _filter.FranceSelected = value;
                OnPropertyChanged("FranceSelected");
            }
        }

        public bool UKSelected
        {
            get { return _filter.UKSelected; }
            set
            {
                _filter.UKSelected = value;
                OnPropertyChanged("UKSelected");
            }
        }

        public bool IsPremium
        {
            get { return _filter.IsPremium; }
            set
            {
                _filter.IsPremium = value;
                OnPropertyChanged("IsPremium");
            }
        }

        public bool IsFavorite
        {
            get { return _filter.IsFavorite; }
            set
            {
                _filter.IsFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TankFilterViewModel()
        {
            _filter = SettingsReader.Get().TankFilterSettings;
        }

        public List<T> Filter<T>(List<T> tanks) where T : ITankFilterable
        {
            return tanks.Where(x =>
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
                                   && (x.IsFavorite || !IsFavorite)
                                   && (x.IsPremium || !IsPremium)
                ).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            AppSettings appSettings = SettingsReader.Get();
            appSettings.TankFilterSettings = _filter;
            SettingsReader.Save(appSettings);
        }
    }
}