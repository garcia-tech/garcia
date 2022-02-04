using GarciaCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.Persistence
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : Entity
    {
        public BaseRepository()
        {
        }

        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task<IReadOnlyList<T>> GetAllAsync();
        public abstract Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value);
        public abstract Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter);
        public abstract Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter);
        public abstract Task<T> AddAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(T entity);
        public abstract Task<IReadOnlyList<T>> GetAllAsync(int page, int size);

        public virtual async Task<T> SaveAsync(T entity)
        {
            if (entity.Id == 0)
            {
                await AddAsync(entity);
            }
            else if (entity.IsDeleted)
            {
                await DeleteAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }

            return entity;
        }
    }
}