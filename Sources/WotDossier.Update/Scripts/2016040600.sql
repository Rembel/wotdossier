ALTER TABLE [Tank] ADD [UniqueId] int NOT NULL default(0);
update [Tank] set [UniqueId] = CountryId * 10000 + TankId;