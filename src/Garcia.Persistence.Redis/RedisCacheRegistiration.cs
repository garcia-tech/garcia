using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Application.Redis.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.Redis
{
    public static class RedisCacheRegistiration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services) => services.AddSingleton<IRedisCache, RedisCache>();
        public static IServiceCollection AddRedisRepositoryCache(this IServiceCollection services) => services.AddSingleton<IGarciaCache, RedisCache>();

    }
}
