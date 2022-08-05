using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Garcia.Application.MongoDb.Contracts.Persistence;
using Garcia.Infrastructure.MongoDb;
using Garcia.Infrastructure.Identity;

namespace Garcia.Persistence.MongoDb
{
    public static class MongoDbServiceRegistration
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(options =>
            {
                string node = options.GetNodeValue();
                options.ConnectionString = configuration.GetSection(node + ":" + options.GetConnectionStringKeyValue()).Value;
                options.DatabaseName = configuration.GetSection(node + ":" + options.GetDatabaseNameKeyValue()).Value;
            });

            services.AddScoped(typeof(IAsyncMongoDbRepository<>), typeof(MongoDbRepository<>));
            services.AddLoggedInUserService<string>();
            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services, Action<MongoDbSettings> options)
        {
            services.Configure(options);
            services.AddScoped(typeof(IAsyncMongoDbRepository<>), typeof(MongoDbRepository<>));
            services.AddLoggedInUserService<string>();
            return services;
        }
    }
}