using System;
using System.Linq;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure
{
    public class GarciaMemoryCache : IGarciaCache
    {
        protected CacheSettings _settings;
        protected IMemoryCache _memoryCache;

        public GarciaMemoryCache(IMemoryCache memoryCache, IOptions<CacheSettings> settings)
        {
            _settings = settings.Value;
            _memoryCache = memoryCache;
        }

        public virtual T Set<T>(string key, T item, int? expirationInMinutes = null)
        {
            int minutes = expirationInMinutes ?? _settings.CacheExpirationInMinutes;

            if (minutes == 0)
            {
                //Provider.CachingEnabled = false;
                return item;
            }

            T result = _memoryCache.Set(key, item, new TimeSpan(0, minutes, 0));
            return result;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public async Task ClearRepositoryCacheAsync<T, TKey>(T entity)
            where T : EntityBase<TKey>
        {
            var keys = _memoryCache.GetKeys<string>()
                .Where(x => x.Contains(typeof(T).Name))
                .ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                _memoryCache.Remove(key);
            }

            await Task.CompletedTask;
        }
    }
}