using Garcia.Infrastructure.ElasticSearch;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Infrastructure.Tests
{
    public class ElasticConnectionFactoryTests
    {
        private readonly Mock<IOptions<ElasticSearchSettings>> _mockOptions = new();
        private ElasticSearchConnectionFactory _connectionFactory;

        [Fact]
        public void ConnectionFactory_Builds_Success()
        {
            var settings = new ElasticSearchSettings
            {
                Uri = "http://localhost:9200/"
            };
            _mockOptions.Setup(s => s.Value).Returns(settings);
            _connectionFactory = new ElasticSearchConnectionFactory(_mockOptions.Object);

            var client = _connectionFactory.GetClient<TestDocument>();
            client.ShouldNotBeNull();
        }
    }
}
