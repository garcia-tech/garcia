using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace GarciaCore.Infrastructure
{
    public class GarciaCoreMemoryCache : MemoryCache
    {
        protected Settings _settings;

        public GarciaCoreMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor, IOptions<Settings> settings) : base(optionsAccessor)
        {
            _settings = settings.Value;
        }

        public GarciaCoreMemoryCache(IOptions<Settings> settings) : this(new MemoryCacheOptions() { }, settings)
        {
        }

        public TItem SetItem<TItem>(object key, TItem item)
        {
            int minutes = _settings.CacheExpirationTimeInMinutes;

            if (minutes == 0)
            {
                //Provider.CachingEnabled = false;
                return item;
            }

            TItem result = this.Set(key, item, new TimeSpan(0, minutes, 0));
            return result;
        }
    }
}
