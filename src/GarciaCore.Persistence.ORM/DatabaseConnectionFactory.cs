using GarciaCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace GarciaCore.Persistence.ORM
{

    public class DatabaseConnectionFactory
    {
        private IOptions<DatabaseSettings> _databaseSettings;

        public DatabaseConnectionFactory(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public DatabaseConnection GetConnection()
        {
            switch (_databaseSettings.Value.DatabaseType)
            {
                case DatabaseType.Mssql:
                    return new MssqlDatabaseConnection(_databaseSettings);
                case DatabaseType.Postgresql:
                    return new PostgresqlDatabaseConnection(_databaseSettings);
                default:
                    throw new InfrastructureException("Database not implemented");
            }
        }

        //public static void SetConnectionString(string connectionString, bool isEncrypted)
        //{
        //    if (isEncrypted)
        //    {
        //        string[] splittedConnectionString = connectionString.Split("password=");

        //        if (splittedConnectionString.Length > 1)
        //        {
        //            string password = splittedConnectionString[1].Split(";")[0];
        //            ConnectionString = connectionString.Replace(password, Encryption.Decrypt(password));
        //        }
        //    }
        //    else
        //    {
        //        ConnectionString = connectionString;
        //    }
        //}
    }
}
