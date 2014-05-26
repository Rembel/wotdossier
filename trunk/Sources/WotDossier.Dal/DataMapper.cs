using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WotDossier.Common;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Dossier.TankV29;
using WotDossier.Domain.Dossier.TankV65;
using WotDossier.Domain.Dossier.TankV77;
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
                armorPiercer = tankJson.Special.armorPiercer,
                battleHeroes = tankJson.Battle.battleHeroes,
                beasthunter = tankJson.Special.beasthunter,
                bombardier = tankJson.Special.bombardier,
                defender = tankJson.Battle.defender,
                diehard = tankJson.Special.diehard,
                diehardSeries = tankJson.Series.diehardSeries,
                evileye = tankJson.Battle.evileye,
                fragsBeast = tankJson.Tankdata.fragsBeast,
                fragsPatton = tankJson.Special.fragsPatton,
                fragsSinai = tankJson.Battle.fragsSinai,
                handOfDeath = tankJson.Special.handOfDeath,
                heroesOfRassenay = tankJson.Special.heroesOfRassenay,
                huntsman = tankJson.Special.huntsman,
                invader = tankJson.Battle.invader,
                invincible = tankJson.Special.invincible,
                invincibleSeries = tankJson.Series.invincibleSeries,
                ironMan = tankJson.Special.ironMan,
                kamikaze = tankJson.Special.kamikaze,
                killingSeries = tankJson.Series.killingSeries,
                luckyDevil = tankJson.Special.luckyDevil,
                lumberjack = tankJson.Special.lumberjack,
                maxDiehardSeries = tankJson.Series.maxDiehardSeries,
                maxInvincibleSeries = tankJson.Series.maxInvincibleSeries,
                maxKillingSeries = tankJson.Series.maxKillingSeries,
                maxPiercingSeries = tankJson.Series.maxPiercingSeries,
                maxSniperSeries = tankJson.Series.maxSniperSeries,
                medalAbrams = tankJson.Major.Abrams,
                medalBillotte = tankJson.Epic.Billotte,
                medalBrothersInArms = tankJson.Epic.BrothersInArms,
                medalBrunoPietro = tankJson.Epic.BrunoPietro,
                medalBurda = tankJson.Epic.Burda,
                medalCarius = tankJson.Major.Carius,
                medalCrucialContribution = tankJson.Epic.CrucialContribution,
                medalDeLanglade = tankJson.Epic.DeLanglade,
                medalDumitru = tankJson.Epic.Dumitru,
                medalEkins = tankJson.Major.Ekins,
                medalFadin = tankJson.Epic.Fadin,
                medalHalonen = tankJson.Epic.Halonen,
                medalKay = tankJson.Major.Kay,
                medalKnispel = tankJson.Major.Knispel,
                medalKolobanov = tankJson.Epic.Kolobanov,
                medalLafayettePool = tankJson.Epic.LafayettePool,
                medalLavrinenko = tankJson.Major.Lavrinenko,
                medalLeClerc = tankJson.Major.LeClerc,
                medalLehvaslaiho = tankJson.Epic.Lehvaslaiho,
                medalNikolas = tankJson.Epic.Nikolas,
                medalOrlik = tankJson.Epic.Orlik,
                medalOskin = tankJson.Epic.Oskin,
                medalPascucci = tankJson.Epic.Pascucci,
                medalPoppel = tankJson.Major.Poppel,
                medalRadleyWalters = tankJson.Epic.RadleyWalters,
                medalTamadaYoshio = tankJson.Epic.TamadaYoshio,
                medalTarczay = tankJson.Epic.Tarczay,
                medalWittmann = tankJson.Epic.Boelter,
                mousebane = tankJson.Special.mousebane,
                pattonValley = tankJson.Special.pattonValley,
                piercingSeries = tankJson.Series.piercingSeries,
                raider = tankJson.Special.raider,
                scout = tankJson.Battle.scout,
                sinai = tankJson.Special.sinai,
                sniper = tankJson.Battle.sniper,
                sniperSeries = tankJson.Series.sniperSeries,
                steelwall = tankJson.Battle.steelwall,
                sturdy = tankJson.Special.sturdy,
                supporter = tankJson.Battle.supporter,
                tankExpertStrg = tankJson.Special.tankExpertStrg,
                titleSniper = tankJson.Special.titleSniper,
                warrior = tankJson.Battle.warrior,
                markOfMastery = tankJson.Special.markOfMastery
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
                armorPiercer = tankJson.Achievements.armorPiercer,
                battleHeroes = tankJson.Achievements.battleHeroes,
                beasthunter = tankJson.Achievements.beasthunter,
                bombardier = tankJson.Achievements.bombardier,
                defender = tankJson.Achievements.defender,
                diehard = tankJson.Achievements.diehard,
                diehardSeries = tankJson.Achievements.diehardSeries,
                evileye = tankJson.Achievements.evileye,
                fragsBeast = tankJson.Achievements.fragsBeast,
                fragsPatton = tankJson.Achievements.fragsPatton,
                fragsSinai = tankJson.Achievements.fragsSinai,
                handOfDeath = tankJson.Achievements.handOfDeath,
                heroesOfRassenay = tankJson.Achievements.heroesOfRassenay,
                huntsman = tankJson.Achievements.huntsman,
                invader = tankJson.Achievements.invader,
                invincible = tankJson.Achievements.invincible,
                invincibleSeries = tankJson.Achievements.invincibleSeries,
                ironMan = tankJson.Achievements.ironMan,
                kamikaze = tankJson.Achievements.kamikaze,
                killingSeries = tankJson.Achievements.killingSeries,
                luckyDevil = tankJson.Achievements.luckyDevil,
                lumberjack = tankJson.Achievements.lumberjack,
                maxDiehardSeries = tankJson.Achievements.maxDiehardSeries,
                maxInvincibleSeries = tankJson.Achievements.maxInvincibleSeries,
                maxKillingSeries = tankJson.Achievements.maxKillingSeries,
                maxPiercingSeries = tankJson.Achievements.maxPiercingSeries,
                maxSniperSeries = tankJson.Achievements.maxSniperSeries,
                medalAbrams = tankJson.Achievements.medalAbrams,
                medalBillotte = tankJson.Achievements.medalBillotte,
                medalBrothersInArms = tankJson.Achievements.medalBrothersInArms,
                medalBrunoPietro = tankJson.Achievements.medalBrunoPietro,
                medalBurda = tankJson.Achievements.medalBurda,
                medalCarius = tankJson.Achievements.medalCarius,
                medalCrucialContribution = tankJson.Achievements.medalCrucialContribution,
                medalDeLanglade = tankJson.Achievements.medalDeLanglade,
                medalDumitru = tankJson.Achievements.medalDumitru,
                medalEkins = tankJson.Achievements.medalEkins,
                medalFadin = tankJson.Achievements.medalFadin,
                medalHalonen = tankJson.Achievements.medalHalonen,
                medalKay = tankJson.Achievements.medalKay,
                medalKnispel = tankJson.Achievements.medalKnispel,
                medalKolobanov = tankJson.Achievements.medalKolobanov,
                medalLafayettePool = tankJson.Achievements.medalLafayettePool,
                medalLavrinenko = tankJson.Achievements.medalLavrinenko,
                medalLeClerc = tankJson.Achievements.medalLeClerc,
                medalLehvaslaiho = tankJson.Achievements.medalLehvaslaiho,
                medalNikolas = tankJson.Achievements.medalNikolas,
                medalOrlik = tankJson.Achievements.medalOrlik,
                medalOskin = tankJson.Achievements.medalOskin,
                medalPascucci = tankJson.Achievements.medalPascucci,
                medalPoppel = tankJson.Achievements.medalPoppel,
                medalRadleyWalters = tankJson.Achievements.medalRadleyWalters,
                medalTamadaYoshio = tankJson.Achievements.medalTamadaYoshio,
                medalTarczay = tankJson.Achievements.medalTarczay,
                medalWittmann = tankJson.Achievements.medalWittmann,
                mousebane = tankJson.Achievements.mousebane,
                pattonValley = tankJson.Achievements.pattonValley,
                piercingSeries = tankJson.Achievements.piercingSeries,
                raider = tankJson.Achievements.raider,
                scout = tankJson.Achievements.scout,
                sinai = tankJson.Achievements.sinai,
                sniper = tankJson.Achievements.sniper,
                sniper2 = tankJson.Achievements.sniper2,
                mainGun = tankJson.Achievements.mainGun,
                sniperSeries = tankJson.Achievements.sniperSeries,
                steelwall = tankJson.Achievements.steelwall,
                sturdy = tankJson.Achievements.sturdy,
                supporter = tankJson.Achievements.supporter,
                tankExpertStrg = tankJson.Achievements.tankExpertStrg,
                titleSniper = tankJson.Achievements.titleSniper,
                warrior = tankJson.Achievements.warrior,
                markOfMastery = tankJson.Achievements.markOfMastery
            };

            //achievements 7x7
            v2.Achievements7x7 = new Achievements7x7
            {
                armoredFist = tankJson.Achievements7X7.armoredFist,
                geniusForWar = tankJson.Achievements7X7.geniusForWar,
                geniusForWarMedal = tankJson.Achievements7X7.geniusForWarMedal,
                kingOfTheHill = tankJson.Achievements7X7.kingOfTheHill,
                maxTacticalBreakthroughSeries = tankJson.Achievements7X7.maxTacticalBreakthroughSeries,
                tacticalBreakthrough = tankJson.Achievements7X7.tacticalBreakthrough,
                tacticalBreakthroughSeries = tankJson.Achievements7X7.tacticalBreakthroughSeries,
                wolfAmongSheep = tankJson.Achievements7X7.wolfAmongSheep,
                wolfAmongSheepMedal = tankJson.Achievements7X7.wolfAmongSheepMedal
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
                armorPiercer = tankJson.Achievements.armorPiercer,
                battleHeroes = tankJson.Achievements.battleHeroes,
                beasthunter = tankJson.Achievements.beasthunter,
                bombardier = tankJson.Achievements.bombardier,
                defender = tankJson.Achievements.defender,
                diehard = tankJson.Achievements.diehard,
                diehardSeries = tankJson.Achievements.diehardSeries,
                evileye = tankJson.Achievements.evileye,
                fragsBeast = tankJson.Achievements.fragsBeast,
                fragsPatton = tankJson.Achievements.fragsPatton,
                fragsSinai = tankJson.Achievements.fragsSinai,
                handOfDeath = tankJson.Achievements.handOfDeath,
                heroesOfRassenay = tankJson.Achievements.heroesOfRassenay,
                huntsman = tankJson.Achievements.huntsman,
                invader = tankJson.Achievements.invader,
                invincible = tankJson.Achievements.invincible,
                invincibleSeries = tankJson.Achievements.invincibleSeries,
                ironMan = tankJson.Achievements.ironMan,
                kamikaze = tankJson.Achievements.kamikaze,
                killingSeries = tankJson.Achievements.killingSeries,
                luckyDevil = tankJson.Achievements.luckyDevil,
                lumberjack = tankJson.Achievements.lumberjack,
                maxDiehardSeries = tankJson.Achievements.maxDiehardSeries,
                maxInvincibleSeries = tankJson.Achievements.maxInvincibleSeries,
                maxKillingSeries = tankJson.Achievements.maxKillingSeries,
                maxPiercingSeries = tankJson.Achievements.maxPiercingSeries,
                maxSniperSeries = tankJson.Achievements.maxSniperSeries,
                medalAbrams = tankJson.Achievements.medalAbrams,
                medalBillotte = tankJson.Achievements.medalBillotte,
                medalBrothersInArms = tankJson.Achievements.medalBrothersInArms,
                medalBrunoPietro = tankJson.Achievements.medalBrunoPietro,
                medalBurda = tankJson.Achievements.medalBurda,
                medalCarius = tankJson.Achievements.medalCarius,
                medalCrucialContribution = tankJson.Achievements.medalCrucialContribution,
                medalDeLanglade = tankJson.Achievements.medalDeLanglade,
                medalDumitru = tankJson.Achievements.medalDumitru,
                medalEkins = tankJson.Achievements.medalEkins,
                medalFadin = tankJson.Achievements.medalFadin,
                medalHalonen = tankJson.Achievements.medalHalonen,
                medalKay = tankJson.Achievements.medalKay,
                medalKnispel = tankJson.Achievements.medalKnispel,
                medalKolobanov = tankJson.Achievements.medalKolobanov,
                medalLafayettePool = tankJson.Achievements.medalLafayettePool,
                medalLavrinenko = tankJson.Achievements.medalLavrinenko,
                medalLeClerc = tankJson.Achievements.medalLeClerc,
                medalLehvaslaiho = tankJson.Achievements.medalLehvaslaiho,
                medalNikolas = tankJson.Achievements.medalNikolas,
                medalOrlik = tankJson.Achievements.medalOrlik,
                medalOskin = tankJson.Achievements.medalOskin,
                medalPascucci = tankJson.Achievements.medalPascucci,
                medalPoppel = tankJson.Achievements.medalPoppel,
                medalRadleyWalters = tankJson.Achievements.medalRadleyWalters,
                medalTamadaYoshio = tankJson.Achievements.medalTamadaYoshio,
                medalTarczay = tankJson.Achievements.medalTarczay,
                medalWittmann = tankJson.Achievements.medalWittmann,
                mousebane = tankJson.Achievements.mousebane,
                pattonValley = tankJson.Achievements.pattonValley,
                piercingSeries = tankJson.Achievements.piercingSeries,
                raider = tankJson.Achievements.raider,
                scout = tankJson.Achievements.scout,
                sinai = tankJson.Achievements.sinai,
                sniper = tankJson.Achievements.sniper,
                sniper2 = tankJson.Achievements.sniper2,
                mainGun = tankJson.Achievements.mainGun,
                sniperSeries = tankJson.Achievements.sniperSeries,
                steelwall = tankJson.Achievements.steelwall,
                sturdy = tankJson.Achievements.sturdy,
                supporter = tankJson.Achievements.supporter,
                tankExpertStrg = tankJson.Achievements.tankExpertStrg,
                titleSniper = tankJson.Achievements.titleSniper,
                warrior = tankJson.Achievements.warrior,
                markOfMastery = tankJson.Achievements.markOfMastery,

                marksOnGun = tankJson.Achievements.marksOnGun,
                movingAvgDamage = tankJson.Achievements.movingAvgDamage,
                medalMonolith = tankJson.Achievements.medalMonolith,
                medalAntiSpgFire = tankJson.Achievements.medalAntiSpgFire,
                medalGore = tankJson.Achievements.medalGore,
                medalCoolBlood = tankJson.Achievements.medalCoolBlood,
                medalStark = tankJson.Achievements.medalStark,
                damageRating = tankJson.Achievements.damageRating,
            };

            //achievements 7x7
            v2.Achievements7x7 = new Achievements7x7
            {
                armoredFist = tankJson.Achievements7X7.armoredFist,
                geniusForWar = tankJson.Achievements7X7.geniusForWar,
                geniusForWarMedal = tankJson.Achievements7X7.geniusForWarMedal,
                kingOfTheHill = tankJson.Achievements7X7.kingOfTheHill,
                maxTacticalBreakthroughSeries = tankJson.Achievements7X7.maxTacticalBreakthroughSeries,
                tacticalBreakthrough = tankJson.Achievements7X7.tacticalBreakthrough,
                tacticalBreakthroughSeries = tankJson.Achievements7X7.tacticalBreakthroughSeries,
                wolfAmongSheep = tankJson.Achievements7X7.wolfAmongSheep,
                wolfAmongSheepMedal = tankJson.Achievements7X7.wolfAmongSheepMedal,
                godOfWar = tankJson.Achievements7X7.godOfWar,
                fightingReconnaissance = tankJson.Achievements7X7.fightingReconnaissance,
                fightingReconnaissanceMedal = tankJson.Achievements7X7.fightingReconnaissanceMedal,
                willToWinSpirit = tankJson.Achievements7X7.willToWinSpirit,
                crucialShot = tankJson.Achievements7X7.crucialShot,
                crucialShotMedal = tankJson.Achievements7X7.crucialShotMedal,
                forTacticalOperations = tankJson.Achievements7X7.forTacticalOperations,
                
                promisingFighter = tankJson.Achievements7X7.promisingFighter,
                promisingFighterMedal = tankJson.Achievements7X7.promisingFighterMedal,
                heavyFire = tankJson.Achievements7X7.heavyFire,
                heavyFireMedal = tankJson.Achievements7X7.heavyFireMedal,
                ranger = tankJson.Achievements7X7.ranger,
                rangerMedal = tankJson.Achievements7X7.rangerMedal,
                fireAndSteel = tankJson.Achievements7X7.fireAndSteel,
                fireAndSteelMedal = tankJson.Achievements7X7.fireAndSteelMedal,
                pyromaniac = tankJson.Achievements7X7.pyromaniac,
                pyromaniacMedal = tankJson.Achievements7X7.pyromaniacMedal,
                noMansLand = tankJson.Achievements7X7.noMansLand,
            };

            v2.AchievementsHistorical = new AchievementsHistorical
            {
                bothSidesWins = tankJson.HistoricalAchievements.bothSidesWins,
                guardsman = tankJson.HistoricalAchievements.guardsman,
                makerOfHistory = tankJson.HistoricalAchievements.makerOfHistory,
                weakVehiclesWins = tankJson.HistoricalAchievements.weakVehiclesWins
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
                armorPiercer = tankJson.series.master_gunner,
                battleHeroes = tankJson.awards.battle_hero,
                beasthunter = tankJson.awards.hunter,
                bombardier = tankJson.awards.bombardier,
                defender = tankJson.awards.defender,
                diehard = tankJson.series.survivor,
                diehardSeries = tankJson.series.survivor_progress,
                evileye = tankJson.awards.patrol_duty,
                fragsBeast = tankJson.amounts.beast_frags,
                fragsPatton = tankJson.amounts.patton_frags,
                fragsSinai = tankJson.amounts.sinai_frags,
                handOfDeath = tankJson.awards.reaper,
                heroesOfRassenay = tankJson.epics.heroes_of_raseiniai,
                huntsman = tankJson.awards.ranger,
                invader = tankJson.awards.invader,
                invincible = tankJson.series.invincible,
                invincibleSeries = tankJson.series.invincible_progress,
                ironMan = tankJson.awards.cool_headed,
                kamikaze = tankJson.awards.kamikaze,
                killingSeries = tankJson.series.reaper_progress,
                luckyDevil = tankJson.awards.lucky_devil,
                lumberjack = 0,
                maxDiehardSeries = tankJson.series.survivor,
                maxInvincibleSeries = tankJson.series.invincible_progress,
                maxKillingSeries = tankJson.series.reaper,
                maxPiercingSeries = tankJson.series.master_gunner,
                maxSniperSeries = tankJson.series.sharpshooter,
                medalAbrams = tankJson.medals.abrams,
                medalBillotte = tankJson.epics.billotte,
                medalBrothersInArms = tankJson.awards.brothers_in_arms,
                medalBrunoPietro = tankJson.epics.bruno_pietro,
                medalBurda = tankJson.epics.burda,
                medalCarius = tankJson.medals.carius,
                medalCrucialContribution = tankJson.awards.crucial_contribution,
                medalDeLanglade = tankJson.epics.de_langlade,
                medalDumitru = tankJson.epics.dumitru,
                medalEkins = tankJson.medals.ekins,
                medalFadin = tankJson.epics.fadin,
                medalHalonen = tankJson.epics.halonen,
                medalKay = tankJson.medals.kay,
                medalKnispel = tankJson.medals.knispel,
                medalKolobanov = tankJson.epics.kolobanov,
                medalLafayettePool = tankJson.epics.lafayette_pool,
                medalLavrinenko = tankJson.medals.lavrinenko,
                medalLeClerc = tankJson.medals.leclerk,
                medalLehvaslaiho = tankJson.epics.lehvaslaiho,
                medalNikolas = tankJson.epics.nikolas,
                medalOrlik = tankJson.epics.orlik,
                medalOskin = tankJson.epics.oskin,
                medalPascucci = tankJson.epics.pascucci,
                medalPoppel = tankJson.medals.poppel,
                medalRadleyWalters = tankJson.epics.radley_walters,
                medalTamadaYoshio = tankJson.epics.tamada_yoshio,
                medalTarczay = tankJson.epics.tarczay,
                medalWittmann = tankJson.epics.boelter,
                mousebane = tankJson.awards.mouse_trap,
                pattonValley = tankJson.awards.patton_valley,
                piercingSeries = tankJson.series.master_gunner_progress,
                raider = tankJson.awards.raider,
                scout = tankJson.awards.scout,
                sinai = tankJson.awards.sinai,
                sniper = tankJson.awards.sniper,
                sniperSeries = tankJson.series.sharpshooter_progress,
                steelwall = tankJson.awards.steel_wall,
                sturdy = tankJson.awards.spartan,
                supporter = tankJson.awards.confederate,
                tankExpertStrg = 0,
                titleSniper = tankJson.series.sharpshooter,
                warrior = tankJson.awards.top_gun,
                markOfMastery = tankJson.awards.mastery_mark
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
            v2.Achievements.markOfMastery = tankJson.mark_of_mastery;

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
            return Map(tankJson.ToObject<TankJson77>());
        }
    }
}
