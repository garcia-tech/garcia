using GarciaCore.Infrastructure;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GarciaCore.ORM
{
    public class PostgresqlDatabaseConnection : DatabaseConnection
    {
        public override string IdentitySqlStatement => string.Format(" returning \"Id\";", TablePrefix);
        public override string RowCountSqlStatement => string.Format(" returning \"Id\";", TablePrefix);
        public override string BeginTranSqlStatement => "begin transaction;";
        public override string CommitTranSqlStatement => "commit transaction";
        public override string GetDateSqlStatement => "now()";
        public override string ColumnPrefix => "\"";
        public override string ColumnPpostfix => "\"";
        public override string TablePrefix => "\"";
        public override string IdKeyword => string.Format("\"Id\"", TablePrefix);

        public PostgresqlDatabaseConnection(IOptions<DatabaseSettings> databaseSettings) : base(databaseSettings)
        {
        }

        public override IDbCommand CreateCommand(string sql, IDbConnection connection)
        {
            return CreateCommand(sql, connection, null);
        }

        public override IDbCommand CreateCommand(string sql, IDbConnection connection, Dictionary<string, object> parameters)
        {
            var command = new NpgsqlCommand(sql, connection as NpgsqlConnection);
            command.Parameters.AddRange(parameters?.Select(x => new NpgsqlParameter(x.Key, x.Value)).ToArray());
            return command;
        }

        public override IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_databaseSettings.ConnectionString);
        }
    }
}
