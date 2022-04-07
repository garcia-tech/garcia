using System;
using GarciaCore.Application.Contracts.Infrastructure;
using GarciaCore.Application.Redis.Contracts.Infrastructure;
using GarciaCore.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GarciaCore.Infrastructure.Redis
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
