using System.Linq.Expressions;
using Cassandra;
using Cassandra.Data.Linq;
using Garcia.Application.Cassandra.Contracts.Persistence;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Domain;
using Garcia.Infrastructure.Cassandra;

namespace Garcia.Persistence.Cassandra
{
    public class CassandraRepository<T> : IAsyncCassandraRepository<T> where T : Entity<Guid>, new()
    {
        private readonly Table<T> _table;
        protected IGarciaCache? GarciaCache { get; }

        public CassandraRepository(CassandraConnectionFactory factory)
        {
            var session = factory.GetSession();
            _table = new Table<T>(session);
        }

        public CassandraRepository(CassandraConnectionFactory factory, IGarciaCache garciaCache)
        {
            var session = factory.GetSession();
            _table = new Table<T>(session);
            GarciaCache = garciaCache;
        }

        public async Task<long> AddAsync(T entity)
        {
            var result = await _table.Insert(entity, true).ExecuteAsync();
            await ClearRepositoryCacheAsync(entity);
            return result.GetRows().Count();
        }

        public async Task<long> AddRangeAsync(IEnumerable<T> entities)
        {
            var batch = _table.GetSession().CreateBatch(BatchType.Logged);

            foreach (var entity in entities)
            {
                batch.Append(_table.Insert(entity, true));
            }
            await ClearRepositoryCacheAsync(entities.First());
            await batch.ExecuteAsync();
            return entities.Count();
        }

        public async Task<long> DeleteAsync(T entity, bool hardDelete = false)
        {
            RowSet result;

            if (!hardDelete)
            {
                entity.Deleted = true;
                result = await _table.Where(x => x.Id == entity.Id)
                    .Select(x => entity)
                    .Update()
                    .ExecuteAsync();
                return result.GetRows().Count();
            }

            result = await _table.Where(x => x.Id == entity.Id)
                .Delete()
                .ExecuteAsync();
            await ClearRepositoryCacheAsync(entity);
            return result.GetRows().Count();
        }

        public async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false)
        {
            RowSet result;

            if (!hardDelete)
            {
                result = await _table.Where(filter)
                    .Select(x => new T { Deleted = true })
                    .Update()
                    .ExecuteAsync();
            }

            result = await _table.Where(filter)
                .Delete()
                .ExecuteAsync();
            await ClearRepositoryCacheAsync(new T());
            return result.GetRows().Count();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(bool getSoftDeletes = false)
        {
            var list = !getSoftDeletes ? await _table.Where(x => !x.Deleted).ExecuteAsync()
                : await _table.ExecuteAsync();
            return list.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Guid referenceId, int size, bool getSoftDeletes = false)
        {
            Expression<Func<T, bool>> expression = !getSoftDeletes ? (x => !x.Deleted && x.Id.CompareTo(referenceId) > 0)
                : x => x.Id.CompareTo(referenceId) > 0;

            if (getSoftDeletes)
            {
                return (await _table.Where(expression).Take(size)
                    .ExecuteAsync())
                    .ToList();
            }

            var proxyEntity = new T();

            if (!proxyEntity.CachingEnabled)
            {
                return (await _table.Where(expression).Take(size)
                    .ExecuteAsync())
                    .ToList();
            }

            var keyPrefix = $"{typeof(T).Name}:{nameof(this.GetAllAsync)}:{referenceId}:{size}";
            var cachedData = GarciaCache?.Get<List<T>>(keyPrefix);

            if (cachedData != null) return cachedData;

            var result = (await _table
                .Where(expression)
                .Take(size)
                .ExecuteAsync())
                .ToList();
            GarciaCache!.Set(keyPrefix, result, proxyEntity.CacheExpirationInMinutes);
            return result;
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false)
        {
            var query = _table.Where(filter);

            if (!getSoftDeletes)
            {
                query.Where(x => !x.Deleted);
            }

            var result = await query.ExecuteAsync();
            return result.ToList();
        }

        public async Task<T> GetByIdAsync(Guid id, bool getSoftDeletes = false)
        {
            if (getSoftDeletes)
            {
                return await _table.FirstOrDefault(x => x.Id == id).ExecuteAsync();
            }

            var proxyEntity = new T();

            if (!proxyEntity.CachingEnabled)
            {
                return await _table.FirstOrDefault(x => !x.Deleted && x.Id == id).ExecuteAsync();
            }

            var keyPrefix = $"{typeof(T).Name}:{nameof(this.GetByIdAsync)}:{id}";
            var cachedData = GarciaCache?.Get<T>(keyPrefix);

            if (cachedData != null) return cachedData;

            var result = await _table.FirstOrDefault(x => !x.Deleted && x.Id == id).ExecuteAsync();
            GarciaCache!.Set(keyPrefix, result, proxyEntity.CacheExpirationInMinutes);
            return result;
        }

        public async Task<long> UpdateAsync(T entity)
        {
            var result = await _table.Where(x => x.Id == entity.Id)
                .Select(x => entity)
                .Update()
                .ExecuteAsync();
            await ClearRepositoryCacheAsync(entity);
            return result.GetRows().Count();
        }

        private async Task ClearRepositoryCacheAsync(T proxyEntity)
        {
            if (proxyEntity.CachingEnabled)
            {
                await GarciaCache!.ClearRepositoryCacheAsync<T, Guid>(proxyEntity);
            }
        }
    }
}
