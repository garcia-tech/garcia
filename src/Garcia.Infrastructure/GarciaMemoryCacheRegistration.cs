using System;
using Garcia.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure
{
    public static class GarciaMemoryCacheRegistration
    {
        public static IServiceCollection AddGarciaMemoryCache(this IServiceCollection services)
        {
            services.Configure<CacheSettings>(options =>
            {
                options.CacheExpirationInMinutes = 1;
            });

            services.AddMemoryCache();
            services.AddScoped<IGarciaCache, GarciaMemoryCache>();
            return services;
        }

        public static IServiceCollection AddGarciaMemoryCache(this IServiceCollection services, IConfiguration configurations)
        {
            services.Configure<CacheSettings>(options =>
            {
                options.CacheExpirationInMinutes = Convert.ToInt32(configurations[$"{nameof(CacheSettings)}:{nameof(options.CacheExpirationInMinutes)}"]);
            });

            services.AddMemoryCache();
            services.AddScoped<IGarciaCache, GarciaMemoryCache>();
            return services;
        }

        public static IServiceCollection AddGarciaMemoryCache(this IServiceCollection services, Action<CacheSettings> options)
        {
            services.Configure(options);
            services.AddMemoryCache();
            services.AddScoped<IGarciaCache, GarciaMemoryCache>();
            return services;
        }
    }
}
