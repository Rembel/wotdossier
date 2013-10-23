using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    public class TankJsonV2Converter
    {
        public static TankJsonV2 Convert(TankJson tankJson)
        {
            TankJsonV2 v2 = new TankJsonV2();

            v2.A15x15 = new StatisticJson8_9();
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

            v2.A15x15_2 = new StatisticJson8_9();
            v2.A15x15.originalXP = tankJson.Tankdata.originalXP;
            v2.A15x15.damageAssistedRadio = tankJson.Tankdata.damageAssistedRadio;
            v2.A15x15.damageAssistedTrack = tankJson.Tankdata.damageAssistedTrack;
            v2.A15x15.shotsReceived = tankJson.Tankdata.shotsReceived;
            v2.A15x15.noDamageShotsReceived = tankJson.Tankdata.noDamageShotsReceived;
            v2.A15x15.piercedReceived = tankJson.Tankdata.piercedReceived;
            v2.A15x15.heHitsReceived = tankJson.Tankdata.heHitsReceived;
            v2.A15x15.he_hits = tankJson.Tankdata.he_hits;
            v2.A15x15.pierced = tankJson.Tankdata.pierced;


            v2.Total = new TotalJson();
            v2.Total.battleLifeTime = tankJson.Tankdata.battleLifeTime;
            v2.Total.lastBattleTime = tankJson.Tankdata.lastBattleTime;
            v2.Total.mileage = tankJson.Tankdata.mileage;
            v2.Total.treesCut = tankJson.Tankdata.treesCut;
            v2.Total.creationTime = tankJson.Tankdata.creationTime;

            v2.Max15x15 = new MaxJson();
            v2.Max15x15.maxFrags = tankJson.Tankdata.maxFrags;
            v2.Max15x15.maxXP = tankJson.Tankdata.maxXP;

            v2.FragsList = tankJson.Kills;

            v2.Achievements = new AchievementsJson();
            v2.Achievements.alaric = tankJson.Special.alaric;
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
            v2.Achievements.medalWittmann = tankJson.Epic.Wittmann;
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

            if (tankJson.Clan != null)
            {
                v2.Clan = new StatisticJson8_9();
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
                v2.Company = new StatisticJson8_9();
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
            v2.Common.updatedR = tankJson.Common.updatedR;

            return v2;
        }
    }
}
