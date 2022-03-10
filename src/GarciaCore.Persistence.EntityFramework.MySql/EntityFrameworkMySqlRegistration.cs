using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GarciaCore.Persistence.EntityFramework.MySql
{
    public static class EntityFrameworkMySqlRegistration
    {
        public static IServiceCollection AddEfCoreMySql<TContext, TOptions>(this IServiceCollection services, IOptions<TOptions> settings)
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            return services.AddDbContext<TContext>(options => options.UseMySQL(settings.Value.ConnectionString,
                x => x.MigrationsAssembly(settings.Value.MigrationsAssembly)));
        }
    }
}