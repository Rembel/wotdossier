using System;
using WotDossier.Domain;
using WotDossier.Framework.EventAggregator;

namespace WotDossier.Applications.ViewModel
{
    public class StatisticPeriodChangedEvent : BaseEvent<StatisticPeriodChangedEvent>
    {
        public StatisticPeriod StatisticPeriod { get; set; }
        public DateTime? PrevDateTime { get; set; }
        public int LastNBattles { get; set; }

        public StatisticPeriodChangedEvent()
        {
        }

        public StatisticPeriodChangedEvent(StatisticPeriod statisticPeriod, DateTime? prevDate, int lastNBattles)
        {
            StatisticPeriod = statisticPeriod;
            PrevDateTime = prevDate;
            LastNBattles = lastNBattles;
        }
    }
}