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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PlayerStatAdapter(List<TankJson> tanks)
        {
            Battles_count = tanks.Sum(x => x.A15x15.battlesCount);
            Wins = tanks.Sum(x => x.A15x15.wins);
            Losses = tanks.Sum(x => x.A15x15.losses);
            Survived_battles = tanks.Sum(x => x.A15x15.survivedBattles);
            Xp = tanks.Sum(x => x.A15x15.xp);
            if (Battles_count > 0)
            {
                Battle_avg_xp = Xp / (double)Battles_count;
            }
            Max_xp = tanks.Max(x => x.A15x15.maxXP);
            Frags = tanks.Sum(x => x.A15x15.frags);
            Spotted = tanks.Sum(x => x.A15x15.spotted);
            Hits_percents = tanks.Sum(x => x.A15x15.hits) / ((double)tanks.Sum(x => x.A15x15.shots)) * 100.0;
            Damage_dealt = tanks.Sum(x => x.A15x15.damageDealt);
            Damage_taken = tanks.Sum(x => x.A15x15.damageReceived);
            Capture_points = tanks.Sum(x => x.A15x15.capturePoints);
            Dropped_capture_points = tanks.Sum(x => x.A15x15.droppedCapturePoints);
            Updated = tanks.Max(x => x.Common.lastBattleTimeR);
            if (Battles_count > 0)
            {
                AvgLevel = tanks.Sum(x => x.Common.tier * x.A15x15.battlesCount) / (double)Battles_count;
            }

            #region [ IRowBattleAwards ]

            Warrior = tanks.Sum(x => x.Achievements.warrior);
            Invader = tanks.Sum(x => x.Achievements.invader);
            Sniper = tanks.Sum(x => x.Achievements.sniper);
            Sniper2 = tanks.Sum(x => x.Achievements.sniper2);
            MainGun = tanks.Sum(x => x.Achievements.mainGun);
            Defender = tanks.Sum(x => x.Achievements.defender);
            SteelWall = tanks.Sum(x => x.Achievements.steelwall);
            Confederate = tanks.Sum(x => x.Achievements.supporter);
            Scout = tanks.Sum(x => x.Achievements.scout);
            PatrolDuty = tanks.Sum(x => x.Achievements.evileye);
            BrothersInArms = tanks.Sum(x => x.Achievements.medalBrothersInArms);
            CrucialContribution = tanks.Sum(x => x.Achievements.medalCrucialContribution);
            CoolHeaded = tanks.Sum(x => x.Achievements.ironMan);
            LuckyDevil = tanks.Sum(x => x.Achievements.luckyDevil);
            Spartan = tanks.Sum(x => x.Achievements.sturdy);

            #endregion

            #region [ IRowEpic ]

            Boelter = tanks.Sum(x => x.Achievements.medalWittmann);
            RadleyWalters = tanks.Sum(x => x.Achievements.medalRadleyWalters);
            LafayettePool = tanks.Sum(x => x.Achievements.medalLafayettePool);
            Orlik = tanks.Sum(x => x.Achievements.medalOrlik);
            Oskin = tanks.Sum(x => x.Achievements.medalOskin);
            Lehvaslaiho = tanks.Sum(x => x.Achievements.medalLehvaslaiho);
            Nikolas = tanks.Sum(x => x.Achievements.medalNikolas);
            Halonen = tanks.Sum(x => x.Achievements.medalHalonen);
            Burda = tanks.Sum(x => x.Achievements.medalBurda);
            Pascucci = tanks.Sum(x => x.Achievements.medalPascucci);
            Dumitru = tanks.Sum(x => x.Achievements.medalDumitru);
            TamadaYoshio = tanks.Sum(x => x.Achievements.medalTamadaYoshio);
            Billotte = tanks.Sum(x => x.Achievements.medalBillotte);
            BrunoPietro = tanks.Sum(x => x.Achievements.medalBrunoPietro);
            Tarczay = tanks.Sum(x => x.Achievements.medalTarczay);
            Kolobanov = tanks.Sum(x => x.Achievements.medalKolobanov);
            Fadin = tanks.Sum(x => x.Achievements.medalFadin);
            HeroesOfRassenay = tanks.Sum(x => x.Achievements.heroesOfRassenay);
            DeLanglade = tanks.Sum(x => x.Achievements.medalDeLanglade);

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

            SharpshooterLongest = tanks.Max(x => x.Achievements.maxSniperSeries);
            MasterGunnerLongest = tanks.Max(x => x.Achievements.maxPiercingSeries);

            #endregion

            #region [ IRowSpecialAwards ]

            Kamikaze = tanks.Sum(x => x.Achievements.kamikaze);
            Raider = tanks.Sum(x => x.Achievements.raider);
            Bombardier = tanks.Sum(x => x.Achievements.bombardier);
            Reaper = tanks.Max(x => x.Achievements.maxKillingSeries);
            Invincible = tanks.Max(x => x.Achievements.maxInvincibleSeries);
            Survivor = tanks.Max(x => x.Achievements.maxDiehardSeries);
            //count Maus frags
            MouseTrap = tanks.Sum(x => x.Frags.Where(f => f.TankUniqueId == 10027).Sum(s => s.Count)) / 10;
            Hunter = tanks.Sum(x => x.Achievements.fragsBeast) / 100;
            Sinai = tanks.Sum(x => x.Achievements.fragsSinai) / 100;
            PattonValley = tanks.Sum(x => x.Achievements.fragsPatton) / 100;
            Ranger = tanks.Sum(x => x.Achievements.huntsman);

            #endregion


        }

        public PlayerStatAdapter(PlayerStat stat)
        {
            Battles_count = stat.dataField.statistics.all.battles;
            Wins = stat.dataField.statistics.all.wins;
            Losses = stat.dataField.statistics.all.losses;
            Survived_battles = stat.dataField.statistics.all.survived_battles;
            Xp = stat.dataField.statistics.all.xp;
            Battle_avg_xp = stat.dataField.statistics.all.battle_avg_xp;
            Max_xp = stat.dataField.statistics.max_xp;
            Frags = stat.dataField.statistics.all.frags;
            Spotted = stat.dataField.statistics.all.spotted;
            Hits_percents = stat.dataField.statistics.all.hits_percents;
            Damage_dealt = stat.dataField.statistics.all.damage_dealt;
            Capture_points = stat.dataField.statistics.all.capture_points;
            Dropped_capture_points = stat.dataField.statistics.all.dropped_capture_points;
            Updated = Utils.UnixDateToDateTime((long)stat.dataField.updated_at).ToLocalTime();
            Created = Utils.UnixDateToDateTime((long)stat.dataField.created_at).ToLocalTime();
            if (Battles_count > 0 && stat.dataField.vehicles != null)
            {
                int battlesCount = stat.dataField.vehicles.Sum(x => x.statistics.all.battles);
                if (battlesCount > 0)
                {
                    AvgLevel = stat.dataField.vehicles.Sum(x => (x.tank != null ? x.tank.level : 1)*x.statistics.all.battles)/(double) battlesCount;
                }
            }

            #region [ IRowBattleAwards ]

            Warrior = stat.dataField.achievements.warrior;
            Invader = stat.dataField.achievements.invader;
            Sniper = stat.dataField.achievements.sniper;
            Defender = stat.dataField.achievements.defender;
            SteelWall = stat.dataField.achievements.steelwall;
            Confederate = stat.dataField.achievements.supporter;
            Scout = stat.dataField.achievements.scout;
            PatrolDuty = stat.dataField.achievements.evileye;
            BrothersInArms = stat.dataField.achievements.medal_brothers_in_arms;
            CrucialContribution = stat.dataField.achievements.medal_crucial_contribution;
            CoolHeaded = stat.dataField.achievements.iron_man;
            LuckyDevil = stat.dataField.achievements.lucky_devil;
            Spartan = stat.dataField.achievements.sturdy;

            #endregion

            #region [ IRowEpic ]

            Boelter = stat.dataField.achievements.medal_boelter;
            RadleyWalters = stat.dataField.achievements.medal_radley_walters;
            LafayettePool = stat.dataField.achievements.medal_lafayette_pool;
            Orlik = stat.dataField.achievements.medal_orlik;
            Oskin = stat.dataField.achievements.medal_oskin;
            Lehvaslaiho = stat.dataField.achievements.medal_lehvaslaiho;
            Nikolas = stat.dataField.achievements.medal_nikolas;
            Halonen = stat.dataField.achievements.medal_halonen;
            Burda = stat.dataField.achievements.medal_burda;
            Pascucci = stat.dataField.achievements.medal_pascucci;
            Dumitru = stat.dataField.achievements.medal_dumitru;
            TamadaYoshio = stat.dataField.achievements.medal_tamada_yoshio;
            Billotte = stat.dataField.achievements.medal_billotte;
            BrunoPietro = stat.dataField.achievements.medal_bruno_pietro;
            Tarczay = stat.dataField.achievements.medal_tarczay;
            Kolobanov = stat.dataField.achievements.medal_kolobanov;
            Fadin = stat.dataField.achievements.medal_fadin;
            HeroesOfRassenay = stat.dataField.achievements.medal_heroes_of_rassenay;
            DeLanglade = stat.dataField.achievements.medal_delanglade;

            #endregion

            #region [ IRowMedals]

            Kay = stat.dataField.achievements.medal_kay;
            Carius = stat.dataField.achievements.medal_carius;
            Knispel = stat.dataField.achievements.medal_knispel;
            Poppel = stat.dataField.achievements.medal_poppel;
            Abrams = stat.dataField.achievements.medal_abrams;
            Leclerk = stat.dataField.achievements.medal_le_clerc;
            Lavrinenko = stat.dataField.achievements.medal_lavrinenko;
            Ekins = stat.dataField.achievements.medal_ekins;

            #endregion

            #region [ IRowSeries ]

            SharpshooterLongest = stat.dataField.achievements.max_sniper_series;
            MasterGunnerLongest = stat.dataField.achievements.max_piercing_series;

            #endregion

            #region [ IRowSpecialAwards ]

            Kamikaze = stat.dataField.achievements.kamikaze;
            Raider = stat.dataField.achievements.raider;
            Bombardier = stat.dataField.achievements.bombardier;
            Reaper = stat.dataField.achievements.max_killing_series;
            Invincible = stat.dataField.achievements.max_invincible_series;
            Survivor = stat.dataField.achievements.max_diehard_series;
            MouseTrap = stat.dataField.achievements.mousebane;
            Hunter = stat.dataField.achievements.beasthunter;
            Sinai = stat.dataField.achievements.sinai;
            PattonValley = stat.dataField.achievements.patton_valley;
            Ranger = stat.dataField.achievements.huntsman;

            #endregion

            Vehicle = stat.dataField.vehicles;
        }

        public List<VehicleStat> Vehicle { get; set; }

        public DateTime Created { get; set; }

        public int Battles_count { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Survived_battles { get; set; }

        public int Xp { get; set; }

        public double Battle_avg_xp { get; set; }

        public int Max_xp { get; set; }

        public int Frags { get; set; }

        public int Spotted { get; set; }

        public double Hits_percents { get; set; }

        public int Damage_dealt { get; set; }

        public int Damage_taken { get; set; }

        public int Capture_points { get; set; }

        public int Dropped_capture_points { get; set; }

        public DateTime Updated { get; set; }

        public double AvgLevel { get; set; }

        #region Achievments

        #region [ ITankRowBattleAwards ]

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