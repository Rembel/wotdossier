using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowXP
    {
        private int _tier;
        private int _icon;
        private string _tank;
        private int _totalXP;
        private int _maximumXp;
        private int _averageXp;

        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        public int Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public string Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }

        public int TotalXP
        {
            get { return _totalXP; }
            set { _totalXP = value; }
        }

        public int MaximumXP
        {
            get { return _maximumXp; }
            set { _maximumXp = value; }
        }

        public int AverageXP
        {
            get { return _averageXp; }
            set { _averageXp = value; }
        }

        public TankRowXP(Tank tank)
        {
            _tier = tank.Common.tier;
            _tank = tank.Name;
            _totalXP = tank.Tankdata.xp;
            _maximumXp = tank.Tankdata.maxXP;
            _averageXp = _totalXP / tank.Tankdata.battlesCount;
        }
    }
}
