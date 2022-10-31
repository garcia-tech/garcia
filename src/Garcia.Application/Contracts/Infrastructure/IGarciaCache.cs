using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IGarciaCache
    {
        /// <summary>
        /// Sets value to the related <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Name of the key.</param>
        /// <param name="item">Object to be setted.</param>
        /// <param name="expirationInMinutes">Expiration time. If not setted, will be used default expiration time.</param>
        /// <returns>Returns setted <typeparamref name="T"/> object</returns>
        T Set<T>(string key, T item, int? expirationInMinutes = null);
        /// <summary>
        /// Removes the specifed key. A key is ignored if does not exists
        /// </summary>
        /// <param name="key">Name of the key.</param>
        /// <returns></returns>
        void Remove(string key);
        /// <summary>
        /// Gets the value of related <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Desired object.</typeparam>
        /// <param name="key">Name of the key.</param>
        /// <returns><typeparamref name="T"/> if key exists and not null; otherwise <see langword="null"/></returns>
        T Get<T>(string key);
        /// <summary>
        /// Removes all keys that the given key matches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task ClearRepositoryCacheAsync<T, TKey>(T entity) where T : EntityBase<TKey>;
    }
}