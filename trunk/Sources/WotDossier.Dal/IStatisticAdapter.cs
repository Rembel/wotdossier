using System;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal
{
    public interface IStatisticAdapter<T> where T : StatisticEntity
    {
        DateTime Updated { get; set; }
        int Battles_count { get; set; }
        void Update(T currentSnapshot);
    }
}
