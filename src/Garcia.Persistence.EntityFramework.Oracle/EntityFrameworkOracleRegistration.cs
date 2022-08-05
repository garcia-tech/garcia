using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Garcia.Infrastructure.Identity;

namespace Garcia.Persistence.EntityFramework.Oracle
{
    public static class EntityFrameworkOracleRegistration
    {
        public static IServiceCollection AddEfCoreOracle<TContext, TOptions>(this IServiceCollection services, TOptions settings)
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            services.AddLoggedInUserService<long>();
            services.AddDbContext<TContext>(options => options.UseOracle(settings.ConnectionString,
                x => x.MigrationsAssembly(settings.MigrationsAssembly)));
            return services;
        }
    }
}