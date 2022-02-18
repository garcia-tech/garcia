using System;
using GarciaCore.Infrastructure;
namespace GarciaCore.Infrastructure.MongoDb
{
    public class MongoDbSettings : DatabaseSettings
    {
        public const string ConnectionStringKeyValue = nameof(ConnectionString);
        public const string DatabaseNameKeyValue = nameof(DatabaseName);
    }
}
