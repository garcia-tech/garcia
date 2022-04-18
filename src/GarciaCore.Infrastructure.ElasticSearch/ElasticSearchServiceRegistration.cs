using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.ElasticSearch.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Infrastructure.ElasticSearch
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
