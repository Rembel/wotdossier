using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Dossier.TankV29;
using WotDossier.Domain.Dossier.TankV65;
using WotDossier.Domain.Dossier.TankV77;
using WotDossier.Domain.Dossier.TankV85;
using WotDossier.Domain.Server;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    public class DataMapper
    {
        /// <summary>
        /// Converts the specified tank json from local cache with structure version less then or equals 29.
        /// </summary>
        /// <param name="tankJson">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(TankJson29 tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson.Tankdata.battlesCount,
                battlesCountBefore8_8 =
                    tankJson.Common.basedonversion > 28
                        ? tankJson.Tankdata.battlesCountBefore8_8
                        : tankJson.Tankdata.battlesCount,
                capturePoints = tankJson.Tankdata.capturePoints,
                damageDealt = tankJson.Tankdata.damageDealt,
                damageReceived = tankJson.Tankdata.damageReceived,
                droppedCapturePoints = tankJson.Tankdata.droppedCapturePoints,
                frags = tankJson.Tankdata.frags,
                frags8p = tankJson.Tankdata.frags8p,
                hits = tankJson.Tankdata.hits,
                losses = tankJson.Tankdata.losses,
                shots = tankJson.Tankdata.shots,
                spotted = tankJson.Tankdata.spotted,
                survivedBattles = tankJson.Tankdata.survivedBattles,
                winAndSurvived = tankJson.Tankdata.winAndSurvived,
                wins = tankJson.Tankdata.wins,
                xp = tankJson.Tankdata.xp,
                xpBefore8_8 = tankJson.Common.basedonversion > 28 ? tankJson.Tankdata.xpBefore8_8 : tankJson.Tankdata.xp,
                originalXP = tankJson.Tankdata.originalXP,
                damageAssistedRadio = tankJson.Tankdata.damageAssistedRadio,
                damageAssistedTrack = tankJson.Tankdata.damageAssistedTrack,
                shotsReceived = tankJson.Tankdata.shotsReceived,
                noDamageShotsReceived = tankJson.Tankdata.noDamageShotsReceived,
                piercedReceived = tankJson.Tankdata.piercedReceived,
                heHitsReceived = tankJson.Tankdata.heHitsReceived,
                he_hits = tankJson.Tankdata.he_hits,
                pierced = tankJson.Tankdata.pierced,
                maxFrags = tankJson.Tankdata.maxFrags,
                maxXP = tankJson.Tankdata.maxXP
            };

            //v2.A15x15.maxDamage = ;

            v2.FragsList = tankJson.Kills;

            //achievements 15x15
            v2.Achievements = new AchievementsJson
            {
                MasterGunner = tankJson.Special.armorPiercer,
                BattleHero = tankJson.Battle.battleHeroes,
                Hunter = tankJson.Special.beasthunter,
                Bombardier = tankJson.Special.bombardier,
                Defender = tankJson.Battle.defender,
                Survivor = tankJson.Special.diehard,
                SurvivorProgress = tankJson.Series.diehardSeries,
                PatrolDuty = tankJson.Battle.evileye,
                FragsBeast = tankJson.Tankdata.fragsBeast,
                FragsPatton = tankJson.Special.fragsPatton,
                FragsSinai = tankJson.Battle.fragsSinai,
                Reaper = tankJson.Special.handOfDeath,
                HeroesOfRassenay = tankJson.Special.heroesOfRassenay,
                Huntsman = tankJson.Special.huntsman,
                Invader = tankJson.Battle.invader,
                Invincible = tankJson.Special.invincible,
                InvincibleProgress = tankJson.Series.invincibleSeries,
                IronMan = tankJson.Special.ironMan,
                Kamikaze = tankJson.Special.kamikaze,
                ReaperProgress = tankJson.Series.killingSeries,
                LuckyDevil = tankJson.Special.luckyDevil,
                Lumberjack = tankJson.Special.lumberjack,
                SurvivorLongest = tankJson.Series.maxDiehardSeries,
                InvincibleLongest = tankJson.Series.maxInvincibleSeries,
                ReaperLongest = tankJson.Series.maxKillingSeries,
                MasterGunnerLongest = tankJson.Series.maxPiercingSeries,
                SharpshooterLongest = tankJson.Series.maxSniperSeries,
                Abrams = tankJson.Major.Abrams,
                Billotte = tankJson.Epic.Billotte,
                BrothersInArms = tankJson.Epic.BrothersInArms,
                BrunoPietro = tankJson.Epic.BrunoPietro,
                Burda = tankJson.Epic.Burda,
                Carius = tankJson.Major.Carius,
                CrucialContribution = tankJson.Epic.CrucialContribution,
                DeLanglade = tankJson.Epic.DeLanglade,
                Dumitru = tankJson.Epic.Dumitru,
                Ekins = tankJson.Major.Ekins,
                Fadin = tankJson.Epic.Fadin,
                Halonen = tankJson.Epic.Halonen,
                Kay = tankJson.Major.Kay,
                Knispel = tankJson.Major.Knispel,
                Kolobanov = tankJson.Epic.Kolobanov,
                LafayettePool = tankJson.Epic.LafayettePool,
                Lavrinenko = tankJson.Major.Lavrinenko,
                Leclerk = tankJson.Major.LeClerc,
                Lehvaslaiho = tankJson.Epic.Lehvaslaiho,
                Nikolas = tankJson.Epic.Nikolas,
                Orlik = tankJson.Epic.Orlik,
                Oskin = tankJson.Epic.Oskin,
                Pascucci = tankJson.Epic.Pascucci,
                Poppel = tankJson.Major.Poppel,
                RadleyWalters = tankJson.Epic.RadleyWalters,
                TamadaYoshio = tankJson.Epic.TamadaYoshio,
                Tarczay = tankJson.Epic.Tarczay,
                Boelter = tankJson.Epic.Boelter,
                MouseTrap = tankJson.Special.mousebane,
                PattonValley = tankJson.Special.pattonValley,
                MasterGunnerProgress = tankJson.Series.piercingSeries,
                Raider = tankJson.Special.raider,
                Scout = tankJson.Battle.scout,
                Sinai = tankJson.Special.sinai,
                Sniper = tankJson.Battle.sniper,
                SharpshooterProgress = tankJson.Series.sniperSeries,
                SteelWall = tankJson.Battle.steelwall,
                Sturdy = tankJson.Special.sturdy,
                Confederate = tankJson.Battle.supporter,
                TankExpertStrg = tankJson.Special.tankExpertStrg,
                Sharpshooter = tankJson.Special.titleSniper,
                Warrior = tankJson.Battle.warrior,
                MarkOfMastery = tankJson.Special.markOfMastery
            };

            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson
                {
                    battlesCount = tankJson.Clan.battlesCount,
                    capturePoints = tankJson.Clan.capturePoints,
                    damageDealt = tankJson.Clan.damageDealt,
                    damageReceived = tankJson.Clan.damageReceived,
                    droppedCapturePoints = tankJson.Clan.droppedCapturePoints,
                    frags = tankJson.Clan.frags,
                    hits = tankJson.Clan.hits,
                    losses = tankJson.Clan.losses,
                    shots = tankJson.Clan.shots,
                    spotted = tankJson.Clan.spotted,
                    survivedBattles = tankJson.Clan.survivedBattles,
                    wins = tankJson.Clan.wins,
                    xp = tankJson.Clan.xp
                };
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson
                {
                    battlesCount = tankJson.Company.battlesCount,
                    capturePoints = tankJson.Company.capturePoints,
                    damageDealt = tankJson.Company.damageDealt,
                    damageReceived = tankJson.Company.damageReceived,
                    droppedCapturePoints = tankJson.Company.droppedCapturePoints,
                    frags = tankJson.Company.frags,
                    hits = tankJson.Company.hits,
                    losses = tankJson.Company.losses,
                    shots = tankJson.Company.shots,
                    spotted = tankJson.Company.spotted,
                    survivedBattles = tankJson.Company.survivedBattles,
                    wins = tankJson.Company.wins,
                    xp = tankJson.Company.xp
                };
            }

            v2.Common = new CommonJson
            {
                basedonversion = tankJson.Common.basedonversion,
                compactDescr = tankJson.Common.compactDescr,
                countryid = tankJson.Common.countryid,
                creationTime = tankJson.Common.creationTime,
                creationTimeR = tankJson.Common.creationTimeR,
                frags = tankJson.Common.frags,
                frags_compare = tankJson.Common.frags_compare,
                has_15x15 = 1,
                has_7x7 = 0,
                has_clan = tankJson.Clan != null ? 1 : 0,
                has_company = tankJson.Company != null ? 1 : 0,
                lastBattleTime = tankJson.Common.lastBattleTime,
                lastBattleTimeR = tankJson.Common.lastBattleTimeR,
                premium = tankJson.Common.premium,
                tankid = tankJson.Common.tankid,
                tanktitle = tankJson.Common.tanktitle,
                tier = tankJson.Common.tier,
                type = tankJson.Common.type,
                updated = tankJson.Common.updated,
                updatedR = Utils.UnixDateToDateTime(tankJson.Common.updated),
                battleLifeTime = tankJson.Tankdata.battleLifeTime,
                mileage = tankJson.Tankdata.mileage,
                treesCut = tankJson.Tankdata.treesCut
            };

            return v2;
        }

        /// <summary>
        /// Converts the specified tank json from local cache with structure version less then or equals 65.
        /// </summary>
        /// <param name="tankJson">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(TankJson65 tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson.A15x15.battlesCount,
                capturePoints = tankJson.A15x15.capturePoints,
                damageDealt = tankJson.A15x15.damageDealt,
                damageReceived = tankJson.A15x15.damageReceived,
                droppedCapturePoints = tankJson.A15x15.droppedCapturePoints,
                frags = tankJson.A15x15.frags,
                frags8p = tankJson.A15x15.frags8p,
                hits = tankJson.A15x15.hits,
                losses = tankJson.A15x15.losses,
                shots = tankJson.A15x15.shots,
                spotted = tankJson.A15x15.spotted,
                survivedBattles = tankJson.A15x15.survivedBattles,
                winAndSurvived = tankJson.A15x15.winAndSurvived,
                wins = tankJson.A15x15.wins,
                xp = tankJson.A15x15.xp,
                battlesCountBefore8_8 = tankJson.A15x15.battlesCountBefore8_8,
                xpBefore8_8 = tankJson.A15x15.xpBefore8_8,
                originalXP = tankJson.A15x15_2.originalXP,
                damageAssistedRadio = tankJson.A15x15_2.damageAssistedRadio,
                damageAssistedTrack = tankJson.A15x15_2.damageAssistedTrack,
                shotsReceived = tankJson.A15x15_2.shotsReceived,
                noDamageShotsReceived = tankJson.A15x15_2.noDamageShotsReceived,
                piercedReceived = tankJson.A15x15_2.piercedReceived,
                heHitsReceived = tankJson.A15x15_2.heHitsReceived,
                he_hits = tankJson.A15x15_2.he_hits,
                pierced = tankJson.A15x15_2.pierced,
                maxDamage = tankJson.Max15x15.maxDamage,
                maxFrags = tankJson.Max15x15.maxFrags,
                maxXP = tankJson.Max15x15.maxXP
            };

            v2.FragsList = tankJson.FragsList;

            //achievements 15x15
            v2.Achievements = new AchievementsJson
            {
                MasterGunner = tankJson.Achievements.armorPiercer,
                BattleHero = tankJson.Achievements.battleHeroes,
                Hunter = tankJson.Achievements.beasthunter,
                Bombardier = tankJson.Achievements.bombardier,
                Defender = tankJson.Achievements.defender,
                Survivor = tankJson.Achievements.diehard,
                SurvivorProgress = tankJson.Achievements.diehardSeries,
                PatrolDuty = tankJson.Achievements.evileye,
                FragsBeast = tankJson.Achievements.fragsBeast,
                FragsPatton = tankJson.Achievements.fragsPatton,
                FragsSinai = tankJson.Achievements.fragsSinai,
                Reaper = tankJson.Achievements.handOfDeath,
                HeroesOfRassenay = tankJson.Achievements.heroesOfRassenay,
                Huntsman = tankJson.Achievements.huntsman,
                Invader = tankJson.Achievements.invader,
                Invincible = tankJson.Achievements.invincible,
                InvincibleProgress = tankJson.Achievements.invincibleSeries,
                IronMan = tankJson.Achievements.ironMan,
                Kamikaze = tankJson.Achievements.kamikaze,
                ReaperProgress = tankJson.Achievements.killingSeries,
                LuckyDevil = tankJson.Achievements.luckyDevil,
                Lumberjack = tankJson.Achievements.lumberjack,
                SurvivorLongest = tankJson.Achievements.maxDiehardSeries,
                InvincibleLongest = tankJson.Achievements.maxInvincibleSeries,
                ReaperLongest = tankJson.Achievements.maxKillingSeries,
                MasterGunnerLongest = tankJson.Achievements.maxPiercingSeries,
                SharpshooterLongest = tankJson.Achievements.maxSniperSeries,
                Abrams = tankJson.Achievements.medalAbrams,
                Billotte = tankJson.Achievements.medalBillotte,
                BrothersInArms = tankJson.Achievements.medalBrothersInArms,
                BrunoPietro = tankJson.Achievements.medalBrunoPietro,
                Burda = tankJson.Achievements.medalBurda,
                Carius = tankJson.Achievements.medalCarius,
                CrucialContribution = tankJson.Achievements.medalCrucialContribution,
                DeLanglade = tankJson.Achievements.medalDeLanglade,
                Dumitru = tankJson.Achievements.medalDumitru,
                Ekins = tankJson.Achievements.medalEkins,
                Fadin = tankJson.Achievements.medalFadin,
                Halonen = tankJson.Achievements.medalHalonen,
                Kay = tankJson.Achievements.medalKay,
                Knispel = tankJson.Achievements.medalKnispel,
                Kolobanov = tankJson.Achievements.medalKolobanov,
                LafayettePool = tankJson.Achievements.medalLafayettePool,
                Lavrinenko = tankJson.Achievements.medalLavrinenko,
                Leclerk = tankJson.Achievements.medalLeClerc,
                Lehvaslaiho = tankJson.Achievements.medalLehvaslaiho,
                Nikolas = tankJson.Achievements.medalNikolas,
                Orlik = tankJson.Achievements.medalOrlik,
                Oskin = tankJson.Achievements.medalOskin,
                Pascucci = tankJson.Achievements.medalPascucci,
                Poppel = tankJson.Achievements.medalPoppel,
                RadleyWalters = tankJson.Achievements.medalRadleyWalters,
                TamadaYoshio = tankJson.Achievements.medalTamadaYoshio,
                Tarczay = tankJson.Achievements.medalTarczay,
                Boelter = tankJson.Achievements.medalWittmann,
                MouseTrap = tankJson.Achievements.mousebane,
                PattonValley = tankJson.Achievements.pattonValley,
                MasterGunnerProgress = tankJson.Achievements.piercingSeries,
                Raider = tankJson.Achievements.raider,
                Scout = tankJson.Achievements.scout,
                Sinai = tankJson.Achievements.sinai,
                Sniper = tankJson.Achievements.sniper,
                Sniper2 = tankJson.Achievements.sniper2,
                MainGun = tankJson.Achievements.mainGun,
                SharpshooterProgress = tankJson.Achievements.sniperSeries,
                SteelWall = tankJson.Achievements.steelwall,
                Sturdy = tankJson.Achievements.sturdy,
                Confederate = tankJson.Achievements.supporter,
                TankExpertStrg = tankJson.Achievements.tankExpertStrg,
                Sharpshooter = tankJson.Achievements.titleSniper,
                Warrior = tankJson.Achievements.warrior,
                MarkOfMastery = tankJson.Achievements.markOfMastery
            };

            //achievements 7x7
            v2.Achievements7x7 = new Achievements7x7
            {
                ArmoredFist = tankJson.Achievements7X7.armoredFist,
                GeniusForWar = tankJson.Achievements7X7.geniusForWar,
                GeniusForWarMedal = tankJson.Achievements7X7.geniusForWarMedal,
                KingOfTheHill = tankJson.Achievements7X7.kingOfTheHill,
                MaxTacticalBreakthroughSeries = tankJson.Achievements7X7.maxTacticalBreakthroughSeries,
                TacticalBreakthrough = tankJson.Achievements7X7.tacticalBreakthrough,
                TacticalBreakthroughSeries = tankJson.Achievements7X7.tacticalBreakthroughSeries,
                WolfAmongSheep = tankJson.Achievements7X7.wolfAmongSheep,
                WolfAmongSheepMedal = tankJson.Achievements7X7.wolfAmongSheepMedal
            };

            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson
                {
                    battlesCount = tankJson.Clan.battlesCount,
                    capturePoints = tankJson.Clan.capturePoints,
                    damageDealt = tankJson.Clan.damageDealt,
                    damageReceived = tankJson.Clan.damageReceived,
                    droppedCapturePoints = tankJson.Clan.droppedCapturePoints,
                    frags = tankJson.Clan.frags,
                    hits = tankJson.Clan.hits,
                    losses = tankJson.Clan.losses,
                    shots = tankJson.Clan.shots,
                    spotted = tankJson.Clan.spotted,
                    survivedBattles = tankJson.Clan.survivedBattles,
                    wins = tankJson.Clan.wins,
                    xp = tankJson.Clan.xp,
                    originalXP = tankJson.Clan2.originalXP,
                    damageAssistedRadio = tankJson.Clan2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Clan2.damageAssistedTrack,
                    shotsReceived = tankJson.Clan2.shotsReceived,
                    noDamageShotsReceived = tankJson.Clan2.noDamageShotsReceived,
                    piercedReceived = tankJson.Clan2.piercedReceived,
                    heHitsReceived = tankJson.Clan2.heHitsReceived,
                    he_hits = tankJson.Clan2.he_hits,
                    pierced = tankJson.Clan2.pierced
                };
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson
                {
                    battlesCount = tankJson.Company.battlesCount,
                    capturePoints = tankJson.Company.capturePoints,
                    damageDealt = tankJson.Company.damageDealt,
                    damageReceived = tankJson.Company.damageReceived,
                    droppedCapturePoints = tankJson.Company.droppedCapturePoints,
                    frags = tankJson.Company.frags,
                    hits = tankJson.Company.hits,
                    losses = tankJson.Company.losses,
                    shots = tankJson.Company.shots,
                    spotted = tankJson.Company.spotted,
                    survivedBattles = tankJson.Company.survivedBattles,
                    wins = tankJson.Company.wins,
                    xp = tankJson.Company.xp,
                    originalXP = tankJson.Company2.originalXP,
                    damageAssistedRadio = tankJson.Company2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Company2.damageAssistedTrack,
                    shotsReceived = tankJson.Company2.shotsReceived,
                    noDamageShotsReceived = tankJson.Company2.noDamageShotsReceived,
                    piercedReceived = tankJson.Company2.piercedReceived,
                    heHitsReceived = tankJson.Company2.heHitsReceived,
                    he_hits = tankJson.Company2.he_hits,
                    pierced = tankJson.Company2.pierced
                };
            }

            if (tankJson.A7x7 != null)
            {
                v2.A7x7 = new StatisticJson
                {
                    battlesCount = tankJson.A7x7.battlesCount,
                    capturePoints = tankJson.A7x7.capturePoints,
                    damageDealt = tankJson.A7x7.damageDealt,
                    damageReceived = tankJson.A7x7.damageReceived,
                    droppedCapturePoints = tankJson.A7x7.droppedCapturePoints,
                    frags = tankJson.A7x7.frags,
                    frags8p = tankJson.A7x7.frags8p,
                    hits = tankJson.A7x7.hits,
                    losses = tankJson.A7x7.losses,
                    shots = tankJson.A7x7.shots,
                    spotted = tankJson.A7x7.spotted,
                    survivedBattles = tankJson.A7x7.survivedBattles,
                    winAndSurvived = tankJson.A7x7.winAndSurvived,
                    wins = tankJson.A7x7.wins,
                    xp = tankJson.A7x7.xp,
                    battlesCountBefore8_8 = tankJson.A7x7.battlesCountBefore8_8,
                    xpBefore8_8 = tankJson.A7x7.xpBefore8_8,
                    originalXP = tankJson.A7x7.originalXP,
                    damageAssistedRadio = tankJson.A7x7.damageAssistedRadio,
                    damageAssistedTrack = tankJson.A7x7.damageAssistedTrack,
                    shotsReceived = tankJson.A7x7.shotsReceived,
                    noDamageShotsReceived = tankJson.A7x7.noDamageShotsReceived,
                    piercedReceived = tankJson.A7x7.piercedReceived,
                    heHitsReceived = tankJson.A7x7.heHitsReceived,
                    he_hits = tankJson.A7x7.he_hits,
                    pierced = tankJson.A7x7.pierced,
                    maxDamage = tankJson.Max7x7.maxDamage,
                    maxFrags = tankJson.Max7x7.maxFrags,
                    maxXP = tankJson.Max7x7.maxXP
                };
            }

            v2.Common = new CommonJson
            {
                basedonversion = tankJson.Common.basedonversion,
                compactDescr = tankJson.Common.compactDescr,
                countryid = tankJson.Common.countryid,
                creationTime = tankJson.Common.creationTime,
                creationTimeR = tankJson.Common.creationTimeR,
                frags = tankJson.Common.frags,
                frags_compare = tankJson.Common.frags_compare,
                has_15x15 = 1,
                has_7x7 = 0,
                has_clan = tankJson.Clan != null ? 1 : 0,
                has_company = tankJson.Company != null ? 1 : 0,
                lastBattleTime = tankJson.Common.lastBattleTime,
                lastBattleTimeR = tankJson.Common.lastBattleTimeR,
                premium = tankJson.Common.premium,
                tankid = tankJson.Common.tankid,
                tanktitle = tankJson.Common.tanktitle,
                tier = tankJson.Common.tier,
                type = tankJson.Common.type,
                updated = tankJson.Common.updated,
                updatedR = Utils.UnixDateToDateTime(tankJson.Common.updated),
                battleLifeTime = tankJson.Total.battleLifeTime,
                mileage = tankJson.Total.mileage,
                treesCut = tankJson.Total.treesCut
            };

            return v2;
        }

        /// <summary>
        /// Converts the specified tank json from local cache with structure version less then or equals 77.
        /// </summary>
        /// <param name="tankJson">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(TankJson77 tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson.A15x15.battlesCount,
                capturePoints = tankJson.A15x15.capturePoints,
                damageDealt = tankJson.A15x15.damageDealt,
                damageReceived = tankJson.A15x15.damageReceived,
                droppedCapturePoints = tankJson.A15x15.droppedCapturePoints,
                frags = tankJson.A15x15.frags,
                frags8p = tankJson.A15x15.frags8p,
                hits = tankJson.A15x15.hits,
                losses = tankJson.A15x15.losses,
                shots = tankJson.A15x15.shots,
                spotted = tankJson.A15x15.spotted,
                survivedBattles = tankJson.A15x15.survivedBattles,
                winAndSurvived = tankJson.A15x15.winAndSurvived,
                wins = tankJson.A15x15.wins,
                xp = tankJson.A15x15.xp,
                battlesCountBefore8_8 = tankJson.A15x15.battlesCountBefore8_8,
                battlesCountBefore9_0 = tankJson.A15x15.battlesCountBefore9_0,
                xpBefore8_8 = tankJson.A15x15.xpBefore8_8,
                originalXP = tankJson.A15x15_2.originalXP,
                damageAssistedRadio = tankJson.A15x15_2.damageAssistedRadio,
                damageAssistedTrack = tankJson.A15x15_2.damageAssistedTrack,
                shotsReceived = tankJson.A15x15_2.shotsReceived,
                noDamageShotsReceived = tankJson.A15x15_2.noDamageShotsReceived,
                piercedReceived = tankJson.A15x15_2.piercedReceived,
                heHitsReceived = tankJson.A15x15_2.heHitsReceived,
                he_hits = tankJson.A15x15_2.he_hits,
                pierced = tankJson.A15x15_2.pierced,
                damageBlockedByArmor = tankJson.A15x15_2.damageBlockedByArmor,
                potentialDamageReceived = tankJson.A15x15_2.potentialDamageReceived,
                maxDamage = tankJson.Max15x15.maxDamage,
                maxFrags = tankJson.Max15x15.maxFrags,
                maxXP = tankJson.Max15x15.maxXP
            };

            v2.FragsList = tankJson.FragsList;

            #region Achievements

            v2.Achievements = new AchievementsJson
            {
                MasterGunner = tankJson.Achievements.armorPiercer,
                BattleHero = tankJson.Achievements.battleHeroes,
                Hunter = tankJson.Achievements.beasthunter,
                Bombardier = tankJson.Achievements.bombardier,
                Defender = tankJson.Achievements.defender,
                Survivor = tankJson.Achievements.diehard,
                SurvivorProgress = tankJson.Achievements.diehardSeries,
                PatrolDuty = tankJson.Achievements.evileye,
                FragsBeast = tankJson.Achievements.fragsBeast,
                FragsPatton = tankJson.Achievements.fragsPatton,
                FragsSinai = tankJson.Achievements.fragsSinai,
                Reaper = tankJson.Achievements.handOfDeath,
                HeroesOfRassenay = tankJson.Achievements.heroesOfRassenay,
                Huntsman = tankJson.Achievements.huntsman,
                Invader = tankJson.Achievements.invader,
                Invincible = tankJson.Achievements.invincible,
                InvincibleProgress = tankJson.Achievements.invincibleSeries,
                IronMan = tankJson.Achievements.ironMan,
                Kamikaze = tankJson.Achievements.kamikaze,
                ReaperProgress = tankJson.Achievements.killingSeries,
                LuckyDevil = tankJson.Achievements.luckyDevil,
                Lumberjack = tankJson.Achievements.lumberjack,
                SurvivorLongest = tankJson.Achievements.maxDiehardSeries,
                InvincibleLongest = tankJson.Achievements.maxInvincibleSeries,
                ReaperLongest = tankJson.Achievements.maxKillingSeries,
                MasterGunnerLongest = tankJson.Achievements.maxPiercingSeries,
                SharpshooterLongest = tankJson.Achievements.maxSniperSeries,
                Abrams = tankJson.Achievements.medalAbrams,
                Billotte = tankJson.Achievements.medalBillotte,
                BrothersInArms = tankJson.Achievements.medalBrothersInArms,
                BrunoPietro = tankJson.Achievements.medalBrunoPietro,
                Burda = tankJson.Achievements.medalBurda,
                Carius = tankJson.Achievements.medalCarius,
                CrucialContribution = tankJson.Achievements.medalCrucialContribution,
                DeLanglade = tankJson.Achievements.medalDeLanglade,
                Dumitru = tankJson.Achievements.medalDumitru,
                Ekins = tankJson.Achievements.medalEkins,
                Fadin = tankJson.Achievements.medalFadin,
                Halonen = tankJson.Achievements.medalHalonen,
                Kay = tankJson.Achievements.medalKay,
                Knispel = tankJson.Achievements.medalKnispel,
                Kolobanov = tankJson.Achievements.medalKolobanov,
                LafayettePool = tankJson.Achievements.medalLafayettePool,
                Lavrinenko = tankJson.Achievements.medalLavrinenko,
                Leclerk = tankJson.Achievements.medalLeClerc,
                Lehvaslaiho = tankJson.Achievements.medalLehvaslaiho,
                Nikolas = tankJson.Achievements.medalNikolas,
                Orlik = tankJson.Achievements.medalOrlik,
                Oskin = tankJson.Achievements.medalOskin,
                Pascucci = tankJson.Achievements.medalPascucci,
                Poppel = tankJson.Achievements.medalPoppel,
                RadleyWalters = tankJson.Achievements.medalRadleyWalters,
                TamadaYoshio = tankJson.Achievements.medalTamadaYoshio,
                Tarczay = tankJson.Achievements.medalTarczay,
                Boelter = tankJson.Achievements.medalWittmann,
                MouseTrap = tankJson.Achievements.mousebane,
                PattonValley = tankJson.Achievements.pattonValley,
                MasterGunnerProgress = tankJson.Achievements.piercingSeries,
                Raider = tankJson.Achievements.raider,
                Scout = tankJson.Achievements.scout,
                Sinai = tankJson.Achievements.sinai,
                Sniper = tankJson.Achievements.sniper,
                Sniper2 = tankJson.Achievements.sniper2,
                MainGun = tankJson.Achievements.mainGun,
                SharpshooterProgress = tankJson.Achievements.sniperSeries,
                SteelWall = tankJson.Achievements.steelwall,
                Sturdy = tankJson.Achievements.sturdy,
                Confederate = tankJson.Achievements.supporter,
                TankExpertStrg = tankJson.Achievements.tankExpertStrg,
                Sharpshooter = tankJson.Achievements.titleSniper,
                Warrior = tankJson.Achievements.warrior,
                MarkOfMastery = tankJson.Achievements.markOfMastery,

                MarksOnGun = tankJson.Achievements.marksOnGun,
                MovingAvgDamage = tankJson.Achievements.movingAvgDamage,
                MedalMonolith = tankJson.Achievements.medalMonolith,
                MedalAntiSpgFire = tankJson.Achievements.medalAntiSpgFire,
                MedalGore = tankJson.Achievements.medalGore,
                MedalCoolBlood = tankJson.Achievements.medalCoolBlood,
                MedalStark = tankJson.Achievements.medalStark,
                DamageRating = tankJson.Achievements.damageRating,
            };

            //achievements 7x7
            v2.Achievements7x7 = new Achievements7x7
            {
                ArmoredFist = tankJson.Achievements7X7.armoredFist,
                GeniusForWar = tankJson.Achievements7X7.geniusForWar,
                GeniusForWarMedal = tankJson.Achievements7X7.geniusForWarMedal,
                KingOfTheHill = tankJson.Achievements7X7.kingOfTheHill,
                MaxTacticalBreakthroughSeries = tankJson.Achievements7X7.maxTacticalBreakthroughSeries,
                TacticalBreakthrough = tankJson.Achievements7X7.tacticalBreakthrough,
                TacticalBreakthroughSeries = tankJson.Achievements7X7.tacticalBreakthroughSeries,
                WolfAmongSheep = tankJson.Achievements7X7.wolfAmongSheep,
                WolfAmongSheepMedal = tankJson.Achievements7X7.wolfAmongSheepMedal,
                GodOfWar = tankJson.Achievements7X7.godOfWar,
                FightingReconnaissance = tankJson.Achievements7X7.fightingReconnaissance,
                FightingReconnaissanceMedal = tankJson.Achievements7X7.fightingReconnaissanceMedal,
                WillToWinSpirit = tankJson.Achievements7X7.willToWinSpirit,
                CrucialShot = tankJson.Achievements7X7.crucialShot,
                CrucialShotMedal = tankJson.Achievements7X7.crucialShotMedal,
                ForTacticalOperations = tankJson.Achievements7X7.forTacticalOperations,
                
                PromisingFighter = tankJson.Achievements7X7.promisingFighter,
                PromisingFighterMedal = tankJson.Achievements7X7.promisingFighterMedal,
                HeavyFire = tankJson.Achievements7X7.heavyFire,
                HeavyFireMedal = tankJson.Achievements7X7.heavyFireMedal,
                Ranger = tankJson.Achievements7X7.ranger,
                RangerMedal = tankJson.Achievements7X7.rangerMedal,
                FireAndSteel = tankJson.Achievements7X7.fireAndSteel,
                FireAndSteelMedal = tankJson.Achievements7X7.fireAndSteelMedal,
                Pyromaniac = tankJson.Achievements7X7.pyromaniac,
                PyromaniacMedal = tankJson.Achievements7X7.pyromaniacMedal,
                NoMansLand = tankJson.Achievements7X7.noMansLand,
            };

            v2.AchievementsHistorical = new AchievementsHistorical
            {
                BothSidesWins = tankJson.HistoricalAchievements.bothSidesWins,
                GuardsMan = tankJson.HistoricalAchievements.guardsman,
                MakerOfHistory = tankJson.HistoricalAchievements.makerOfHistory,
                WeakVehiclesWins = tankJson.HistoricalAchievements.weakVehiclesWins
            };

            #endregion


            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson
                {
                    battlesCount = tankJson.Clan.battlesCount,
                    capturePoints = tankJson.Clan.capturePoints,
                    damageDealt = tankJson.Clan.damageDealt,
                    damageReceived = tankJson.Clan.damageReceived,
                    droppedCapturePoints = tankJson.Clan.droppedCapturePoints,
                    frags = tankJson.Clan.frags,
                    hits = tankJson.Clan.hits,
                    losses = tankJson.Clan.losses,
                    shots = tankJson.Clan.shots,
                    spotted = tankJson.Clan.spotted,
                    survivedBattles = tankJson.Clan.survivedBattles,
                    wins = tankJson.Clan.wins,
                    xp = tankJson.Clan.xp,
                    battlesCountBefore8_8 = tankJson.Clan.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.Clan.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.Clan.xpBefore8_8,
                    originalXP = tankJson.Clan2.originalXP,
                    damageAssistedRadio = tankJson.Clan2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Clan2.damageAssistedTrack,
                    shotsReceived = tankJson.Clan2.shotsReceived,
                    noDamageShotsReceived = tankJson.Clan2.noDamageShotsReceived,
                    piercedReceived = tankJson.Clan2.piercedReceived,
                    heHitsReceived = tankJson.Clan2.heHitsReceived,
                    he_hits = tankJson.Clan2.he_hits,
                    pierced = tankJson.Clan2.pierced,
                    damageBlockedByArmor = tankJson.Clan2.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.Clan2.potentialDamageReceived
                };
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson
                {
                    battlesCount = tankJson.Company.battlesCount,
                    capturePoints = tankJson.Company.capturePoints,
                    damageDealt = tankJson.Company.damageDealt,
                    damageReceived = tankJson.Company.damageReceived,
                    droppedCapturePoints = tankJson.Company.droppedCapturePoints,
                    frags = tankJson.Company.frags,
                    hits = tankJson.Company.hits,
                    losses = tankJson.Company.losses,
                    shots = tankJson.Company.shots,
                    spotted = tankJson.Company.spotted,
                    survivedBattles = tankJson.Company.survivedBattles,
                    wins = tankJson.Company.wins,
                    xp = tankJson.Company.xp,
                    battlesCountBefore8_8 = tankJson.Company.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.Company.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.Company.xpBefore8_8,
                    originalXP = tankJson.Company2.originalXP,
                    damageAssistedRadio = tankJson.Company2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Company2.damageAssistedTrack,
                    shotsReceived = tankJson.Company2.shotsReceived,
                    noDamageShotsReceived = tankJson.Company2.noDamageShotsReceived,
                    piercedReceived = tankJson.Company2.piercedReceived,
                    heHitsReceived = tankJson.Company2.heHitsReceived,
                    he_hits = tankJson.Company2.he_hits,
                    pierced = tankJson.Company2.pierced,
                    damageBlockedByArmor = tankJson.Company2.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.Company2.potentialDamageReceived
                };
            }

            if (tankJson.A7x7 != null)
            {
                v2.A7x7 = new StatisticJson
                {
                    battlesCount = tankJson.A7x7.battlesCount,
                    capturePoints = tankJson.A7x7.capturePoints,
                    damageDealt = tankJson.A7x7.damageDealt,
                    damageReceived = tankJson.A7x7.damageReceived,
                    droppedCapturePoints = tankJson.A7x7.droppedCapturePoints,
                    frags = tankJson.A7x7.frags,
                    frags8p = tankJson.A7x7.frags8p,
                    hits = tankJson.A7x7.hits,
                    losses = tankJson.A7x7.losses,
                    shots = tankJson.A7x7.shots,
                    spotted = tankJson.A7x7.spotted,
                    survivedBattles = tankJson.A7x7.survivedBattles,
                    winAndSurvived = tankJson.A7x7.winAndSurvived,
                    wins = tankJson.A7x7.wins,
                    xp = tankJson.A7x7.xp,
                    battlesCountBefore8_8 = tankJson.A7x7.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.A7x7.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.A7x7.xpBefore8_8,
                    originalXP = tankJson.A7x7.originalXP,
                    damageAssistedRadio = tankJson.A7x7.damageAssistedRadio,
                    damageAssistedTrack = tankJson.A7x7.damageAssistedTrack,
                    shotsReceived = tankJson.A7x7.shotsReceived,
                    noDamageShotsReceived = tankJson.A7x7.noDamageShotsReceived,
                    piercedReceived = tankJson.A7x7.piercedReceived,
                    heHitsReceived = tankJson.A7x7.heHitsReceived,
                    he_hits = tankJson.A7x7.he_hits,
                    pierced = tankJson.A7x7.pierced,
                    damageBlockedByArmor = tankJson.A7x7.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.A7x7.potentialDamageReceived,
                    maxDamage = tankJson.Max7x7.maxDamage,
                    maxFrags = tankJson.Max7x7.maxFrags,
                    maxXP = tankJson.Max7x7.maxXP
                };
            }

            if (tankJson.Historical != null)
            {
                v2.Historical = new StatisticJson
                {
                    battlesCount = tankJson.Historical.battlesCount,
                    capturePoints = tankJson.Historical.capturePoints,
                    damageDealt = tankJson.Historical.damageDealt,
                    damageReceived = tankJson.Historical.damageReceived,
                    droppedCapturePoints = tankJson.Historical.droppedCapturePoints,
                    frags = tankJson.Historical.frags,
                    frags8p = tankJson.Historical.frags8p,
                    hits = tankJson.Historical.hits,
                    losses = tankJson.Historical.losses,
                    shots = tankJson.Historical.shots,
                    spotted = tankJson.Historical.spotted,
                    survivedBattles = tankJson.Historical.survivedBattles,
                    winAndSurvived = tankJson.Historical.winAndSurvived,
                    wins = tankJson.Historical.wins,
                    xp = tankJson.Historical.xp,
                    originalXP = tankJson.Historical.originalXP,
                    damageAssistedRadio = tankJson.Historical.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Historical.damageAssistedTrack,
                    shotsReceived = tankJson.Historical.shotsReceived,
                    noDamageShotsReceived = tankJson.Historical.noDamageShotsReceived,
                    piercedReceived = tankJson.Historical.piercedReceived,
                    heHitsReceived = tankJson.Historical.heHitsReceived,
                    he_hits = tankJson.Historical.he_hits,
                    pierced = tankJson.Historical.pierced,
                    maxDamage = tankJson.MaxHistorical.maxDamage,
                    maxFrags = tankJson.MaxHistorical.maxFrags,
                    maxXP = tankJson.MaxHistorical.maxXP
                };
            }

            v2.Common = new CommonJson();
            v2.Common.basedonversion = tankJson.Common.basedonversion;
            v2.Common.compactDescr = tankJson.Common.compactDescr;
            v2.Common.countryid = tankJson.Common.countryid;
            v2.Common.creationTime = tankJson.Common.creationTime;
            v2.Common.creationTimeR = tankJson.Common.creationTimeR;
            v2.Common.frags = tankJson.Common.frags;
            v2.Common.frags_compare = tankJson.Common.frags_compare;
            v2.Common.has_15x15 = 1;
            v2.Common.has_7x7 = 0;
            v2.Common.has_clan = tankJson.Clan != null ? 1 : 0;
            v2.Common.has_company = tankJson.Company != null ? 1 : 0;
            v2.Common.lastBattleTime = tankJson.Common.lastBattleTime;
            v2.Common.lastBattleTimeR = tankJson.Common.lastBattleTimeR;
            v2.Common.premium = tankJson.Common.premium;
            v2.Common.tankid = tankJson.Common.tankid;
            v2.Common.tanktitle = tankJson.Common.tanktitle;
            v2.Common.tier = tankJson.Common.tier;
            v2.Common.type = tankJson.Common.type;
            v2.Common.updated = tankJson.Common.updated;
            v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.Common.updated);
            v2.Common.battleLifeTime = tankJson.Total.battleLifeTime;
            v2.Common.mileage = tankJson.Total.mileage;
            v2.Common.treesCut = tankJson.Total.treesCut;

            return v2;
        }

        private static TankJson Map(TankJson85 tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson.A15x15.battlesCount,
                capturePoints = tankJson.A15x15.capturePoints,
                damageDealt = tankJson.A15x15.damageDealt,
                damageReceived = tankJson.A15x15.damageReceived,
                droppedCapturePoints = tankJson.A15x15.droppedCapturePoints,
                frags = tankJson.A15x15.frags,
                frags8p = tankJson.A15x15.frags8p,
                hits = tankJson.A15x15.hits,
                losses = tankJson.A15x15.losses,
                shots = tankJson.A15x15.shots,
                spotted = tankJson.A15x15.spotted,
                survivedBattles = tankJson.A15x15.survivedBattles,
                winAndSurvived = tankJson.A15x15.winAndSurvived,
                wins = tankJson.A15x15.wins,
                xp = tankJson.A15x15.xp,
                battlesCountBefore8_8 = tankJson.A15x15.battlesCountBefore8_8,
                battlesCountBefore9_0 = tankJson.A15x15.battlesCountBefore9_0,
                xpBefore8_8 = tankJson.A15x15.xpBefore8_8,
                originalXP = tankJson.A15x15_2.originalXP,
                damageAssistedRadio = tankJson.A15x15_2.damageAssistedRadio,
                damageAssistedTrack = tankJson.A15x15_2.damageAssistedTrack,
                shotsReceived = tankJson.A15x15_2.shotsReceived,
                noDamageShotsReceived = tankJson.A15x15_2.noDamageShotsReceived,
                piercedReceived = tankJson.A15x15_2.piercedReceived,
                heHitsReceived = tankJson.A15x15_2.heHitsReceived,
                he_hits = tankJson.A15x15_2.he_hits,
                pierced = tankJson.A15x15_2.pierced,
                damageBlockedByArmor = tankJson.A15x15_2.damageBlockedByArmor,
                potentialDamageReceived = tankJson.A15x15_2.potentialDamageReceived,
                maxDamage = tankJson.Max15x15.maxDamage,
                maxFrags = tankJson.Max15x15.maxFrags,
                maxXP = tankJson.Max15x15.maxXP
            };

            v2.FragsList = tankJson.FragsList;

            #region Achievements

            v2.Achievements = new AchievementsJson
            {
                MasterGunner = tankJson.SingleAchievements.armorPiercer,
                BattleHero = tankJson.Achievements.battleHeroes,
                Hunter = tankJson.Achievements.beasthunter,
                Bombardier = tankJson.Achievements.bombardier,
                Defender = tankJson.Achievements.defender,
                Survivor = tankJson.SingleAchievements.diehard,
                SurvivorProgress = tankJson.Achievements.diehardSeries,
                PatrolDuty = tankJson.Achievements.evileye,
                FragsBeast = tankJson.Achievements.fragsBeast,
                FragsPatton = tankJson.Achievements.fragsPatton,
                FragsSinai = tankJson.Achievements.fragsSinai,
                Reaper = tankJson.SingleAchievements.handOfDeath,
                HeroesOfRassenay = tankJson.Achievements.heroesOfRassenay,
                Huntsman = tankJson.Achievements.huntsman,
                Invader = tankJson.Achievements.invader,
                Invincible = tankJson.SingleAchievements.invincible,
                InvincibleProgress = tankJson.Achievements.invincibleSeries,
                IronMan = tankJson.Achievements.ironMan,
                Kamikaze = tankJson.Achievements.kamikaze,
                ReaperProgress = tankJson.Achievements.killingSeries,
                LuckyDevil = tankJson.Achievements.luckyDevil,
                Lumberjack = tankJson.Achievements.lumberjack,
                SurvivorLongest = tankJson.Achievements.maxDiehardSeries,
                InvincibleLongest = tankJson.Achievements.maxInvincibleSeries,
                ReaperLongest = tankJson.Achievements.maxKillingSeries,
                MasterGunnerLongest = tankJson.Achievements.maxPiercingSeries,
                SharpshooterLongest = tankJson.Achievements.maxSniperSeries,
                Abrams = tankJson.Achievements.medalAbrams,
                Billotte = tankJson.Achievements.medalBillotte,
                BrothersInArms = tankJson.Achievements.medalBrothersInArms,
                BrunoPietro = tankJson.Achievements.medalBrunoPietro,
                Burda = tankJson.Achievements.medalBurda,
                Carius = tankJson.Achievements.medalCarius,
                CrucialContribution = tankJson.Achievements.medalCrucialContribution,
                DeLanglade = tankJson.Achievements.medalDeLanglade,
                Dumitru = tankJson.Achievements.medalDumitru,
                Ekins = tankJson.Achievements.medalEkins,
                Fadin = tankJson.Achievements.medalFadin,
                Halonen = tankJson.Achievements.medalHalonen,
                Kay = tankJson.Achievements.medalKay,
                Knispel = tankJson.Achievements.medalKnispel,
                Kolobanov = tankJson.Achievements.medalKolobanov,
                LafayettePool = tankJson.Achievements.medalLafayettePool,
                Lavrinenko = tankJson.Achievements.medalLavrinenko,
                Leclerk = tankJson.Achievements.medalLeClerc,
                Lehvaslaiho = tankJson.Achievements.medalLehvaslaiho,
                Nikolas = tankJson.Achievements.medalNikolas,
                Orlik = tankJson.Achievements.medalOrlik,
                Oskin = tankJson.Achievements.medalOskin,
                Pascucci = tankJson.Achievements.medalPascucci,
                Poppel = tankJson.Achievements.medalPoppel,
                RadleyWalters = tankJson.Achievements.medalRadleyWalters,
                TamadaYoshio = tankJson.Achievements.medalTamadaYoshio,
                Tarczay = tankJson.Achievements.medalTarczay,
                Boelter = tankJson.Achievements.medalWittmann,
                MouseTrap = tankJson.Achievements.mousebane,
                PattonValley = tankJson.Achievements.pattonValley,
                MasterGunnerProgress = tankJson.Achievements.piercingSeries,
                Raider = tankJson.Achievements.raider,
                Scout = tankJson.Achievements.scout,
                Sinai = tankJson.Achievements.sinai,
                Sniper = tankJson.Achievements.sniper,
                Sniper2 = tankJson.Achievements.sniper2,
                MainGun = tankJson.Achievements.mainGun,
                SharpshooterProgress = tankJson.Achievements.sniperSeries,
                SteelWall = tankJson.Achievements.steelwall,
                Sturdy = tankJson.Achievements.sturdy,
                Confederate = tankJson.Achievements.supporter,
                TankExpertStrg = tankJson.Achievements.tankExpertStrg,
                Sharpshooter = tankJson.SingleAchievements.titleSniper,
                Warrior = tankJson.Achievements.warrior,
                MarkOfMastery = tankJson.Achievements.markOfMastery,

                MarksOnGun = tankJson.Achievements.marksOnGun,
                MovingAvgDamage = tankJson.Achievements.movingAvgDamage,
                MedalMonolith = tankJson.Achievements.medalMonolith,
                MedalAntiSpgFire = tankJson.Achievements.medalAntiSpgFire,
                MedalGore = tankJson.Achievements.medalGore,
                MedalCoolBlood = tankJson.Achievements.medalCoolBlood,
                MedalStark = tankJson.Achievements.medalStark,
                DamageRating = tankJson.Achievements.damageRating,
            };

            //achievements 7x7
            v2.Achievements7x7 = new Achievements7x7
            {
                ArmoredFist = tankJson.Achievements7X7.armoredFist,
                GeniusForWar = tankJson.Achievements7X7.geniusForWar,
                GeniusForWarMedal = tankJson.Achievements7X7.geniusForWarMedal,
                KingOfTheHill = tankJson.Achievements7X7.kingOfTheHill,
                MaxTacticalBreakthroughSeries = tankJson.Achievements7X7.maxTacticalBreakthroughSeries,
                TacticalBreakthrough = tankJson.SingleAchievements.tacticalBreakthrough,
                TacticalBreakthroughSeries = tankJson.Achievements7X7.tacticalBreakthroughSeries,
                WolfAmongSheep = tankJson.Achievements7X7.wolfAmongSheep,
                WolfAmongSheepMedal = tankJson.Achievements7X7.wolfAmongSheepMedal,
                GodOfWar = tankJson.Achievements7X7.godOfWar,
                FightingReconnaissance = tankJson.Achievements7X7.fightingReconnaissance,
                FightingReconnaissanceMedal = tankJson.Achievements7X7.fightingReconnaissanceMedal,
                WillToWinSpirit = tankJson.Achievements7X7.willToWinSpirit,
                CrucialShot = tankJson.Achievements7X7.crucialShot,
                CrucialShotMedal = tankJson.Achievements7X7.crucialShotMedal,
                ForTacticalOperations = tankJson.Achievements7X7.forTacticalOperations,

                PromisingFighter = tankJson.Achievements7X7.promisingFighter,
                PromisingFighterMedal = tankJson.Achievements7X7.promisingFighterMedal,
                HeavyFire = tankJson.Achievements7X7.heavyFire,
                HeavyFireMedal = tankJson.Achievements7X7.heavyFireMedal,
                Ranger = tankJson.Achievements7X7.ranger,
                RangerMedal = tankJson.Achievements7X7.rangerMedal,
                FireAndSteel = tankJson.Achievements7X7.fireAndSteel,
                FireAndSteelMedal = tankJson.Achievements7X7.fireAndSteelMedal,
                Pyromaniac = tankJson.Achievements7X7.pyromaniac,
                PyromaniacMedal = tankJson.Achievements7X7.pyromaniacMedal,
                NoMansLand = tankJson.Achievements7X7.noMansLand,

                Guerrilla = tankJson.Achievements7X7.guerrilla,
                GuerrillaMedal = tankJson.Achievements7X7.guerrillaMedal,
                Infiltrator = tankJson.Achievements7X7.infiltrator,
                InfiltratorMedal = tankJson.Achievements7X7.infiltratorMedal,
                Sentinel = tankJson.Achievements7X7.sentinel,
                SentinelMedal = tankJson.Achievements7X7.sentinelMedal,
                PrematureDetonation = tankJson.Achievements7X7.prematureDetonation,
                PrematureDetonationMedal = tankJson.Achievements7X7.prematureDetonationMedal,
                BruteForce = tankJson.Achievements7X7.bruteForce,
                BruteForceMedal = tankJson.Achievements7X7.bruteForceMedal,
                AwardCount = tankJson.Achievements7X7.awardCount,
                BattleTested = tankJson.Achievements7X7.battleTested,
            };

            v2.AchievementsHistorical = new AchievementsHistorical
            {
                BothSidesWins = tankJson.HistoricalAchievements.bothSidesWins,
                GuardsMan = tankJson.HistoricalAchievements.guardsman,
                MakerOfHistory = tankJson.HistoricalAchievements.makerOfHistory,
                WeakVehiclesWins = tankJson.HistoricalAchievements.weakVehiclesWins
            };

            v2.AchievementsClan = new AchievementsClan()
            {
                MedalRotmistrov = tankJson.ClanAchievements.medalRotmistrov
            };

            #endregion


            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson
                {
                    battlesCount = tankJson.Clan.battlesCount,
                    capturePoints = tankJson.Clan.capturePoints,
                    damageDealt = tankJson.Clan.damageDealt,
                    damageReceived = tankJson.Clan.damageReceived,
                    droppedCapturePoints = tankJson.Clan.droppedCapturePoints,
                    frags = tankJson.Clan.frags,
                    hits = tankJson.Clan.hits,
                    losses = tankJson.Clan.losses,
                    shots = tankJson.Clan.shots,
                    spotted = tankJson.Clan.spotted,
                    survivedBattles = tankJson.Clan.survivedBattles,
                    wins = tankJson.Clan.wins,
                    xp = tankJson.Clan.xp,
                    battlesCountBefore8_8 = tankJson.Clan.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.Clan.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.Clan.xpBefore8_8,
                    originalXP = tankJson.Clan2.originalXP,
                    damageAssistedRadio = tankJson.Clan2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Clan2.damageAssistedTrack,
                    shotsReceived = tankJson.Clan2.shotsReceived,
                    noDamageShotsReceived = tankJson.Clan2.noDamageShotsReceived,
                    piercedReceived = tankJson.Clan2.piercedReceived,
                    heHitsReceived = tankJson.Clan2.heHitsReceived,
                    he_hits = tankJson.Clan2.he_hits,
                    pierced = tankJson.Clan2.pierced,
                    damageBlockedByArmor = tankJson.Clan2.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.Clan2.potentialDamageReceived
                };
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson
                {
                    battlesCount = tankJson.Company.battlesCount,
                    capturePoints = tankJson.Company.capturePoints,
                    damageDealt = tankJson.Company.damageDealt,
                    damageReceived = tankJson.Company.damageReceived,
                    droppedCapturePoints = tankJson.Company.droppedCapturePoints,
                    frags = tankJson.Company.frags,
                    hits = tankJson.Company.hits,
                    losses = tankJson.Company.losses,
                    shots = tankJson.Company.shots,
                    spotted = tankJson.Company.spotted,
                    survivedBattles = tankJson.Company.survivedBattles,
                    wins = tankJson.Company.wins,
                    xp = tankJson.Company.xp,
                    battlesCountBefore8_8 = tankJson.Company.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.Company.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.Company.xpBefore8_8,
                    originalXP = tankJson.Company2.originalXP,
                    damageAssistedRadio = tankJson.Company2.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Company2.damageAssistedTrack,
                    shotsReceived = tankJson.Company2.shotsReceived,
                    noDamageShotsReceived = tankJson.Company2.noDamageShotsReceived,
                    piercedReceived = tankJson.Company2.piercedReceived,
                    heHitsReceived = tankJson.Company2.heHitsReceived,
                    he_hits = tankJson.Company2.he_hits,
                    pierced = tankJson.Company2.pierced,
                    damageBlockedByArmor = tankJson.Company2.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.Company2.potentialDamageReceived
                };
            }

            if (tankJson.A7x7 != null)
            {
                v2.A7x7 = new StatisticJson
                {
                    battlesCount = tankJson.A7x7.battlesCount,
                    capturePoints = tankJson.A7x7.capturePoints,
                    damageDealt = tankJson.A7x7.damageDealt,
                    damageReceived = tankJson.A7x7.damageReceived,
                    droppedCapturePoints = tankJson.A7x7.droppedCapturePoints,
                    frags = tankJson.A7x7.frags,
                    frags8p = tankJson.A7x7.frags8p,
                    hits = tankJson.A7x7.hits,
                    losses = tankJson.A7x7.losses,
                    shots = tankJson.A7x7.shots,
                    spotted = tankJson.A7x7.spotted,
                    survivedBattles = tankJson.A7x7.survivedBattles,
                    winAndSurvived = tankJson.A7x7.winAndSurvived,
                    wins = tankJson.A7x7.wins,
                    xp = tankJson.A7x7.xp,
                    battlesCountBefore8_8 = tankJson.A7x7.battlesCountBefore8_8,
                    battlesCountBefore9_0 = tankJson.A7x7.battlesCountBefore9_0,
                    xpBefore8_8 = tankJson.A7x7.xpBefore8_8,
                    originalXP = tankJson.A7x7.originalXP,
                    damageAssistedRadio = tankJson.A7x7.damageAssistedRadio,
                    damageAssistedTrack = tankJson.A7x7.damageAssistedTrack,
                    shotsReceived = tankJson.A7x7.shotsReceived,
                    noDamageShotsReceived = tankJson.A7x7.noDamageShotsReceived,
                    piercedReceived = tankJson.A7x7.piercedReceived,
                    heHitsReceived = tankJson.A7x7.heHitsReceived,
                    he_hits = tankJson.A7x7.he_hits,
                    pierced = tankJson.A7x7.pierced,
                    damageBlockedByArmor = tankJson.A7x7.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.A7x7.potentialDamageReceived,
                    maxDamage = tankJson.Max7x7.maxDamage,
                    maxFrags = tankJson.Max7x7.maxFrags,
                    maxXP = tankJson.Max7x7.maxXP
                };
            }

            if (tankJson.Historical != null)
            {
                v2.Historical = new StatisticJson
                {
                    battlesCount = tankJson.Historical.battlesCount,
                    capturePoints = tankJson.Historical.capturePoints,
                    damageDealt = tankJson.Historical.damageDealt,
                    damageReceived = tankJson.Historical.damageReceived,
                    droppedCapturePoints = tankJson.Historical.droppedCapturePoints,
                    frags = tankJson.Historical.frags,
                    frags8p = tankJson.Historical.frags8p,
                    hits = tankJson.Historical.hits,
                    losses = tankJson.Historical.losses,
                    shots = tankJson.Historical.shots,
                    spotted = tankJson.Historical.spotted,
                    survivedBattles = tankJson.Historical.survivedBattles,
                    winAndSurvived = tankJson.Historical.winAndSurvived,
                    wins = tankJson.Historical.wins,
                    xp = tankJson.Historical.xp,
                    originalXP = tankJson.Historical.originalXP,
                    damageAssistedRadio = tankJson.Historical.damageAssistedRadio,
                    damageAssistedTrack = tankJson.Historical.damageAssistedTrack,
                    shotsReceived = tankJson.Historical.shotsReceived,
                    noDamageShotsReceived = tankJson.Historical.noDamageShotsReceived,
                    piercedReceived = tankJson.Historical.piercedReceived,
                    heHitsReceived = tankJson.Historical.heHitsReceived,
                    he_hits = tankJson.Historical.he_hits,
                    pierced = tankJson.Historical.pierced,
                    maxDamage = tankJson.MaxHistorical.maxDamage,
                    maxFrags = tankJson.MaxHistorical.maxFrags,
                    maxXP = tankJson.MaxHistorical.maxXP
                };
            }

            if (tankJson.FortBattles != null)
            {
                v2.FortBattles = new StatisticJson
                {
                    battlesCount = tankJson.FortBattles.battlesCount,
                    capturePoints = tankJson.FortBattles.capturePoints,
                    damageDealt = tankJson.FortBattles.damageDealt,
                    damageReceived = tankJson.FortBattles.damageReceived,
                    droppedCapturePoints = tankJson.FortBattles.droppedCapturePoints,
                    frags = tankJson.FortBattles.frags,
                    frags8p = tankJson.FortBattles.frags8p,
                    hits = tankJson.FortBattles.hits,
                    losses = tankJson.FortBattles.losses,
                    shots = tankJson.FortBattles.shots,
                    spotted = tankJson.FortBattles.spotted,
                    survivedBattles = tankJson.FortBattles.survivedBattles,
                    winAndSurvived = tankJson.FortBattles.winAndSurvived,
                    wins = tankJson.FortBattles.wins,
                    xp = tankJson.FortBattles.xp,
                    originalXP = tankJson.FortBattles.originalXP,
                    damageAssistedRadio = tankJson.FortBattles.damageAssistedRadio,
                    damageAssistedTrack = tankJson.FortBattles.damageAssistedTrack,
                    shotsReceived = tankJson.FortBattles.shotsReceived,
                    noDamageShotsReceived = tankJson.FortBattles.noDamageShotsReceived,
                    piercedReceived = tankJson.FortBattles.piercedReceived,
                    heHitsReceived = tankJson.FortBattles.heHitsReceived,
                    he_hits = tankJson.FortBattles.he_hits,
                    pierced = tankJson.FortBattles.pierced,
                    damageBlockedByArmor = tankJson.FortBattles.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.FortBattles.potentialDamageReceived,
                    //maxDamage = tankJson.Max7x7.maxDamage,
                    //maxFrags = tankJson.Max7x7.maxFrags,
                    //maxXP = tankJson.Max7x7.maxXP
                };
            }

            if (tankJson.FortSorties != null)
            {
                v2.FortSorties = new StatisticJson
                {
                    battlesCount = tankJson.FortSorties.battlesCount,
                    capturePoints = tankJson.FortSorties.capturePoints,
                    damageDealt = tankJson.FortSorties.damageDealt,
                    damageReceived = tankJson.FortSorties.damageReceived,
                    droppedCapturePoints = tankJson.FortSorties.droppedCapturePoints,
                    frags = tankJson.FortSorties.frags,
                    frags8p = tankJson.FortSorties.frags8p,
                    hits = tankJson.FortSorties.hits,
                    losses = tankJson.FortSorties.losses,
                    shots = tankJson.FortSorties.shots,
                    spotted = tankJson.FortSorties.spotted,
                    survivedBattles = tankJson.FortSorties.survivedBattles,
                    winAndSurvived = tankJson.FortSorties.winAndSurvived,
                    wins = tankJson.FortSorties.wins,
                    xp = tankJson.FortSorties.xp,
                    originalXP = tankJson.FortSorties.originalXP,
                    damageAssistedRadio = tankJson.FortSorties.damageAssistedRadio,
                    damageAssistedTrack = tankJson.FortSorties.damageAssistedTrack,
                    shotsReceived = tankJson.FortSorties.shotsReceived,
                    noDamageShotsReceived = tankJson.FortSorties.noDamageShotsReceived,
                    piercedReceived = tankJson.FortSorties.piercedReceived,
                    heHitsReceived = tankJson.FortSorties.heHitsReceived,
                    he_hits = tankJson.FortSorties.he_hits,
                    pierced = tankJson.FortSorties.pierced,
                    damageBlockedByArmor = tankJson.FortSorties.damageBlockedByArmor,
                    potentialDamageReceived = tankJson.FortSorties.potentialDamageReceived,
                    //maxDamage = tankJson.Max7x7.maxDamage,
                    //maxFrags = tankJson.Max7x7.maxFrags,
                    //maxXP = tankJson.Max7x7.maxXP
                };
            }

            v2.Common = new CommonJson();
            v2.Common.basedonversion = tankJson.Common.basedonversion;
            v2.Common.compactDescr = tankJson.Common.compactDescr;
            v2.Common.countryid = tankJson.Common.countryid;
            v2.Common.creationTime = tankJson.Common.creationTime;
            v2.Common.creationTimeR = tankJson.Common.creationTimeR;
            v2.Common.frags = tankJson.Common.frags;
            v2.Common.frags_compare = tankJson.Common.frags_compare;
            v2.Common.has_15x15 = 1;
            v2.Common.has_7x7 = 0;
            v2.Common.has_clan = tankJson.Clan != null ? 1 : 0;
            v2.Common.has_company = tankJson.Company != null ? 1 : 0;
            v2.Common.lastBattleTime = tankJson.Common.lastBattleTime;
            v2.Common.lastBattleTimeR = tankJson.Common.lastBattleTimeR;
            v2.Common.premium = tankJson.Common.premium;
            v2.Common.tankid = tankJson.Common.tankid;
            v2.Common.tanktitle = tankJson.Common.tanktitle;
            v2.Common.tier = tankJson.Common.tier;
            v2.Common.type = tankJson.Common.type;
            v2.Common.updated = tankJson.Common.updated;
            v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.Common.updated);
            v2.Common.battleLifeTime = tankJson.Total.battleLifeTime;
            v2.Common.mileage = tankJson.Total.mileage;
            v2.Common.treesCut = tankJson.Total.treesCut;

            return v2;
        }

        /// <summary>
        /// Converts the tank json data from http://wot-dossier.appspot.com/.
        /// </summary>
        /// <param name="tankJson">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(Tank tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson._15x15.battles,
                battlesCountBefore8_8 = tankJson.amounts.battles,
                capturePoints = tankJson._15x15.capture_points,
                damageDealt = tankJson._15x15.damage_dealt,
                damageReceived = tankJson._15x15.damage_received,
                droppedCapturePoints = tankJson._15x15.defence_points,
                frags = tankJson._15x15.frags,
                frags8p = tankJson._15x15.tier8_frags,
                hits = tankJson._15x15.hits,
                losses = tankJson._15x15.losses,
                shots = tankJson._15x15.shots,
                spotted = tankJson._15x15.spotted,
                survivedBattles = tankJson._15x15.survived,
                winAndSurvived = tankJson._15x15.survived_with_victory,
                wins = tankJson._15x15.victories,
                xp = tankJson._15x15.experience,
                xpBefore8_8 = tankJson._15x15.experience,
                originalXP = 0,
                damageAssistedRadio = 0,
                damageAssistedTrack = 0,
                shotsReceived = 0,
                noDamageShotsReceived = 0,
                piercedReceived = 0,
                heHitsReceived = 0,
                he_hits = 0,
                pierced = 0,
                maxFrags = tankJson._15x15.max_frags,
                maxXP = tankJson._15x15.max_experience
            };

            //v2.A15x15.maxDamage = ;

            v2.FragsList = tankJson.frag_counts;

            //achievements 15x15
            v2.Achievements = new AchievementsJson
            {
                MasterGunner = tankJson.series.master_gunner,
                BattleHero = tankJson.awards.battle_hero,
                Hunter = tankJson.awards.hunter,
                Bombardier = tankJson.awards.bombardier,
                Defender = tankJson.awards.defender,
                Survivor = tankJson.series.survivor,
                SurvivorProgress = tankJson.series.survivor_progress,
                PatrolDuty = tankJson.awards.patrol_duty,
                FragsBeast = tankJson.amounts.beast_frags,
                FragsPatton = tankJson.amounts.patton_frags,
                FragsSinai = tankJson.amounts.sinai_frags,
                Reaper = tankJson.awards.reaper,
                HeroesOfRassenay = tankJson.epics.heroes_of_raseiniai,
                Huntsman = tankJson.awards.ranger,
                Invader = tankJson.awards.invader,
                Invincible = tankJson.series.invincible,
                InvincibleProgress = tankJson.series.invincible_progress,
                IronMan = tankJson.awards.cool_headed,
                Kamikaze = tankJson.awards.kamikaze,
                ReaperProgress = tankJson.series.reaper_progress,
                LuckyDevil = tankJson.awards.lucky_devil,
                Lumberjack = 0,
                SurvivorLongest = tankJson.series.survivor,
                InvincibleLongest = tankJson.series.invincible_progress,
                ReaperLongest = tankJson.series.reaper,
                MasterGunnerLongest = tankJson.series.master_gunner,
                SharpshooterLongest = tankJson.series.sharpshooter,
                Abrams = tankJson.medals.abrams,
                Billotte = tankJson.epics.billotte,
                BrothersInArms = tankJson.awards.brothers_in_arms,
                BrunoPietro = tankJson.epics.bruno_pietro,
                Burda = tankJson.epics.burda,
                Carius = tankJson.medals.carius,
                CrucialContribution = tankJson.awards.crucial_contribution,
                DeLanglade = tankJson.epics.de_langlade,
                Dumitru = tankJson.epics.dumitru,
                Ekins = tankJson.medals.ekins,
                Fadin = tankJson.epics.fadin,
                Halonen = tankJson.epics.halonen,
                Kay = tankJson.medals.kay,
                Knispel = tankJson.medals.knispel,
                Kolobanov = tankJson.epics.kolobanov,
                LafayettePool = tankJson.epics.lafayette_pool,
                Lavrinenko = tankJson.medals.lavrinenko,
                Leclerk = tankJson.medals.leclerk,
                Lehvaslaiho = tankJson.epics.lehvaslaiho,
                Nikolas = tankJson.epics.nikolas,
                Orlik = tankJson.epics.orlik,
                Oskin = tankJson.epics.oskin,
                Pascucci = tankJson.epics.pascucci,
                Poppel = tankJson.medals.poppel,
                RadleyWalters = tankJson.epics.radley_walters,
                TamadaYoshio = tankJson.epics.tamada_yoshio,
                Tarczay = tankJson.epics.tarczay,
                Boelter = tankJson.epics.boelter,
                MouseTrap = tankJson.awards.mouse_trap,
                PattonValley = tankJson.awards.patton_valley,
                MasterGunnerProgress = tankJson.series.master_gunner_progress,
                Raider = tankJson.awards.raider,
                Scout = tankJson.awards.scout,
                Sinai = tankJson.awards.sinai,
                Sniper = tankJson.awards.sniper,
                SharpshooterProgress = tankJson.series.sharpshooter_progress,
                SteelWall = tankJson.awards.steel_wall,
                Sturdy = tankJson.awards.spartan,
                Confederate = tankJson.awards.confederate,
                TankExpertStrg = 0,
                Sharpshooter = tankJson.series.sharpshooter,
                Warrior = tankJson.awards.top_gun,
                MarkOfMastery = tankJson.awards.mastery_mark
            };

            v2.Common = new CommonJson();
            v2.Common.basedonversion = tankJson.version;
            v2.Common.compactDescr = 0;
            v2.Common.countryid = tankJson.country;
            v2.Common.creationTime = 0;
            v2.Common.creationTimeR = DateTime.MinValue;
            v2.Common.frags = tankJson._15x15.frags;
            v2.Common.frags_compare = 0;
            v2.Common.has_15x15 = 1;
            v2.Common.has_7x7 = 0;
            v2.Common.has_clan = 0;
            v2.Common.has_company = 0;
            v2.Common.lastBattleTime = tankJson.last_time_played;
            v2.Common.lastBattleTimeR = Utils.UnixDateToDateTime(tankJson.last_time_played);
            v2.Common.tankid = tankJson.id;
            v2.Common.premium = Dictionaries.Instance.Tanks[v2.UniqueId()].Premium;
            v2.Common.tanktitle = Dictionaries.Instance.Tanks[v2.UniqueId()].Title;
            v2.Common.tier = Dictionaries.Instance.Tanks[v2.UniqueId()].Tier;
            v2.Common.type = Dictionaries.Instance.Tanks[v2.UniqueId()].Type;
            v2.Common.updated = tankJson.updated;
            v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.updated);
            v2.Common.battleLifeTime = tankJson.play_time;
            v2.Common.mileage = 0;
            v2.Common.treesCut = tankJson.amounts.trees_knocked_down;

            return v2;
        }

        /// <summary>
        /// Converts the specified tank json from WOT API.
        /// </summary>
        /// <param name="tankJson">The tank json.</param>
        /// <returns></returns>
        public static TankJson Map(Vehicle tankJson)
        {
            TankJson v2 = new TankJson();

            //statistic 15x15
            v2.A15x15 = new StatisticJson
            {
                battlesCount = tankJson.all.battles,
                battlesCountBefore8_8 = tankJson.all.battles,
                capturePoints = tankJson.all.capture_points,
                damageDealt = tankJson.all.damage_dealt,
                damageReceived = tankJson.all.damage_received,
                droppedCapturePoints = tankJson.all.dropped_capture_points,
                frags = tankJson.all.frags,
                frags8p = tankJson.all.frags,
                hits = tankJson.all.hits,
                losses = tankJson.all.losses,
                shots = tankJson.all.shots,
                spotted = tankJson.all.spotted,
                survivedBattles = tankJson.all.survived_battles,
                winAndSurvived = tankJson.all.survived_battles,
                wins = tankJson.all.wins,
                xp = tankJson.all.xp,
                xpBefore8_8 = tankJson.all.xp,
                originalXP = 0,
                damageAssistedRadio = 0,
                damageAssistedTrack = 0,
                shotsReceived = 0,
                noDamageShotsReceived = 0,
                piercedReceived = 0,
                heHitsReceived = 0,
                he_hits = 0,
                pierced = 0,
                maxFrags = tankJson.max_frags,
                maxXP = tankJson.max_xp
            };

            //v2.A15x15.maxDamage = tankJson;

            v2.FragsList = new List<IList<string>>();

            //achievements 15x15
            v2.Achievements = new AchievementsJson();
            //v2.Achievements.armorPiercer = tankJson.series.master_gunner;
            //v2.Achievements.battleHeroes = tankJson.awards.battle_hero;
            //v2.Achievements.beasthunter = tankJson.awards.hunter;
            //v2.Achievements.bombardier = tankJson.awards.bombardier;
            //v2.Achievements.defender = tankJson.awards.defender;
            //v2.Achievements.diehard = tankJson.series.survivor;
            //v2.Achievements.diehardSeries = tankJson.series.survivor_progress;
            //v2.Achievements.evileye = tankJson.awards.patrol_duty;
            //v2.Achievements.fragsBeast = tankJson.amounts.beast_frags;
            //v2.Achievements.fragsPatton = tankJson.amounts.patton_frags;
            //v2.Achievements.fragsSinai = tankJson.amounts.sinai_frags;
            //v2.Achievements.handOfDeath = tankJson.awards.reaper;
            //v2.Achievements.heroesOfRassenay = tankJson.epics.heroes_of_raseiniai;
            //v2.Achievements.huntsman = tankJson.awards.ranger;
            //v2.Achievements.invader = tankJson.awards.invader;
            //v2.Achievements.invincible = tankJson.series.invincible;
            //v2.Achievements.invincibleSeries = tankJson.series.invincible_progress;
            //v2.Achievements.ironMan = tankJson.awards.cool_headed;
            //v2.Achievements.kamikaze = tankJson.awards.kamikaze;
            //v2.Achievements.killingSeries = tankJson.series.reaper_progress;
            //v2.Achievements.luckyDevil = tankJson.awards.lucky_devil;
            //v2.Achievements.lumberjack = 0;
            //v2.Achievements.maxDiehardSeries = tankJson.series.survivor;
            //v2.Achievements.maxInvincibleSeries = tankJson.series.invincible_progress;
            //v2.Achievements.maxKillingSeries = tankJson.series.reaper;
            //v2.Achievements.maxPiercingSeries = tankJson.series.master_gunner;
            //v2.Achievements.maxSniperSeries = tankJson.series.sharpshooter;
            //v2.Achievements.medalAbrams = tankJson.medals.abrams;
            //v2.Achievements.medalBillotte = tankJson.epics.billotte;
            //v2.Achievements.medalBrothersInArms = tankJson.awards.brothers_in_arms;
            //v2.Achievements.medalBrunoPietro = tankJson.epics.bruno_pietro;
            //v2.Achievements.medalBurda = tankJson.epics.burda;
            //v2.Achievements.medalCarius = tankJson.medals.carius;
            //v2.Achievements.medalCrucialContribution = tankJson.awards.crucial_contribution;
            //v2.Achievements.medalDeLanglade = tankJson.epics.de_langlade;
            //v2.Achievements.medalDumitru = tankJson.epics.dumitru;
            //v2.Achievements.medalEkins = tankJson.medals.ekins;
            //v2.Achievements.medalFadin = tankJson.epics.fadin;
            //v2.Achievements.medalHalonen = tankJson.epics.halonen;
            //v2.Achievements.medalKay = tankJson.medals.kay;
            //v2.Achievements.medalKnispel = tankJson.medals.knispel;
            //v2.Achievements.medalKolobanov = tankJson.epics.kolobanov;
            //v2.Achievements.medalLafayettePool = tankJson.epics.lafayette_pool;
            //v2.Achievements.medalLavrinenko = tankJson.medals.lavrinenko;
            //v2.Achievements.medalLeClerc = tankJson.medals.leclerk;
            //v2.Achievements.medalLehvaslaiho = tankJson.epics.lehvaslaiho;
            //v2.Achievements.medalNikolas = tankJson.epics.nikolas;
            //v2.Achievements.medalOrlik = tankJson.epics.orlik;
            //v2.Achievements.medalOskin = tankJson.epics.oskin;
            //v2.Achievements.medalPascucci = tankJson.epics.pascucci;
            //v2.Achievements.medalPoppel = tankJson.medals.poppel;
            //v2.Achievements.medalRadleyWalters = tankJson.epics.radley_walters;
            //v2.Achievements.medalTamadaYoshio = tankJson.epics.tamada_yoshio;
            //v2.Achievements.medalTarczay = tankJson.epics.tarczay;
            //v2.Achievements.medalWittmann = tankJson.epics.boelter;
            //v2.Achievements.mousebane = tankJson.awards.mouse_trap;
            //v2.Achievements.pattonValley = tankJson.awards.patton_valley;
            //v2.Achievements.piercingSeries = tankJson.series.master_gunner_progress;
            //v2.Achievements.raider = tankJson.awards.raider;
            //v2.Achievements.scout = tankJson.awards.scout;
            //v2.Achievements.sinai = tankJson.awards.sinai;
            //v2.Achievements.sniper = tankJson.awards.sniper;
            //v2.Achievements.sniperSeries = tankJson.series.sharpshooter_progress;
            //v2.Achievements.steelwall = tankJson.awards.steel_wall;
            //v2.Achievements.sturdy = tankJson.awards.spartan;
            //v2.Achievements.supporter = tankJson.awards.confederate;
            //v2.Achievements.tankExpertStrg = 0;
            //v2.Achievements.titleSniper = tankJson.series.sharpshooter;
            //v2.Achievements.warrior = tankJson.awards.top_gun;
            v2.Achievements.MarkOfMastery = tankJson.mark_of_mastery;

            v2.Common = new CommonJson();
            v2.Common.basedonversion = 65;
            v2.Common.compactDescr = 0;
            v2.Common.countryid = tankJson.description.CountryId;
            v2.Common.creationTime = 0;
            v2.Common.creationTimeR = DateTime.MinValue;
            v2.Common.frags = tankJson.all.frags;
            v2.Common.frags_compare = 0;
            v2.Common.has_15x15 = 1;
            v2.Common.has_7x7 = 0;
            v2.Common.has_clan = 0;
            v2.Common.has_company = 0;
            v2.Common.tankid = tankJson.description.TankId;
            v2.Common.premium = Dictionaries.Instance.Tanks[v2.UniqueId()].Premium;
            v2.Common.tanktitle = Dictionaries.Instance.Tanks[v2.UniqueId()].Title;
            v2.Common.tier = Dictionaries.Instance.Tanks[v2.UniqueId()].Tier;
            v2.Common.type = Dictionaries.Instance.Tanks[v2.UniqueId()].Type;
            v2.Common.mileage = 0;
            //v2.Common.lastBattleTime = tankJson.last_time_played;
            //v2.Common.lastBattleTimeR = Utils.UnixDateToDateTime(tankJson.last_time_played);
            //v2.Common.updated = tankJson.updated;
            //v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.updated);
            //v2.Common.battleLifeTime = tankJson.play_time;
            //v2.Common.treesCut = tankJson.amounts.trees_knocked_down;
            v2.Description = tankJson.description;
            return v2;
        }

        public static TankJson Map(JToken tankJson, int version)
        {
            if (version <= 29)
            {
                return Map(tankJson.ToObject<TankJson29>());
            }
            if (version <= 69)
            {
                return Map(tankJson.ToObject<TankJson65>());
            }
            if (version < 85)
            {
                return Map(tankJson.ToObject<TankJson77>());
            }
            return Map(tankJson.ToObject<TankJson85>());
        }
    }
}
