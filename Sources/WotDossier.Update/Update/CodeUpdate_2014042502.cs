using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using WotDossier.Domain.Entities;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete double tank statistic rows
    /// </summary>
    public class CodeUpdate_2014042502 : CodeUpdateBase
    {
        private long _version = 2014042502;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            List<TankRandomBattlesStatisticEntity> recordsToUpdate = GetTankStatisticRecordsToUpdate(sqlCeConnection, transaction);
            
            List<TankRandomBattlesStatisticEntity> backup = BackupRecordsActualData(sqlCeConnection, transaction, recordsToUpdate);

            foreach (TankRandomBattlesStatisticEntity entity in backup)
            {
                const string commandText = @"Delete from TankStatistic where TankId = @TankId and BattlesCount = @BattlesCount;
Insert Into TankStatistic(TankId, Updated, Version, Raw, BattlesCount) values (@TankId, @Updated, @Version, @Raw, @BattlesCount);";
                SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@TankId", DbType.Int32).Value = entity.TankId;
                command.Parameters.Add("@Updated", DbType.DateTime).Value = entity.Updated;
                command.Parameters.Add("@Version", DbType.Int32).Value = entity.Version;
                command.Parameters.Add("@Raw", DbType.Binary).Value = entity.Raw;
                command.Parameters.Add("@BattlesCount", DbType.Int32).Value = entity.BattlesCount;
                command.ExecuteNonQuery();
            }
        }

        private static List<TankRandomBattlesStatisticEntity> BackupRecordsActualData(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction,
            List<TankRandomBattlesStatisticEntity> recordsToUpdate)
        {
            List<TankRandomBattlesStatisticEntity> backup = new List<TankRandomBattlesStatisticEntity>();

            foreach (var statisticEntity in recordsToUpdate)
            {
                const string commandText = @"select TankId, Updated, Version, Raw, BattlesCount from TankStatistic where TankId = @TankId and BattlesCount = @BattlesCount order by id desc limit 1";
                SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@TankId", DbType.Int32).Value = statisticEntity.TankId;
                command.Parameters.Add("@BattlesCount", DbType.Int32).Value = statisticEntity.BattlesCount;
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TankRandomBattlesStatisticEntity entity = new TankRandomBattlesStatisticEntity();
                        entity.TankId = (int) reader["TankId"];
                        entity.Updated = (DateTime) reader["Updated"];
                        entity.Version = (int) reader["Version"];
                        entity.Raw = (byte[]) reader["Raw"];
                        entity.BattlesCount = (int) reader["BattlesCount"];

                        backup.Add(entity);
                    }
                }
            }
            return backup;
        }

        private static List<TankRandomBattlesStatisticEntity> GetTankStatisticRecordsToUpdate(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            const string commandText = @"select TankStatistic.TankId, BattlesCount, count(1) as RecCount from TankStatistic
                                    group by TankStatistic.TankId, BattlesCount having RecCount > 1 order by TankStatistic.TankId";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
            List<TankRandomBattlesStatisticEntity> list = new List<TankRandomBattlesStatisticEntity>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TankRandomBattlesStatisticEntity entity = new TankRandomBattlesStatisticEntity();
                    entity.TankId = (int) reader["TankId"];
                    entity.BattlesCount = (int) reader["BattlesCount"];

                    list.Add(entity);
                }
            }
            return list;
        }
    }
}