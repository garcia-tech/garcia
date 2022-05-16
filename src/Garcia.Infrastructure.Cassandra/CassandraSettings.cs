using Garcia.Exceptions.Cassandra;

namespace Garcia.Infrastructure.Cassandra
{
    public class CassandraSettings
    {
        public string[] ContactPoints { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConncetionString { get; set; }
        public string Keyspace { get; set; }
    }
}