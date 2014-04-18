using System;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal
{
    public interface IStatisticAdapter<T> where T : StatisticEntity
    {
        /// <summary>
        /// Gets or sets the updated datetime.
        /// </summary>
        DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the battles count.
        /// </summary>
        int BattlesCount { get; set; }

        /// <summary>
        /// Updates the specified snapshot.
        /// </summary>
        /// <param name="currentSnapshot">The current snapshot.</param>
        void Update(T currentSnapshot);
    }
}
