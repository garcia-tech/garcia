using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GarciaCore.Persistence.EntityFramework.PostgreSql
{
    public static class EntityFrameworkPostgreSqlRegistration
    {
        public static IServiceCollection AddEfCorePostgreSql<TContext,TOptions>(this IServiceCollection services, IOptions<TOptions> settings) 
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            return services.AddDbContext<TContext>(options => options.UseNpgsql(settings.Value.ConnectionString,
                x => x.MigrationsAssembly(settings.Value.MigrationsAssembly)));
        }
    }
}