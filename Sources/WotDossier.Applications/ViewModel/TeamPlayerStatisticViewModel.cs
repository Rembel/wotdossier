using System.Collections.Generic;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel
{
    public class TeamPlayerStatisticViewModel : PlayerStatisticViewModel
    {
        public TeamPlayerStatisticViewModel(TeamBattlesStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public TeamPlayerStatisticViewModel(TeamBattlesStatisticEntity stat, List<PlayerStatisticViewModel> list)
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
            }

            #endregion
        }
    }
}