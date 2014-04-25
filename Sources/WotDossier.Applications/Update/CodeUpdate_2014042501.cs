using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Update
{
    /// <summary>
    /// BattleCount column added to tank statistic
    /// </summary>
    public class CodeUpdate_2014042501 : CodeUpdateBase
    {
        private long _version = 2014042501;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            string commandText = @"Select Id, Raw from TankStatistic";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
            List<TankStatisticEntity> list = new List<TankStatisticEntity>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TankStatisticEntity entity = new TankStatisticEntity();
                    entity.Id = (int)(long) reader["Id"];
                    entity.Raw = (byte[]) reader["Raw"];

                    list.Add(entity);
                }
            }

            foreach (TankStatisticEntity entity in list)
            {
                TankJson tank = CompressHelper.DecompressObject<TankJson>(entity.Raw);
                commandText = @"Update TankStatistic set BattlesCount=@count, Version = @version where Id=@id";
                command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@count", DbType.Int32).Value = tank.A15x15.battlesCount;
                command.Parameters.Add("@id", DbType.Int32).Value = entity.Id;
                command.Parameters.Add("@version", DbType.Int32).Value = tank.Common.basedonversion;
                command.ExecuteNonQuery();
            }
        }
    }
}