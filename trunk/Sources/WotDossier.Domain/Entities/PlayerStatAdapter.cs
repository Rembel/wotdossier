using System;
using System.Collections.Generic;
using System.Linq;
using WotDossier.Common;
using WotDossier.Domain.Player;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Entities
{
    public class PlayerStatAdapter
    {
        private readonly List<TankJson> _tanks;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks)
        {
            _tanks = tanks;

            Battles_count = _tanks.Sum(x => x.Tankdata.battlesCount);
            Wins = _tanks.Sum(x => x.Tankdata.wins);
            Losses = _tanks.Sum(x => x.Tankdata.losses);
            Survived_battles = _tanks.Sum(x => x.Tankdata.survivedBattles);
            Xp = _tanks.Sum(x => x.Tankdata.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp/Battles_count;
            }
            Max_xp = _tanks.Max(x => x.Tankdata.maxXP);
            Frags = _tanks.Sum(x => x.Tankdata.frags);
            Spotted = _tanks.Sum(x => x.Tankdata.spotted);
            Hits_percents = (int)(_tanks.Sum(x => x.Tankdata.hits) / ((double)_tanks.Sum(x => x.Tankdata.shots)) * 100.0);
            Damage_dealt = _tanks.Sum(x => x.Tankdata.damageDealt);
            Capture_points = _tanks.Sum(x => x.Tankdata.capturePoints);
            Dropped_capture_points = _tanks.Sum(x => x.Tankdata.droppedCapturePoints);
            Updated = _tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier*x.Tankdata.battlesCount)/(double) Battles_count;
            }

            #region [ IRowBattleAwards ]

            Warrior = _tanks.Sum(x => x.Battle.warrior);
            Invader = _tanks.Sum(x => x.Battle.invader);
            Sniper = _tanks.Sum(x => x.Battle.sniper);
            Defender = _tanks.Sum(x => x.Battle.defender);
            SteelWall = _tanks.Sum(x => x.Battle.steelwall);
            Confederate = _tanks.Sum(x => x.Battle.supporter);
            Scout = _tanks.Sum(x => x.Battle.scout);
            PatrolDuty = _tanks.Sum(x => x.Battle.evileye);
            BrothersInArms = _tanks.Sum(x => x.Epic.BrothersInArms);
            CrucialContribution = _tanks.Sum(x => x.Epic.CrucialContribution);
            CoolHeaded = _tanks.Sum(x => x.Special.ironMan);
            LuckyDevil = _tanks.Sum(x => x.Special.luckyDevil);
            Spartan = _tanks.Sum(x => x.Special.sturdy);

            #endregion

            #region [ IRowEpic ]

            Boelter = _tanks.Sum(x => x.Epic.Boelter);
            RadleyWalters = _tanks.Sum(x => x.Epic.RadleyWalters);
            LafayettePool = _tanks.Sum(x => x.Epic.LafayettePool);
            Orlik = _tanks.Sum(x => x.Epic.Orlik);
            Oskin = _tanks.Sum(x => x.Epic.Oskin);
            Lehvaslaiho = _tanks.Sum(x => x.Epic.Lehvaslaiho);
            Nikolas = _tanks.Sum(x => x.Epic.Nikolas);
            Halonen = _tanks.Sum(x => x.Epic.Halonen);
            Burda = _tanks.Sum(x => x.Epic.Burda);
            Pascucci = _tanks.Sum(x => x.Epic.Pascucci);
            Dumitru = _tanks.Sum(x => x.Epic.Dumitru);
            TamadaYoshio = _tanks.Sum(x => x.Epic.TamadaYoshio);
            Billotte = _tanks.Sum(x => x.Epic.Billotte);
            BrunoPietro = _tanks.Sum(x => x.Epic.BrunoPietro);
            Tarczay = _tanks.Sum(x => x.Epic.Tarczay);
            Kolobanov = _tanks.Sum(x => x.Epic.Kolobanov);
            Fadin = _tanks.Sum(x => x.Epic.Fadin);
            HeroesOfRassenay = _tanks.Sum(x => x.Special.heroesOfRassenay);
            DeLanglade = _tanks.Sum(x => x.Epic.DeLanglade);

            #endregion

            #region [ IRowMedals]

            //Kay = stat.data.achievements.medalKay;
            //Carius = stat.data.achievements.medalCarius;
            //Knispel = stat.data.achievements.medalKnispel;
            //Poppel = stat.data.achievements.medalPoppel;
            //Abrams = stat.data.achievements.medalAbrams;
            //Leclerk = stat.data.achievements.medalLeClerc;
            //Lavrinenko = stat.data.achievements.medalLavrinenko;
            //Ekins = stat.data.achievements.medalEkins;

            #endregion

            #region [ IRowSeries ]

            SharpshooterLongest = _tanks.Max(x => x.Series.maxSniperSeries);
            MasterGunnerLongest = _tanks.Max(x => x.Series.maxPiercingSeries);

            #endregion

            #region [ IRowSpecialAwards ]

            Kamikaze = _tanks.Sum(x => x.Special.kamikaze);
            Raider = _tanks.Sum(x => x.Special.raider);
            Bombardier = _tanks.Sum(x => x.Special.bombardier);
            Reaper = _tanks.Max(x => x.Series.maxKillingSeries);
            Invincible = _tanks.Max(x => x.Series.maxInvincibleSeries);
            Survivor = _tanks.Max(x => x.Series.maxDiehardSeries);
            MouseTrap = _tanks.Sum(x => x.Special.mousebane);
            Hunter = _tanks.Sum(x => x.Tankdata.fragsBeast)/100;
            Sinai = _tanks.Sum(x => x.Battle.fragsSinai)/100;
            PattonValley = _tanks.Sum(x => x.Special.fragsPatton)/100;
            Ranger = _tanks.Sum(x => x.Special.huntsman);

            #endregion
        }

        public PlayerStatAdapter(PlayerStat stat)
        {
            Battles_count = stat.data.summary.Battles_count;
            Wins = stat.data.summary.Wins;
            Losses = stat.data.summary.Losses;
            Survived_battles = stat.data.summary.Survived_battles;
            Xp = stat.data.experience.Xp;
            Battle_avg_xp = stat.data.experience.Battle_avg_xp;
            Max_xp = stat.data.experience.Max_xp;
            Frags = stat.data.battles.Frags;
            Spotted = stat.data.battles.Spotted;
            Hits_percents = stat.data.battles.Hits_percents;
            Damage_dealt = stat.data.battles.Damage_dealt;
            Capture_points = stat.data.battles.Capture_points;
            Dropped_capture_points = stat.data.battles.Dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.data.updated_at).ToLocalTime();
            if (Battles_count > 0)
            {
                AvgLevel = stat.data.vehicles.Sum(x => x.level*x.battle_count)/(double) stat.data.summary.Battles_count;
            }

            #region [ IRowBattleAwards ]

            Warrior = stat.data.achievements.warrior;
            Invader = stat.data.achievements.invader;
            Sniper = stat.data.achievements.sniper;
            Defender = stat.data.achievements.defende;
            SteelWall = stat.data.achievements.steelwall;
            Confederate = stat.data.achievements.supporter;
            Scout = stat.data.achievements.scout;
            PatrolDuty = stat.data.achievements.evileye;
            BrothersInArms = stat.data.achievements.medalBrothersInArms;
            CrucialContribution = stat.data.achievements.medalCrucialContribution;
            CoolHeaded = stat.data.achievements.ironMan;
            LuckyDevil = stat.data.achievements.luckyDevil;
            Spartan = stat.data.achievements.sturdy;

            #endregion

            #region [ IRowEpic ]

            Boelter = stat.data.achievements.medalBoelter;
            RadleyWalters = stat.data.achievements.medalRadleyWalters;
            LafayettePool = stat.data.achievements.medalLafayettePool;
            Orlik = stat.data.achievements.medalOrlik;
            Oskin = stat.data.achievements.medalOskin;
            Lehvaslaiho = stat.data.achievements.medalLehvaslaiho;
            Nikolas = stat.data.achievements.medalNikolas;
            Halonen = stat.data.achievements.medalHalonen;
            Burda = stat.data.achievements.medalBurda;
            Pascucci = stat.data.achievements.medalPascucci;
            Dumitru = stat.data.achievements.medalDumitru;
            TamadaYoshio = stat.data.achievements.medalTamadaYoshio;
            Billotte = stat.data.achievements.medalBillotte;
            BrunoPietro = stat.data.achievements.medalBrunoPietro;
            Tarczay = stat.data.achievements.medalTarczay;
            Kolobanov = stat.data.achievements.medalKolobanov;
            Fadin = stat.data.achievements.medalFadin;
            HeroesOfRassenay = stat.data.achievements.heroesOfRassenay;
            DeLanglade = stat.data.achievements.medalDeLanglade;

            #endregion

            #region [ IRowMedals]

            Kay = stat.data.achievements.medalKay;
            Carius = stat.data.achievements.medalCarius;
            Knispel = stat.data.achievements.medalKnispel;
            Poppel = stat.data.achievements.medalPoppel;
            Abrams = stat.data.achievements.medalAbrams;
            Leclerk = stat.data.achievements.medalLeClerc;
            Lavrinenko = stat.data.achievements.medalLavrinenko;
            Ekins = stat.data.achievements.medalEkins;

            #endregion

            #region [ IRowSeries ]

            SharpshooterLongest = stat.data.achievements.maxSniperSeries;
            MasterGunnerLongest = stat.data.achievements.maxPiercingSeries;

            #endregion

            #region [ IRowSpecialAwards ]

            Kamikaze = stat.data.achievements.kamikaze;
            Raider = stat.data.achievements.raider;
            Bombardier = stat.data.achievements.bombardier;
            Reaper = stat.data.achievements.maxKillingSeries;
            Invincible = stat.data.achievements.maxInvincibleSeries;
            Survivor = stat.data.achievements.maxDiehardSeries;
            MouseTrap = stat.data.achievements.mousebane;
            Hunter = stat.data.achievements.beasthunter;
            Sinai = stat.data.achievements.sinai;
            PattonValley = stat.data.achievements.pattonValley;
            Ranger = stat.data.achievements.huntsman;

            #endregion
        }

        public int Battles_count { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Survived_battles { get; set; }

        public int Xp { get; set; }

        public int Battle_avg_xp { get; set; }

        public int Max_xp { get; set; }

        public int Frags { get; set; }

        public int Spotted { get; set; }

        public int Hits_percents { get; set; }

        public int Damage_dealt { get; set; }

        public int Capture_points { get; set; }

        public int Dropped_capture_points { get; set; }

        public DateTime Updated { get; set; }

        public double AvgLevel { get; set; }

        #region Achievments

        #region [ ITankRowBattleAwards ]

        public int Warrior { get; set; }

        public int Invader { get; set; }

        public int Sniper { get; set; }

        public int Defender { get; set; }

        public int SteelWall { get; set; }

        public int Confederate { get; set; }

        public int Scout { get; set; }

        public int PatrolDuty { get; set; }

        public int BrothersInArms { get; set; }

        public int CrucialContribution { get; set; }

        public int CoolHeaded { get; set; }

        public int LuckyDevil { get; set; }

        public int Spartan { get; set; }

        public int Jager { get; set; }

        #endregion

        #region [ ITankRowEpic ]

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

        #region [ ITankRowSpecialAwards ]

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

        public int Ranger { get; set; }

        #endregion

        #region [ ITankRowMedals]

        public int Kay { get; set; }

        public int Carius { get; set; }

        public int Knispel { get; set; }

        public int Poppel { get; set; }

        public int Abrams { get; set; }

        public int Leclerk { get; set; }

        public int Lavrinenko { get; set; }

        public int Ekins { get; set; }

        #endregion

        #region [ ITankRowSeries ]

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

        #endregion
    }
}