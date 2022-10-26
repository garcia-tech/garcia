using System;
using System.Threading.Tasks;
using Garcia.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Infrastructure.Tests
{
    public class RedisServiceTests
    {
        private static readonly Mock<IOptions<RedisSettings>> _mockOptions = new();
        private static RedisConnectionFactory _redisConnectionFactory;
        private static RedisService _service;

        public RedisServiceTests()
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
            _service = new RedisService(_redisConnectionFactory);
        }

        [Fact]
        public async Task Publish_SubScribe_Should_Success()
        {
            string channel = "TestPubSubCh";
            string message = "TestPubSubMsg";

            await _service.PublishAsync(channel, message);
            await _service.SubscribeAsync(channel, msg =>
            {
                msg.ShouldBeEquivalentTo(message);
            });
        }
        [Fact]
        public async Task AcquireLockAsync()
        {
            string key = "lock";
            int expiryInMilliSeconds = 1000;
            bool lockObjectFree = await _service.AcquireLockAsync(key, expiryInMilliSeconds);
            lockObjectFree.ShouldBeTrue();
            lockObjectFree = await _service.AcquireLockAsync(key, expiryInMilliSeconds);
            lockObjectFree.ShouldBeFalse();
        }

        [Fact]
        public async Task ExecuteWithLockAsync()
        {
            int expiryInMilliSeconds = 1000;

            await _service.ExecuteWithLockAsync(() =>
            {
                Console.WriteLine("Hello World");
            }, expiryInMilliSeconds);

            bool lockObjectFree = await _service.AcquireLockAsync("lock",
                expiryInMilliSeconds);
            lockObjectFree.ShouldBeTrue();
        }
    }
}
