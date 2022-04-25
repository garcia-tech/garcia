using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _multiplexer = CreateLazyConnection(options).Value;
            _cacheExpirationInMinutes = options.Value.CacheExpirationInMinutes;
            _server = _multiplexer.GetServer($"{options.Value.Host}:{options.Value.Port}");
            _serviceLockKey = options.Value.ServiceLockKey;
        }

        private ConnectionMultiplexer CreateConnection(IOptions<RedisSettings> options)
        {
            return ConnectionMultiplexer.Connect($"{options.Value.Host}:{options.Value.Port}, password={options.Value.Password}");
        }

        private Lazy<ConnectionMultiplexer> CreateLazyConnection(IOptions<RedisSettings> options)
        {
            return new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect($"{options.Value.Host}:{options.Value.Port}, password={options.Value.Password}")
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
