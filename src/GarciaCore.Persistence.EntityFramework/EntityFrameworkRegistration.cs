using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GarciaCore.Application.Contracts.Persistence;

namespace GarciaCore.Persistence.EntityFramework
{
    public static class EntityFrameworkRegistration
    {
        public static IServiceCollection AddEfCore<TContext>(this IServiceCollection service, Action<DbContextOptionsBuilder> options) where TContext : BaseContext
        {
            return service.AddDbContext<TContext>(options);
        }

        public static IServiceCollection AddEfCoreInMemory<TContext>(this IServiceCollection service, string databaseName) where TContext : BaseContext
        {
            return service.AddDbContext<TContext>(options => options.UseInMemoryDatabase(databaseName));
        }

        public static IServiceCollection AddEfCoreRepository(this IServiceCollection service)
        {
            return service.AddScoped(typeof(IAsyncRepository<>), typeof(EntityFrameworkRepository<>));
        }
    }
}
