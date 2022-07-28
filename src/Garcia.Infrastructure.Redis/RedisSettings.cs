using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Infrastructure.Redis
{
    public class RedisSettings : DatabaseSettings, ICacheSettings
    {
        public string Host { get; set; }
        public string Port { get; set; } = "6379";
        public string ServiceLockKey { get; set; } = "lock";
        public int CacheExpirationInMinutes { get; set; } = 1;
        public virtual string GetHostKeyValue() => nameof(Host);
        public virtual string GetPortKeyValue() => nameof(Port);
        public virtual string GetPasswordKeyValue() => nameof(Password);
        public virtual string GetCacheExpirationInMinutesKeyValue() => nameof(CacheExpirationInMinutes);
        public virtual string GetServiceLockKeyKeyValue() => nameof(ServiceLockKey);
        public virtual string GetNodeValue() => nameof(RedisSettings);
    }
}
