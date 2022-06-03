using System;
using Garcia.Application.Redis.Contracts.Infrastructure;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.Redis
{
    public static class RedisServiceRegistiration
    {
        public static IServiceCollection AddRedisConnection<T>(this IServiceCollection services, IOptions<T> options) where T : RedisSettings
        {
            services.Configure<T>(o =>
            {
                o.CacheExpirationInMinutes = options.Value.CacheExpirationInMinutes;
                o.Host = options.Value.Host;
                o.Port = options.Value.Port;
                o.Password = options.Value.Password;
            });

            services.AddSingleton<RedisConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRedisConnection<T>(this IServiceCollection services, T settings) where T : RedisSettings
        {
            services.Configure<T>(o =>
            {
                o.CacheExpirationInMinutes = settings.CacheExpirationInMinutes;
                o.Host = settings.Host;
                o.Port = settings.Port;
                o.Password = settings.Password;
            });

            services.AddSingleton<RedisConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisSettings>(o =>
            {
                o.CacheExpirationInMinutes = Convert.ToInt32(configuration.GetSection(o.GetNodeValue() + ":" + o.GetCacheExpirationInMinutesKeyValue()).Value);
                o.Host = configuration.GetSection(o.GetNodeValue() + ":" + o.GetHostKeyValue()).Value;
                o.Port = configuration.GetSection(o.GetNodeValue() + ":" + o.GetPortKeyValue()).Value;
                o.Password = configuration.GetSection(o.GetNodeValue() + ":" + o.GetPasswordKeyValue()).Value;
            });

            services.AddSingleton<RedisConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services)
        {
            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
    }
}
