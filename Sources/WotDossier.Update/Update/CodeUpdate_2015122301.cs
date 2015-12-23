using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete double tank statistic rows
    /// </summary>
    public class CodeUpdate_2015122301 : CodeUpdateBase
    {
        private long _version = 2015122301;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            //var dataProvider = CompositionContainerFactory.Instance.GetExport<DataProvider>();

            //dataProvider.OpenSession();
            //IList<PlayerEntity> list = dataProvider.QueryOver<PlayerEntity>().List<PlayerEntity>();

            //foreach (var playerEntity in list)
            //{
            //    playerEntity.UId = Guid.NewGuid();
            //    dataProvider.Save(playerEntity);
            //}

            //dataProvider.CloseSession();

            List<string> tablesList = new List<string>
            {
                "HistoricalBattlesAchievements",
"HistoricalBattlesStatistic",
"Player",
"PlayerAchievements",
"PlayerStatistic",
"Replay",
"Tank",
"TankHistoricalBattleStatistic",
"TankStatistic",
"TankTeamBattleStatistic",
"TeamBattlesAchievements",
"TeamBattlesStatistic",
            };
            foreach (var tableName in tablesList)
            {
                string commandText = string.Format(@"Select Id from {0}", tableName);
                SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                List<int> list = new List<int>();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add((int)(long)reader["Id"]);
                    }
                }

                foreach (var id in list)
                {
                    string uid = Guid.NewGuid().ToString();

                    commandText = string.Format("Update {0} set UId = @uid where Id = @id", tableName);
                    command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                    command.Parameters.Add("@uid", DbType.String).Value = uid;
                    command.Parameters.Add("@id", DbType.Int32).Value = id;
                    command.ExecuteNonQuery();

                    if (tableName == "Tank")
                    {
                        commandText = @"Update [TankStatistic] set TankUId = @uid where TankId = @id;
                            Update [TankTeamBattleStatistic] set TankUId = @uid where TankId = @id;
                            Update [TankHistoricalBattleStatistic] set TankUId = @uid where TankId = @id;";
                        command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                        command.Parameters.Add("@uid", DbType.String).Value = uid;
                        command.Parameters.Add("@id", DbType.Int32).Value = id;
                        command.ExecuteNonQuery();
                    }

                    if (tableName == "Player")
                    {
                        commandText = @"Update [HistoricalBattlesStatistic] set PlayerUId = @uid where PlayerId = @id;
                            Update [PlayerStatistic] set PlayerUId = @uid where PlayerId = @id;
                            Update [TeamBattlesStatistic] set PlayerUId = @uid where PlayerId = @id;
                            Update [Tank] set PlayerUId = @uid where PlayerId = @id;";
                        command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                        command.Parameters.Add("@uid", DbType.String).Value = uid;
                        command.Parameters.Add("@id", DbType.Int32).Value = id;
                        command.ExecuteNonQuery();
                    }

                    if (tableName == "HistoricalBattlesAchievements")
                    {
                        commandText = @"Update [HistoricalBattlesStatistic] set [AchievementsUId] = @uid where AchievementsId = @id;";
                        command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                        command.Parameters.Add("@uid", DbType.String).Value = uid;
                        command.Parameters.Add("@id", DbType.Int32).Value = id;
                        command.ExecuteNonQuery();
                    }

                    if (tableName == "PlayerAchievements")
                    {
                        commandText = @"Update [PlayerStatistic] set [AchievementsUId] = @uid where AchievementsId = @id;";
                        command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                        command.Parameters.Add("@uid", DbType.String).Value = uid;
                        command.Parameters.Add("@id", DbType.Int32).Value = id;
                        command.ExecuteNonQuery();
                    }

                    if (tableName == "TeamBattlesAchievements")
                    {
                        commandText = @"Update [TeamBattlesStatistic] set [AchievementsUId] = @uid where AchievementsId = @id;";
                        command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                        command.Parameters.Add("@uid", DbType.String).Value = uid;
                        command.Parameters.Add("@id", DbType.Int32).Value = id;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}