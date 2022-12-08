using System.Text.Json.Serialization;

namespace Garcia.Infrastructure.Logging.Serilog
{
    public class Serilog
    {
        public string[] Using { get; set; }
        public string MinimumLevel { get; set; }
        public List<Writeto> WriteTo { get; set; }
        public Properties Properties { get; set; }
        public List<Filter> Filter { get; set; }
    }

    public class Properties
    {
        public string Application { get; set; }
    }

    public class Writeto
    {
        public string Name { get; set; }
        public WriteToArgs Args { get; set; }
    }

    public class WriteToArgs
    {
        [JsonPropertyName("hostnameOrAddress")]
        public string HostnameOrAddress { get; set; }
        [JsonPropertyName("port")]
        public string Port { get; set; }
        [JsonPropertyName("transportType")]
        public string TransportType { get; set; }
    }

    public class Filter
    {
        public string Name { get; set; }
        public FilterArgs Args { get; set; }
    }

    public class FilterArgs
    {
        [JsonPropertyName("expression")]
        public string Expression { get; set; }
    }
}
