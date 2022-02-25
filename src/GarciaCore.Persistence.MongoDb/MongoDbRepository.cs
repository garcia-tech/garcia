using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GarciaCore.Infrastructure;
using GarciaCore.Domain.MongoDb;
using GarciaCore.Infrastructure.MongoDb;

namespace GarciaCore.Persistence.MongoDb
{
    public class MongoDbRepository<T> : IAsyncMongoDbRepository<T> where T : MongoDbEntity
    {
        protected readonly IMongoCollection<T> Collection;

        public MongoDbRepository(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new InsertManyOptions
            {
                IsOrdered = false,
                BypassDocumentValidation = false
            };

            await Collection.InsertManyAsync(entities, options);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            var count = await Collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task DeleteAsync(T entity)
        {
            await Collection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await (await Collection
                .FindAsync(_ => true))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(int page, int size)
        {
            return await Collection
                .Aggregate()
                .Match(_ => true)
                .Skip((page - 1) * size)
                .Limit(size)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await (await Collection
                .FindAsync(filter))
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await (await Collection
                .FindAsync(x => x.Id == id))
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await Collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
        }

        public async Task UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> definition)
        {
            await Collection.UpdateManyAsync(filter, definition);
        }
    }
}
