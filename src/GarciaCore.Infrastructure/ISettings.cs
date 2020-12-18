using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public interface ISettings
    {
        public int CacheExpirationTimeInMinutes { get; set; }
    }
}
