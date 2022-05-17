using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Consul
{
    public static class ConsulServiceRegistrations
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, Action<ConsulClientConfiguration> configOverride)
        {
            return services.AddSingleton<IConsulClient, ConsulClient>(x =>
                new ConsulClient(configOverride));
        }

        public static IServiceCollection AddConsul(this IServiceCollection services, string uri)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(x =>
                new ConsulClient(x =>
                {
                    x.Address = new Uri(uri);
                }));

            return services;
        }

        public static async Task<IApplicationBuilder> UseConsul(this IApplicationBuilder app, AgentServiceRegistration registration)
        {
            var client = app.ApplicationServices.GetRequiredService<IConsulClient>();
            await client.Agent.ServiceDeregister(registration.ID);
            await client.Agent.ServiceRegister(registration);
            return app;
        }
    }
}
