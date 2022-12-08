using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using MongoDB.Driver;

namespace Garcia.Application.MongoDb.Contracts.Persistence
{
    public interface IAsyncMongoDbRepository<T> : IAsyncRepository<T, string> where T : class
    {
        /// <summary>
        /// Updates multiple document matching the <paramref name="filter"/> by <paramref name="definition"/>
        /// </summary>
        /// <param name="filter">Query of the operation.</param>
        /// <param name="definition">Definition of the update operation.</param>
        /// <returns></returns>
        Task<long> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> definition);
        /// <summary>
        /// Checks if there are any entities that match the filter
        /// </summary>
        /// <param name="filter">Query of the operation.</param>
        /// <returns><see langword="true"/> if any matching entity; otherwise <see langword="false"/></returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        /// <summary>
        /// Gets all entities with pagination.
        /// </summary>
        /// <param name="page">Desired page.</param>
        /// <param name="size">Count of entity.</param>
        /// <returns><c>IReadonlyList</c> of <typeparamref name="T"/> entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(int page, int size, bool getSoftDeletes = false);
    }
}
