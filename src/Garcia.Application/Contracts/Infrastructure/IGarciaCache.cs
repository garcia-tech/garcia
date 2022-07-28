using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IGarciaCache
    {
        T Set<T>(string key, T item, int? expirationInMinutes = null);
        void Remove(string key);
        T Get<T>(string key);
        Task ClearRepositoryCacheAsync<T, TKey>(T entity) where T : EntityBase<TKey>;
    }
}