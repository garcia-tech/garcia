using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace GarciaCore.Infrastructure
{
    public class GarciaCoreMemoryCache : IGarciaCoreCache
    {
        protected Settings _settings;
        protected MemoryCache _memoryCache;

        public GarciaCoreMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor, IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _memoryCache = new MemoryCache(optionsAccessor);
        }

        public GarciaCoreMemoryCache(IOptions<Settings> settings) : this(new MemoryCacheOptions() { }, settings)
        {
        }

        public virtual TItem Set<TItem>(object key, TItem item)
        {
            int minutes = _settings.CacheExpirationTimeInMinutes;

            if (minutes == 0)
            {
                //Provider.CachingEnabled = false;
                return item;
            }

            TItem result = _memoryCache.Set(key, item, new TimeSpan(0, minutes, 0));
            return result;
        }

        public void Remove(string name)
        {
            _memoryCache.Remove(name);
        }

        public TItem Get<TItem>(object key)
        {
            return _memoryCache.Get<TItem>(key);
        }
    }
}
