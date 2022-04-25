using Garcia.Infrastructure.Logging.Serilog.Configurations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Garcia.Infrastructure.Logging.Serilog
{
    public static class SerilogServiceRegistrations
    {
        public static ILoggingBuilder AddGarciaSerilog(this ILoggingBuilder logging, LoggerConfiguration loggerConfigurations)
        {
            var logger = loggerConfigurations.CreateLogger();
            return logging.AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogConsole(this ILoggingBuilder logging, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, params CustomProperty[] customProperties)
        {
            var logger = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull)
                .CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogFile(this ILoggingBuilder logging, FileLoggerConfiguration configurations, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var logConfiguration = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .WriteTo.Async(c =>
                    c.File(
                        path: configurations.Path,
                        restrictedToMinimumLevel: minimumLevel,
                        outputTemplate: configurations.OutputTemplate,
                        fileSizeLimitBytes: configurations.FileSizeLimitBytes,
                        retainedFileCountLimit: configurations.RetainedFileCountLimit,
                        rollingInterval: configurations.Interval
                        ), bufferSize, blockWhenFull);

            if (logConsole)
            {
                logConfiguration.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }

            var logger = logConfiguration.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogFile(this ILoggingBuilder logging, string filePath, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var logConfiguration = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File(filePath), bufferSize, blockWhenFull);

            if (logConsole)
            {
                logConfiguration.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }

            var logger = logConfiguration.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }
    }
}
