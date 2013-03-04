using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowBattles
    {
        private int _tier;
        private int _icon;
        private string _tank;
        private int _battles;
        private int _won;
        private int _wonPercent;
        private int _lost;
        private int _lostPercent;
        private int _draws;
        private int _drawsPercent;
        private int _survived;
        private int _survivedPercent;
        private int _survivedAndWon;
        private int _survivedAndWonPercent;

        public TankRowBattles(Tank tank)
        {
            _tier = tank.Common.tier;
            //Icon = tank.;
            _tank = tank.Name;
            _battles = tank.Tankdata.battlesCount;
            _won = tank.Tankdata.wins;
            _wonPercent = (int) (_won / (double)_battles * 100.0);
            _lost = tank.Tankdata.losses;
            _lostPercent = (int)(_lost / (double)_battles * 100.0);
            _draws = _battles - _won - _lost;
            _drawsPercent = (int)(_draws / (double)_battles * 100.0);
            _survived = tank.Tankdata.survivedBattles;
            _survivedPercent = (int)(_survived / (double)_battles * 100.0);
            _survivedAndWon = tank.Tankdata.winAndSurvived;
            _survivedAndWonPercent = (int)(_survivedAndWon / (double)_battles * 100.0);
        }

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

        public int Won
        {
            get { return _won; }
            set { _won = value; }
        }

        public int WonPercent
        {
            get { return _wonPercent; }
            set { _wonPercent = value; }
        }

        public int Lost
        {
            get { return _lost; }
            set { _lost = value; }
        }

        public int LostPercent
        {
            get { return _lostPercent; }
            set { _lostPercent = value; }
        }

        public int Draws
        {
            get { return _draws; }
            set { _draws = value; }
        }

        public int DrawsPercent
        {
            get { return _drawsPercent; }
            set { _drawsPercent = value; }
        }

        public int Survived
        {
            get { return _survived; }
            set { _survived = value; }
        }

        public int SurvivedPercent
        {
            get { return _survivedPercent; }
            set { _survivedPercent = value; }
        }

        public int SurvivedAndWon
        {
            get { return _survivedAndWon; }
            set { _survivedAndWon = value; }
        }

        public int SurvivedAndWonPercent
        {
            get { return _survivedAndWonPercent; }
            set { _survivedAndWonPercent = value; }
        }
    }
}
