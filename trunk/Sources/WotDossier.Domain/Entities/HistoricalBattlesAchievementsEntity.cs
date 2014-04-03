using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'HistoricalBattlesAchievements'.
    /// </summary>
    [Serializable]
    public class HistoricalBattlesAchievementsEntity : EntityBase
    {
        #region Property names

        public static readonly string PropGuardsMan = TypeHelper<HistoricalBattlesAchievementsEntity>.PropertyName(v => v.GuardsMan);
        public static readonly string PropMakerOfHistory = TypeHelper<HistoricalBattlesAchievementsEntity>.PropertyName(v => v.MakerOfHistory);
        public static readonly string PropBothSidesWins = TypeHelper<HistoricalBattlesAchievementsEntity>.PropertyName(v => v.BothSidesWins);
        public static readonly string PropWeakVehiclesWins = TypeHelper<HistoricalBattlesAchievementsEntity>.PropertyName(v => v.WeakVehiclesWins);

        #endregion

        public virtual int GuardsMan { get; set; }

        public virtual int MakerOfHistory { get; set; }

        public virtual int BothSidesWins { get; set; }

        public virtual int WeakVehiclesWins { get; set; }

        #region Collections

        private IList<HistoricalBattlesAchievementsEntity> _historicalBattlesAchievementsEntities;

        /// <summary>
        ///     Gets/Sets the <see cref="PlayerStatisticEntity" /> collection.
        /// </summary>
        public virtual IList<HistoricalBattlesAchievementsEntity> HistoricalBattlesAchievementsEntities
        {
            get { return _historicalBattlesAchievementsEntities ?? (_historicalBattlesAchievementsEntities = new List<HistoricalBattlesAchievementsEntity>()); }
            set { _historicalBattlesAchievementsEntities = value; }
        }

        #endregion Collections
    }
}