CREATE TABLE [RandomBattlesAchievements] (
  [Id] INTEGER NOT NULL
, [UId] nvarchar(36) NULL
, [Warrior] int NOT NULL
, [Sniper] int NOT NULL
, [Invader] int NOT NULL
, [Defender] int NOT NULL
, [SteelWall] int NOT NULL
, [Confederate] int NOT NULL
, [Scout] int NOT NULL
, [PatrolDuty] int NOT NULL
, [HeroesOfRassenay] int NOT NULL
, [LafayettePool] int NOT NULL
, [RadleyWalters] int NOT NULL
, [CrucialContribution] int NOT NULL
, [BrothersInArms] int NOT NULL
, [Kolobanov] int NOT NULL
, [Nikolas] int NOT NULL
, [Orlik] int NOT NULL
, [Oskin] int NOT NULL
, [Halonen] int NOT NULL
, [Lehvaslaiho] int NOT NULL
, [DeLanglade] int NOT NULL
, [Burda] int NOT NULL
, [Dumitru] int NOT NULL
, [Pascucci] int NOT NULL
, [TamadaYoshio] int NOT NULL
, [Boelter] int NOT NULL
, [Fadin] int NOT NULL
, [Tarczay] int NOT NULL
, [BrunoPietro] int NOT NULL
, [Billotte] int NOT NULL
, [Survivor] int NOT NULL
, [Kamikaze] int NOT NULL
, [Invincible] int NOT NULL
, [Raider] int NOT NULL
, [Bombardier] int NOT NULL
, [Reaper] int NOT NULL
, [MouseTrap] int NOT NULL
, [PattonValley] int NOT NULL
, [Hunter] int NOT NULL
, [Sinai] int NOT NULL
, [MasterGunnerLongest] int NOT NULL
, [SharpshooterLongest] int NOT NULL
, [Ranger] int NOT NULL
, [CoolHeaded] int NOT NULL
, [Spartan] int NOT NULL
, [LuckyDevil] int NOT NULL
, [Kay] int NOT NULL
, [Carius] int NOT NULL
, [Knispel] int NOT NULL
, [Poppel] int NOT NULL
, [Abrams] int NOT NULL
, [Leclerk] int NOT NULL
, [Lavrinenko] int NOT NULL
, [Ekins] int NOT NULL
, [Sniper2] int  NOT NULL default(0)
, [MainGun] int  NOT NULL default(0)
, [MarksOnGun] int  NOT NULL default(0)
, [MovingAvgDamage] int  NOT NULL default(0)
, [MedalMonolith] int  NOT NULL default(0)
, [MedalAntiSpgFire] int  NOT NULL default(0)
, [MedalGore] int  NOT NULL default(0)
, [MedalCoolBlood] int  NOT NULL default(0)
, [MedalStark] int  NOT NULL default(0)
, [Impenetrable] int  NOT NULL default(0)
, [MaxAimerSeries] int  NOT NULL default(0)
, [ShootToKill] int  NOT NULL default(0)
, [Fighter] int  NOT NULL default(0)
, [Duelist] int  NOT NULL default(0)
, [Demolition] int  NOT NULL default(0)
, [Arsonist] int  NOT NULL default(0)
, [Bonecrusher] int  NOT NULL default(0)
, [Charmed] int  NOT NULL default(0)
, [Even] int  NOT NULL default(0)
, [Rev] int NOT NULL default(2015122300)
, CONSTRAINT [PK_RandomBattlesAchievements] PRIMARY KEY ([Id])
);

CREATE TABLE [RandomBattlesStatistic] (
  [Id] INTEGER NOT NULL
, [UId] nvarchar(36) NULL
, [PlayerId] int NOT NULL
, [PlayerUId] nvarchar(36) NULL
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
, [AchievementsUId] nvarchar(36) NULL
, [RBR] float  NOT NULL default(0)
, [WN8Rating] float  NOT NULL default(0)
, [PerformanceRating] float  NOT NULL default(0)
, [DamageTaken] int  NOT NULL default(0)
, [MaxFrags] int  NOT NULL default(0)
, [MaxDamage] int  NOT NULL default(0)
, [MarkOfMastery] int  NOT NULL default(0)
, [Rev] int NOT NULL default(2015122300)
, CONSTRAINT [PK_RandomBattlesStatistic] PRIMARY KEY ([Id])
, FOREIGN KEY ([PlayerId]) REFERENCES [Player] ([Id])
, FOREIGN KEY ([AchievementsId]) REFERENCES [RandomBattlesAchievements] ([Id])
);

CREATE TABLE [TankRandomBattlesStatistic] (
  [Id] INTEGER NOT NULL
, [UId] nvarchar(36) NULL
, [TankId] int NOT NULL
, [TankUId] nvarchar(36) NULL
, [BattlesCount] int  NOT NULL default(0)
, [Updated] datetime NOT NULL
, [Version] int NOT NULL
, [Rev] int NOT NULL default(2015122300)
, [Raw] image NOT NULL
, CONSTRAINT [PK_TankStatistic] PRIMARY KEY ([Id])
, FOREIGN KEY ([TankId]) REFERENCES Tank ([Id])
);

/*migrate data*/
insert into [TankRandomBattlesStatistic] (
  [Id]
, [UId]
, [TankId]
, [TankUId]
, [BattlesCount]
, [Updated]
, [Version]
, [Rev]
, [Raw])
select [Id]
, [UId]
, [TankId]
, [TankUId]
, [BattlesCount]
, [Updated]
, [Version]
, [Rev]
, [Raw] from [TankStatistic];

drop table [TankStatistic];

insert into [RandomBattlesAchievements] (
  [Id]
, [UId]
, [Warrior]
, [Sniper]
, [Invader]
, [Defender]
, [SteelWall]
, [Confederate]
, [Scout]
, [PatrolDuty]
, [HeroesOfRassenay]
, [LafayettePool]
, [RadleyWalters]
, [CrucialContribution]
, [BrothersInArms]
, [Kolobanov]
, [Nikolas]
, [Orlik]
, [Oskin]
, [Halonen]
, [Lehvaslaiho]
, [DeLanglade]
, [Burda]
, [Dumitru]
, [Pascucci]
, [TamadaYoshio]
, [Boelter]
, [Fadin]
, [Tarczay]
, [BrunoPietro]
, [Billotte]
, [Survivor]
, [Kamikaze]
, [Invincible]
, [Raider]
, [Bombardier]
, [Reaper]
, [MouseTrap]
, [PattonValley]
, [Hunter]
, [Sinai]
, [MasterGunnerLongest]
, [SharpshooterLongest]
, [Ranger]
, [CoolHeaded]
, [Spartan]
, [LuckyDevil]
, [Kay]
, [Carius]
, [Knispel]
, [Poppel]
, [Abrams]
, [Leclerk]
, [Lavrinenko]
, [Ekins]
, [Sniper2]
, [MainGun]
, [MarksOnGun]
, [MovingAvgDamage]
, [MedalMonolith]
, [MedalAntiSpgFire]
, [MedalGore]
, [MedalCoolBlood]
, [MedalStark]
, [Impenetrable]
, [MaxAimerSeries]
, [ShootToKill]
, [Fighter]
, [Duelist]
, [Demolition]
, [Arsonist]
, [Bonecrusher]
, [Charmed]
, [Even]
, [Rev])
select [Id]
, [UId]
, [Warrior]
, [Sniper]
, [Invader]
, [Defender]
, [SteelWall]
, [Confederate]
, [Scout]
, [PatrolDuty]
, [HeroesOfRassenay]
, [LafayettePool]
, [RadleyWalters]
, [CrucialContribution]
, [BrothersInArms]
, [Kolobanov]
, [Nikolas]
, [Orlik]
, [Oskin]
, [Halonen]
, [Lehvaslaiho]
, [DeLanglade]
, [Burda]
, [Dumitru]
, [Pascucci]
, [TamadaYoshio]
, [Boelter]
, [Fadin]
, [Tarczay]
, [BrunoPietro]
, [Billotte]
, [Survivor]
, [Kamikaze]
, [Invincible]
, [Raider]
, [Bombardier]
, [Reaper]
, [MouseTrap]
, [PattonValley]
, [Hunter]
, [Sinai]
, [MasterGunnerLongest]
, [SharpshooterLongest]
, [Ranger]
, [CoolHeaded]
, [Spartan]
, [LuckyDevil]
, [Kay]
, [Carius]
, [Knispel]
, [Poppel]
, [Abrams]
, [Leclerk]
, [Lavrinenko]
, [Ekins]
, [Sniper2]
, [MainGun]
, [MarksOnGun]
, [MovingAvgDamage]
, [MedalMonolith]
, [MedalAntiSpgFire]
, [MedalGore]
, [MedalCoolBlood]
, [MedalStark]
, [Impenetrable]
, [MaxAimerSeries]
, [ShootToKill]
, [Fighter]
, [Duelist]
, [Demolition]
, [Arsonist]
, [Bonecrusher]
, [Charmed]
, [Even]
, [Rev] from [PlayerAchievements];

drop table [PlayerAchievements];

insert into [RandomBattlesStatistic] (
  [Id]
, [UId]
, [PlayerId]
, [PlayerUId]
, [Updated]
, [Wins]
, [Losses]
, [SurvivedBattles]
, [Xp]
, [BattleAvgXp]
, [MaxXp]
, [Frags]
, [Spotted]
, [HitsPercents]
, [DamageDealt]
, [CapturePoints]
, [DroppedCapturePoints]
, [BattlesCount]
, [AvgLevel]
, [AchievementsId]
, [AchievementsUId]
, [RBR]
, [WN8Rating]
, [PerformanceRating]
, [DamageTaken]
, [MaxFrags]
, [MaxDamage]
, [MarkOfMastery]
, [Rev])
select [Id]
, [UId]
, [PlayerId]
, [PlayerUId]
, [Updated]
, [Wins]
, [Losses]
, [SurvivedBattles]
, [Xp]
, [BattleAvgXp]
, [MaxXp]
, [Frags]
, [Spotted]
, [HitsPercents]
, [DamageDealt]
, [CapturePoints]
, [DroppedCapturePoints]
, [BattlesCount]
, [AvgLevel]
, [AchievementsId]
, [AchievementsUId]
, [RBR]
, [WN8Rating]
, [PerformanceRating]
, [DamageTaken]
, [MaxFrags]
, [MaxDamage]
, [MarkOfMastery]
, [Rev] from [PlayerStatistic];

drop table [PlayerStatistic];
