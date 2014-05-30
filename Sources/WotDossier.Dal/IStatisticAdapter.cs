using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Dal
{
    public interface IStatisticAdapter<T> : IStatisticBase where T : StatisticEntity
    {
        /// <summary>
        /// Updates the specified snapshot.
        /// </summary>
        /// <param name="currentSnapshot">The current snapshot.</param>
        void Update(T currentSnapshot);
    }
}
