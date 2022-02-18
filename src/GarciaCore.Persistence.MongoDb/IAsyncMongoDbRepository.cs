using GarciaCore.Domain.MongoDb;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GarciaCore.Persistence.MongoDb
{
    public interface IAsyncMongoDbRepository<T> : IAsyncRepository<T, string> where T : MongoDbEntity
    {
        Task UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> definition);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    }
}
