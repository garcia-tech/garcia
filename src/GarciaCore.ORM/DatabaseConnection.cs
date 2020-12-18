using GarciaCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GarciaCore.ORM
{
    public abstract class DatabaseConnection
    {
        protected DatabaseSettings _databaseSettings;
        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand(string sql, IDbConnection connection);
        public abstract IDbCommand CreateCommand(string sql, IDbConnection connection, Dictionary<string, object> parameters);

        public DatabaseConnection(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
    }
}
