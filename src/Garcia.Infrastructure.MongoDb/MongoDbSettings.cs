﻿namespace Garcia.Infrastructure.MongoDb
{
    public class MongoDbSettings : DatabaseSettings
    {
        public virtual string GetNodeValue() => nameof(MongoDbSettings);
        public virtual string GetConnectionStringKeyValue() => nameof(ConnectionString);
        public virtual string GetDatabaseNameKeyValue() => nameof(DatabaseName);
    }
}
