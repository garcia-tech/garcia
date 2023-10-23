﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;
using Garcia.Exceptions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Garcia.Persistence.EntityFramework
{
    public partial class EntityFrameworkRepository<T> : BaseRepository<T> where T : Entity<long>, new()
    {
        protected readonly BaseContext _dbContext;
        protected IGarciaCache? GarciaCache { get; }

        public EntityFrameworkRepository(BaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EntityFrameworkRepository(BaseContext dbContext, IGarciaCache garciaCache)
        {
            _dbContext = dbContext;
            GarciaCache = garciaCache;
        }

        public override async Task<T> GetByIdAsync(long id, bool getSoftDeletes = false)
        {
            if (getSoftDeletes)
            {
                return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            }

            var proxyEntity = new T();

            if (!proxyEntity.CachingEnabled)
            {
                return await _dbContext.Set<T>()
                    .FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
            }

            try
            {
                var keyPrefix = $"{typeof(T).Name}:{nameof(this.GetByIdAsync)}:{id}";
                var cachedData = GarciaCache?.Get<T>(keyPrefix);

                if (cachedData != null) return cachedData;

                var result = await _dbContext.Set<T>().FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
                GarciaCache!.Set(keyPrefix, result, proxyEntity.CacheExpirationInMinutes);
                return result;
            }
            catch (ArgumentNullException)
            {

                throw new GarciaCacheInstanceMissingException();
            }
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync(bool getSoftDeletes = false)
        {
            return !getSoftDeletes ? (await _dbContext.Set<T>().Where(x => !x.Deleted)
                .ToListAsync()) : await _dbContext.Set<T>().ToListAsync();
        }

        public override async Task<long> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await ClearRepositoryCacheAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await ClearRepositoryCacheAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> UpdateManyAsync(IEnumerable<T> updatedList)
        {
            _dbContext.Set<T>().UpdateRange(updatedList);
            await ClearRepositoryCacheAsync(new T());
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> DeleteAsync(T entity, bool hardDelete = false)
        {
            if (!hardDelete)
            {
                entity.Deleted = true;
                entity.DeletedOn = DateTime.Now;
                _dbContext.Entry(entity).State = EntityState.Modified;
                await ClearRepositoryCacheAsync(entity);
                return await _dbContext.SaveChangesAsync();
            }

            _dbContext.Set<T>().Remove(entity);
            await ClearRepositoryCacheAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<IReadOnlyList<T>> GetAllAsync(int page, int size, bool getSoftDeletes = false)
        {
            if (getSoftDeletes)
            {
                return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
            }

            var proxyEntity = new T();

            if (!proxyEntity.CachingEnabled)
            {
                return await _dbContext.Set<T>().Where(x => !x.Deleted)
                    .Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
            }

            try
            {
                var keyPrefix = $"{typeof(T).Name}:{nameof(this.GetAllAsync)}:{page}:{size}";
                var cachedData = GarciaCache?.Get<List<T>>(keyPrefix);

                if (cachedData != null) return cachedData;

                var result = await _dbContext.Set<T>().Where(x => !x.Deleted)
                    .Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
                GarciaCache!.Set(keyPrefix, result, proxyEntity.CacheExpirationInMinutes);
                return result;

            }
            catch (ArgumentNullException)
            {
                throw new GarciaCacheInstanceMissingException();
            }
        }

        public override async Task<IReadOnlyList<T>> GetAllWithNavigationsAsync(Expression<Func<T, bool>> filter = null, bool getSoftDeletes = false, int? page = null, int? size = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            var navigations = _dbContext.Model.FindEntityType(typeof(T))?
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

            if (navigations != null && navigations.Any())
            {
                foreach (var property in navigations)
                    query = query.Include(property.Name);
            }

            if(!getSoftDeletes)
            {
                query = query.Where(x => !x.Deleted);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(page != null && size != null)
            {
                query = query.Skip(((page - 1) * size).Value).Take(size.Value);
            }

            return await query.ToListAsync();
        }

        public override async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false)
        {
            if (!getSoftDeletes)
            {
                return await _dbContext.Set<T>().AsNoTracking().Where(filter).Where(x => !x.Deleted)
                    .ToListAsync();
            }

            return await _dbContext.Set<T>().AsNoTracking().Where(filter)
                .ToListAsync();
        }

        public override async Task<long> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await ClearRepositoryCacheAsync(entities.First());
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false)
        {
            var entities = _dbContext.Set<T>().Where(filter);

            if (!entities.Any()) return 0;

            if (!hardDelete)
            {
                await entities.ForEachAsync(x =>
                {
                    x.Deleted = true;
                    x.DeletedOn = DateTime.Now;
                    _dbContext.Entry(x).State = EntityState.Modified;
                });
                await ClearRepositoryCacheAsync(entities.First());
                return await _dbContext.SaveChangesAsync();
            }

            _dbContext.Set<T>().RemoveRange(entities);
            await ClearRepositoryCacheAsync(entities.First());
            return await _dbContext.SaveChangesAsync();
        }

        public override async Task<T> GetByIdWithNavigationsAsync(long id, bool getSoftDeletes = false)
        {

            try
            {
                var proxyEntity = new T();
                var keyPrefix = string.Empty;

                if (proxyEntity.CachingEnabled)
                {
                    keyPrefix = $"{typeof(T).Name}:{nameof(this.GetByIdWithNavigationsAsync)}:{id}";
                    var cachedData = GarciaCache?.Get<T>(keyPrefix);

                    if (cachedData != null) return cachedData;
                }

                var query = _dbContext.Set<T>().AsQueryable();
                var navigations = _dbContext.Model.FindEntityType(typeof(T))?
                        .GetDerivedTypesInclusive()
                        .SelectMany(type => type.GetNavigations())
                        .Distinct();

                if (navigations != null && navigations.Any())
                {
                    foreach (var property in navigations)
                        query = query.Include(property.Name);
                }

                if (getSoftDeletes)
                {
                    return await query.FirstOrDefaultAsync(x => x.Id == id);
                }

                var result = await query.FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);

                if (!proxyEntity.CachingEnabled || result == null)
                {
                    return result;
                }

                GarciaCache!.Set(keyPrefix, result, proxyEntity.CacheExpirationInMinutes);
                return result;
            }
            catch (ArgumentNullException)
            {
                throw new GarciaCacheInstanceMissingException();
            }
        }

        public override async Task<T> GetByFilterWithNavigationsAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false)
        {

            var query = _dbContext.Set<T>().AsQueryable();
            var navigations = _dbContext.Model.FindEntityType(typeof(T))?
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

            if (navigations != null && navigations.Any())
            {
                foreach (var property in navigations)
                    query = query.Include(property.Name);
            }

            if (getSoftDeletes)
            {
                return await query.FirstOrDefaultAsync(filter);
            }

            var result = await query.Where(x => !x.Deleted).FirstOrDefaultAsync(filter);
            return result;

        }

        public override async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false)
        {
            if (!getSoftDeletes)
            {
                return await _dbContext.Set<T>()
                    .Where(x => !x.Deleted)
                    .FirstOrDefaultAsync(filter);
            }

            return await _dbContext.Set<T>()
                .FirstOrDefaultAsync(filter);
        }

        public override async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().AnyAsync(filter);
        }

        private async Task ClearRepositoryCacheAsync(T proxyEntity)
        {
            if (proxyEntity.CachingEnabled)
            {
                try
                {
                    await GarciaCache!.ClearRepositoryCacheAsync<T, long>(proxyEntity);
                }
                catch (ArgumentNullException)
                {
                    throw new GarciaCacheInstanceMissingException();
                }
            }
        }

        public override async Task<long> CountAsync(Expression<Func<T, bool>> filter, bool countSoftDeletes = false)
        {
            if (!countSoftDeletes)
            {
                return await _dbContext.Set<T>().Where(x => !x.Deleted)
                    .CountAsync(filter);
            }

            return await _dbContext.Set<T>().CountAsync(filter);
        }
    }
}