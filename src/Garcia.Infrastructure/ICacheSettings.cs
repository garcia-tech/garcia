namespace Garcia.Infrastructure
{
    public interface ICacheSettings
    {
        public int CacheExpirationInMinutes { get; set; }
    }
}
