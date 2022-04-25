using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;
using Microsoft.EntityFrameworkCore;

namespace Garcia.Persistence.EntityFramework
{
    public partial class EntityFrameworkRepository<T> : BaseRepository<T> where T : Entity<long>
    {
        protected readonly DbContext _dbContext;

        public EntityFrameworkRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public override async Task<long> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync(int page, int size)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        //public override Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value)
        //{
        //    throw new NotImplementedException();
        //}

        public override async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        public override async Task<long> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var entities = _dbContext.Set<T>().Where(filter);
            _dbContext.Set<T>().RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<T> GetByIdWithNavigationsAsync(long id)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            var navigations = _dbContext.Model.FindEntityType(typeof(T))?
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

            if (navigations != null && navigations.Count() > 0)
            {
                foreach (var property in navigations)
                    query = query.Include(property.Name);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}