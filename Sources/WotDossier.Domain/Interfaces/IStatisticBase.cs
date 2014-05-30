using System;

namespace WotDossier.Domain.Interfaces
{
    public interface IStatisticBase : IStatisticXp, IStatisticBattles, IStatisticDamage, IStatisticFrags, IStatisticPerformance
    {
        /// <summary>
        /// Stat updated
        /// </summary>
        DateTime Updated { get; set; }
    }
}