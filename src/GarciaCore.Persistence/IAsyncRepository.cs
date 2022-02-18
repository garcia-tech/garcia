using GarciaCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.Persistence
{
    public interface IAsyncRepository<T, TKey> where TKey : IEquatable<TKey>
    {
        Task<T> GetByIdAsync(TKey id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task DeleteManyAsync(Expression<Func<T, bool>> filter);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetAllAsync(int page, int size);
        //Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter);
        //Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter);
    }
}