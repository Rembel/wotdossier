using System;

namespace WotDossier.Domain.Rows
{
    public interface ITankRowTime
    {
        DateTime LastBattle { get; set; }
        TimeSpan PlayTime { get; set; }
        TimeSpan AverageBattleTime { get; set; }
    }
}