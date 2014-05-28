using System;

namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticBase : IStatisticXp, IRandomBattlesAchievements, ITeamBattlesAchievements, IHistoricalBattlesAchievements
    {
        /// <summary>
        /// Stat updated
        /// </summary>
        DateTime Updated { get; set; }

        int BattlesPerDay { get; set; }
    }
}