using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GarciaCore.Persistence.EntityFramework.SqlServer
{
    public static class EntityFrameworkSqlServerRegistration
    {
        public static IServiceCollection AddEfCoreSqlServer<TContext,TOptions>(this IServiceCollection services, IOptions<TOptions> settings) 
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            return services.AddDbContext<TContext>(options => options.UseSqlServer(settings.Value.ConnectionString,
                x => x.MigrationsAssembly(settings.Value.MigrationsAssembly)));
        }
    }
}