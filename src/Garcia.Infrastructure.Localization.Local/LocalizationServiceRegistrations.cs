using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Localization.Local
{
    public static class LocalizationServiceRegistrations
    {
        public static IServiceCollection AddLocalization<T>(this IServiceCollection services)
            where T : class, ILocalizationItemService
        {
            services.AddScoped<ILocalizationItemService, T>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            return services;
        }

        public static IServiceCollection AddLocalization(this IServiceCollection services)
        {
            services.AddScoped<ILocalizationItemService, LocalizationItemService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            return services;
        }

        public static IServiceCollection AddLocalization<TService, TItemServive>(this IServiceCollection services)
            where TItemServive : class, ILocalizationItemService
            where TService : class, ILocalizationService
        {
            services.AddScoped<ILocalizationItemService, TItemServive>();
            services.AddScoped<ILocalizationService, TService>();
            return services;
        }
    }
}
