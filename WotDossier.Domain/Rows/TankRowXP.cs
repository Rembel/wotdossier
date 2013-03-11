﻿namespace WotDossier.Domain.Rows
{
    public class TankRowXP : TankRowBase
    {
        private int _totalXP;
        private int _maximumXp;
        private int _averageXp;

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
            Tier = tank.Common.tier;
            Tank = tank.Name;
            Icon = tank.TankContour;
            _totalXP = tank.Tankdata.xp;
            _maximumXp = tank.Tankdata.maxXP;
            _averageXp = _totalXP / tank.Tankdata.battlesCount;
        }
    }
}