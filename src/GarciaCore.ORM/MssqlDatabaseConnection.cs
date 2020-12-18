using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using GarciaCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Linq;

namespace GarciaCore.ORM
{
    public class MssqlDatabaseConnection : DatabaseConnection
    {
        public MssqlDatabaseConnection(IOptions<DatabaseSettings> databaseSettings) : base(databaseSettings)
        {
        }

        public override IDbCommand CreateCommand(string sql, IDbConnection connection)
        {
            return CreateCommand(sql, connection, null);
        }

        public override IDbCommand CreateCommand(string sql, IDbConnection connection, Dictionary<string, object> parameters)
        {
            var command = new SqlCommand(sql, connection as SqlConnection);
            command.Parameters.AddRange(parameters?.Select(x => new SqlParameter(x.Key, x.Value)).ToArray());
            return command;
        }

        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(_databaseSettings.ConnectionString);
        }
    }
}
