using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Infrastructure
{
    public class CacheSettings : ICacheSettings
    {
        public int CacheExpirationInMinutes { get; set; } = 1;
    }
}
