using Garcia.Application.Contracts.Identity;
using Garcia.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Identity
{
    public static class LoggedInServiceRegistration
    {
        public static IServiceCollection AddLoggedInUserService<TKey>(this IServiceCollection services)
            where TKey : IEquatable<TKey>
        {
            services.AddHttpContextAccessor();
            services.AddTransient<ILoggedInUserService<TKey>, LoggedInUserService<TKey>>();
            return services;
        }
    }
}
