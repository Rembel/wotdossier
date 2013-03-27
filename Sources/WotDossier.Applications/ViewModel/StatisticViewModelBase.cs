using System;

namespace WotDossier.Applications.ViewModel
{
    public abstract class StatisticViewModelBase
    {
        private int _battlesPerDay;

        #region Percents
    
        public int BattlesCount { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SurvivedBattles { get; set; }
        public int Xp { get; set; }
        public int BattleAvgXp { get; set; }
        public int MaxXp { get; set; }
        public int Frags { get; set; }
        public int Spotted { get; set; }
        public int HitsPercents { get; set; }
        public int DamageDealt { get; set; }
        public int CapturePoints { get; set; }
        public int DroppedCapturePoints { get; set; }
        public double WinsPercent { get; set; }
        public double LossesPercent { get; set; }
        public double SurvivedBattlesPercent { get; set; }
        
        #endregion

        #region Unofficial ratings
        
        public double WN6Rating { get; set; }

        public double EffRating { get; set; }

        public double KievArmorRating { get; set; }
        
        #endregion
        
        /// <summary>
        /// Stat updated
        /// </summary>
        public DateTime Updated { get; set; }

        public int BattlesPerDay
        {
            get { return _battlesPerDay; }
            set { _battlesPerDay = value; }
        }
    }
}