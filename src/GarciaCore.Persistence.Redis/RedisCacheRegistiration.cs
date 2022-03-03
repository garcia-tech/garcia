using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.Redis.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Persistence.Redis
{
    public static class RedisCacheRegistiration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services) => services.AddSingleton<IRedisCache, RedisCache>();
    }
}
