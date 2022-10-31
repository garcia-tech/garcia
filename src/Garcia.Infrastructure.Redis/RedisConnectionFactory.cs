using System;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Garcia.Infrastructure.Redis
{
    public class RedisConnectionFactory
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly int _cacheExpirationInMinutes;
        private readonly IServer _server;
        private readonly string _serviceLockKey;

        public RedisConnectionFactory(IOptions<RedisSettings> options)
        {
            _multiplexer = CreateLazyConnection(options.Value).Value;
            _cacheExpirationInMinutes = options.Value.CacheExpirationInMinutes;
            _server = _multiplexer.GetServer($"{options.Value.Host}:{options.Value.Port}");
            _serviceLockKey = options.Value.ServiceLockKey;
        }

        public RedisConnectionFactory(RedisSettings settings)
        {
            _multiplexer = CreateLazyConnection(settings).Value;
            _cacheExpirationInMinutes = settings.CacheExpirationInMinutes;
            _server = _multiplexer.GetServer($"{settings.Host}:{settings.Port}");
            _serviceLockKey = settings.ServiceLockKey;
        }

        private ConnectionMultiplexer CreateConnection(RedisSettings settings)
        {
            return ConnectionMultiplexer.Connect($"{settings.Host}:{settings.Port}, password={settings.Password}");
        }

        private Lazy<ConnectionMultiplexer> CreateLazyConnection(RedisSettings settings)
        {
            return new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect($"{settings.Host}:{settings.Port}, password={settings.Password}")
            );
        }

        internal string GetServiceLockKey() => _serviceLockKey;
        /// <summary>
        /// Gets a connection object that you can use without any configuration.
        /// </summary>
        /// <returns><see cref="IConnectionMultiplexer"/></returns>
        public IConnectionMultiplexer GetConnection() => _multiplexer;
        /// <summary>
        /// Gets a server object that you can use without any configuration.
        /// </summary>
        /// <returns><see cref="IServer"/></returns>
        public IServer GetServer() => _server;
        /// <summary>
        /// Gets a cache expiration duration.
        /// </summary>
        /// <returns>Value set in configuration or default duration</returns>
        public int GetCacheExpirationDuration() => _cacheExpirationInMinutes;
        public void DisposeConnection()
        {
            if (_multiplexer.IsConnected)
                _multiplexer.Dispose();
        }
    }
}
