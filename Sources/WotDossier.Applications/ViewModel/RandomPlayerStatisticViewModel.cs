using System.Collections.Generic;
using WotDossier.Domain.Entities;

namespace WotDossier.Applications.ViewModel
{
    public class RandomPlayerStatisticViewModel : PlayerStatisticViewModel
    {
        public RandomPlayerStatisticViewModel(PlayerStatisticEntity stat) : this(stat, new List<PlayerStatisticViewModel>())
        {
        }

        public RandomPlayerStatisticViewModel(PlayerStatisticEntity stat, List<PlayerStatisticViewModel> list)
            : base(stat, list)
        {
            #region Ratings init

            //PR-->
            //Private Rating
            Rating_IntegratedValue = stat.RatingIntegratedValue;
            Rating_IntegratedPlace = stat.RatingIntegratedPlace;
            //W/B-->
            //Victories/Battles
            Rating_BattleAvgPerformanceValue = stat.RatingWinsRatioValue;
            Rating_BattleAvgPerformancePlace = stat.RatingWinsRatioPlace;
            //E/B-->
            //Average Experience per Battle
            Rating_BattleAvgXpValue = stat.RatingBattleAvgXpValue;
            Rating_BattleAvgXpPlace = stat.RatingBattleAvgXpPlace;
            //WIN-->
            //Victories
            Rating_BattleWinsValue = stat.RatingBattleWinsValue;
            Rating_BattleWinsPlace = stat.RatingBattleWinsPlace;
            //GPL-->
            //Battles Participated
            Rating_BattlesValue = stat.RatingBattlesValue;
            Rating_BattlesPlace = stat.RatingBattlesPlace;
            //CPT-->
            //Capture Points
            Rating_CapturedPointsValue = stat.RatingCapturedPointsValue;
            Rating_CapturedPointsPlace = stat.RatingCapturedPointsPlace;
            //DMG-->
            //Damage Caused
            Rating_DamageDealtValue = stat.RatingDamageDealtValue;
            Rating_DamageDealtPlace = stat.RatingDamageDealtPlace;
            //DPT-->
            //Defense Points
            Rating_DroppedPointsValue = stat.RatingDroppedPointsValue;
            Rating_DroppedPointsPlace = stat.RatingDroppedPointsPlace;
            //FRG-->
            //Targets Destroyed
            Rating_FragsValue = stat.RatingFragsValue;
            Rating_FragsPlace = stat.RatingFragsPlace;
            //SPT-->
            //Targets Detected
            Rating_SpottedValue = stat.RatingSpottedValue;
            Rating_SpottedPlace = stat.RatingSpottedPlace;
            //EXP-->
            //Total Experience
            Rating_XpValue = stat.RatingXpValue;
            Rating_XpPlace = stat.RatingXpPlace;
            //MXP-->
            //Max Experience
            Rating_MaxXpValue = stat.RatingMaxXpValue;
            Rating_MaxXpPlace = stat.RatingMaxXpPlace;
            //HR-->
            //Hits Percent
            Rating_HitsPercentsValue = stat.RatingHitsPercentsValue;
            Rating_HitsPercentsPlace = stat.RatingHitsPercentsPlace;

            #endregion

            #region Achievements

            DamageTaken = stat.DamageTaken;

            if (stat.AchievementsIdObject != null)
            {
                #region [ BattleAwards ]

                //BattleHero = stat.AchievementsIdObject.battleHeroes;
                Warrior = stat.AchievementsIdObject.Warrior;
                Invader = stat.AchievementsIdObject.Invader;
                Sniper = stat.AchievementsIdObject.Sniper;
                Sniper2 = stat.AchievementsIdObject.Sniper2;
                MainGun = stat.AchievementsIdObject.MainGun;
                Defender = stat.AchievementsIdObject.Defender;
                SteelWall = stat.AchievementsIdObject.SteelWall;
                Confederate = stat.AchievementsIdObject.Confederate;
                Scout = stat.AchievementsIdObject.Scout;
                PatrolDuty = stat.AchievementsIdObject.PatrolDuty;
                BrothersInArms = stat.AchievementsIdObject.BrothersInArms;
                CrucialContribution = stat.AchievementsIdObject.CrucialContribution;
                IronMan = stat.AchievementsIdObject.IronMan;
                LuckyDevil = stat.AchievementsIdObject.LuckyDevil;
                Sturdy = stat.AchievementsIdObject.Sturdy;

                #endregion

                #region [ Epic ]

                Boelter = stat.AchievementsIdObject.Boelter;
                RadleyWalters = stat.AchievementsIdObject.RadleyWalters;
                LafayettePool = stat.AchievementsIdObject.LafayettePool;
                Orlik = stat.AchievementsIdObject.Orlik;
                Oskin = stat.AchievementsIdObject.Oskin;
                Lehvaslaiho = stat.AchievementsIdObject.Lehvaslaiho;
                Nikolas = stat.AchievementsIdObject.Nikolas;
                Halonen = stat.AchievementsIdObject.Halonen;
                Burda = stat.AchievementsIdObject.Burda;
                Pascucci = stat.AchievementsIdObject.Pascucci;
                Dumitru = stat.AchievementsIdObject.Dumitru;
                TamadaYoshio = stat.AchievementsIdObject.TamadaYoshio;
                Billotte = stat.AchievementsIdObject.Billotte;
                BrunoPietro = stat.AchievementsIdObject.BrunoPietro;
                Tarczay = stat.AchievementsIdObject.Tarczay;
                Kolobanov = stat.AchievementsIdObject.Kolobanov;
                Fadin = stat.AchievementsIdObject.Fadin;
                HeroesOfRassenay = stat.AchievementsIdObject.HeroesOfRassenay;
                DeLanglade = stat.AchievementsIdObject.DeLanglade;

                #endregion

                #region [ Medals]

                //Kay = stat.AchievementsIdObject.Major.Kay;
                //Carius = stat.AchievementsIdObject.Major.Carius;
                //Knispel = stat.AchievementsIdObject.Major.Knispel;
                //Poppel = stat.AchievementsIdObject.Major.Poppel;
                //Abrams = stat.AchievementsIdObject.Major.Abrams;
                //Leclerk = stat.AchievementsIdObject.Major.LeClerc;
                //Lavrinenko = stat.AchievementsIdObject.Major.Lavrinenko;
                //Ekins = stat.AchievementsIdObject.Major.Ekins;

                #endregion

                #region [ Series ]

                ReaperLongest = stat.AchievementsIdObject.Reaper;
                SharpshooterLongest = stat.AchievementsIdObject.SharpshooterLongest;
                MasterGunnerLongest = stat.AchievementsIdObject.MasterGunnerLongest;
                InvincibleLongest = stat.AchievementsIdObject.Invincible;
                SurvivorLongest = stat.AchievementsIdObject.Survivor;

                #endregion

                #region [ SpecialAwards ]

                Kamikaze = stat.AchievementsIdObject.Kamikaze;
                Raider = stat.AchievementsIdObject.Raider;
                Bombardier = stat.AchievementsIdObject.Bombardier;
                Reaper = stat.AchievementsIdObject.Reaper;
                Sharpshooter = stat.AchievementsIdObject.SharpshooterLongest;
                Invincible = stat.AchievementsIdObject.Invincible;
                Survivor = stat.AchievementsIdObject.Survivor;
                MouseTrap = stat.AchievementsIdObject.MouseTrap;
                Hunter = stat.AchievementsIdObject.Hunter;
                Sinai = stat.AchievementsIdObject.Sinai;
                PattonValley = stat.AchievementsIdObject.PattonValley;
                Huntsman = stat.AchievementsIdObject.Huntsman;

                #endregion

                MedalMonolith = stat.AchievementsIdObject.MedalMonolith;
                MedalAntiSpgFire = stat.AchievementsIdObject.MedalAntiSpgFire;
                MedalGore = stat.AchievementsIdObject.MedalGore;
                MedalCoolBlood = stat.AchievementsIdObject.MedalCoolBlood;
                MedalStark = stat.AchievementsIdObject.MedalStark;
                DamageRating = stat.AchievementsIdObject.DamageRating;
            }
            #endregion
        }
    }
}