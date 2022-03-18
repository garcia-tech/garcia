using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using GarciaCore.Domain;
using GarciaCore.Application;
using GarciaCore.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Test.Utils
{
    public static class MockCreator
    {
        /// <summary>
        /// Mocks up base repository methods.
        /// Returns an <see cref="Mock{T}" /> of <see cref="IAsyncRepository{T, TKey}"/>
        /// of <typeparamref name="TEntity"/> , <typeparamref name="TKey"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="mockDataSet">Mock dataset that mock repository will use.</param>
        /// <param name="testId">A test id of type TKey. Mock repository will use it for generating some expressions.</param>
        /// <returns></returns>
        public static Mock<IAsyncRepository<TEntity, TKey>> CreateMockRepository<TEntity, TKey>(List<TEntity> mockDataSet, TKey testId) 
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            var repository = new Mock<IAsyncRepository<TEntity, TKey>>();

            repository.Setup(x => x.AddAsync(It.IsAny<TEntity>()))
                .ReturnsAsync((TEntity entity) =>
                {
                    mockDataSet.Add(entity);
                    return 1;
                });

            repository.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .ReturnsAsync((IEnumerable<TEntity> entities) =>
                {
                    mockDataSet.AddRange(entities);
                    return entities.Count();
                });

            repository.Setup(x => x.DeleteAsync(It.IsAny<TEntity>()))
                .ReturnsAsync((TEntity entity) =>
                {
                    var result = mockDataSet.Remove(entity);
                    return Convert.ToInt32(result);
                });

            repository.Setup(x => x.DeleteManyAsync(x => x.Id.Equals(testId)))
                .ReturnsAsync(() =>
                {
                    mockDataSet.RemoveAll(x => x.Id.Equals(testId));
                    return mockDataSet.Count(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetByIdAsync(testId))
                .ReturnsAsync(() => 
                {
                    return mockDataSet.FirstOrDefault(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() =>
                {
                    return mockDataSet;
                });

            return repository;
        }

        /// <summary>
        /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.WebApplicationFactory"/>
        /// </summary>
        /// <typeparam name="TEntryPoint"></typeparam>
        /// <returns><see cref="HttpClient"/></returns>
        public static HttpClient CreateTestClient<TEntryPoint>() where TEntryPoint : class
        {
            return new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder => { })
                .CreateClient();
        }

        /// <summary>
        /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.WebApplicationFactory"/>
        /// </summary>
        /// <typeparam name="TEntryPoint"></typeparam>
        /// <returns><see cref="HttpClient"/></returns>
        public static HttpClient CreateTestClient<TEntryPoint>(Action<IServiceCollection> services) where TEntryPoint : class
        {
            return new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services);
                })
                .CreateClient();
        }
    }
}