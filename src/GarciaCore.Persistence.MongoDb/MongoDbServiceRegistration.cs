using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GarciaCore.Infrastructure.MongoDb;
using GarciaCore.Domain.MongoDb;

namespace GarciaCore.Persistence.MongoDb
{
    public static class MongoDbServiceRegistration
    {
        public static IServiceCollection AddMongoDbSettings(this IServiceCollection services,
           IConfiguration configuration)
        {
            return services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = configuration
                    .GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.ConnectionStringKeyValue).Value;
                options.DatabaseName = configuration
                    .GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.DatabaseNameKeyValue).Value;
            });
        }
        public static IServiceCollection AddMongoDbRepository<T>(this IServiceCollection services) where T : MongoDbEntity
        {
            services.AddScoped<IAsyncMongoDbRepository<T>, MongoDbRepository<T>>();
            return services;
        }
    }
}