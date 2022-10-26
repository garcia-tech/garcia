using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Eureka;

namespace Garcia.Infrastructure.Ocelot
{
    public static class ServiceRegistrations
    {
        public static IOcelotBuilder AddGarciaOcelot(this IServiceCollection services, IConfiguration configuration, bool enableCaching = false, Action<ConfigurationBuilderCachePart>? cacheManagerSettings = null)
        {
            services.AddSwaggerForOcelot(configuration);

            if (!enableCaching)
            {
                return services.AddOcelot(configuration);
            }

            Action<ConfigurationBuilderCachePart> settings = cacheManagerSettings ??
                new Action<ConfigurationBuilderCachePart>(x =>
                {
                    x.WithDictionaryHandle();
                });

            return services.AddOcelot(configuration).AddCacheManager(settings);
        }

        public static IOcelotBuilder AddGarciaOcelotWithConsul(this IServiceCollection services, IConfiguration configuration, bool enableCaching = false, Action<ConfigurationBuilderCachePart>? cacheManagerSettings = null)
        {
            services.AddSwaggerForOcelot(configuration);

            if (!enableCaching)
            {
                return services.AddOcelot(configuration);
            }

            Action<ConfigurationBuilderCachePart> settings = cacheManagerSettings ??
                new Action<ConfigurationBuilderCachePart>(x =>
                {
                    x.WithDictionaryHandle();
                });

            return services
                    .AddOcelot(configuration)
                    .AddCacheManager(settings)
                    .AddConsul();
        }

        public static IOcelotBuilder AddGarciaOcelotWithEureka(this IServiceCollection services, IConfiguration configuration, bool enableCaching = false, Action<ConfigurationBuilderCachePart>? cacheManagerSettings = null)
        {
            services.AddSwaggerForOcelot(configuration);

            if (!enableCaching)
            {
                return services.AddOcelot(configuration);
            }

            Action<ConfigurationBuilderCachePart> settings = cacheManagerSettings ??
                new Action<ConfigurationBuilderCachePart>(x =>
                {
                    x.WithDictionaryHandle();
                });

            return services
                    .AddOcelot(configuration)
                    .AddCacheManager(settings)
                    .AddEureka();
        }

        public static IOcelotBuilder AddGarciaOcelot<TCacheManager>(this IServiceCollection services, IConfiguration configuration)
            where TCacheManager : class, IOcelotCache<CachedResponse>
        {
            services.AddSwaggerForOcelot(configuration);
            services.AddSingleton<IOcelotCache<CachedResponse>, TCacheManager>();

            return services
                .AddOcelot(configuration)
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });
        }

        public static IOcelotBuilder AddGarciaOcelot<TCacheManager>(this IServiceCollection services, IConfiguration configuration, Action<ConfigurationBuilderCachePart> cacheManagerSettings)
            where TCacheManager : class, IOcelotCache<CachedResponse>
        {
            services.AddSwaggerForOcelot(configuration);
            services.AddSingleton<IOcelotCache<CachedResponse>, TCacheManager>();
            return services.AddOcelot(configuration).AddCacheManager(cacheManagerSettings);
        }
    }
}