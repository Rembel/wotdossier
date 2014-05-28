using System;

namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticTime
    {
        DateTime LastBattle { get; set; }
        TimeSpan PlayTime { get; set; }
        TimeSpan AverageBattleTime { get; set; }
    }
}