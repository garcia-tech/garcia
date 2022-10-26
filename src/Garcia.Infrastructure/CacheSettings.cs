namespace Garcia.Infrastructure
{
    public class CacheSettings : ICacheSettings
    {
        public int CacheExpirationInMinutes { get; set; } = 1;
    }
}
