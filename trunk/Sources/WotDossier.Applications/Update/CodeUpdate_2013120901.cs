using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;
using WotDossier.Dal;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Update
{
    /// <summary>
    /// patch 0.8.9 support
    /// dossier cache structure version change
    /// updates all old structures saved in db to version 65
    /// </summary>
    public class CodeUpdate_2013120901 : CodeUpdateBase
    {
        private long _version = 2013120901;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            string commandText = @"Select Id, Version, Raw from TankStatistic";
            SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
            List<TankStatisticEntity> list = new List<TankStatisticEntity>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TankStatisticEntity entity = new TankStatisticEntity();

                    entity.Id = (int) reader[0];
                    entity.Version = (int) reader[1];
                    entity.Raw = (byte[]) reader[2];

                    list.Add(entity);
                }
            }

            foreach (TankStatisticEntity entity in list)
            {
                TankJson tank = WotApiHelper.UnZipObject<TankJson>(entity.Raw);

                if (tank.Common.basedonversion < 29)
                {
                    tank.A15x15.battlesCountBefore8_8 = tank.A15x15.battlesCount;
                    tank.A15x15.xpBefore8_8 = tank.A15x15.xp;
                }

                byte[] zip = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));

                commandText = @"Update TankStatistic set Raw=@raw where Id=@id";
                command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@raw", DbType.Binary).Value = zip;
                command.Parameters.Add("@id", DbType.Int32).Value = entity.Id;
                command.ExecuteNonQuery();
            }
        }
    }
}