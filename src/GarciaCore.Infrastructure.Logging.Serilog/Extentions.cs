using Serilog;
using GarciaCore.Infrastructure.Logging.Serilog.Configurations;

namespace GarciaCore.Infrastructure.Logging.Serilog
{
    public static class Extentions
    {
        public static LoggerConfiguration AddCustomProperties(this LoggerConfiguration configuration, CustomProperty[] properties)
        {
            if (properties == null || properties.Length == 0) return configuration;

            foreach (var property in properties)
            {
                configuration.Enrich.WithProperty(property.Name, property.Value);
            }

            return configuration;
        }
    }
}
