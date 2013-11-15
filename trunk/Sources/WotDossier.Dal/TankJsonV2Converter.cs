using System;
using WotDossier.Common;
using WotDossier.Domain.Dossier.AppSpot;
using WotDossier.Domain.Dossier.TankV29;
using WotDossier.Domain.Dossier.TankV65;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    public class TankJsonV2Converter
    {
        public static TankJson Convert(TankJson29 tankJson)
        {
            TankJson v2 = new TankJson();

            v2.A15x15 = new StatisticJson();
            v2.A15x15.battlesCount = tankJson.Tankdata.battlesCount;
            v2.A15x15.battlesCountBefore8_8 = tankJson.Tankdata.battlesCountBefore8_8;
            v2.A15x15.capturePoints = tankJson.Tankdata.capturePoints;
            v2.A15x15.damageDealt = tankJson.Tankdata.damageDealt;
            v2.A15x15.damageReceived = tankJson.Tankdata.damageReceived;
            v2.A15x15.droppedCapturePoints = tankJson.Tankdata.droppedCapturePoints;
            v2.A15x15.frags = tankJson.Tankdata.frags;
            v2.A15x15.frags8p = tankJson.Tankdata.frags8p;
            v2.A15x15.fragsBeast = tankJson.Tankdata.fragsBeast;
            v2.A15x15.hits = tankJson.Tankdata.hits;
            v2.A15x15.losses = tankJson.Tankdata.losses;
            v2.A15x15.shots = tankJson.Tankdata.shots;
            v2.A15x15.spotted = tankJson.Tankdata.spotted;
            v2.A15x15.survivedBattles = tankJson.Tankdata.survivedBattles;
            v2.A15x15.winAndSurvived = tankJson.Tankdata.winAndSurvived;
            v2.A15x15.wins = tankJson.Tankdata.wins;
            v2.A15x15.xp = tankJson.Tankdata.xp;
            v2.A15x15.xpBefore8_8 = tankJson.Tankdata.xpBefore8_8;

            v2.A15x15.originalXP = tankJson.Tankdata.originalXP;
            v2.A15x15.damageAssistedRadio = tankJson.Tankdata.damageAssistedRadio;
            v2.A15x15.damageAssistedTrack = tankJson.Tankdata.damageAssistedTrack;
            v2.A15x15.shotsReceived = tankJson.Tankdata.shotsReceived;
            v2.A15x15.noDamageShotsReceived = tankJson.Tankdata.noDamageShotsReceived;
            v2.A15x15.piercedReceived = tankJson.Tankdata.piercedReceived;
            v2.A15x15.heHitsReceived = tankJson.Tankdata.heHitsReceived;
            v2.A15x15.he_hits = tankJson.Tankdata.he_hits;
            v2.A15x15.pierced = tankJson.Tankdata.pierced;

            //v2.A15x15.maxDamage = ;
            v2.A15x15.maxFrags = tankJson.Tankdata.maxFrags;
            v2.A15x15.maxXP = tankJson.Tankdata.maxXP;

            v2.FragsList = tankJson.Kills;

            v2.Achievements = new AchievementsJson();
            v2.Achievements.armorPiercer = tankJson.Special.armorPiercer;
            v2.Achievements.battleHeroes = tankJson.Battle.battleHeroes;
            v2.Achievements.beasthunter = tankJson.Special.beasthunter;
            v2.Achievements.bombardier = tankJson.Special.bombardier;
            v2.Achievements.defender = tankJson.Battle.defender;
            v2.Achievements.diehard = tankJson.Special.diehard;
            v2.Achievements.diehardSeries = tankJson.Series.diehardSeries;
            v2.Achievements.evileye = tankJson.Battle.evileye;
            v2.Achievements.fragsBeast = tankJson.Tankdata.fragsBeast;
            v2.Achievements.fragsPatton = tankJson.Special.fragsPatton;
            v2.Achievements.fragsSinai = tankJson.Battle.fragsSinai;
            v2.Achievements.handOfDeath = tankJson.Special.handOfDeath;
            v2.Achievements.heroesOfRassenay = tankJson.Special.heroesOfRassenay;
            v2.Achievements.huntsman = tankJson.Special.huntsman;
            v2.Achievements.invader = tankJson.Battle.invader;
            v2.Achievements.invincible = tankJson.Special.invincible;
            v2.Achievements.invincibleSeries = tankJson.Series.invincibleSeries;
            v2.Achievements.ironMan = tankJson.Special.ironMan;
            v2.Achievements.kamikaze = tankJson.Special.kamikaze;
            v2.Achievements.killingSeries = tankJson.Series.killingSeries;
            v2.Achievements.luckyDevil = tankJson.Special.luckyDevil;
            v2.Achievements.lumberjack = tankJson.Special.lumberjack;
            v2.Achievements.maxDiehardSeries = tankJson.Series.maxDiehardSeries;
            v2.Achievements.maxInvincibleSeries = tankJson.Series.maxInvincibleSeries;
            v2.Achievements.maxKillingSeries = tankJson.Series.maxKillingSeries;
            v2.Achievements.maxPiercingSeries = tankJson.Series.maxPiercingSeries;
            v2.Achievements.maxSniperSeries = tankJson.Series.maxSniperSeries;
            v2.Achievements.medalAbrams = tankJson.Major.Abrams;
            v2.Achievements.medalBillotte = tankJson.Epic.Billotte;
            v2.Achievements.medalBrothersInArms = tankJson.Epic.BrothersInArms;
            v2.Achievements.medalBrunoPietro = tankJson.Epic.BrunoPietro;
            v2.Achievements.medalBurda = tankJson.Epic.Burda;
            v2.Achievements.medalCarius = tankJson.Major.Carius;
            v2.Achievements.medalCrucialContribution = tankJson.Epic.CrucialContribution;
            v2.Achievements.medalDeLanglade = tankJson.Epic.DeLanglade;
            v2.Achievements.medalDumitru = tankJson.Epic.Dumitru;
            v2.Achievements.medalEkins = tankJson.Major.Ekins;
            v2.Achievements.medalFadin = tankJson.Epic.Fadin;
            v2.Achievements.medalHalonen = tankJson.Epic.Halonen;
            v2.Achievements.medalKay = tankJson.Major.Kay;
            v2.Achievements.medalKnispel = tankJson.Major.Knispel;
            v2.Achievements.medalKolobanov = tankJson.Epic.Kolobanov;
            v2.Achievements.medalLafayettePool = tankJson.Epic.LafayettePool;
            v2.Achievements.medalLavrinenko = tankJson.Major.Lavrinenko;
            v2.Achievements.medalLeClerc = tankJson.Major.LeClerc;
            v2.Achievements.medalLehvaslaiho = tankJson.Epic.Lehvaslaiho;
            v2.Achievements.medalNikolas = tankJson.Epic.Nikolas;
            v2.Achievements.medalOrlik = tankJson.Epic.Orlik;
            v2.Achievements.medalOskin = tankJson.Epic.Oskin;
            v2.Achievements.medalPascucci = tankJson.Epic.Pascucci;
            v2.Achievements.medalPoppel = tankJson.Major.Poppel;
            v2.Achievements.medalRadleyWalters = tankJson.Epic.RadleyWalters;
            v2.Achievements.medalTamadaYoshio = tankJson.Epic.TamadaYoshio;
            v2.Achievements.medalTarczay = tankJson.Epic.Tarczay;
            v2.Achievements.medalWittmann = tankJson.Epic.Boelter;
            v2.Achievements.mousebane = tankJson.Special.mousebane;
            v2.Achievements.pattonValley = tankJson.Special.pattonValley;
            v2.Achievements.piercingSeries = tankJson.Series.piercingSeries;
            v2.Achievements.raider = tankJson.Special.raider;
            v2.Achievements.scout = tankJson.Battle.scout;
            v2.Achievements.sinai = tankJson.Special.sinai;
            v2.Achievements.sniper = tankJson.Battle.sniper;
            v2.Achievements.sniperSeries = tankJson.Series.sniperSeries;
            v2.Achievements.steelwall = tankJson.Battle.steelwall;
            v2.Achievements.sturdy = tankJson.Special.sturdy;
            v2.Achievements.supporter = tankJson.Battle.supporter;
            v2.Achievements.tankExpertStrg = tankJson.Special.tankExpertStrg;
            v2.Achievements.titleSniper = tankJson.Special.titleSniper;
            v2.Achievements.warrior = tankJson.Battle.warrior;
            v2.Achievements.markOfMastery = tankJson.Special.markOfMastery;

            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson();
                v2.Clan.battlesCount = tankJson.Clan.battlesCount;
                v2.Clan.capturePoints = tankJson.Clan.capturePoints;
                v2.Clan.damageDealt = tankJson.Clan.damageDealt;
                v2.Clan.damageReceived = tankJson.Clan.damageReceived;
                v2.Clan.droppedCapturePoints = tankJson.Clan.droppedCapturePoints;
                v2.Clan.frags = tankJson.Clan.frags;
                v2.Clan.hits = tankJson.Clan.hits;
                v2.Clan.losses = tankJson.Clan.losses;
                v2.Clan.shots = tankJson.Clan.shots;
                v2.Clan.spotted = tankJson.Clan.spotted;
                v2.Clan.survivedBattles = tankJson.Clan.survivedBattles;
                v2.Clan.wins = tankJson.Clan.wins;
                v2.Clan.xp = tankJson.Clan.xp;
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson();
                v2.Company.battlesCount = tankJson.Company.battlesCount;
                v2.Company.capturePoints = tankJson.Company.capturePoints;
                v2.Company.damageDealt = tankJson.Company.damageDealt;
                v2.Company.damageReceived = tankJson.Company.damageReceived;
                v2.Company.droppedCapturePoints = tankJson.Company.droppedCapturePoints;
                v2.Company.frags = tankJson.Company.frags;
                v2.Company.hits = tankJson.Company.hits;
                v2.Company.losses = tankJson.Company.losses;
                v2.Company.shots = tankJson.Company.shots;
                v2.Company.spotted = tankJson.Company.spotted;
                v2.Company.survivedBattles = tankJson.Company.survivedBattles;
                v2.Company.wins = tankJson.Company.wins;
                v2.Company.xp = tankJson.Company.xp;
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
            v2.Common.battleLifeTime = tankJson.Tankdata.battleLifeTime;
            v2.Common.mileage = tankJson.Tankdata.mileage;
            v2.Common.treesCut = tankJson.Tankdata.treesCut;

            return v2;
        }

        public static TankJson Convert(TankJson65 tankJson)
        {
            TankJson v2 = new TankJson();

            v2.A15x15 = new StatisticJson();
            v2.A15x15.battlesCount = tankJson.A15x15.battlesCount;
            v2.A15x15.capturePoints = tankJson.A15x15.capturePoints;
            v2.A15x15.damageDealt = tankJson.A15x15.damageDealt;
            v2.A15x15.damageReceived = tankJson.A15x15.damageReceived;
            v2.A15x15.droppedCapturePoints = tankJson.A15x15.droppedCapturePoints;
            v2.A15x15.frags = tankJson.A15x15.frags;
            v2.A15x15.frags8p = tankJson.A15x15.frags8p;
            v2.A15x15.fragsBeast = tankJson.A15x15.fragsBeast;
            v2.A15x15.hits = tankJson.A15x15.hits;
            v2.A15x15.losses = tankJson.A15x15.losses;
            v2.A15x15.shots = tankJson.A15x15.shots;
            v2.A15x15.spotted = tankJson.A15x15.spotted;
            v2.A15x15.survivedBattles = tankJson.A15x15.survivedBattles;
            v2.A15x15.winAndSurvived = tankJson.A15x15.winAndSurvived;
            v2.A15x15.wins = tankJson.A15x15.wins;
            v2.A15x15.xp = tankJson.A15x15.xp;
            v2.A15x15.battlesCountBefore8_8 = tankJson.A15x15.battlesCountBefore8_8;
            v2.A15x15.xpBefore8_8 = tankJson.A15x15.xpBefore8_8;
            
            v2.A15x15.originalXP = tankJson.A15x15_2.originalXP;
            v2.A15x15.damageAssistedRadio = tankJson.A15x15_2.damageAssistedRadio;
            v2.A15x15.damageAssistedTrack = tankJson.A15x15_2.damageAssistedTrack;
            v2.A15x15.shotsReceived = tankJson.A15x15_2.shotsReceived;
            v2.A15x15.noDamageShotsReceived = tankJson.A15x15_2.noDamageShotsReceived;
            v2.A15x15.piercedReceived = tankJson.A15x15_2.piercedReceived;
            v2.A15x15.heHitsReceived = tankJson.A15x15_2.heHitsReceived;
            v2.A15x15.he_hits = tankJson.A15x15_2.he_hits;
            v2.A15x15.pierced = tankJson.A15x15_2.pierced;

            v2.A15x15.maxDamage = tankJson.Max15x15.maxDamage;
            v2.A15x15.maxFrags = tankJson.Max15x15.maxFrags;
            v2.A15x15.maxXP = tankJson.Max15x15.maxXP;

            v2.FragsList = tankJson.FragsList;

            v2.Achievements = new AchievementsJson();
            v2.Achievements.armorPiercer = tankJson.Achievements.armorPiercer;
            v2.Achievements.battleHeroes = tankJson.Achievements.battleHeroes;
            v2.Achievements.beasthunter = tankJson.Achievements.beasthunter;
            v2.Achievements.bombardier = tankJson.Achievements.bombardier;
            v2.Achievements.defender = tankJson.Achievements.defender;
            v2.Achievements.diehard = tankJson.Achievements.diehard;
            v2.Achievements.diehardSeries = tankJson.Achievements.diehardSeries;
            v2.Achievements.evileye = tankJson.Achievements.evileye;
            v2.Achievements.fragsBeast = tankJson.Achievements.fragsBeast;
            v2.Achievements.fragsPatton = tankJson.Achievements.fragsPatton;
            v2.Achievements.fragsSinai = tankJson.Achievements.fragsSinai;
            v2.Achievements.handOfDeath = tankJson.Achievements.handOfDeath;
            v2.Achievements.heroesOfRassenay = tankJson.Achievements.heroesOfRassenay;
            v2.Achievements.huntsman = tankJson.Achievements.huntsman;
            v2.Achievements.invader = tankJson.Achievements.invader;
            v2.Achievements.invincible = tankJson.Achievements.invincible;
            v2.Achievements.invincibleSeries = tankJson.Achievements.invincibleSeries;
            v2.Achievements.ironMan = tankJson.Achievements.ironMan;
            v2.Achievements.kamikaze = tankJson.Achievements.kamikaze;
            v2.Achievements.killingSeries = tankJson.Achievements.killingSeries;
            v2.Achievements.luckyDevil = tankJson.Achievements.luckyDevil;
            v2.Achievements.lumberjack = tankJson.Achievements.lumberjack;
            v2.Achievements.maxDiehardSeries = tankJson.Achievements.maxDiehardSeries;
            v2.Achievements.maxInvincibleSeries = tankJson.Achievements.maxInvincibleSeries;
            v2.Achievements.maxKillingSeries = tankJson.Achievements.maxKillingSeries;
            v2.Achievements.maxPiercingSeries = tankJson.Achievements.maxPiercingSeries;
            v2.Achievements.maxSniperSeries = tankJson.Achievements.maxSniperSeries;
            v2.Achievements.medalAbrams = tankJson.Achievements.medalAbrams;
            v2.Achievements.medalBillotte = tankJson.Achievements.medalBillotte;
            v2.Achievements.medalBrothersInArms = tankJson.Achievements.medalBrothersInArms;
            v2.Achievements.medalBrunoPietro = tankJson.Achievements.medalBrunoPietro;
            v2.Achievements.medalBurda = tankJson.Achievements.medalBurda;
            v2.Achievements.medalCarius = tankJson.Achievements.medalCarius;
            v2.Achievements.medalCrucialContribution = tankJson.Achievements.medalCrucialContribution;
            v2.Achievements.medalDeLanglade = tankJson.Achievements.medalDeLanglade;
            v2.Achievements.medalDumitru = tankJson.Achievements.medalDumitru;
            v2.Achievements.medalEkins = tankJson.Achievements.medalEkins;
            v2.Achievements.medalFadin = tankJson.Achievements.medalFadin;
            v2.Achievements.medalHalonen = tankJson.Achievements.medalHalonen;
            v2.Achievements.medalKay = tankJson.Achievements.medalKay;
            v2.Achievements.medalKnispel = tankJson.Achievements.medalKnispel;
            v2.Achievements.medalKolobanov = tankJson.Achievements.medalKolobanov;
            v2.Achievements.medalLafayettePool = tankJson.Achievements.medalLafayettePool;
            v2.Achievements.medalLavrinenko = tankJson.Achievements.medalLavrinenko;
            v2.Achievements.medalLeClerc = tankJson.Achievements.medalLeClerc;
            v2.Achievements.medalLehvaslaiho = tankJson.Achievements.medalLehvaslaiho;
            v2.Achievements.medalNikolas = tankJson.Achievements.medalNikolas;
            v2.Achievements.medalOrlik = tankJson.Achievements.medalOrlik;
            v2.Achievements.medalOskin = tankJson.Achievements.medalOskin;
            v2.Achievements.medalPascucci = tankJson.Achievements.medalPascucci;
            v2.Achievements.medalPoppel = tankJson.Achievements.medalPoppel;
            v2.Achievements.medalRadleyWalters = tankJson.Achievements.medalRadleyWalters;
            v2.Achievements.medalTamadaYoshio = tankJson.Achievements.medalTamadaYoshio;
            v2.Achievements.medalTarczay = tankJson.Achievements.medalTarczay;
            v2.Achievements.medalWittmann = tankJson.Achievements.medalWittmann;
            v2.Achievements.mousebane = tankJson.Achievements.mousebane;
            v2.Achievements.pattonValley = tankJson.Achievements.pattonValley;
            v2.Achievements.piercingSeries = tankJson.Achievements.piercingSeries;
            v2.Achievements.raider = tankJson.Achievements.raider;
            v2.Achievements.scout = tankJson.Achievements.scout;
            v2.Achievements.sinai = tankJson.Achievements.sinai;
            v2.Achievements.sniper = tankJson.Achievements.sniper;
            v2.Achievements.sniperSeries = tankJson.Achievements.sniperSeries;
            v2.Achievements.steelwall = tankJson.Achievements.steelwall;
            v2.Achievements.sturdy = tankJson.Achievements.sturdy;
            v2.Achievements.supporter = tankJson.Achievements.supporter;
            v2.Achievements.tankExpertStrg = tankJson.Achievements.tankExpertStrg;
            v2.Achievements.titleSniper = tankJson.Achievements.titleSniper;
            v2.Achievements.warrior = tankJson.Achievements.warrior;
            v2.Achievements.markOfMastery = tankJson.Achievements.markOfMastery;

            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson();
                v2.Clan.battlesCount = tankJson.Clan.battlesCount;
                v2.Clan.capturePoints = tankJson.Clan.capturePoints;
                v2.Clan.damageDealt = tankJson.Clan.damageDealt;
                v2.Clan.damageReceived = tankJson.Clan.damageReceived;
                v2.Clan.droppedCapturePoints = tankJson.Clan.droppedCapturePoints;
                v2.Clan.frags = tankJson.Clan.frags;
                v2.Clan.hits = tankJson.Clan.hits;
                v2.Clan.losses = tankJson.Clan.losses;
                v2.Clan.shots = tankJson.Clan.shots;
                v2.Clan.spotted = tankJson.Clan.spotted;
                v2.Clan.survivedBattles = tankJson.Clan.survivedBattles;
                v2.Clan.wins = tankJson.Clan.wins;
                v2.Clan.xp = tankJson.Clan.xp;

                v2.Clan.originalXP = tankJson.Clan2.originalXP;
                v2.Clan.damageAssistedRadio = tankJson.Clan2.damageAssistedRadio;
                v2.Clan.damageAssistedTrack = tankJson.Clan2.damageAssistedTrack;
                v2.Clan.shotsReceived = tankJson.Clan2.shotsReceived;
                v2.Clan.noDamageShotsReceived = tankJson.Clan2.noDamageShotsReceived;
                v2.Clan.piercedReceived = tankJson.Clan2.piercedReceived;
                v2.Clan.heHitsReceived = tankJson.Clan2.heHitsReceived;
                v2.Clan.he_hits = tankJson.Clan2.he_hits;
                v2.Clan.pierced = tankJson.Clan2.pierced;
            }

            if (tankJson.Company != null)
            {
                v2.Company = new StatisticJson();
                v2.Company.battlesCount = tankJson.Company.battlesCount;
                v2.Company.capturePoints = tankJson.Company.capturePoints;
                v2.Company.damageDealt = tankJson.Company.damageDealt;
                v2.Company.damageReceived = tankJson.Company.damageReceived;
                v2.Company.droppedCapturePoints = tankJson.Company.droppedCapturePoints;
                v2.Company.frags = tankJson.Company.frags;
                v2.Company.hits = tankJson.Company.hits;
                v2.Company.losses = tankJson.Company.losses;
                v2.Company.shots = tankJson.Company.shots;
                v2.Company.spotted = tankJson.Company.spotted;
                v2.Company.survivedBattles = tankJson.Company.survivedBattles;
                v2.Company.wins = tankJson.Company.wins;
                v2.Company.xp = tankJson.Company.xp;

                v2.Company.originalXP = tankJson.Company2.originalXP;
                v2.Company.damageAssistedRadio = tankJson.Company2.damageAssistedRadio;
                v2.Company.damageAssistedTrack = tankJson.Company2.damageAssistedTrack;
                v2.Company.shotsReceived = tankJson.Company2.shotsReceived;
                v2.Company.noDamageShotsReceived = tankJson.Company2.noDamageShotsReceived;
                v2.Company.piercedReceived = tankJson.Company2.piercedReceived;
                v2.Company.heHitsReceived = tankJson.Company2.heHitsReceived;
                v2.Company.he_hits = tankJson.Company2.he_hits;
                v2.Company.pierced = tankJson.Company2.pierced;
            }

            if (tankJson.A7x7 != null)
            {
                v2.A7x7 = new StatisticJson();
                v2.A7x7.battlesCount = tankJson.A7x7.battlesCount;
                v2.A7x7.capturePoints = tankJson.A7x7.capturePoints;
                v2.A7x7.damageDealt = tankJson.A7x7.damageDealt;
                v2.A7x7.damageReceived = tankJson.A7x7.damageReceived;
                v2.A7x7.droppedCapturePoints = tankJson.A7x7.droppedCapturePoints;
                v2.A7x7.frags = tankJson.A7x7.frags;
                v2.A7x7.frags8p = tankJson.A7x7.frags8p;
                v2.A7x7.fragsBeast = tankJson.A7x7.fragsBeast;
                v2.A7x7.hits = tankJson.A7x7.hits;
                v2.A7x7.losses = tankJson.A7x7.losses;
                v2.A7x7.shots = tankJson.A7x7.shots;
                v2.A7x7.spotted = tankJson.A7x7.spotted;
                v2.A7x7.survivedBattles = tankJson.A7x7.survivedBattles;
                v2.A7x7.winAndSurvived = tankJson.A7x7.winAndSurvived;
                v2.A7x7.wins = tankJson.A7x7.wins;
                v2.A7x7.xp = tankJson.A7x7.xp;
                v2.A7x7.battlesCountBefore8_8 = tankJson.A7x7.battlesCountBefore8_8;
                v2.A7x7.xpBefore8_8 = tankJson.A7x7.xpBefore8_8;

                //v2.A7x7.originalXP = tankJson.A7x7_2.originalXP;
                //v2.A7x7.damageAssistedRadio = tankJson.A7x7_2.damageAssistedRadio;
                //v2.A7x7.damageAssistedTrack = tankJson.A7x7_2.damageAssistedTrack;
                //v2.A7x7.shotsReceived = tankJson.A7x7_2.shotsReceived;
                //v2.A7x7.noDamageShotsReceived = tankJson.A7x7_2.noDamageShotsReceived;
                //v2.A7x7.piercedReceived = tankJson.A7x7_2.piercedReceived;
                //v2.A7x7.heHitsReceived = tankJson.A7x7_2.heHitsReceived;
                //v2.A7x7.he_hits = tankJson.A7x7_2.he_hits;
                //v2.A7x7.pierced = tankJson.A7x7_2.pierced;

                v2.A7x7.maxDamage = tankJson.Max7x7.maxDamage;
                v2.A7x7.maxFrags = tankJson.Max7x7.maxFrags;
                v2.A7x7.maxXP = tankJson.Max7x7.maxXP;
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

        public static TankJson Convert(Tank tankJson)
        {
            TankJson v2 = new TankJson();

            v2.A15x15 = new StatisticJson();
            v2.A15x15.battlesCount = tankJson.amounts.battles;
            v2.A15x15.battlesCountBefore8_8 = tankJson.amounts.battles;
            v2.A15x15.capturePoints = tankJson.amounts.capture_points;
            v2.A15x15.damageDealt = tankJson.amounts.damage_dealt;
            v2.A15x15.damageReceived = tankJson.amounts.damage_received;
            v2.A15x15.droppedCapturePoints = tankJson.amounts.defence_points;
            v2.A15x15.frags = tankJson.amounts.frags;
            v2.A15x15.frags8p = tankJson.amounts.tier8_frags;
            v2.A15x15.fragsBeast = tankJson.amounts.beast_frags;
            v2.A15x15.hits = tankJson.amounts.hits;
            v2.A15x15.losses = tankJson.amounts.losses;
            v2.A15x15.shots = tankJson.amounts.shots;
            v2.A15x15.spotted = tankJson.amounts.spotted;
            v2.A15x15.survivedBattles = tankJson.amounts.survived;
            v2.A15x15.winAndSurvived = tankJson.amounts.survived_with_victory;
            v2.A15x15.wins = tankJson.amounts.victories;
            v2.A15x15.xp = tankJson.amounts.experience;
            v2.A15x15.xpBefore8_8 = tankJson.amounts.experience;

            v2.A15x15.originalXP = 0;
            v2.A15x15.damageAssistedRadio = 0;
            v2.A15x15.damageAssistedTrack = 0;
            v2.A15x15.shotsReceived = 0;
            v2.A15x15.noDamageShotsReceived = 0;
            v2.A15x15.piercedReceived = 0;
            v2.A15x15.heHitsReceived = 0;
            v2.A15x15.he_hits = 0;
            v2.A15x15.pierced = 0;

            //v2.A15x15.maxDamage = ;
            v2.A15x15.maxFrags = tankJson.amounts.max_frags;
            v2.A15x15.maxXP = tankJson.amounts.max_experience;

            v2.FragsList = tankJson.frag_counts;

            v2.Achievements = new AchievementsJson();
            v2.Achievements.armorPiercer = tankJson.series.master_gunner;
            v2.Achievements.battleHeroes = tankJson.awards.battle_hero;
            v2.Achievements.beasthunter = tankJson.awards.hunter;
            v2.Achievements.bombardier = tankJson.awards.bombardier;
            v2.Achievements.defender = tankJson.awards.defender;
            v2.Achievements.diehard = tankJson.series.survivor;
            v2.Achievements.diehardSeries = tankJson.series.survivor_progress;
            v2.Achievements.evileye = tankJson.awards.patrol_duty;
            v2.Achievements.fragsBeast = tankJson.amounts.beast_frags;
            v2.Achievements.fragsPatton = tankJson.amounts.patton_frags;
            v2.Achievements.fragsSinai = tankJson.amounts.sinai_frags;
            v2.Achievements.handOfDeath = tankJson.awards.reaper;
            v2.Achievements.heroesOfRassenay = tankJson.epics.heroes_of_raseiniai;
            v2.Achievements.huntsman = tankJson.awards.ranger;
            v2.Achievements.invader = tankJson.awards.invader;
            v2.Achievements.invincible = tankJson.series.invincible;
            v2.Achievements.invincibleSeries = tankJson.series.invincible_progress;
            v2.Achievements.ironMan = tankJson.awards.cool_headed;
            v2.Achievements.kamikaze = tankJson.awards.kamikaze;
            v2.Achievements.killingSeries = tankJson.series.reaper_progress;
            v2.Achievements.luckyDevil = tankJson.awards.lucky_devil;
            v2.Achievements.lumberjack = 0;
            v2.Achievements.maxDiehardSeries = tankJson.series.survivor;
            v2.Achievements.maxInvincibleSeries = tankJson.series.invincible_progress;
            v2.Achievements.maxKillingSeries = tankJson.series.reaper;
            v2.Achievements.maxPiercingSeries = tankJson.series.master_gunner;
            v2.Achievements.maxSniperSeries = tankJson.series.sharpshooter;
            v2.Achievements.medalAbrams = tankJson.medals.abrams;
            v2.Achievements.medalBillotte = tankJson.epics.billotte;
            v2.Achievements.medalBrothersInArms = tankJson.awards.brothers_in_arms;
            v2.Achievements.medalBrunoPietro = tankJson.epics.bruno_pietro;
            v2.Achievements.medalBurda = tankJson.epics.burda;
            v2.Achievements.medalCarius = tankJson.medals.carius;
            v2.Achievements.medalCrucialContribution = tankJson.awards.crucial_contribution;
            v2.Achievements.medalDeLanglade = tankJson.epics.de_langlade;
            v2.Achievements.medalDumitru = tankJson.epics.dumitru;
            v2.Achievements.medalEkins = tankJson.medals.ekins;
            v2.Achievements.medalFadin = tankJson.epics.fadin;
            v2.Achievements.medalHalonen = tankJson.epics.halonen;
            v2.Achievements.medalKay = tankJson.medals.kay;
            v2.Achievements.medalKnispel = tankJson.medals.knispel;
            v2.Achievements.medalKolobanov = tankJson.epics.kolobanov;
            v2.Achievements.medalLafayettePool = tankJson.epics.lafayette_pool;
            v2.Achievements.medalLavrinenko = tankJson.medals.lavrinenko;
            v2.Achievements.medalLeClerc = tankJson.medals.leclerk;
            v2.Achievements.medalLehvaslaiho = tankJson.epics.lehvaslaiho;
            v2.Achievements.medalNikolas = tankJson.epics.nikolas;
            v2.Achievements.medalOrlik = tankJson.epics.orlik;
            v2.Achievements.medalOskin = tankJson.epics.oskin;
            v2.Achievements.medalPascucci = tankJson.epics.pascucci;
            v2.Achievements.medalPoppel = tankJson.medals.poppel;
            v2.Achievements.medalRadleyWalters = tankJson.epics.radley_walters;
            v2.Achievements.medalTamadaYoshio = tankJson.epics.tamada_yoshio;
            v2.Achievements.medalTarczay = tankJson.epics.tarczay;
            v2.Achievements.medalWittmann = tankJson.epics.boelter;
            v2.Achievements.mousebane = tankJson.awards.mouse_trap;
            v2.Achievements.pattonValley = tankJson.awards.patton_valley;
            v2.Achievements.piercingSeries = tankJson.series.master_gunner_progress;
            v2.Achievements.raider = tankJson.awards.raider;
            v2.Achievements.scout = tankJson.awards.scout;
            v2.Achievements.sinai = tankJson.awards.sinai;
            v2.Achievements.sniper = tankJson.awards.sniper;
            v2.Achievements.sniperSeries = tankJson.series.sharpshooter_progress;
            v2.Achievements.steelwall = tankJson.awards.steel_wall;
            v2.Achievements.sturdy = tankJson.awards.spartan;
            v2.Achievements.supporter = tankJson.awards.confederate;
            v2.Achievements.tankExpertStrg = 0;
            v2.Achievements.titleSniper = tankJson.series.sharpshooter;
            v2.Achievements.warrior = tankJson.awards.top_gun;
            v2.Achievements.markOfMastery = tankJson.awards.mastery_mark;

            v2.Common = new CommonJson();
            v2.Common.basedonversion = tankJson.version;
            v2.Common.compactDescr = 0;
            v2.Common.countryid = tankJson.country;
            v2.Common.creationTime = 0;
            v2.Common.creationTimeR = DateTime.MinValue;
            v2.Common.frags = tankJson.amounts.frags;
            v2.Common.frags_compare = 0;
            v2.Common.has_15x15 = 1;
            v2.Common.has_7x7 = 0;
            v2.Common.has_clan = 0;
            v2.Common.has_company = 0;
            v2.Common.lastBattleTime = tankJson.last_time_played;
            v2.Common.lastBattleTimeR = Utils.UnixDateToDateTime(tankJson.last_time_played);
            v2.Common.premium = 0;
            v2.Common.tankid = tankJson.id;
            v2.Common.tanktitle = string.Empty;
            v2.Common.tier = 0;
            v2.Common.type = 0;
            v2.Common.updated = tankJson.updated;
            v2.Common.updatedR = Utils.UnixDateToDateTime(tankJson.updated);
            v2.Common.battleLifeTime = tankJson.play_time;
            v2.Common.mileage = 0;
            v2.Common.treesCut = 0;

            return v2;
        }
    }
}
