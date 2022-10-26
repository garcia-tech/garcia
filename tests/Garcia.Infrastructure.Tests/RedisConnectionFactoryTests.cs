using Garcia.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Infrastructure.Tests
{
    public class RedisConnectionFactoryTests
    {
        private static readonly Mock<IOptions<RedisSettings>> _mockOptions = new();
        private static RedisConnectionFactory _redisConnectionFactory;

        [Fact]
        public void ConnectionFactory_Builds_Success()
        {
            var settings = new RedisSettings
            {
                CacheExpirationInMinutes = 1,
                Host = "127.0.0.1",
                Port = "6379",
                ServiceLockKey = "lock",
                Password = "testredis"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _redisConnectionFactory = new RedisConnectionFactory(_mockOptions.Object);

            var connection = _redisConnectionFactory.GetConnection();
            connection.ShouldNotBeNull();
            connection.ClientName.ShouldNotBeNullOrEmpty();

            var database = connection.GetDatabase();
            database.ShouldNotBeNull();

            var server = _redisConnectionFactory.GetServer();
            server.ShouldNotBeNull();
        }
        [Fact]
        public void Connection_Should_Not_Success_Without_Password()
        {
            var settings = new RedisSettings
            {
                CacheExpirationInMinutes = 1,
                Host = "127.0.0.1",
                Port = "6379",
                ServiceLockKey = "lock"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            Assert.Throws<StackExchange.Redis.RedisConnectionException>(
                () => _redisConnectionFactory = new RedisConnectionFactory(_mockOptions.Object));
        }
    }
}
