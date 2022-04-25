namespace Garcia.Infrastructure
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        public string AuthMechanism { get; set; }
        public ReplicaSet ReplicaSet { get; set; }
        public bool UseReplicaSet { get; set; }
        bool SaveDBLogs { get; set; }
        DatabaseType DatabaseType { get; set; }
    }
}