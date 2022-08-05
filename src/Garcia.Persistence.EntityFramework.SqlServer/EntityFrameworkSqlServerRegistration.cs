using Garcia.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.EntityFramework.SqlServer
{
    public static class EntityFrameworkSqlServerRegistration
    {
        public static IServiceCollection AddEfCoreSqlServer<TContext, TOptions>(this IServiceCollection services, TOptions settings)
            where TOptions : EfCoreSettings
            where TContext : BaseContext
        {
            services.AddLoggedInUserService<long>();
            services.AddDbContext<TContext>(options => options.UseSqlServer(settings.ConnectionString,
                x => x.MigrationsAssembly(settings.MigrationsAssembly)));
            return services;
        }
    }
}