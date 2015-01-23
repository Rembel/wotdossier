CREATE TEMPORARY TABLE replay_backup([Id], [ReplayId], [PlayerId], [Link]);
INSERT INTO replay_backup SELECT [Id], [ReplayId], [PlayerId], [Link] FROM [Replay];
DROP TABLE [Replay];
CREATE TABLE [Replay] (
  [Id] INTEGER NOT NULL
, [ReplayId] bigint NOT NULL
, [PlayerId] bigint NOT NULL
, [Link] nvarchar(256) NULL
, [Raw] image
, CONSTRAINT [PK_Replay] PRIMARY KEY ([Id])
);
INSERT INTO [Replay] SELECT [Id], [ReplayId], [PlayerId], [Link], null FROM replay_backup;
DROP TABLE replay_backup;