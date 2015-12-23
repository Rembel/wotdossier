var path = require('path');
var bodyParser = require('body-parser');
var async = require('async');
//var cluster = require('cluster');
var cluster = require('express-cluster');

//var numCPUs = require('os').cpus().length;

//if (cluster.isMaster) {
//    // Fork workers.
//    for (var i = 0; i < numCPUs; i++) {
//        cluster.fork();
//    }

//    cluster.on('exit', function (worker, code, signal) {
//        console.log('worker ' + worker.process.pid + ' died');
//    });
//} else {
//    serverWorker();
//}

var serverWorker = function (worker) {

    var pg = require('pg');
    pg.defaults.poolSize = 200;
    pg.defaults.poolIdleTimeout = 300000;


    var conString = "";

    var handleError = function (err, res, client, done) {
        // no error occurred, continue with the request
        if (!err) return false;

        // An error occurred, remove the client from the connection pool.
        // A truthy value passed to done will remove the connection from the pool
        // instead of simply returning it to be reused.
        // In this case, if we have successfully received a client (truthy)
        // then it will be removed from the pool.
        if (done) {
            done(client);
        }
        //res.writeHead(500, {'content-type': 'text/plain'});
        res.end('An error occurred: ' + err);
        console.log(err);
        return true;
    };

    var Client = require('pg-native');
    var express = require('express');
    var app = express();
    app.set('port', process.env.PORT || 8080);
    app.set('views', path.join(__dirname, 'views')); //A
    app.set('view engine', 'jade'); //B

    app.use(express.static(path.join(__dirname, 'public')));
    app.use(bodyParser.json({ limit: '50mb' }));
    app.use(bodyParser.urlencoded({ extended: true }));

    app.get('/', function (req, res) {
        res.send('<html><body><h1>Hello World</h1></body></html>');
    });

    app.get('/api/dbversion', function (req, res) {
        pg.connect(conString, function (err, client, done) {

            // get the total number of visits today (including the current visit)
            client.query('SELECT * FROM dbversion', function (err, result) {

                // handle an error from the query
                if (handleError(err, res, client, done)) return;

                // return the client to the connection pool for other requests to reuse
                done();
                res.json(result.rows);
            });
        });
    });

    app.get('/api/:cluster/player/:playerid', function (req, res) {
        var client = new Client();

        client.connectSync(conString);

        console.log('[' + worker.id + '] ' + 'get player data revision: ' + req.params.playerid);

        // get the player info
        res.json(client.querySync('SELECT * FROM player where playerid = $1::int AND server = $2', [req.params.playerid, req.params.cluster]));
    });

    app.delete('/api/:cluster/player/:playerid', function (req, res) {
        pg.connect(conString, function (err, client, done) {

            console.log('[' + worker.id + '] ' + 'delete player data: ' + req.params.playerid);

            // get the player info
            client.query('DELETE FROM PlayerStatistic', function (err, result) {

                // handle an error from the query
                if (handleError(err, res, client, done)) return;

                client.query('DELETE FROM PlayerAchievements', function (err, result) {

                    // handle an error from the query
                    if (handleError(err, res, client, done)) return;

                    client.query('DELETE FROM Tank', function (err, result) {

                        // handle an error from the query
                        if (handleError(err, res, client, done)) return;

                        client.query('DELETE FROM player where playerid = $1::int AND server = $2', [req.params.playerid, req.params.cluster], function (err, result) {

                            // handle an error from the query
                            if (handleError(err, res, client, done)) return;

                            done();

                            res.end('');
                        });
                    });
                });
            });
        });
    });

    //app.post('/api/statistic/:rev', function (req, res) {

    //    //console.log(req.body);

    //    var data = req.body;


    //    pg.connect(conString, function (err, client, done) {

    //        console.log('upload statistic: ' + data.Player.PlayerId);

    //        if (req.params.rev == 0) {
    //            client.query('INSERT INTO Player (uid, id, name, playerid, server, rev, created) values ($1,$2,$3,$4,$5,$6,$7)',
    //				[data.Player.UId, data.Player.Id, data.Player.Name, data.Player.PlayerId, data.Player.Server, data.Player.Rev, data.Player.Creaded], function (err, result) {

    //				    // handle an error from the query
    //				    if (handleError(err, res, client, done)) return;

    //				    // return the client to the connection pool for other requests to reuse
    //				    done();
    //				});
    //        }
    //        else {
    //            client.query('Update Player set Rev = $1 where uid = $2',
    //				[data.Player.Rev, data.Player.UId], function (err, result) {

    //				    // handle an error from the query
    //				    if (handleError(err, res, client, done)) return;

    //				    // return the client to the connection pool for other requests to reuse
    //				    done();
    //				});
    //        }


    //        for (index = 0; index < data.Tanks.length; ++index) {
    //            var tank = data.Tanks[index];

    //            //console.log(tank);

    //            client.query('INSERT INTO Tank (uid, id, tankid, name, tier, countryid, icon, tanktype, ispremium, playerid, playeruid, isfavorite) values ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12)',
    //				[tank.UId, tank.Id, tank.TankId, tank.Name, tank.Tier, tank.CountryId, tank.Icon, tank.TankType, tank.IsPremium, tank.PlayerId, tank.PlayerUId, tank.IsFavorite], function (err, result) {

    //				    // handle an error from the query
    //				    if (handleError(err, res, client, done)) return;

    //                done();
    //            });
    //        }

    //        for (index = 0; index < data.RandomStatistic.length; ++index) {

    //            var playerstatistic = data.RandomStatistic[index];

    //            var insertPlayerStatistic = {
    //                name: 'insert_playerstatistic',
    //                text: 'INSERT INTO PlayerStatistic (Id,PlayerId,Updated,Wins,Losses,SurvivedBattles,Xp,BattleAvgXp,MaxXp,Frags,Spotted,HitsPercents,DamageDealt,CapturePoints,DroppedCapturePoints,BattlesCount,Rating_IntegratedValue,Rating_IntegratedPlace,Rating_BattleAvgPerformanceValue,Rating_BattleAvgPerformancePlace,Rating_BattleAvgXpValue,Rating_BattleAvgXpPlace,Rating_BattleWinsValue,Rating_BattleWinsPlace,Rating_BattlesValue,Rating_BattlesPlace,Rating_CapturedPointsValue,Rating_CapturedPointsPlace,Rating_DamageDealtValue,Rating_DamageDealtPlace,Rating_DroppedPointsValue,Rating_DroppedPointsPlace,Rating_FragsValue,Rating_FragsPlace,Rating_SpottedValue,Rating_SpottedPlace,Rating_XpValue,Rating_XpPlace,AvgLevel,AchievementsId,Rating_HitsPercentsValue,Rating_HitsPercentsPlace,Rating_MaxXpValue,Rating_MaxXpPlace,RBR,WN8Rating,PerformanceRating,DamageTaken,MaxFrags,MaxDamage,MarkOfMastery,UId,PlayerUId,AchievementsUId,Rev) VALUES ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12,$13,$14,$15,$16,$17,$18,$19,$20,$21,$22,$23,$24,$25,$26,$27,$28,$29,$30,$31,$32,$33,$34,$35,$36,$37,$38,$39,$40,$41,$42,$43,$44,$45,$46,$47,$48,$49,$50,$51,$52,$53,$54,$55)',
    //                values: [playerstatistic.Id, playerstatistic.PlayerId, playerstatistic.Updated, playerstatistic.Wins, playerstatistic.Losses, playerstatistic.SurvivedBattles, playerstatistic.Xp, playerstatistic.BattleAvgXp,
    //                    playerstatistic.MaxXp, playerstatistic.Frags, playerstatistic.Spotted, playerstatistic.HitsPercents, playerstatistic.DamageDealt, playerstatistic.CapturePoints, playerstatistic.DroppedCapturePoints,
    //                    playerstatistic.BattlesCount, playerstatistic.RatingIntegratedValue, playerstatistic.RatingIntegratedPlace, playerstatistic.RatingWinsRatioValue, playerstatistic.RatingWinsRatioPlace,
    //                    playerstatistic.RatingBattleAvgXpValue, playerstatistic.RatingBattleAvgXpPlace, playerstatistic.RatingBattleWinsValue, playerstatistic.RatingBattleWinsPlace, playerstatistic.RatingBattlesValue,
    //                    playerstatistic.RatingBattlesPlace, playerstatistic.RatingCapturedPointsValue, playerstatistic.RatingCapturedPointsPlace, playerstatistic.RatingDamageDealtValue, playerstatistic.RatingDamageDealtPlace,
    //                    playerstatistic.RatingDroppedPointsValue, playerstatistic.RatingDroppedPointsPlace, playerstatistic.RatingFragsValue, playerstatistic.RatingFragsPlace, playerstatistic.RatingSpottedValue, playerstatistic.RatingSpottedPlace,
    //                    playerstatistic.RatingXpValue, playerstatistic.RatingXpPlace, playerstatistic.AvgLevel, playerstatistic.AchievementsId, playerstatistic.RatingHitsPercentsValue, playerstatistic.RatingHitsPercentsPlace,
    //                    playerstatistic.RatingMaxXpValue, playerstatistic.RatingMaxXpPlace, playerstatistic.RBR, playerstatistic.WN8Rating, playerstatistic.PerformanceRating, playerstatistic.DamageTaken, playerstatistic.MaxFrags, playerstatistic.MaxDamage,
    //                    playerstatistic.MarkOfMastery, playerstatistic.UId, playerstatistic.PlayerUId, playerstatistic.AchievementsUId, playerstatistic.Rev]
    //            };

    //            var playerachievements = playerstatistic.Achievements;

    //            if (playerachievements) {

    //                console.log('uplayerachievements: ' + playerachievements.Id);

    //                var insertPlayerAchievements = {
    //                    name: 'insert_playerachievements',
    //                    text: 'INSERT INTO PlayerAchievements (Id,Warrior,Sniper,Invader,Defender,SteelWall,Confederate,Scout,PatrolDuty,HeroesOfRassenay,LafayettePool,RadleyWalters,CrucialContribution,BrothersInArms,Kolobanov,Nikolas,Orlik,Oskin,Halonen,Lehvaslaiho,DeLanglade,Burda,Dumitru,Pascucci,TamadaYoshio,Boelter,Fadin,Tarczay,BrunoPietro,Billotte,Survivor,Kamikaze,Invincible,Raider,Bombardier,Reaper,MouseTrap,PattonValley,Hunter,Sinai,MasterGunnerLongest,SharpshooterLongest,Ranger,CoolHeaded,Spartan,LuckyDevil,Kay,Carius,Knispel,Poppel,Abrams,Leclerk,Lavrinenko,Ekins,Sniper2,MainGun,MarksOnGun,MovingAvgDamage,MedalMonolith,MedalAntiSpgFire,MedalGore,MedalCoolBlood,MedalStark,Impenetrable,MaxAimerSeries,ShootToKill,Fighter,Duelist,Demolition,Arsonist,Bonecrusher,Charmed,Even,UId,Rev) VALUES ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12,$13,$14,$15,$16,$17,$18,$19,$20,$21,$22,$23,$24,$25,$26,$27,$28,$29,$30,$31,$32,$33,$34,$35,$36,$37,$38,$39,$40,$41,$42,$43,$44,$45,$46,$47,$48,$49,$50,$51,$52,$53,$54,$55,$56,$57,$58,$59,$60,$61,$62,$63,$64,$65,$66,$67,$68,$69,$70,$71,$72,$73,$74,$75)',
    //                    values: [playerachievements.Id, playerachievements.Warrior, playerachievements.Sniper, playerachievements.Invader, playerachievements.Defender, playerachievements.SteelWall, playerachievements.Confederate,
    //                            playerachievements.Scout, playerachievements.PatrolDuty, playerachievements.HeroesOfRassenay, playerachievements.LafayettePool, playerachievements.RadleyWalters, playerachievements.CrucialContribution,
    //                            playerachievements.BrothersInArms, playerachievements.Kolobanov, playerachievements.Nikolas, playerachievements.Orlik, playerachievements.Oskin, playerachievements.Halonen, playerachievements.Lehvaslaiho,
    //                            playerachievements.DeLanglade, playerachievements.Burda, playerachievements.Dumitru, playerachievements.Pascucci, playerachievements.TamadaYoshio, playerachievements.Boelter, playerachievements.Fadin,
    //                            playerachievements.Tarczay, playerachievements.BrunoPietro, playerachievements.Billotte, playerachievements.Survivor, playerachievements.Kamikaze, playerachievements.Invincible, playerachievements.Raider,
    //                            playerachievements.Bombardier, playerachievements.Reaper, playerachievements.MouseTrap, playerachievements.PattonValley, playerachievements.Hunter, playerachievements.Sinai, playerachievements.MasterGunnerLongest,
    //                            playerachievements.SharpshooterLongest, playerachievements.Huntsman, playerachievements.IronMan, playerachievements.Sturdy, playerachievements.LuckyDevil, playerachievements.Kay, playerachievements.Carius,
    //                            playerachievements.Knispel, playerachievements.Poppel, playerachievements.Abrams, playerachievements.Leclerk, playerachievements.Lavrinenko, playerachievements.Ekins, playerachievements.Sniper2, playerachievements.MainGun,
    //                            playerachievements.MarksOnGun, playerachievements.MovingAvgDamage, playerachievements.MedalMonolith, playerachievements.MedalAntiSpgFire, playerachievements.MedalGore, playerachievements.MedalCoolBlood,
    //                            playerachievements.MedalStark, playerachievements.Impenetrable, playerachievements.MaxAimerSeries, playerachievements.ShootToKill, playerachievements.Fighter, playerachievements.Duelist, playerachievements.Demolition,
    //                            playerachievements.Arsonist, playerachievements.Bonecrusher, playerachievements.Charmed, playerachievements.Even, playerachievements.UId, playerachievements.Rev]
    //                };

    //                client.query(insertPlayerAchievements);

    //                client.query(insertPlayerStatistic, function (err, result) {

    //                        // handle an error from the query
    //                        if (handleError(err, res, client, done)) return;

    //                        // return the client to the connection pool for other requests to reuse
    //                        done();
    //                    });
    //            }
    //            else {
    //                console.log('playerstatistic 2: ' + playerstatistic.Id);

    //                client.query(insertPlayerStatistic, function (err, result) {

    //                    // handle an error from the query
    //                    if (handleError(err, res, client, done)) return;

    //                    // return the client to the connection pool for other requests to reuse
    //                    done();
    //                });
    //            }
    //        }
    //    });
    //});

    app.post('/api/statistic/:rev', function (req, res) {

        var execute = function(x, done) {
            var data = req.body;

            var client = new Client();

            client.connectSync(conString);

            console.log('[' + worker.id + '] ' + 'update player info: ' + data.Player.PlayerId);

            if (req.params.rev == 0) {
                client.querySync('INSERT INTO Player (uid, id, name, playerid, server, rev, created) values ($1,$2,$3,$4,$5,$6,$7)',
                [data.Player.UId, data.Player.Id, data.Player.Name, data.Player.PlayerId, data.Player.Server, data.Player.Rev, data.Player.Creaded]);
            } else {
                client.querySync('Update Player set Rev = $1 where uid = $2',
                [data.Player.Rev, data.Player.UId]);
            }

            console.log('[' + worker.id + '] ' + 'upload tanks: ' + data.Player.PlayerId);
            for (index = 0; index < data.Tanks.length; ++index) {
                var tank = data.Tanks[index];

                client.querySync('INSERT INTO Tank (uid, id, tankid, name, tier, countryid, icon, tanktype, ispremium, playerid, playeruid, isfavorite) values ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12)',
                [tank.UId, tank.Id, tank.TankId, tank.Name, tank.Tier, tank.CountryId, tank.Icon, tank.TankType, tank.IsPremium, tank.PlayerId, tank.PlayerUId, tank.IsFavorite]);
            }

            console.log('[' + worker.id + '] ' + 'upload player statistic: ' + data.Player.PlayerId);

            for (index = 0; index < data.RandomStatistic.length; ++index) {

                var playerstatistic = data.RandomStatistic[index];

                var playerachievements = playerstatistic.Achievements;

                if (playerachievements) {

                    client.querySync('INSERT INTO PlayerAchievements (Id,Warrior,Sniper,Invader,Defender,SteelWall,Confederate,Scout,PatrolDuty,HeroesOfRassenay,LafayettePool,RadleyWalters,CrucialContribution,BrothersInArms,Kolobanov,Nikolas,Orlik,Oskin,Halonen,Lehvaslaiho,DeLanglade,Burda,Dumitru,Pascucci,TamadaYoshio,Boelter,Fadin,Tarczay,BrunoPietro,Billotte,Survivor,Kamikaze,Invincible,Raider,Bombardier,Reaper,MouseTrap,PattonValley,Hunter,Sinai,MasterGunnerLongest,SharpshooterLongest,Ranger,CoolHeaded,Spartan,LuckyDevil,Kay,Carius,Knispel,Poppel,Abrams,Leclerk,Lavrinenko,Ekins,Sniper2,MainGun,MarksOnGun,MovingAvgDamage,MedalMonolith,MedalAntiSpgFire,MedalGore,MedalCoolBlood,MedalStark,Impenetrable,MaxAimerSeries,ShootToKill,Fighter,Duelist,Demolition,Arsonist,Bonecrusher,Charmed,Even,UId,Rev) VALUES ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12,$13,$14,$15,$16,$17,$18,$19,$20,$21,$22,$23,$24,$25,$26,$27,$28,$29,$30,$31,$32,$33,$34,$35,$36,$37,$38,$39,$40,$41,$42,$43,$44,$45,$46,$47,$48,$49,$50,$51,$52,$53,$54,$55,$56,$57,$58,$59,$60,$61,$62,$63,$64,$65,$66,$67,$68,$69,$70,$71,$72,$73,$74,$75)',
                    [
                        playerachievements.Id, playerachievements.Warrior, playerachievements.Sniper, playerachievements.Invader, playerachievements.Defender, playerachievements.SteelWall, playerachievements.Confederate,
                        playerachievements.Scout, playerachievements.PatrolDuty, playerachievements.HeroesOfRassenay, playerachievements.LafayettePool, playerachievements.RadleyWalters, playerachievements.CrucialContribution,
                        playerachievements.BrothersInArms, playerachievements.Kolobanov, playerachievements.Nikolas, playerachievements.Orlik, playerachievements.Oskin, playerachievements.Halonen, playerachievements.Lehvaslaiho,
                        playerachievements.DeLanglade, playerachievements.Burda, playerachievements.Dumitru, playerachievements.Pascucci, playerachievements.TamadaYoshio, playerachievements.Boelter, playerachievements.Fadin,
                        playerachievements.Tarczay, playerachievements.BrunoPietro, playerachievements.Billotte, playerachievements.Survivor, playerachievements.Kamikaze, playerachievements.Invincible, playerachievements.Raider,
                        playerachievements.Bombardier, playerachievements.Reaper, playerachievements.MouseTrap, playerachievements.PattonValley, playerachievements.Hunter, playerachievements.Sinai, playerachievements.MasterGunnerLongest,
                        playerachievements.SharpshooterLongest, playerachievements.Huntsman, playerachievements.IronMan, playerachievements.Sturdy, playerachievements.LuckyDevil, playerachievements.Kay, playerachievements.Carius,
                        playerachievements.Knispel, playerachievements.Poppel, playerachievements.Abrams, playerachievements.Leclerk, playerachievements.Lavrinenko, playerachievements.Ekins, playerachievements.Sniper2, playerachievements.MainGun,
                        playerachievements.MarksOnGun, playerachievements.MovingAvgDamage, playerachievements.MedalMonolith, playerachievements.MedalAntiSpgFire, playerachievements.MedalGore, playerachievements.MedalCoolBlood,
                        playerachievements.MedalStark, playerachievements.Impenetrable, playerachievements.MaxAimerSeries, playerachievements.ShootToKill, playerachievements.Fighter, playerachievements.Duelist, playerachievements.Demolition,
                        playerachievements.Arsonist, playerachievements.Bonecrusher, playerachievements.Charmed, playerachievements.Even, playerachievements.UId, playerachievements.Rev
                    ]);
                }

                client.querySync('INSERT INTO PlayerStatistic (Id,PlayerId,Updated,Wins,Losses,SurvivedBattles,Xp,BattleAvgXp,MaxXp,Frags,Spotted,HitsPercents,DamageDealt,CapturePoints,DroppedCapturePoints,BattlesCount,Rating_IntegratedValue,Rating_IntegratedPlace,Rating_BattleAvgPerformanceValue,Rating_BattleAvgPerformancePlace,Rating_BattleAvgXpValue,Rating_BattleAvgXpPlace,Rating_BattleWinsValue,Rating_BattleWinsPlace,Rating_BattlesValue,Rating_BattlesPlace,Rating_CapturedPointsValue,Rating_CapturedPointsPlace,Rating_DamageDealtValue,Rating_DamageDealtPlace,Rating_DroppedPointsValue,Rating_DroppedPointsPlace,Rating_FragsValue,Rating_FragsPlace,Rating_SpottedValue,Rating_SpottedPlace,Rating_XpValue,Rating_XpPlace,AvgLevel,AchievementsId,Rating_HitsPercentsValue,Rating_HitsPercentsPlace,Rating_MaxXpValue,Rating_MaxXpPlace,RBR,WN8Rating,PerformanceRating,DamageTaken,MaxFrags,MaxDamage,MarkOfMastery,UId,PlayerUId,AchievementsUId,Rev) VALUES ($1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12,$13,$14,$15,$16,$17,$18,$19,$20,$21,$22,$23,$24,$25,$26,$27,$28,$29,$30,$31,$32,$33,$34,$35,$36,$37,$38,$39,$40,$41,$42,$43,$44,$45,$46,$47,$48,$49,$50,$51,$52,$53,$54,$55)',
                [
                    playerstatistic.Id, playerstatistic.PlayerId, playerstatistic.Updated, playerstatistic.Wins, playerstatistic.Losses, playerstatistic.SurvivedBattles, playerstatistic.Xp, playerstatistic.BattleAvgXp,
                    playerstatistic.MaxXp, playerstatistic.Frags, playerstatistic.Spotted, playerstatistic.HitsPercents, playerstatistic.DamageDealt, playerstatistic.CapturePoints, playerstatistic.DroppedCapturePoints,
                    playerstatistic.BattlesCount, playerstatistic.RatingIntegratedValue, playerstatistic.RatingIntegratedPlace, playerstatistic.RatingWinsRatioValue, playerstatistic.RatingWinsRatioPlace,
                    playerstatistic.RatingBattleAvgXpValue, playerstatistic.RatingBattleAvgXpPlace, playerstatistic.RatingBattleWinsValue, playerstatistic.RatingBattleWinsPlace, playerstatistic.RatingBattlesValue,
                    playerstatistic.RatingBattlesPlace, playerstatistic.RatingCapturedPointsValue, playerstatistic.RatingCapturedPointsPlace, playerstatistic.RatingDamageDealtValue, playerstatistic.RatingDamageDealtPlace,
                    playerstatistic.RatingDroppedPointsValue, playerstatistic.RatingDroppedPointsPlace, playerstatistic.RatingFragsValue, playerstatistic.RatingFragsPlace, playerstatistic.RatingSpottedValue, playerstatistic.RatingSpottedPlace,
                    playerstatistic.RatingXpValue, playerstatistic.RatingXpPlace, playerstatistic.AvgLevel, playerstatistic.AchievementsId, playerstatistic.RatingHitsPercentsValue, playerstatistic.RatingHitsPercentsPlace,
                    playerstatistic.RatingMaxXpValue, playerstatistic.RatingMaxXpPlace, playerstatistic.RBR, playerstatistic.WN8Rating, playerstatistic.PerformanceRating, playerstatistic.DamageTaken, playerstatistic.MaxFrags, playerstatistic.MaxDamage,
                    playerstatistic.MarkOfMastery, playerstatistic.UId, playerstatistic.PlayerUId, playerstatistic.AchievementsUId, playerstatistic.Rev
                ]);
            }
        };

        async.times(1, execute, function () {
            console.log('end');
        });
    });

    app.use(function (req, res) {
        res.render('404', { url: req.url });
    });

    // Workers can share any TCP connection
    // In this case its a HTTP server
    return app.listen(app.get('port'));
};

//http.createServer(app).listen(app.get('port'), function () {
//    console.log('Express server listening on port ' + app.get('port'));
//});

cluster(serverWorker, { count: 5 });