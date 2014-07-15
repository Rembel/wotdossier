using System.Collections.Generic;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public class TeamBattlesPlayerStatisticViewModel : PlayerStatisticViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public TeamBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBattlesPlayerStatisticViewModel"/> class.
        /// </summary>
        /// <param name="stat">The stat.</param>
        /// <param name="list">The list.</param>
        public TeamBattlesPlayerStatisticViewModel(TeamBattlesStatisticEntity stat, List<PlayerStatisticViewModel> list)
            : base(stat, list)
        {
            #region Awards

            if (stat.AchievementsIdObject != null)
            {
                ArmoredFist = stat.AchievementsIdObject.ArmoredFist;
                CrucialShot = stat.AchievementsIdObject.CrucialShot;
                CrucialShotMedal = stat.AchievementsIdObject.CrucialShotMedal;
                FightingReconnaissance = stat.AchievementsIdObject.FightingReconnaissance;
                FightingReconnaissanceMedal = stat.AchievementsIdObject.FightingReconnaissanceMedal;
                ForTacticalOperations = stat.AchievementsIdObject.ForTacticalOperations;
                GeniusForWar = stat.AchievementsIdObject.GeniusForWar;
                GeniusForWarMedal = stat.AchievementsIdObject.GeniusForWarMedal;
                GodOfWar = stat.AchievementsIdObject.GodOfWar;
                KingOfTheHill = stat.AchievementsIdObject.KingOfTheHill;
                MaxTacticalBreakthroughSeries = stat.AchievementsIdObject.MaxTacticalBreakthroughSeries;
                TacticalBreakthrough = stat.AchievementsIdObject.TacticalBreakthrough;
                TacticalBreakthroughSeries = stat.AchievementsIdObject.TacticalBreakthroughSeries;
                WillToWinSpirit = stat.AchievementsIdObject.WillToWinSpirit;
                WolfAmongSheep = stat.AchievementsIdObject.WolfAmongSheep;
                WolfAmongSheepMedal = stat.AchievementsIdObject.WolfAmongSheepMedal;

                NoMansLand = stat.AchievementsIdObject.NoMansLand;
                PyromaniacMedal = stat.AchievementsIdObject.PyromaniacMedal;
                Pyromaniac = stat.AchievementsIdObject.Pyromaniac;
                FireAndSteel = stat.AchievementsIdObject.FireAndSteel;
                FireAndSteelMedal = stat.AchievementsIdObject.FireAndSteelMedal;
                RangerMedal = stat.AchievementsIdObject.RangerMedal;
                Ranger = stat.AchievementsIdObject.Ranger;
                HeavyFireMedal = stat.AchievementsIdObject.HeavyFireMedal;
                HeavyFire = stat.AchievementsIdObject.HeavyFire;
                PromisingFighterMedal = stat.AchievementsIdObject.PromisingFighterMedal;
                PromisingFighter = stat.AchievementsIdObject.PromisingFighter;

                Guerrilla = stat.AchievementsIdObject.Guerrilla;
                GuerrillaMedal = stat.AchievementsIdObject.GuerrillaMedal;
                Infiltrator = stat.AchievementsIdObject.Infiltrator;
                InfiltratorMedal = stat.AchievementsIdObject.InfiltratorMedal;
                Sentinel = stat.AchievementsIdObject.Sentinel;
                SentinelMedal = stat.AchievementsIdObject.SentinelMedal;
                PrematureDetonation = stat.AchievementsIdObject.PrematureDetonation;
                PrematureDetonationMedal = stat.AchievementsIdObject.PrematureDetonationMedal;
                BruteForce = stat.AchievementsIdObject.BruteForce;
                BruteForceMedal = stat.AchievementsIdObject.BruteForceMedal;
                AwardCount = stat.AchievementsIdObject.AwardCount;
                BattleTested = stat.AchievementsIdObject.BattleTested;
            }

            #endregion
        }
    }
}