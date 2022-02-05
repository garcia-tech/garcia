using GarciaCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.Persistence
{
    public class Repository<T> : BaseRepository<T> where T : Entity
    {
        private IAsyncRepository<T> _repository;

        public Repository(IAsyncRepository<T> repository)
        {
            _repository = repository;
        }

        public override async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public override async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return (await _repository.GetAllAsync()).ToList();
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync(int page, int size)
        {
            return await _repository.GetAllAsync(page, size);
        }

        public override async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.GetAsync(filter);
        }

        public override async Task<T> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public override async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }
    }
}