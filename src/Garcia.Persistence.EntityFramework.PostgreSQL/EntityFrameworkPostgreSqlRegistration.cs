using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.EntityFramework.PostgreSql
{
    public static class EntityFrameworkPostgreSqlRegistration
    {
        public static IServiceCollection AddEfCorePostgreSql<TContext, TOptions>(this IServiceCollection services, TOptions settings)
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            return services.AddDbContext<TContext>(options => options.UseNpgsql(settings.ConnectionString,
                x => x.MigrationsAssembly(settings.MigrationsAssembly)));
        }
    }
}