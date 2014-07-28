using System.Collections.Generic;
using System.Linq;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Logic.Adapter
{
    public class ClanBattlesStatAdapter : AbstractStatisticAdapter<PlayerStatisticEntity>, IRandomBattlesAchievements
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ClanBattlesStatAdapter(List<TankJson> tanks) : base(tanks, tank => tank.Clan)
        {
            #region [ BattleAwards ]

            Warrior = tanks.Sum(x => x.Achievements.Warrior);
            Invader = tanks.Sum(x => x.Achievements.Invader);
            Sniper = tanks.Sum(x => x.Achievements.Sniper);
            Sniper2 = tanks.Sum(x => x.Achievements.Sniper2);
            MainGun = tanks.Sum(x => x.Achievements.MainGun);
            Defender = tanks.Sum(x => x.Achievements.Defender);
            SteelWall = tanks.Sum(x => x.Achievements.SteelWall);
            Confederate = tanks.Sum(x => x.Achievements.Confederate);
            Scout = tanks.Sum(x => x.Achievements.Scout);
            PatrolDuty = tanks.Sum(x => x.Achievements.PatrolDuty);
            BrothersInArms = tanks.Sum(x => x.Achievements.BrothersInArms);
            CrucialContribution = tanks.Sum(x => x.Achievements.CrucialContribution);
            IronMan = tanks.Sum(x => x.Achievements.IronMan);
            LuckyDevil = tanks.Sum(x => x.Achievements.LuckyDevil);
            Sturdy = tanks.Sum(x => x.Achievements.Sturdy);

            #endregion

            #region [ Epic ]

            Boelter = tanks.Sum(x => x.Achievements.Boelter);
            RadleyWalters = tanks.Sum(x => x.Achievements.RadleyWalters);
            LafayettePool = tanks.Sum(x => x.Achievements.LafayettePool);
            Orlik = tanks.Sum(x => x.Achievements.Orlik);
            Oskin = tanks.Sum(x => x.Achievements.Oskin);
            Lehvaslaiho = tanks.Sum(x => x.Achievements.Lehvaslaiho);
            Nikolas = tanks.Sum(x => x.Achievements.Nikolas);
            Halonen = tanks.Sum(x => x.Achievements.Halonen);
            Burda = tanks.Sum(x => x.Achievements.Burda);
            Pascucci = tanks.Sum(x => x.Achievements.Pascucci);
            Dumitru = tanks.Sum(x => x.Achievements.Dumitru);
            TamadaYoshio = tanks.Sum(x => x.Achievements.TamadaYoshio);
            Billotte = tanks.Sum(x => x.Achievements.Billotte);
            BrunoPietro = tanks.Sum(x => x.Achievements.BrunoPietro);
            Tarczay = tanks.Sum(x => x.Achievements.Tarczay);
            Kolobanov = tanks.Sum(x => x.Achievements.Kolobanov);
            Fadin = tanks.Sum(x => x.Achievements.Fadin);
            HeroesOfRassenay = tanks.Sum(x => x.Achievements.HeroesOfRassenay);
            DeLanglade = tanks.Sum(x => x.Achievements.DeLanglade);

            #endregion

            #region [ Series ]

            SharpshooterLongest = tanks.Max(x => x.Achievements.SharpshooterLongest);
            MasterGunnerLongest = tanks.Max(x => x.Achievements.MasterGunnerLongest);

            #endregion

            #region [ SpecialAwards ]

            Kamikaze = tanks.Sum(x => x.Achievements.Kamikaze);
            Raider = tanks.Sum(x => x.Achievements.Raider);
            Bombardier = tanks.Sum(x => x.Achievements.Bombardier);
            Reaper = tanks.Max(x => x.Achievements.ReaperLongest);
            Invincible = tanks.Max(x => x.Achievements.InvincibleLongest);
            Survivor = tanks.Max(x => x.Achievements.SurvivorLongest);
            //count Maus frags
            MouseTrap = tanks.Sum(x => x.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count)) / 10;
            Hunter = tanks.Sum(x => x.Achievements.FragsBeast) / 100;
            Sinai = tanks.Sum(x => x.Achievements.FragsSinai) / 100;
            PattonValley = tanks.Sum(x => x.Achievements.FragsPatton) / 100;
            Huntsman = tanks.Sum(x => x.Achievements.Huntsman);

            #endregion

            MarksOnGun = tanks.Max(x => x.Achievements.MarksOnGun);
            MovingAvgDamage = (int) tanks.Average(x => x.Achievements.MovingAvgDamage);
            MedalMonolith = tanks.Sum(x => x.Achievements.MedalMonolith);
            MedalAntiSpgFire = tanks.Sum(x => x.Achievements.MedalAntiSpgFire);
            MedalGore = tanks.Sum(x => x.Achievements.MedalGore);
            MedalCoolBlood = tanks.Sum(x => x.Achievements.MedalCoolBlood);
            MedalStark = tanks.Sum(x => x.Achievements.MedalStark);
            DamageRating = tanks.Max(x => x.Achievements.DamageRating);

            MasterGunner = tanks.Sum(x => x.Achievements.MasterGunner);
            Alaric = tanks.Sum(x => x.Achievements.Alaric);
        }

        public List<ITankStatisticRow> Tanks { get; set; }

        #region Achievments

        #region [ BattleAwards ]

        public int Warrior { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Sniper2 { get; set; }

        public int MainGun { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int IronMan { get; set; }

        public int LuckyDevil { get; set; }

        public int Sturdy { get; set; }

        #endregion

        #region [ RowEpic ]

        public int Boelter { get; set; }

        public int RadleyWalters { get; set; }

        public int LafayettePool { get; set; }

        public int Orlik { get; set; }

        public int Oskin { get; set; }

        public int Lehvaslaiho { get; set; }

        public int Nikolas { get; set; }

        public int Halonen { get; set; }

        public int Burda { get; set; }

        public int Pascucci { get; set; }

        public int Dumitru { get; set; }

        public int TamadaYoshio { get; set; }

        public int Billotte { get; set; }

        public int BrunoPietro { get; set; }

        public int Tarczay { get; set; }

        public int Kolobanov { get; set; }

        public int Fadin { get; set; }

        public int HeroesOfRassenay { get; set; }

        public int DeLanglade { get; set; }

        #endregion

        #region [ SpecialAwards ]

        public int Kamikaze { get; set; }

        public int Raider { get; set; }

        public int Bombardier { get; set; }

        public int Reaper { get; set; }

        public int Sharpshooter { get; set; }

        public int Invincible { get; set; }

        public int Survivor { get; set; }

        public int MouseTrap { get; set; }

        public int Hunter { get; set; }

        public int Sinai { get; set; }

        public int PattonValley { get; set; }

        public int Huntsman { get; set; }

        #endregion

        #region [ Medals]

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

        #endregion

        #region [ Series ]

        public int ReaperLongest { get; set; }

        public int ReaperProgress { get; set; }

        public int SharpshooterLongest { get; set; }

        public int SharpshooterProgress { get; set; }

        public int MasterGunnerLongest { get; set; }

        public int MasterGunnerProgress { get; set; }

        public int InvincibleLongest { get; set; }

        public int InvincibleProgress { get; set; }

        public int SurvivorLongest { get; set; }

        public int SurvivorProgress { get; set; }

        #endregion

        public int MarksOnGun { get; set; }

        public int MovingAvgDamage { get; set; }

        public int MedalMonolith { get; set; }
        
        public int MedalAntiSpgFire { get; set; }
        
        public int MedalGore { get; set; }
        
        public int MedalCoolBlood { get; set; }
        
        public int MedalStark { get; set; }

        public int BattleHero { get; set; }

        public int DamageRating { get; set; }

        public int Lumberjack { get; set; }
        public int MasterGunner { get; set; }
        public int Alaric { get; set; }

        #endregion

        public override void Update(PlayerStatisticEntity entity)
        {
            base.Update(entity);

            if (entity.AchievementsIdObject == null)
            {
                entity.AchievementsIdObject = new PlayerAchievementsEntity();
            }

            Mapper.Map<IRandomBattlesAchievements>(this, entity.AchievementsIdObject);
        }
    }
}