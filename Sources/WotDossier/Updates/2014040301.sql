CREATE TABLE [HistoricalBattlesAchievements] (
  [Id] INTEGER NOT NULL
  ,[GuardsMan]  INTEGER NOT NULL
  ,[MakerOfHistory]  INTEGER NOT NULL
  ,[BothSidesWins]  INTEGER NOT NULL
  ,[WeakVehiclesWins]  INTEGER NOT NULL
, CONSTRAINT [PK_HistoricalBattlesAchievements] PRIMARY KEY ([Id])
)