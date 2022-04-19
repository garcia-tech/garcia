using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace GarciaCore.Infrastructure.ElasticSearch
{
    public class ElasticSearchConnectionFactory
    {
        private readonly ElasticSearchSettings _settings;

        public ElasticSearchConnectionFactory(IOptions<ElasticSearchSettings> options)
        {
            _settings = options.Value;
        }

        protected virtual ConnectionSettings GetConnection<T>(string index) where T : class
        {
            var pool = new SingleNodeConnectionPool(new Uri(_settings.Uri));
            var connection = new ConnectionSettings(pool)
                .DefaultIndex(index)
                .DefaultMappingFor<T>(m => m.IndexName(index));

            if (_settings.AuthenticationRequired)
                connection.BasicAuthentication(_settings.Username, _settings.Password);

            return connection;
        }

        public ElasticClient GetClient<T>() where T : class 
            => new ElasticClient(GetConnection<T>(typeof(T).Name.ToLower()));
    }
}
