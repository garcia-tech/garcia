using System;
using System.Collections.Generic;
using System.Linq;
using Garcia.Domain;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;

namespace Garcia.Persistence
{
    public class Repository<T> : BaseRepository<T> where T : Entity<long>
    {
        private IAsyncRepository<T> _repository;

        public Repository(IAsyncRepository<T> repository)
        {
            _repository = repository;
        }

        public override async Task<long> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public override async Task<long> AddRangeAsync(IEnumerable<T> entities)
        {
            return await _repository.AddRangeAsync(entities);
        }

        public override async Task<long> DeleteAsync(T entity, bool hardDelete = false)
        {
            return await _repository.DeleteAsync(entity, hardDelete);
        }

        public override async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false)
        {
            return await _repository.DeleteManyAsync(filter, hardDelete);
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

        public override Task<T> GetByIdWithNavigationsAsync(long id)
        {
            throw new NotImplementedException();
        }

        public override async Task<long> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}