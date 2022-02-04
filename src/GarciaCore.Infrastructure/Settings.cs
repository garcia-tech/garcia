namespace GarciaCore.Infrastructure
{
    public class Settings : ISettings
    {
        public int CacheExpirationTimeInMinutes { get; set; }
    }
}