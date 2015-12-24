using System;
using System.Collections.Generic;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Entities
{
    /// <summary>
    ///     Object representation for table 'TeamBattlesAchievements'.
    /// </summary>
    [Serializable]
    public class TeamBattlesAchievementsEntity : EntityBase, ITeamBattlesAchievements
    {
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

        public virtual int PromisingFighter { get; set; }

        public virtual int PromisingFighterMedal { get; set; }

        public virtual int HeavyFire { get; set; }

        public virtual int HeavyFireMedal { get; set; }

        public virtual int Ranger { get; set; }

        public virtual int RangerMedal { get; set; }

        public virtual int FireAndSteel { get; set; }

        public virtual int FireAndSteelMedal { get; set; }

        public virtual int Pyromaniac { get; set; }

        public virtual int PyromaniacMedal { get; set; }

        public virtual int NoMansLand { get; set; }

        public virtual int Guerrilla { get; set; }
        public virtual int GuerrillaMedal { get; set; }
        public virtual int Infiltrator { get; set; }
        public virtual int InfiltratorMedal { get; set; }
        public virtual int Sentinel { get; set; }
        public virtual int SentinelMedal { get; set; }
        public virtual int PrematureDetonation { get; set; }
        public virtual int PrematureDetonationMedal { get; set; }
        public virtual int BruteForce { get; set; }
        public virtual int BruteForceMedal { get; set; }
        public virtual int AwardCount { get; set; }
        public virtual int BattleTested { get; set; }

        #region Collections

        private IList<TeamBattlesStatisticEntity> _teamBattlesStatisticEntities;

        /// <summary>
        ///     Gets/Sets the <see cref="RandomBattlesStatisticEntity" /> collection.
        /// </summary>
        public virtual IList<TeamBattlesStatisticEntity> TeamBattlesStatisticEntities
        {
            get { return _teamBattlesStatisticEntities ?? (_teamBattlesStatisticEntities = new List<TeamBattlesStatisticEntity>()); }
            set { _teamBattlesStatisticEntities = value; }
        }

        #endregion Collections
    }
}