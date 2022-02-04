using GarciaCore.Domain;
using GarciaCore.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.EntityFramework
{
    public partial class EntityFrameworkRepository<T> : BaseRepository<T> where T : Entity
    {
        protected readonly DbContext _dbContext;

        public EntityFrameworkRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public override async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public override async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public override async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync(int page, int size)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public override Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public override async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        public override Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter)
        {
            throw new NotImplementedException();
        }
    }
}