CREATE TABLE [TankTeamBattleStatistic] (
  [Id] INTEGER NOT NULL
, [TankId] int NOT NULL
, [Updated] datetime NOT NULL
, [Version] int NOT NULL
, [Raw] image NOT NULL
, CONSTRAINT [PK_TankTeamBattleStatistic] PRIMARY KEY ([Id])
, FOREIGN KEY ([TankId]) REFERENCES [Tank] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)