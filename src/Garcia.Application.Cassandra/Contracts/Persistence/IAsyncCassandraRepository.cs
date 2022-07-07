using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;

namespace Garcia.Application.Cassandra.Contracts.Persistence
{
    public interface IAsyncCassandraRepository<T> : IAsyncRepository<T, Guid> where T : Entity<Guid>
    {
        /// <summary>
        /// Gets all entities with pagination.
        /// </summary>
        /// <param name="referenceId">Reference id of entity list.</param>
        /// <param name="size">Count of entity.</param>
        /// <returns>Returns the desired size of data with an id greater than the reference id <see cref="IReadOnlyCollection{T}"/> entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(Guid referenceId, int size, bool getSoftDeletes = false);
    }
}
