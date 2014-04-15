using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;
using WotDossier.Dal;
using WotDossier.Domain.Dossier.TankV29;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Tank;

namespace WotDossier.Applications.Update
{
    /// <summary>
    /// patch 0.8.9 support
    /// dossier cache structure version change
    /// updates all old structures saved in db to version 65
    /// </summary>
    public class CodeUpdate_2013102401 : CodeUpdateBase
    {
        private long _version = 2013102401;

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
                if (entity.Version < 65)
                {
                    TankJson29 tankV29 = WotApiHelper.UnZipObject<TankJson29>(entity.Raw);
                    TankJson tank = DataMapper.Map(tankV29);

                    byte[] zip = WotApiHelper.Zip(JsonConvert.SerializeObject(tank));
                    
                    commandText = @"Update TankStatistic set Version=65, Raw=@raw where Id=@id";
                    command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                    command.Parameters.Add("@raw", DbType.Binary).Value = zip;
                    command.Parameters.Add("@id", DbType.Int32).Value = entity.Id;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}