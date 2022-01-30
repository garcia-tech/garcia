using GarciaCore.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GarciaCore.Persistence.ORM;

public class MssqlDatabaseConnection : DatabaseConnection
{
    public override string IdentitySqlStatement => ";select @@identity;";
    public override string RowCountSqlStatement => "select @@rowcount;";
    public override string BeginTranSqlStatement => "begin transaction;";
    public override string CommitTranSqlStatement => "commit transaction";
    public override string GetDateSqlStatement => "getdate()";
    public override string ColumnPrefix => "";
    public override string ColumnPpostfix => "";
    public override string TablePrefix => "";
    public override string IdKeyword => string.Format("Id", TablePrefix);

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