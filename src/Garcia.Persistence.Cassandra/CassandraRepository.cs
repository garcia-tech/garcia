using System.Linq.Expressions;
using Cassandra;
using Cassandra.Data.Linq;
using Garcia.Application.Cassandra.Contracts.Persistence;
using Garcia.Domain;
using Garcia.Infrastructure.Cassandra;

namespace Garcia.Persistence.Cassandra
{
    public class CassandraRepository<T> : IAsyncCassandraRepository<T> where T : Entity<Guid>
    {
        private readonly Table<T> _table;
        public CassandraRepository(CassandraConnectionFactory factory)
        {
            var session = factory.GetSession();
            _table = new Table<T>(session);
        }

        public async Task<long> AddAsync(T entity)
        {
            var result = await _table.Insert(entity, true).ExecuteAsync();
            return result.GetRows().Count();
        }

        public async Task<long> AddRangeAsync(IEnumerable<T> entities)
        {
            var batch = _table.GetSession().CreateBatch(BatchType.Logged);

            foreach (var entity in entities)
            {
                batch.Append(_table.Insert(entity, true));
            }

            await batch.ExecuteAsync();
            return entities.Count();
        }

        public async Task<long> DeleteAsync(T entity)
        {
            var result = await _table.Where(x => x.Id == entity.Id)
                .Delete()
                .ExecuteAsync();
            return result.GetRows().Count();
        }

        public async Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var result = await _table.Where(filter)
                .Delete()
                .ExecuteAsync();
            return result.GetRows().Count();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var list = await _table.ExecuteAsync();
            return list.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Guid referenceId, int size)
        {
            var list = await _table.Where(x => x.Id.CompareTo(referenceId) > 0).Take(size)
                .ExecuteAsync();
            return list.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            var result = await _table.Where(filter).ExecuteAsync();
            return result.ToList();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _table.FirstOrDefault(x => x.Id == id).ExecuteAsync();
        }

        public async Task<long> UpdateAsync(T entity)
        {
            var result = await _table.Where(x => x.Id == entity.Id)
                .Select(x => entity)
                .Update()
                .ExecuteAsync();
            return result.GetRows().Count();
        }
    }
}
