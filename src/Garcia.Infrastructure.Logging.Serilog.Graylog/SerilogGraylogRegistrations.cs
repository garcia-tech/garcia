﻿using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Filters;
using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;
using Garcia.Infrastructure.Logging.Serilog.Configurations;

namespace Garcia.Infrastructure.Logging.Serilog.Graylog
{
    public static class SerilogGraylogRegistrations
    {
        public static ILoggingBuilder AddGarciaSerilogGraylog(this ILoggingBuilder logging, GraylogSinkOptions options, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Graylog(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }

            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }

        public static ILoggingBuilder AddGarciaSerilogGraylog(this ILoggingBuilder logging, string hostNameOrAddress, int port, string? username = null, string? password = null, LogEventLevel minimumLevel = LogEventLevel.Debug, int bufferSize = 1000, bool blockWhenFull = false, bool logConsole = false, params CustomProperty[] customProperties)
        {
            var options = new GraylogSinkOptions
            {
                HostnameOrAddress = hostNameOrAddress,
                Port = port,
                TransportType = TransportType.Http

            };

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                options.PasswordInHttp = password;
                options.UsernameInHttp = username;
            }

            var loggerConfigurations = new LoggerConfiguration()
                .AddCustomProperties(customProperties)
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.FromSource("System"))
                .WriteTo.Async(c => c.Graylog(options), bufferSize, blockWhenFull);

            if (logConsole)
            {
                loggerConfigurations.WriteTo.Async(c => c.Console(), bufferSize, blockWhenFull);
            }
            var logger = loggerConfigurations.CreateLogger();
            return logging.ClearProviders().AddSerilog(logger);
        }
    }
}