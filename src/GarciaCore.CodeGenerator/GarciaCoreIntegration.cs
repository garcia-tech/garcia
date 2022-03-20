using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class GarciaCoreIntegration
    {
        public Dictionary<string, List<string>> Integrations { get; set; } = new();

        public GarciaCoreIntegration()
        {
            Integrations.Add("MongoDb", new List<string> { "GarciaCore.Application.MongoDb", "GarciaCore.Domain.MongoDb" });
            Integrations.Add("Redis", new List<string> { "GarciaCore.Application.Redis" });
            Integrations.Add("MongoDb", new List<string> { });
            Integrations.Add("MongoDb", new List<string> { });
        }
    }
}