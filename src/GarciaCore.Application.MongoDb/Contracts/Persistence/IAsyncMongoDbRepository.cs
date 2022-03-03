using GarciaCore.Application.Contracts.Persistence;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.Application.MongoDb.Contracts.Persistence
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
    }
}
