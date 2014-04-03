using System;
using System.Collections.Generic;
using WotDossier.Common;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'TeamBattlesAchievements'.
    /// </summary>
    [Serializable]
    public class TeamBattlesAchievementsEntity : EntityBase
    {
        #region Property names
        
        public static readonly string PropWolfAmongSheep = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.WolfAmongSheep);
        public static readonly string PropWolfAmongSheepMedal = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.WolfAmongSheepMedal);
        public static readonly string PropGeniusForWar = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.GeniusForWar);
        public static readonly string PropGeniusForWarMedal = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.GeniusForWarMedal);
        public static readonly string PropKingOfTheHill = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.KingOfTheHill);
        public static readonly string PropTacticalBreakthroughSeries = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.TacticalBreakthroughSeries);
        public static readonly string PropMaxTacticalBreakthroughSeries = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.MaxTacticalBreakthroughSeries);
        public static readonly string PropArmoredFist = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.ArmoredFist);
        public static readonly string PropTacticalBreakthrough = TypeHelper<TeamBattlesAchievementsEntity>.PropertyName(v => v.TacticalBreakthrough);

        #endregion

        public virtual int WolfAmongSheep { get; set; }

        public virtual int WolfAmongSheepMedal { get; set; }

        public virtual int GeniusForWar { get; set; }

        public virtual int GeniusForWarMedal { get; set; }

        public virtual int KingOfTheHill { get; set; }

        public virtual int TacticalBreakthroughSeries { get; set; }

        public virtual int MaxTacticalBreakthroughSeries { get; set; }

        public virtual int ArmoredFist { get; set; }

        public virtual int TacticalBreakthrough { get; set; }

        public virtual int GodOfWar { get; set; }

        public virtual int FightingReconnaissance { get; set; }

        public virtual int FightingReconnaissanceMedal { get; set; }

        public virtual int WillToWinSpirit { get; set; }

        public virtual int CrucialShot { get; set; }

        public virtual int CrucialShotMedal { get; set; }

        public virtual int ForTacticalOperations { get; set; }

        #region Collections

        private IList<TeamBattlesStatisticEntity> _teamBattlesStatisticEntities;

        /// <summary>
        ///     Gets/Sets the <see cref="PlayerStatisticEntity" /> collection.
        /// </summary>
        public virtual IList<TeamBattlesStatisticEntity> TeamBattlesStatisticEntities
        {
            get { return _teamBattlesStatisticEntities ?? (_teamBattlesStatisticEntities = new List<TeamBattlesStatisticEntity>()); }
            set { _teamBattlesStatisticEntities = value; }
        }

        #endregion Collections
    }
}