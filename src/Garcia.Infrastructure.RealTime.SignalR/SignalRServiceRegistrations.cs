using Garcia.Infrastructure.RealTime.SignalR.Clients;
using Garcia.Infrastructure.RealTime.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.RealTime.SignalR
{
    public static class SignalRServiceRegistrations
    {
        public static IServiceCollection AddGarciaSignalR(this IServiceCollection services, Action<SignalRSettings> options)
        {
            services.Configure(options);

            services.AddSignalR(configuration =>
            {
                configuration.EnableDetailedErrors = false;
                configuration.HandshakeTimeout = new TimeSpan(0, 0, 15);
            })
                .AddJsonProtocol(protocol =>
                {
                    protocol.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            return services;
        }

        public static IServiceCollection AddGarciaSignalR(this IServiceCollection services)
        {
            services.AddSignalR(configuration =>
            {
                configuration.EnableDetailedErrors = false;
                configuration.HandshakeTimeout = new TimeSpan(0, 0, 15);
            })
                .AddJsonProtocol(protocol =>
                {
                    protocol.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            return services;
        }

        public static IApplicationBuilder MapBaseHub<THub>(this WebApplication app, string endpoint)
            where THub : BaseHub
        {
            app.MapHub<THub>(endpoint);
            return app;
        }

        public static IApplicationBuilder MapBaseHub<THub>(this WebApplication app, string endpoint, Action<HttpConnectionDispatcherOptions>? configureOptions)
            where THub : BaseHub
        {
            app.MapHub<THub>(endpoint, configureOptions);
            return app;
        }

        public static IApplicationBuilder MapBaseHub<THub, TClient>(this WebApplication app, string endpoint)
            where THub : BaseHub<TClient>
            where TClient : class, IBaseHubClient
        {
            app.MapHub<THub>(endpoint);
            return app;
        }

        public static IApplicationBuilder MapBaseHub<THub, TClient>(this WebApplication app, string endpoint, Action<HttpConnectionDispatcherOptions>? configureOptions)
            where THub : BaseHub<TClient>
            where TClient : class, IBaseHubClient
        {
            app.MapHub<THub>(endpoint, configureOptions);
            return app;
        }
    }
}
