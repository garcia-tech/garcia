using Garcia.Application.ElasticSearch.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.ElasticSearch
{
    public static class ElasticSearchServiceRegistration
    {
        public static IServiceCollection AddElasticSearchConnection<T>(this IServiceCollection services, Action<T> options)
            where T : ElasticSearchSettings
        {
            services.Configure(options);
            services.AddSingleton<ElasticSearchConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddElasticSearchServices<T>(this IServiceCollection services)
        {
            return services.AddSingleton(typeof(IElasticSearchService<,>), typeof(ElasticSearchService<,>));
        }
    }
}
