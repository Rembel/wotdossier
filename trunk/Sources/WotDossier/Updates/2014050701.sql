CREATE TEMPORARY TABLE tank_backup([Id], [TankId], [Name], [Tier], [CountryId], [Icon], [TankType], [IsPremium], [PlayerId], [IsFavorite]);
INSERT INTO tank_backup SELECT [Id], [TankId], [Name], [Tier], [CountryId], [Icon], [TankType], [IsPremium], [PlayerId], [IsFavorite] FROM [Tank];
DROP TABLE [Tank];
CREATE TABLE [Tank] (
  [Id] INTEGER NOT NULL
, [TankId] int NOT NULL
, [Name] nvarchar(100) NOT NULL
, [Tier] int NOT NULL
, [CountryId] int NOT NULL
, [Icon] nvarchar(255) NOT NULL
, [TankType] int NOT NULL
, [IsPremium] bit NOT NULL
, [PlayerId] int NOT NULL
, [IsFavorite] bit DEFAULT (0) NOT NULL
, CONSTRAINT [PK_Tank] PRIMARY KEY ([Id])
, FOREIGN KEY ([PlayerId]) REFERENCES [Player] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
INSERT INTO [Tank] SELECT [Id], [TankId], [Name], [Tier], [CountryId], [Icon], [TankType], [IsPremium], [PlayerId], [IsFavorite] FROM tank_backup;
DROP TABLE tank_backup;