﻿using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.ViewModel
{
    public class FraggsCountViewModel : TankFilterViewModel
    {
        public static readonly string PropTankFrags = TypeHelper<FraggsCountViewModel>.PropertyName(v => v.TankFrags);
        
        private const int KEY_ALL_VALUES = -1;
        private List<FragsJson> _tankFrags;
        private KeyValue<int, string> _selectedTank;

        public List<KeyValue<int, string>> Tanks { get; set; }

        public KeyValue<int, string> SelectedTank
        {
            get { return _selectedTank; }
            set
            {
                _selectedTank = value;
                OnPropertyChanged(PropTankFrags);
            }
        }

        public List<FragsJson> TankFrags
        {
            get { return AggregateFilter(_tankFrags); }
            set
            {
                _tankFrags = value;
                OnPropertyChanged(PropTankFrags);
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (!PropTankFrags.Equals(propertyName))
            {
                OnPropertyChanged(PropTankFrags);
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
                   || SelectedTank.Key == KEY_ALL_VALUES
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

        public void Init(List<TankJson> tanks)
        {
            TankFrags = tanks.SelectMany(x => x.Frags).ToList();
            Tanks = tanks.OrderBy(x => x.Common.tanktitle).Select(x => new KeyValue<int, string>(x.UniqueId(), x.Common.tanktitle)).ToList();
            Tanks.Insert(0, new KeyValue<int, string>(KEY_ALL_VALUES, Resources.Resources.TankFilterPanel_All));
            OnPropertyChanged("Tanks");
        }
    }
}