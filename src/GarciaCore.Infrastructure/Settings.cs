using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public class Settings : ISettings
    {
        public int CacheExpirationTimeInMinutes { get; set; }
    }
}
