using Garcia.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Persistence
{
    public interface IAsyncRepository
    {
    }

    public interface IAsyncRepository<T, TKey> : IAsyncRepository where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets a single entity by id.
        /// </summary>
        /// <param name="id">Id of related entity.</param>
        /// <returns>A single entity <typeparamref name="T"/></returns>
        Task<T> GetByIdAsync(TKey id, bool getSoftDeletes = false);
        /// <summary>
        /// Gets all entities without any <paramref name="filter"/>.
        /// </summary>
        /// <returns><c>IReadonlyList</c> of <typeparamref name="T"/> all entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(bool getSoftDeletes = false);
        /// <summary>
        /// Adds a single entity.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>Number of affected records.</returns>
        Task<long> AddAsync(T entity);
        /// <summary>
        /// Adds many entities, bulk insert operation.
        /// </summary>
        /// <param name="entities">Entity list to be inserted.</param>
        /// <returns>Number of affected records.</returns>
        Task<long> AddRangeAsync(IEnumerable<T> entities);
        /// <summary>
        /// Deletes multiple entities matching the <paramref name="filter"/> if <paramref name="hardDelete"/> true.
        /// Otherwise sets Deleted field of entities to true.
        /// </summary>
        /// <param name="filter">Query of the operation.</param>
        /// <param name="hardDelete"></param>
        /// <returns>Number of affected records.</returns>
        Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, bool hardDelete = false);
        /// <summary>
        /// Updates single entity <typeparamref name="T"/>
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>Number of affected records.</returns>
        Task<long> UpdateAsync(T entity);
        /// <summary>
        /// Removes single entity <typeparamref name="T"/> if <paramref name="hardDelete"/> is true.
        /// If not, it sets the entity's Deleted field to true.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="hardDelete"></param>
        /// <returns>Number of affected records.</returns>
        Task<long> DeleteAsync(T entity, bool hardDelete = false);
        //Task<IReadOnlyList<T>> GetByKeyAsync(string key, object value);
        /// <summary>
        /// Gets multiple enetities matching the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">Query of the operation.</param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        //Task<IReadOnlyList<T>> GetAsync(Dictionary<string, object> filter);
    }

    public interface IAsyncRepository<T> : IAsyncRepository<T, long> where T : EntityBase<long>
    {
        /// <summary>
        /// Gets all entities with pagination.
        /// </summary>
        /// <param name="page">Desired page.</param>
        /// <param name="size">Count of entity.</param>
        /// <returns><see cref="IReadOnlyCollection{T}"/></returns>
        Task<IReadOnlyList<T>> GetAllAsync(int page, int size, bool getSoftDeletes = false);
        Task<T> GetByIdWithNavigationsAsync(long id, bool getSoftDeletes = false);
        Task<T> GetByFilterWithNavigationsAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool getSoftDeletes = false);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    }
}