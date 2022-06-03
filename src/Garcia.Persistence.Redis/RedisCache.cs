using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Garcia.Application.Redis.Contracts.Persistence;
using Garcia.Infrastructure.Redis;
using Garcia.Exceptions.Redis;
using StackExchange.Redis;

namespace Garcia.Persistence.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connection;
        private readonly RedisConnectionFactory _connectionFactory;

        private int CacheExpirationInMinutes { get; }

        public RedisCache(RedisConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.GetConnection();
            _database = _connection.GetDatabase();

            CacheExpirationInMinutes = _connectionFactory.GetCacheExpirationDuration();

        }

        public async Task<T> GetAsync<T>(string key)
        {
            var data = await _database.StringGetAsync(key);

            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(data, typeof(T));

            try
            {
                return data.HasValue ? JsonSerializer.Deserialize<T>(data.ToString()) : default;
            }
            catch (JsonException e)
            {
                throw new KeyValueMatchException(typeof(T).FullName, e);
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task<T> SetAsync<T>(string key, T model, bool persist = false)
        {
            await _database.StringSetAsync(key, JsonSerializer.Serialize<T>(model), persist ? null : TimeSpan.FromMinutes(CacheExpirationInMinutes));
            return model;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var result = await _database.KeyExistsAsync(key);
            return result;
        }

        public async Task<IEnumerable<string>> GetMatchingKeysAsync(string key)
        {
            var server = _connectionFactory.GetServer();
            return await server.KeysAsync(pattern: "*" + key + "*")
                   .Select(x => x.ToString())
                   .ToListAsync();
        }

        public async Task RemoveMatchingKeysAsync(string key)
        {
            var server = _connectionFactory.GetServer();
            var keys = await server.KeysAsync(pattern: "*" + key + "*")
                .ToListAsync();
            await _database.KeyDeleteAsync(keys.ToArray());
        }

        public async Task HashSetAsync<T>(string key, T model)
        {
            var entries = model.GetType()
                .GetProperties()
                .Where(x => x.GetValue(model) != null)
                .Select(x =>
                {
                    return new HashEntry(x.Name, x.GetValue(model).ToString());
                }).ToArray();

            await _database.HashSetAsync(key, entries);
        }
        public async Task<TField> HashGetAsync<T, TField>(string key, Expression<Func<T, TField>> field) where T : class
        {
            var fieldName = ((MemberExpression)field.Body).Member.Name;
            var fieldValue = await _database.HashGetAsync(key, fieldName);

            if (!fieldValue.HasValue) return default;

            try
            {
                if (typeof(TField) == typeof(string))
                {
                    return (TField)Convert.ChangeType(fieldValue, typeof(TField));
                }

                if (typeof(TField) == typeof(DateTimeOffset))
                {
                    return (TField)Convert.ChangeType(DateTimeOffset.Parse(fieldValue.ToString()), typeof(TField));
                }

                return JsonSerializer.Deserialize<TField>(fieldValue);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    _ when e is JsonException
                    || e is InvalidCastException
                    || e is FormatException => new KeyValueMatchException(typeof(T).FullName, e),
                    _ => e
                };
            }
        }

        public async Task<T> HashGetAllAsync<T>(string key) where T : class
        {
            var entries = await _database.HashGetAllAsync(key);
            if (entries is null || entries.Length == 0)
                return default;

            T desiredObject = (T)Activator.CreateInstance(typeof(T), true);

            foreach (var field in entries)
            {
                try
                {
                    Type type = desiredObject.GetType().GetProperty(field.Name).PropertyType;
                    var converter = TypeDescriptor.GetConverter(type);
                    var obj = converter.ConvertFromString(field.Value);
                    var fieldHasSetter = desiredObject.GetType().GetProperty(field.Name)?.GetSetMethod() != null;
                    
                    if (fieldHasSetter)
                    {
                        desiredObject.GetType().GetProperty(field.Name)?.SetValue(desiredObject, obj);
                    }
                }
                catch (System.Reflection.AmbiguousMatchException e)
                {
                    throw new KeyValueMatchException(desiredObject.GetType().FullName, e);
                }
            }
            return desiredObject;
        }

        public async Task<RedisResult> ExecuteAsync(string cmd, params object[] args)
        {
            return await _database.ExecuteAsync(cmd, args);
        }

        public void Dispose()
        {
            _connectionFactory.DisposeConnection();
        }
    }
}

