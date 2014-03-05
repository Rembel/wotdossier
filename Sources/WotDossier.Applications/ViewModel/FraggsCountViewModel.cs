using System.Collections.Generic;
using WotDossier.Applications.ViewModel.Rows;
using System.Linq;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class FraggsCountViewModel : TankFilterViewModel
    {
        private const string TANK_FRAGS_PROPERTY_NAME = "TankFrags";
        private List<FragsJson> _tankFrags;
        private KeyValue<int, string> _selectedTank;

        public List<KeyValue<int, string>> Tanks { get; set; }

        public KeyValue<int, string> SelectedTank
        {
            get { return _selectedTank; }
            set
            {
                _selectedTank = value;
                OnPropertyChanged(TANK_FRAGS_PROPERTY_NAME);
            }
        }

        public List<FragsJson> TankFrags
        {
            get { return AggregateFilter(_tankFrags); }
            set
            {
                _tankFrags = value;
                OnPropertyChanged(TANK_FRAGS_PROPERTY_NAME);
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (!TANK_FRAGS_PROPERTY_NAME.Equals(propertyName))
            {
                OnPropertyChanged(TANK_FRAGS_PROPERTY_NAME);
            }
        }

        private List<FragsJson> AggregateFilter(List<FragsJson> tankFrags)
        {
            if (tankFrags == null)
            {
                return new List<FragsJson>();
            }

            IEnumerable<FragsJson> filter = Filter(tankFrags)
                .Where(x => (SelectedTank == null
                   || SelectedTank.Key == 0
                   || x.KilledByTankUniqueId == SelectedTank.Key))
                .GroupBy(x => new
                    {   
                        x.CountryId, 
                        x.TankId, 
                        x.Icon, 
                        x.TankUniqueId, 
                        x.Type, x.Tank, 
                        x.Tier
                    })
                .Select(g => new FragsJson
                    {
                        Count = g.Sum(x => x.Count),
                        TankId = g.First().TankId,
                        Icon = g.First().Icon,
                        TankUniqueId = g.First().TankUniqueId,
                        Type = g.First().Type,
                        Tier = g.First().Tier,
                        Tank = g.First().Tank,
                        CountryId = g.First().CountryId
                    })
                .OrderByDescending(x => x.Tier)
                .ThenByDescending(x => x.Count)
                ;
            return filter.ToList();
        }

        public void Init(List<ITankStatisticRow> tanks)
        {
            TankFrags = tanks.SelectMany(x => x.TankFrags).ToList();
            Tanks = tanks.OrderBy(x => x.Tank).Select(x => new KeyValue<int, string>(x.TankUniqueId, x.Tank)).ToList();
            Tanks.Insert(0, new KeyValue<int, string>(0, Resources.Resources.TankFilterPanel_All));
            OnPropertyChanged("Tanks");
        }
    }
}