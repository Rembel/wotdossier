ALTER TABLE [HistoricalBattlesAchievements] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [HistoricalBattlesStatistic] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [Player] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [PlayerAchievements] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [PlayerStatistic] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [Replay] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [Tank] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [TankHistoricalBattleStatistic] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [TankStatistic] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [TankTeamBattleStatistic] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [TeamBattlesAchievements] ADD [UId] nvarchar(36) NULL;
ALTER TABLE [TeamBattlesStatistic] ADD [UId] nvarchar(36) NULL;

ALTER TABLE [HistoricalBattlesStatistic] ADD [PlayerUId] nvarchar(36) NULL;
ALTER TABLE [HistoricalBattlesStatistic] ADD [AchievementsUId] nvarchar(36) NULL;
ALTER TABLE [PlayerStatistic] ADD [PlayerUId] nvarchar(36) NULL;
ALTER TABLE [PlayerStatistic] ADD [AchievementsUId] nvarchar(36) NULL;
ALTER TABLE [TeamBattlesStatistic] ADD [PlayerUId] nvarchar(36) NULL;
ALTER TABLE [TeamBattlesStatistic] ADD [AchievementsUId] nvarchar(36) NULL;
ALTER TABLE [Tank] ADD [PlayerUId] nvarchar(36) NULL;
ALTER TABLE [TankStatistic] ADD [TankUId] nvarchar(36) NULL;
ALTER TABLE [TankTeamBattleStatistic] ADD [TankUId] nvarchar(36) NULL;
ALTER TABLE [TankHistoricalBattleStatistic] ADD [TankUId] nvarchar(36) NULL;


ALTER TABLE [HistoricalBattlesAchievements] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [HistoricalBattlesStatistic] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [Player] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [PlayerAchievements] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [PlayerStatistic] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [Replay] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [Tank] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [TankHistoricalBattleStatistic] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [TankStatistic] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [TankTeamBattleStatistic] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [TeamBattlesAchievements] ADD [Rev] int NOT NULL default(201412200);
ALTER TABLE [TeamBattlesStatistic] ADD [Rev] int NOT NULL default(201412200);
