CREATE TABLE [TeamBattlesStatistic] (
  [Id] INTEGER NOT NULL
, [PlayerId] int NOT NULL
, [Updated] datetime NOT NULL
, [Wins] int NOT NULL
, [Losses] int NOT NULL
, [SurvivedBattles] int NOT NULL
, [Xp] int NOT NULL
, [BattleAvgXp] float NULL
, [MaxXp] int NOT NULL
, [Frags] int NOT NULL
, [Spotted] int NOT NULL
, [HitsPercents] float NULL
, [DamageDealt] int NOT NULL
, [CapturePoints] int NOT NULL
, [DroppedCapturePoints] int NOT NULL
, [BattlesCount] int NOT NULL
, [AvgLevel] float NOT NULL
, [AchievementsId] int NULL
, CONSTRAINT [PK__TeamBattlesStatistic__0000000000000031] PRIMARY KEY ([Id])
, FOREIGN KEY ([PlayerId]) REFERENCES [Player] ([Id])
, FOREIGN KEY ([AchievementsId]) REFERENCES [TeamBattlesAchievements] ([Id])
);