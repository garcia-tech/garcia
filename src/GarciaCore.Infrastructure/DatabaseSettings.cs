namespace GarciaCore.Infrastructure;

public class DatabaseSettings : IDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AuthMechanism { get; set; }
    public ReplicaSet ReplicaSet { get; set; }
    public bool UseReplicaSet { get; set; }
    public bool SaveDBLogs { get; set; }
    public DatabaseType DatabaseType { get; set; }
}