namespace Garcia.Infrastructure
{
    public interface ISettings
    {
        int CacheExpirationTimeInMinutes { get; set; }
    }
}