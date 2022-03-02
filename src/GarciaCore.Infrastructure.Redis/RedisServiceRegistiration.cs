using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            });

            services.AddSingleton<RedisConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services) => services.AddSingleton<IRedisService, RedisService>();
    }
}
