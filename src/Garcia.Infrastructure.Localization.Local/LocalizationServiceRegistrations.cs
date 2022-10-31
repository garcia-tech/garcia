using Garcia.Application.Contracts.Localization;
using Garcia.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Localization.Local
{
    public static class LocalizationServiceRegistrations
    {
        public static IServiceCollection AddGarciaLocalization<T, TKey, TRepository>(this IServiceCollection services)
            where T : LocalizationItem<TKey>, new()
            where TKey: struct, IEquatable<TKey>
            where TRepository: class, IAsyncRepository<T, TKey>
        {
            services.AddScoped<ILocalizationItemService<T>, LocalizationItemService<T, TKey>>();
            services.AddScoped<ILocalizationService<T>, LocalizationService<T>>();
            services.AddScoped<IAsyncRepository<T, TKey>, TRepository>();
            return services;
        }
    }
}
