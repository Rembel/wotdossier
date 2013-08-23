CREATE TABLE [Replay] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [ReplayId] bigint NOT NULL
, [PlayerId] bigint NOT NULL
, [Link] nvarchar(256) NOT NULL
);