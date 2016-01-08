using System;

namespace WotDossier.Domain
{
    /// <summary>
    /// Provides the configuration setting for statistic period.
    /// </summary>
    public class PeriodSettings
    {
        private int _lastNBattles = 100;
        private StatisticPeriod _period;
        private DateTime? _prevDate;

        /// <summary>
        /// Gets or sets the count of battles for period.
        /// </summary>
        public int LastNBattles
        {
            get { return _lastNBattles; }
            set { _lastNBattles = value; }
        }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public StatisticPeriod Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// Gets or sets the prev date.
        /// </summary>
        /// <value>
        /// The prev date.
        /// </value>
        public DateTime? PrevDate
        {
            get { return _prevDate; }
            set { _prevDate = value; }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="PrevDate"/> specified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if <see cref="PrevDate"/> specified otherwise, <c>false</c>.
        /// </value>
        public bool PrevDateSpecified
        {
            get { return _prevDate != null; }
        }
    }
}