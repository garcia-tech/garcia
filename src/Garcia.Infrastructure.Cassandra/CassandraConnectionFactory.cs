using Cassandra;
using Garcia.Exceptions.Cassandra;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Cassandra
{
    public class CassandraConnectionFactory
    {
        private readonly CassandraSettings _settings;

        public CassandraConnectionFactory(IOptions<CassandraSettings> options)
        {
            _settings = options.Value;
        }

        public Cluster GetCluster()
        {
            var builder = Cluster.Builder();

            if (_settings.ContactPoints?.Length < 1)
                throw new ContactPointsEmptyException();

            if(!string.IsNullOrEmpty(_settings.ConncetionString))
            {
                return builder.WithConnectionString(_settings.ConncetionString)
                    .Build();
            }

            if(!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
            {
                return builder.AddContactPoints(_settings.ContactPoints)
                    .WithCredentials(_settings.Username, _settings.Password)
                    .Build();
            }

            return builder.AddContactPoints(_settings.ContactPoints).Build();
        }

        public ISession GetSession()
        {
            var cluster = GetCluster();
            return string.IsNullOrEmpty(_settings.Keyspace) ? cluster.Connect()
                : cluster.Connect(_settings.Keyspace);
        }
    }
}
