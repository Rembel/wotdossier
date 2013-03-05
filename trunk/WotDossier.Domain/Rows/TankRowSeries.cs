﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain.Rows
{
    public class TankRowSeries
    {
        private int _tier;
        private int _icon;
        private string _tank;
        private int _reaperLongest;
        private int _reaperProgress;
        private int _sharpshooterLongest;
        private int _sharpshooterProgress;
        private int _masterGunnerLongest;
        private int _masterGunnerProgress;
        private int _invincibleLongest;
        private int _invincibleProgress;
        private int _survivorLongest;
        private int _survivorProgress;

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

        public int ReaperLongest
        {
            get { return _reaperLongest; }
            set { _reaperLongest = value; }
        }

        public int ReaperProgress
        {
            get { return _reaperProgress; }
            set { _reaperProgress = value; }
        }

        public int SharpshooterLongest
        {
            get { return _sharpshooterLongest; }
            set { _sharpshooterLongest = value; }
        }

        public int SharpshooterProgress
        {
            get { return _sharpshooterProgress; }
            set { _sharpshooterProgress = value; }
        }

        public int MasterGunnerLongest
        {
            get { return _masterGunnerLongest; }
            set { _masterGunnerLongest = value; }
        }

        public int MasterGunnerProgress
        {
            get { return _masterGunnerProgress; }
            set { _masterGunnerProgress = value; }
        }

        public int InvincibleLongest
        {
            get { return _invincibleLongest; }
            set { _invincibleLongest = value; }
        }

        public int InvincibleProgress
        {
            get { return _invincibleProgress; }
            set { _invincibleProgress = value; }
        }

        public int SurvivorLongest
        {
            get { return _survivorLongest; }
            set { _survivorLongest = value; }
        }

        public int SurvivorProgress
        {
            get { return _survivorProgress; }
            set { _survivorProgress = value; }
        }

        public TankRowSeries(Tank tank)
        {
            _tier = tank.Common.tier;
            _tank = tank.Name;
            _reaperLongest = tank.Series.maxKillingSeries;
            _reaperProgress = tank.Series.killingSeries;
            _sharpshooterLongest = tank.Series.maxSniperSeries;
            _sharpshooterProgress = tank.Series.sniperSeries;
            _masterGunnerLongest = tank.Series.maxPiercingSeries;
            _masterGunnerProgress = tank.Series.piercingSeries;
            _invincibleLongest = tank.Series.maxInvincibleSeries;
            _invincibleProgress = tank.Series.invincibleSeries;
            _survivorLongest = tank.Series.maxDiehardSeries;
            _survivorProgress = tank.Series.diehardSeries;
        }
    }
}