using Garcia.Application.Cassandra.Contracts.Persistence;
using Garcia.Infrastructure.Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Persistence.Cassandra
{
    public static class CassandraServiceRegistrations
    {
        public static IServiceCollection AddCassandra(this IServiceCollection services, Action<CassandraSettings> options)
        {
            services.Configure(options);
            services.AddScoped<CassandraConnectionFactory>();
            services.AddScoped(typeof(IAsyncCassandraRepository<>), typeof(CassandraRepository<>));
            return services;
        }

        public static IServiceCollection AddCassandra(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CassandraSettings>(options =>
            {
                options.ConncetionString = configuration.GetSection(nameof(CassandraSettings))
                    .GetValue<string>(nameof(CassandraSettings.ConncetionString));
                options.ContactPoints = configuration.GetSection(nameof(CassandraSettings))
                    .GetValue<string[]>(nameof(CassandraSettings.ContactPoints));
                options.Keyspace = configuration.GetSection(nameof(CassandraSettings))
                    .GetValue<string>(nameof(CassandraSettings.Keyspace));
                options.Username = configuration.GetSection(nameof(CassandraSettings))
                    .GetValue<string>(nameof(CassandraSettings.Username));
                options.Password = configuration.GetSection(nameof(CassandraSettings))
                    .GetValue<string>(nameof(CassandraSettings.Password));
            });

            services.AddScoped<CassandraConnectionFactory>();
            services.AddScoped(typeof(IAsyncCassandraRepository<>), typeof(CassandraRepository<>));
            return services;
        }
    }
}
