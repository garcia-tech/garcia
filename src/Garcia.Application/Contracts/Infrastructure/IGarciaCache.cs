namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IGarciaCache
    {
        T Set<T>(string key, T item, int? expirationInMinutes = null);
        void Remove(string key);
        T Get<T>(string key);
    }
}