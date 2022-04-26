using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Garcia.Infrastructure.Ocelot
{
    public static class OcelotConfigurationBuilder
    {
        public static ConfigurationManager ConfigureOcelot(this WebApplicationBuilder builder, string configurationFileName)
        {
            builder.Configuration
                   .AddJsonFile("apsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{builder.Environment}.json", optional: true, reloadOnChange: true)
                   .AddJsonFile(configurationFileName)
                   .AddJsonFile($"{configurationFileName}.{builder.Environment}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();

            return builder.Configuration;
        }
    }
}
