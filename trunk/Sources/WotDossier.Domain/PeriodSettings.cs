using System;
using System.Xml.Serialization;

namespace WotDossier.Domain
{
    public class PeriodSettings
    {
        private int _lastNBattles = 100;
        private StatisticPeriod _period;
        private DateTime? _prevDate;

        public int LastNBattles
        {
            get { return _lastNBattles; }
            set { _lastNBattles = value; }
        }

        public StatisticPeriod Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public DateTime? PrevDate
        {
            get { return _prevDate; }
            set { _prevDate = value; }
        }

        [XmlIgnore]
        public bool PrevDateSpecified
        {
            get { return _prevDate != null; }
        }
    }
}