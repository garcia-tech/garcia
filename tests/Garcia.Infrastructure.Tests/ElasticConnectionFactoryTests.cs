using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Shouldly;
using Garcia.Infrastructure.ElasticSearch;

namespace Garcia.Infrastructure.Tests
{
    public class ElasticConnectionFactoryTests
    {
        private Mock<IOptions<ElasticSearchSettings>> _mockOptions = new();
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
