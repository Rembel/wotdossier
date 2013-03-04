using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowFrags
    {
        private int _tier;
        private int _icon;
        private string _tank;
        private int _battles;
        private int _frags;
        private int _maxFrags;
        private double _fragsPerBattle;
        private double _killDeathRatio;
        private int _tier8Frags;
        private int _beastFrags;
        private int _sinaiFrags;

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

        public int Battles
        {
            get { return _battles; }
            set { _battles = value; }
        }

        public int Frags
        {
            get { return _frags; }
            set { _frags = value; }
        }

        public int MaxFrags
        {
            get { return _maxFrags; }
            set { _maxFrags = value; }
        }

        public double FragsPerBattle
        {
            get { return _fragsPerBattle; }
            set { _fragsPerBattle = value; }
        }

        public double KillDeathRatio
        {
            get { return _killDeathRatio; }
            set { _killDeathRatio = value; }
        }

        public int Tier8Frags
        {
            get { return _tier8Frags; }
            set { _tier8Frags = value; }
        }

        public int BeastFrags
        {
            get { return _beastFrags; }
            set { _beastFrags = value; }
        }

        public int SinaiFrags
        {
            get { return _sinaiFrags; }
            set { _sinaiFrags = value; }
        }

        public TankRowFrags(Tank tank)
        {
            _tier = tank.Common.tier;
            _tank = tank.Name;
            _battles = tank.Tankdata.battlesCount;
            _frags = tank.Tankdata.frags;
            _maxFrags = tank.Tankdata.maxFrags;
            _fragsPerBattle = _frags / (double)_battles;
            _killDeathRatio = _frags / (double)(Battles - tank.Tankdata.survivedBattles);
            _tier8Frags = tank.Tankdata.frags8p;
            _beastFrags = tank.Tankdata.fragsBeast;
            _sinaiFrags = tank.Battle.fragsSinai;
        }
    }
}
