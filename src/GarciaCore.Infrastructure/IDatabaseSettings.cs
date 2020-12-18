using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
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

    public class ReplicaSet
    {
        public string Name { get; set; }
        public List<Endpoint> EndPoints { get; set; } = new List<Endpoint>();
    }

    public class Endpoint
    {
        public string Name { get; set; }
        public int Port { get; set; }
    }
}
