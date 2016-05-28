using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WotDossier.Domain;
using WotDossier.Domain.Entities;

namespace WotDossier.Web.Logic
{
    public class SyncManager : IDisposable
    {
        private readonly dossierContext _context;

        public SyncManager(dossierContext context)
        {
            _context = context;
        }

        public dbversion GetDbVersion()
        {
            var dbversion = _context.dbversion.First();
            return dbversion;
        }

        public void ProcessStatistic(ClientStat stat)
        {
            var player = _context.player.FirstOrDefault(x => x.accountid == stat.Player.AccountId && x.server == stat.Player.Server);

            if (player == null)
            {
                AddStatistic(stat);
            }
            else
            {
                UpdateStatistic(stat, player);
            }

            _context.SaveChanges();
        }

        private void UpdateStatistic(ClientStat stat, player player)
        {
            player.rev = stat.Player.Rev;

            AddTanks(stat);

            UpdateRandomStatistic(stat);

            UpdateTankRandomStatistic(stat);

            _context.player.Update(player);
        }

        private void UpdateTankRandomStatistic(ClientStat stat)
        {
            if (stat.TankRandomStatistic?.Any() ?? false)
            {
                var minRev = stat.TankRandomStatistic.Min(x => x.Rev);

                foreach (var statistic in stat.TankRandomStatistic.Where(x => x.Rev == minRev))
                {
                    var tankrandombattlesstatistic = _context.tankrandombattlesstatistic.FirstOrDefault(x => x.uid == statistic.UId);

                    bool create = false;

                    if (tankrandombattlesstatistic == null)
                    {
                        tankrandombattlesstatistic = new tankrandombattlesstatistic();
                        create = true;
                    }

                    Copy(stat, tankrandombattlesstatistic, statistic);

                    if (create)
                        _context.tankrandombattlesstatistic.Add(tankrandombattlesstatistic);
                    else
                        _context.tankrandombattlesstatistic.Update(tankrandombattlesstatistic);
                }
            }
        }

        private void UpdateRandomStatistic(ClientStat stat)
        {
            if (stat.RandomStatistic?.Any() ?? false)
            {
                var minRev = stat.RandomStatistic.Min(x => x.Rev);

                AddRandomStatistic(stat, stat.RandomStatistic.Where(x => x.Rev > minRev));

                foreach (var statistic in stat.RandomStatistic.Where(x => x.Rev == minRev))
                {
                    var randombattlesstatistic = _context.randombattlesstatistic.FirstOrDefault(x => x.uid == statistic.UId);

                    var create = false;

                    if (randombattlesstatistic == null)
                    {
                        randombattlesstatistic = new randombattlesstatistic();
                        create = true;
                    }

                    Copy(randombattlesstatistic, statistic);

                    if (create)
                        _context.randombattlesstatistic.Add(randombattlesstatistic);
                    else
                        _context.randombattlesstatistic.Update(randombattlesstatistic);

                    if (statistic.AchievementsIdObject != null)
                    {
                        var randombattlesachievements = _context.randombattlesachievements.FirstOrDefault(x => x.uid == statistic.UId);

                        create = false;

                        if (randombattlesachievements == null)
                        {
                            randombattlesachievements = new randombattlesachievements();
                            create = true;
                        }

                        Copy(stat, randombattlesachievements, statistic.AchievementsIdObject);

                        if (create)
                            _context.randombattlesachievements.Add(randombattlesachievements);
                        else
                            _context.randombattlesachievements.Update(randombattlesachievements);
                    }
                }
            }
        }

        public void AddStatistic(ClientStat stat)
        {
            var player = new player();

            Copy(stat.Player, player);

            _context.player.Add(player);

            AddTanks(stat);

            var randomStatistic = stat.RandomStatistic;

            AddRandomStatistic(stat, randomStatistic);

            foreach (var entity in stat.TankRandomStatistic)
            {
                var tankrandombattlesstatistic = new tankrandombattlesstatistic();

                Copy(stat, tankrandombattlesstatistic, entity);

                _context.tankrandombattlesstatistic.Add(tankrandombattlesstatistic);
            }
        }

        private void AddRandomStatistic(ClientStat stat, IEnumerable<RandomBattlesStatisticEntity> randomStatistic)
        {
            foreach (var fromEntity in randomStatistic)
            {
                if (fromEntity.AchievementsIdObject != null)
                {
                    var randombattlesachievements = new randombattlesachievements();
                    Copy(stat, randombattlesachievements, fromEntity.AchievementsIdObject);
                    _context.randombattlesachievements.Add(randombattlesachievements);
                }

                var randombattlesstatistic = new randombattlesstatistic();
                Copy(randombattlesstatistic, fromEntity);
                _context.randombattlesstatistic.Add(randombattlesstatistic);
            }
        }

        private void AddTanks(ClientStat stat)
        {
            foreach (TankEntity tank in stat.Tanks)
            {
                _context.tank.Add(new tank
                {
                    id = tank.Id,
                    uid = tank.UId,
                    countryid = tank.CountryId,
                    playerid = tank.PlayerId,
                    tankid = tank.TankId,
                    tanktype = tank.TankType,
                    tier = tank.Tier,
                    rev = tank.Rev,
                    icon = tank.Icon,
                    isfavorite = tank.IsFavorite,
                    ispremium = tank.IsPremium,
                    name = tank.Name,
                    playeruid = tank.PlayerUId
                });
            }
        }

        public player GetPlayer(string server, int id)
        {
            player player = _context.player.FirstOrDefault(m => m.server == server && m.accountid == id);
            return player;
        }

        public void DeletePlayer(player player)
        {
            _context.Database.ExecuteSqlCommand("DELETE FROM randombattlesstatistic WHERE PlayerUId = {0}", player.uid);
            _context.Database.ExecuteSqlCommand("DELETE FROM randombattlesachievements WHERE PlayerUId = {0}", player.uid);
            _context.Database.ExecuteSqlCommand("DELETE FROM tankrandombattlesstatistic WHERE PlayerUId = {0}", player.uid);
            _context.Database.ExecuteSqlCommand("DELETE FROM tank WHERE PlayerUId = {0}", player.uid);
            _context.player.Remove(player);
            _context.SaveChanges();
        }

        private static void Copy(PlayerEntity fromEntity, player toEntity)
        {
            toEntity.id = fromEntity.Id;
            toEntity.uid = fromEntity.UId;
            toEntity.accountid = fromEntity.AccountId;
            toEntity.server = fromEntity.Server;
            toEntity.creaded = fromEntity.Creaded;
            toEntity.rev = fromEntity.Rev;
            toEntity.name = fromEntity.Name;
        }

        private static void Copy(ClientStat stat, randombattlesachievements toEntity,
            RandomBattlesAchievementsEntity fromEntity)
        {
            toEntity.uid = fromEntity.UId;
            toEntity.playeruid = stat.Player.UId;
            toEntity.abrams = fromEntity.Abrams;
            toEntity.arsonist = fromEntity.Arsonist;
            toEntity.billotte = fromEntity.Billotte;
            toEntity.boelter = fromEntity.Boelter;
            toEntity.bombardier = fromEntity.Bombardier;
            toEntity.bonecrusher = fromEntity.Bonecrusher;
            toEntity.brothersinarms = fromEntity.BrothersInArms;
            toEntity.brunopietro = fromEntity.BrunoPietro;
            toEntity.burda = fromEntity.Burda;
            toEntity.carius = fromEntity.Carius;
            toEntity.charmed = fromEntity.Charmed;
            toEntity.confederate = fromEntity.Confederate;
            toEntity.coolheaded = fromEntity.IronMan;
            toEntity.crucialcontribution = fromEntity.CrucialContribution;
            toEntity.defender = fromEntity.Defender;
            toEntity.delanglade = fromEntity.DeLanglade;
            toEntity.demolition = fromEntity.Demolition;
            toEntity.duelist = fromEntity.Duelist;
            toEntity.dumitru = fromEntity.Dumitru;
            toEntity.ekins = fromEntity.Ekins;
            toEntity.even = fromEntity.Even;
            toEntity.fadin = fromEntity.Fadin;
            toEntity.fighter = fromEntity.Fighter;
            toEntity.halonen = fromEntity.Halonen;
            toEntity.heroesofrassenay = fromEntity.HeroesOfRassenay;
            toEntity.hunter = fromEntity.Hunter;
            toEntity.id = fromEntity.Id;
            toEntity.impenetrable = fromEntity.Impenetrable;
            toEntity.invader = fromEntity.Invader;
            toEntity.invincible = fromEntity.Invincible;
            toEntity.kamikaze = fromEntity.Kamikaze;
            toEntity.kay = fromEntity.Kay;
            toEntity.knispel = fromEntity.Knispel;
            toEntity.kolobanov = fromEntity.Kolobanov;
            toEntity.lafayettepool = fromEntity.LafayettePool;
            toEntity.lavrinenko = fromEntity.Lavrinenko;
            toEntity.leclerk = fromEntity.Leclerk;
            toEntity.lehvaslaiho = fromEntity.Lehvaslaiho;
            toEntity.luckydevil = fromEntity.LuckyDevil;
            toEntity.maingun = fromEntity.MainGun;
            toEntity.marksongun = fromEntity.MarksOnGun;
            toEntity.mastergunnerlongest = fromEntity.MasterGunnerLongest;
            toEntity.maxaimerseries = fromEntity.MaxAimerSeries;
            toEntity.medalantispgfire = fromEntity.MedalAntiSpgFire;
            toEntity.medalcoolblood = fromEntity.MedalCoolBlood;
            toEntity.medalgore = fromEntity.MedalGore;
            toEntity.medalmonolith = fromEntity.MedalMonolith;
            toEntity.medalstark = fromEntity.MedalStark;
            toEntity.mousetrap = fromEntity.MouseTrap;
            toEntity.movingavgdamage = fromEntity.MovingAvgDamage;
            toEntity.nikolas = fromEntity.Nikolas;
            toEntity.orlik = fromEntity.Orlik;
            toEntity.oskin = fromEntity.Oskin;
            toEntity.pascucci = fromEntity.Pascucci;
            toEntity.patrolduty = fromEntity.PatrolDuty;
            toEntity.pattonvalley = fromEntity.PattonValley;
            toEntity.poppel = fromEntity.Poppel;
            toEntity.radleywalters = fromEntity.RadleyWalters;
            toEntity.raider = fromEntity.Raider;
            toEntity.ranger = fromEntity.Huntsman;
            toEntity.reaper = fromEntity.Reaper;
            toEntity.rev = fromEntity.Rev;
            toEntity.scout = fromEntity.Scout;
            toEntity.sharpshooterlongest = fromEntity.SharpshooterLongest;
            toEntity.shoottokill = fromEntity.ShootToKill;
            toEntity.sinai = fromEntity.Sinai;
            toEntity.sniper = fromEntity.Sniper;
            toEntity.sniper2 = fromEntity.Sniper2;
            toEntity.spartan = fromEntity.Sturdy;
            toEntity.steelwall = fromEntity.SteelWall;
            toEntity.survivor = fromEntity.Survivor;
            toEntity.tamadayoshio = fromEntity.TamadaYoshio;
            toEntity.tarczay = fromEntity.Tarczay;
            toEntity.warrior = fromEntity.Warrior;
        }

        private static void Copy(randombattlesstatistic toEntity, RandomBattlesStatisticEntity fromEntity)
        {
            toEntity.id = fromEntity.Id;
            toEntity.uid = fromEntity.UId;
            toEntity.playeruid = fromEntity.PlayerUId;
            toEntity.rev = fromEntity.Rev;
            toEntity.achievementsid = fromEntity.AchievementsId;
            toEntity.achievementsuid = fromEntity.AchievementsUId;
            toEntity.avglevel = fromEntity.AvgLevel;
            toEntity.battleavgxp = fromEntity.BattleAvgXp;
            toEntity.battlescount = fromEntity.BattlesCount;
            toEntity.capturepoints = fromEntity.CapturePoints;
            toEntity.damagedealt = fromEntity.DamageDealt;
            toEntity.damagetaken = fromEntity.DamageTaken;
            toEntity.droppedcapturepoints = fromEntity.DroppedCapturePoints;
            toEntity.frags = fromEntity.Frags;
            toEntity.hitspercents = fromEntity.HitsPercents;
            toEntity.losses = fromEntity.Losses;
            toEntity.markofmastery = fromEntity.MarkOfMastery;
            toEntity.maxdamage = fromEntity.MaxDamage;
            toEntity.maxfrags = fromEntity.MaxFrags;
            toEntity.maxxp = fromEntity.MaxXp;
            toEntity.performancerating = fromEntity.PerformanceRating;
            toEntity.playerid = fromEntity.PlayerId;
            toEntity.rbr = fromEntity.RBR;
            toEntity.spotted = fromEntity.Spotted;
            toEntity.survivedbattles = fromEntity.SurvivedBattles;
            toEntity.updated = fromEntity.Updated;
            toEntity.wn8rating = fromEntity.WN8Rating;
            toEntity.wins = fromEntity.Wins;
            toEntity.xp = fromEntity.Xp;
        }

        private static void Copy(ClientStat stat, tankrandombattlesstatistic toEntity,
            TankRandomBattlesStatisticEntity fromEntity)
        {
            toEntity.id = fromEntity.Id;
            toEntity.tankid = fromEntity.TankId;
            toEntity.tankuid = fromEntity.TankUId;
            toEntity.playeruid = stat.Player.UId;
            toEntity.rev = fromEntity.Rev;
            toEntity.battlescount = fromEntity.BattlesCount;
            toEntity.updated = fromEntity.Updated;
            toEntity.uid = fromEntity.UId;
            toEntity.raw = fromEntity.Raw;
            toEntity.version = fromEntity.Version;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
