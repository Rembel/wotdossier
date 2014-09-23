﻿using System.ComponentModel;
using Microsoft.Research.DynamicDataDisplay;

namespace WotDossier.Applications.ViewModel.Chart
{
    public sealed class SellInfo : INotifyPropertyChanged
    {
        private double _winPercent;
        /// <summary>
        /// Gets or sets the win percent.
        /// </summary>
        /// <value>
        /// The win percent.
        /// </value>
        public double WinPercent
        {
            get { return _winPercent; }
            set { _winPercent = value; PropertyChanged.Raise(this, "WinPercent"); }
        }

        private string _tankName;
        /// <summary>
        /// Gets or sets the name of the tank.
        /// </summary>
        /// <value>
        /// The name of the tank.
        /// </value>
        public string TankName
        {
            get { return _tankName; }
            set { _tankName = value; PropertyChanged.Raise(this, "TankName"); }
        }

        private double _battles;
        /// <summary>
        /// Gets or sets the battles.
        /// </summary>
        /// <value>
        /// The battles.
        /// </value>
        public double Battles
        {
            get { return _battles; }
            set { _battles = value; PropertyChanged.Raise(this, "Battles"); }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public override string ToString()
        {
            return string.Format("{0}: battles: {1}, {2:0.0}%", TankName, Battles, WinPercent);
        }
    }
}