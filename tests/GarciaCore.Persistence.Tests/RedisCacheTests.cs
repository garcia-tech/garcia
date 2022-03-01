using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using GarciaCore.Infrastructure.Redis;
using GarciaCore.Persistence.Redis;
using Xunit;
using Shouldly;

namespace GarciaCore.Persistence.Tests
{
    public class RedisCacheTests
    {
        private static Mock<IOptions<RedisSettings>> _mockOptions = new();
        private static RedisConnectionFactory _redisConnectionFactory;
        private static RedisCache _redisCache;

        public RedisCacheTests()
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
            _redisCache = new RedisCache(_redisConnectionFactory);
        }

        [Fact]
        public async Task ExistsAsync()
        {
            var result = await _redisCache.ExistsAsync("hello");
            result.ShouldBe(true);
        }
        [Fact]
        public async Task GetAsync()
        {
            var result = await _redisCache.GetAsync<string>("hello");
            result.ShouldNotBe(null);
            result.ShouldBe("world");
        }

        [Fact]
        public async Task SetAsync()
        {
            var key = "test";
            var addedData = await _redisCache.SetAsync(key, new TestEntity(1,"testData"));
            var result = await _redisCache.GetAsync<TestEntity>(key);
            result.Name.ShouldBe(addedData.Name);
        }
        [Fact]
        public async Task GetMatchingKeysAsync()
        {
            await _redisCache.SetAsync("test1", 1);
            await _redisCache.SetAsync("test2", 1);
            var result = await _redisCache.GetMatchingKeysAsync("test");
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThanOrEqualTo(2);
        }
        [Fact]
        public async Task RemoveMatchingKeysAsync()
        {
            await _redisCache.RemoveMatchingKeysAsync("test");
            var result = await _redisCache.GetMatchingKeysAsync("test");
            result?.Count().ShouldBe(0);
        }
        [Fact]
        public async Task RemoveAsync()
        {
            await _redisCache.SetAsync("test", 1);
            await _redisCache.RemoveAsync("test");
            var result = await _redisCache.ExistsAsync("test");
            result.ShouldBeFalse();
        }
        [Fact]
        public async Task HashSetAsync()
        {
            string key = "hashSetTest";
            var model = new TestEntity(2, "Test");
            await _redisCache.HashSetAsync(key, model);
            var result = await _redisCache.ExistsAsync(key);
            result.ShouldBe(true);
        }
        [Theory]
        [InlineData("Test")]
        public async Task HashGetAsync(string exceptedName)
        {
            string result = await _redisCache.HashGetAsync<TestEntity, string>("hashSetTest",
                x => x.Name);

            Assert.Equal(exceptedName, result);
        }
        [Fact]
        public async Task HashGetAllAsync()
        {
            string key = "hashSetTest";
            var result = await _redisCache.HashGetAllAsync<TestEntity>(key);
            result.Name.ShouldNotBe(null);
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            await _redisCache.ExecuteAsync("set", "execute:test", "helloworld");
            var result = await _redisCache.ExistsAsync("execute:test");
            result.ShouldBeTrue();
        }
    }
}
