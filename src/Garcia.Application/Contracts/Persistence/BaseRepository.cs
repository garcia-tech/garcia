using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Persistence
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : Entity<long>
    {
        public BaseRepository()
        {
        }

        public abstract Task<T> GetByIdAsync(long id);
        public abstract Task<IReadOnlyList<T>> GetAllAsync();
        //public abstract Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value);
        public abstract Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter);
        //public abstract Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter);
        public abstract Task<long> AddAsync(T entity);
        public abstract Task<long> AddRangeAsync(IEnumerable<T> entities);
        public abstract Task<long> UpdateAsync(T entity);
        public abstract Task<long> DeleteAsync(T entity, bool hardDelete = false);
        public abstract Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false);
        public abstract Task<IReadOnlyList<T>> GetAllAsync(int page, int size);
        public abstract Task<T> GetByIdWithNavigationsAsync(long id);

        public virtual async Task<long> SaveAsync(T entity)
        {
            if (entity.Id == 0)
            {
                return await AddAsync(entity);
            }
            else if (entity.Deleted)
            {
                return await DeleteAsync(entity);
            }
            else
            {
                return await UpdateAsync(entity);
            }
        }
    }
}