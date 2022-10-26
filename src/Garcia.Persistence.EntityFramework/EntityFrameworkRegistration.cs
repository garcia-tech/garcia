using System;
using Garcia.Application.Contracts.Persistence;
using Garcia.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.EntityFramework
{
    public static class EntityFrameworkRegistration
    {
        public static IServiceCollection AddEfCore<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TContext : BaseContext
        {
            services.AddLoggedInUserService<long>();
            services.AddDbContext<TContext>(options);
            return services;
        }

        public static IServiceCollection AddEfCoreInMemory<TContext>(this IServiceCollection services, string databaseName) where TContext : BaseContext
        {
            services.AddLoggedInUserService<long>();
            services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(databaseName));
            return services;
        }

        public static IServiceCollection AddEfCoreRepository(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IAsyncRepository<>), typeof(EntityFrameworkRepository<>));
        }
    }
}
