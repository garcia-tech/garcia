namespace GarciaCore.Infrastructure
{
    public interface ISettings
    {
        public int CacheExpirationTimeInMinutes { get; set; }
    }
}