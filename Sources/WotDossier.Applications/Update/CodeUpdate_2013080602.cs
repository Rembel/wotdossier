using System;
using System.Data;
using System.Data.SqlServerCe;

namespace WotDossier.Applications.Update
{
    public class CodeUpdate_2013080602 : CodeUpdateBase
    {
        private long _version = 2013080602;

        public override long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction)
        {
            TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);

            string commandText = @"Update PlayerStatistic set Updated = DATEADD(hour,@zone,Updated)";
            SqlCeCommand command = new SqlCeCommand(commandText, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;
            command.Parameters.Add("@zone", SqlDbType.Int).Value = utcOffset.Hours;

            command.ExecuteNonQuery();
        }
    }
}