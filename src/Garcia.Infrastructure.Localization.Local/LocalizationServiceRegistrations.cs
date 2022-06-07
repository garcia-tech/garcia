using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application;
using Garcia.Application.Contracts.Localization;
using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Localization.Local
{
    public static class LocalizationServiceRegistrations
    {
        public static IServiceCollection AddGarciaLocalization<T>(this IServiceCollection services)
            where T : class, ILocalizationItemService
        {
            services.AddScoped<ILocalizationItemService, T>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            return services;
        }

        public static IServiceCollection AddGarciaLocalization(this IServiceCollection services)
        {
            services.AddScoped<ILocalizationItemService, LocalizationItemService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            return services;
        }

        public static IServiceCollection AddGarciaLocalization<TService, TItemService>(this IServiceCollection services)
            where TItemService : class, ILocalizationItemService
            where TService : class, ILocalizationService
        {
            services.AddScoped<ILocalizationItemService, TItemService>();
            services.AddScoped<ILocalizationService, TService>();
            return services;
        }

        public static IServiceCollection AddGarciaLocalization<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TContext : BaseContext
        {
            //services.AddDbContext<TContext>(options);
            //services.AddLoggedInUserService();
            services.AddEfCore<TContext>(options);
            services.AddScoped<ILocalizationItemService, LocalizationItemService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizationItemRepository<LocalizationItem>, LocalizationItemRepository>();
            return services;
        }

        public static IServiceCollection AddGarciaLocalization<TContext>(this IServiceCollection services, string databaseName) where TContext : BaseContext
        {
            //services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(databaseName));
            //services.AddLoggedInUserService();
            services.AddEfCoreInMemory<TContext>(databaseName);
            services.AddScoped<ILocalizationItemService, LocalizationItemService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizationItemRepository<LocalizationItem>, LocalizationItemRepository>();
            return services;
        }
    }
}
