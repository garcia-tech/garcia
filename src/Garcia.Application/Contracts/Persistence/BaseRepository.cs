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

        public abstract Task<T> GetByIdAsync(long id, bool getSoftDeletes = false);
        public abstract Task<IReadOnlyList<T>> GetAllAsync(bool getSoftDeletes = false);
        //public abstract Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value);
        public abstract Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        //public abstract Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter);
        public abstract Task<long> AddAsync(T entity);
        public abstract Task<long> AddRangeAsync(IEnumerable<T> entities);
        public abstract Task<long> UpdateAsync(T entity);
        public abstract Task<long> DeleteAsync(T entity, bool hardDelete = false);
        public abstract Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false);
        public abstract Task<IReadOnlyList<T>> GetAllAsync(int page, int size, bool getSoftDeletes = false);
        public abstract Task<T> GetByIdWithNavigationsAsync(long id, bool getSoftDeletes = false);
        public abstract Task<T> GetByFilterWithNavigationsAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        public abstract Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        public abstract Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        public abstract Task<long> CountAsync(Expression<Func<T, bool>> filter, bool countSoftDeletes = false);
        public abstract Task<long> UpdateManyAsync(IEnumerable<T> updatedList);

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