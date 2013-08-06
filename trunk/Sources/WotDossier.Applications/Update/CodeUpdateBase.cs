using System;
using System.Data;
using System.Data.SqlServerCe;

namespace WotDossier.Applications.Update
{
    public abstract class CodeUpdateBase : IDbUpdate
    {
        public abstract long Version { get; set; }

        public abstract void Execute(SqlCeConnection sqlCeConnection, SqlCeTransaction transaction);
    }

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

            string commandText = @"Update ";
            SqlCeCommand command = new SqlCeCommand(commandText, sqlCeConnection, transaction);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }
    }
}