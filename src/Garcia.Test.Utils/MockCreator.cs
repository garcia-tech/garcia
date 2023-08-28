using System.Linq.Expressions;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Garcia.Test.Utils
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

            repository.Setup(x => x.DeleteAsync(It.IsAny<TEntity>(), true))
                .ReturnsAsync((TEntity entity, bool hardDelete) =>
                {
                    var result = mockDataSet.Remove(entity);
                    return Convert.ToInt32(result);
                });

            repository.Setup(x => x.DeleteManyAsync(x => x.Id.Equals(testId), true))
                .ReturnsAsync(() =>
                {
                    mockDataSet.RemoveAll(x => x.Id.Equals(testId));
                    return mockDataSet.Count(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetByIdAsync(testId, false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet.FirstOrDefault(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetAllAsync(false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet;
                });

            repository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), false))
                .ReturnsAsync((Expression<Func<TEntity, bool>> expression, bool getSoftDeletes) =>
                {
                    var result = mockDataSet.Where(expression.Compile());
                    return result.ToList();
                });

            return repository;
        }

        /// <summary>
        /// Mocks up base repository methods.
        /// Returns an <typeparamref name="TMockRepository"/>
        /// of <typeparamref name="TEntity"/> , <typeparamref name="TKey"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="mockDataSet">Mock dataset that mock repository will use.</param>
        /// <param name="testId">A test id of type TKey. Mock repository will use it for generating some expressions.</param>
        /// <returns><typeparamref name="TMockRepository"/></returns>
        public static TMockRepository CreateMockRepository<TMockRepository, TEntity, TKey>(TMockRepository repository, List<TEntity> mockDataSet, TKey testId)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
            where TMockRepository : Mock<IAsyncRepository<TEntity, TKey>>
        {
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

            repository.Setup(x => x.DeleteAsync(It.IsAny<TEntity>(), true))
                .ReturnsAsync((TEntity entity, bool hardDelete) =>
                {
                    var result = mockDataSet.Remove(entity);
                    return Convert.ToInt32(result);
                });

            repository.Setup(x => x.DeleteManyAsync(x => x.Id.Equals(testId), true))
                .ReturnsAsync(() =>
                {
                    mockDataSet.RemoveAll(x => x.Id.Equals(testId));
                    return mockDataSet.Count(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetByIdAsync(testId, false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet.FirstOrDefault(x => x.Id.Equals(testId));
                });

            repository.Setup(x => x.GetAllAsync(false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet;
                });

            repository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), false))
                .ReturnsAsync((Expression<Func<TEntity, bool>> expression, bool getSoftDeletes) =>
                {
                    var result = mockDataSet.Where(expression.Compile());
                    return result.ToList();
                });

            return repository;
        }
        /// <summary>
        /// Mocks up base repository methods.
        /// </summary>
        /// <typeparam name="TRepository">Desired repository type</typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="mockDataSet"></param>
        /// <param name="testId"></param>
        /// <returns><typeparamref name="TRepository"/></returns>
        public static TRepository CreateMockRepository<TRepository, TEntity, TKey>(IAsyncRepository<TEntity, TKey> repository, List<TEntity> mockDataSet, TKey testId)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
            where TRepository : IAsyncRepository<TEntity, TKey>
        {
            var mock = Mock.Get(repository);

            mock.Setup(x => x.AddAsync(It.IsAny<TEntity>()))
                .ReturnsAsync((TEntity entity) =>
                {
                    mockDataSet.Add(entity);
                    return 1;
                });

            mock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .ReturnsAsync((IEnumerable<TEntity> entities) =>
                {
                    mockDataSet.AddRange(entities);
                    return entities.Count();
                });

            mock.Setup(x => x.DeleteAsync(It.IsAny<TEntity>(), true))
                .ReturnsAsync((TEntity entity, bool hardDelete) =>
                {
                    var result = mockDataSet.Remove(entity);
                    return Convert.ToInt32(result);
                });

            mock.Setup(x => x.DeleteManyAsync(x => x.Id.Equals(testId), true))
                .ReturnsAsync(() =>
                {
                    mockDataSet.RemoveAll(x => x.Id.Equals(testId));
                    return mockDataSet.Count(x => x.Id.Equals(testId));
                });

            mock.Setup(x => x.GetByIdAsync(testId, false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet.FirstOrDefault(x => x.Id.Equals(testId));
                });

            mock.Setup(x => x.GetAllAsync(false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet;
                });

            mock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), false))
                .ReturnsAsync((Expression<Func<TEntity, bool>> expression, bool getSoftDeletes) =>
                {
                    var result = mockDataSet.Where(expression.Compile());
                    return result.ToList();
                });

            return (TRepository)mock.Object;
        }

        /// <summary>
        /// Mocks up base repository methods.
        /// Returns as instance of desired repository type
        /// </summary>
        /// <typeparam name="TRepository">Desired repository type</typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="mockDataSet"></param>
        /// <param name="testId"></param>
        /// <returns><typeparamref name="TRepository"/></returns>
        public static TRepository CreateMockRepository<TRepository, TEntity, TKey>(List<TEntity> mockDataSet, TKey testId)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
            where TRepository : class, IAsyncRepository<TEntity, TKey>
        {
            var mock = new Mock<IAsyncRepository<TEntity, TKey>>();

            mock.Setup(x => x.AddAsync(It.IsAny<TEntity>()))
                .ReturnsAsync((TEntity entity) =>
                {
                    mockDataSet.Add(entity);
                    return 1;
                });

            mock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                .ReturnsAsync((IEnumerable<TEntity> entities) =>
                {
                    mockDataSet.AddRange(entities);
                    return entities.Count();
                });

            mock.Setup(x => x.DeleteAsync(It.IsAny<TEntity>(), true))
                .ReturnsAsync((TEntity entity, bool hardDelete) =>
                {
                    var result = mockDataSet.Remove(entity);
                    return Convert.ToInt32(result);
                });

            mock.Setup(x => x.DeleteManyAsync(x => x.Id.Equals(testId), true))
                .ReturnsAsync(() =>
                {
                    mockDataSet.RemoveAll(x => x.Id.Equals(testId));
                    return mockDataSet.Count(x => x.Id.Equals(testId));
                });

            mock.Setup(x => x.GetByIdAsync(testId, false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet.FirstOrDefault(x => x.Id.Equals(testId));
                });

            mock.Setup(x => x.GetAllAsync(false))
                .ReturnsAsync(() =>
                {
                    return mockDataSet;
                });

            mock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), false))
                .ReturnsAsync((Expression<Func<TEntity, bool>> expression, bool getSoftDeletes) =>
                {
                    var result = mockDataSet.Where(expression.Compile());
                    return result.ToList();
                });

            return mock.As<TRepository>().Object;
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

        /// <summary>
        /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.WebApplicationFactory"/>
        /// </summary>
        /// <typeparam name="TEntryPoint"></typeparam>
        /// <returns><see cref="HttpClient"/></returns>
        public static HttpClient CreateTestClient<TEntryPoint>(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate, Action<IServiceCollection> services)
            where TEntryPoint : class
        {
            return new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services);
                    builder.ConfigureAppConfiguration(configureDelegate);
                })
                .CreateClient();
        }
    }
}