using System;
using System.Threading.Tasks;
using Garcia.Domain;
using StackExchange.Redis;
using Newtonsoft.Json;
using Garcia.Application.Redis.Contracts.Infrastructure;

namespace Garcia.Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly RedisConnectionFactory _connectionFactory;

        public RedisService(RedisConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.GetConnection();
        }

        public async Task PublishAsync(string channel, string message)
        {
            await _connection.GetSubscriber().PublishAsync(channel, message);
        }

        public async Task PublishAsync<T>(T message) where T : IMessage
        {
            var channel = typeof(T).Name;
            var msg = JsonConvert.SerializeObject(message);
            await _connection.GetSubscriber().PublishAsync(channel, msg);
        }

        public async Task SubscribeAsync(string channel, Action<string> action)
        {
            await _connection.GetSubscriber().SubscribeAsync(channel, (ch, message) =>
                action(message)
            );
        }

        public async Task SubscribeAsync<T>(Func<T, Task> receiveHandler, Func<Exception, string, Task> rejectHandler) where T : IMessage
        {
            var channel = typeof(T).Name;

            await _connection.GetSubscriber().SubscribeAsync(channel, async (ch, message) =>
            {
                try
                {
                    var model = JsonConvert.DeserializeObject<T>(message);
                    await receiveHandler(model);
                }
                catch (Exception exception)
                {
                    await rejectHandler(exception, message);
                }

            });
        }

        public async Task ExecuteWithLockAsync(Action action, int expiryInMilliSeconds)
        {
            string key = action.Method.IsSpecialName ? action.Method.Name : _connectionFactory.GetServiceLockKey();
            TimeSpan expiration = new TimeSpan(0, 0, 0, 0, expiryInMilliSeconds);
            bool locked = false;

            while (!locked)
            {
                locked = await GetLockAsync(key, expiration);
                if (locked)
                {
                    await Task.Run(action);
                    await ReleaseLockAsync(key);
                }
            }

        }

        public async Task<bool> AcquireLockAsync(string key, int expiryInMilliSeconds) => await GetLockAsync(key, new TimeSpan(0, 0, 0, 0, expiryInMilliSeconds));

        public void Dispose()
        {
            _connectionFactory.DisposeConnection();
        }

        private async Task<bool> GetLockAsync(string key, TimeSpan expiration)
        {
            return await _connection.GetDatabase()
                .StringSetAsync(key, Guid.NewGuid().ToString(), expiration, when: When.NotExists);
        }

        private async Task ReleaseLockAsync(string key)
        {
            await _connection.GetDatabase().KeyDeleteAsync(key);
        }
    }
}
