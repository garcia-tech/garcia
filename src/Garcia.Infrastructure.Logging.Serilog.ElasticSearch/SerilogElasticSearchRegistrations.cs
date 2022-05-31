using Garcia.Infrastructure.ElasticSearch;
using Garcia.Infrastructure.Logging.Serilog.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;

namespace Garcia.Infrastructure.Logging.Serilog.ElasticSearch
{
    public static class SerilogElasticSearchRegistrations
    {
        public static ILoggingBuilder AddGarciaSerilogElastic(this ILoggingBuilder logging, ElasticsearchSinkOptions options, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Elasticsearch(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }

            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogElastic(this ILoggingBuilder logging, string connectionString, string indexFormat, string username = null, string password = null, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var options = new ElasticsearchSinkOptions(new Uri(connectionString))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                IndexFormat = indexFormat,
                FailureCallback = e => Console.WriteLine(e.Exception)
            };

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                options.ModifyConnectionSettings = x => x.BasicAuthentication(username, password);
            }

            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Elasticsearch(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }
            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogElastic(this ILoggingBuilder logging, IConfiguration configuration, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var options = new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearchSettings:Uri"]))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                IndexFormat = configuration["ElasticSearchSettings:IndexFormat"],
                FailureCallback = e => Console.WriteLine(e.Exception)
            };

            if (!string.IsNullOrEmpty(configuration["ElasticSearchSettings:Username"]) && !string.IsNullOrEmpty(configuration["ElasticSearchSettings:Password"]))
            {
                options.ModifyConnectionSettings = x => x.BasicAuthentication(configuration["ElasticSearchSettings:Username"], configuration["ElasticSearchSettings:Password"]);
            }

            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Elasticsearch(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }
            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogElastic(this ILoggingBuilder logging, ElasticSearchSettings settings, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var options = new ElasticsearchSinkOptions(new Uri(settings.Uri))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                IndexFormat = settings.IndexFormat,
                FailureCallback = e => Console.WriteLine(e.Exception)
            };

            if (!string.IsNullOrEmpty(settings.Username) && !string.IsNullOrEmpty(settings.Password))
            {
                options.ModifyConnectionSettings = x => x.BasicAuthentication(settings.Username, settings.Password);
            }

            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Elasticsearch(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }
            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }
    }
}