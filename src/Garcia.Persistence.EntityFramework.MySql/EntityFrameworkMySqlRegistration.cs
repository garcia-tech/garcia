using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.EntityFramework.MySql
{
    public static class EntityFrameworkMySqlRegistration
    {
        public static IServiceCollection AddEfCoreMySql<TContext, TOptions>(this IServiceCollection services, TOptions settings)
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            return services.AddDbContext<TContext>(options => options.UseMySQL(settings.ConnectionString,
                x => x.MigrationsAssembly(settings.MigrationsAssembly)));
        }
    }
}