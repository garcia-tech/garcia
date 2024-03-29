﻿using System.Collections.Generic;
using System.Data;
using Garcia.Infrastructure;
using Microsoft.Extensions.Options;

namespace Garcia.Persistence.ORM
{
    public abstract class DatabaseConnection
    {
        protected DatabaseSettings _databaseSettings;
        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand(string sql, IDbConnection connection);
        public abstract IDbCommand CreateCommand(string sql, IDbConnection connection, Dictionary<string, object> parameters);

        public abstract string IdentitySqlStatement { get; }
        public abstract string RowCountSqlStatement { get; }
        public abstract string BeginTranSqlStatement { get; }
        public abstract string CommitTranSqlStatement { get; }
        public abstract string GetDateSqlStatement { get; }
        public abstract string ColumnPrefix { get; }
        public abstract string ColumnPpostfix { get; }
        public abstract string TablePrefix { get; }
        public abstract string IdKeyword { get; }

        public DatabaseConnection(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
    }
}