using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;
using Shouldly;
using Mongo2Go;
using GarciaCore.Infrastructure;
using GarciaCore.Persistence.MongoDb;
using static GarciaCore.Persistence.Tests.Utils.Helpers;
using GarciaCore.Infrastructure.MongoDb;

namespace GarciaCore.Persistence.Tests
{
    public class MongoDbTests
    {
        private static Mock<IOptions<MongoDbSettings>> _mockOptions;
        private static MongoDbRepository<TestMongoEntity> _repository;
        private static MongoDbRunner _runner;

        public MongoDbTests()
        {
            _mockOptions = new Mock<IOptions<MongoDbSettings>>();
        }

        private static void CreateConnection()
        {
            _runner = MongoDbRunner.Start();

            var settings = new MongoDbSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "test"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _repository = new MongoDbRepository<TestMongoEntity>(_mockOptions.Object);
        }

        private static void DisposeConnection() => _runner.Dispose();

        [Fact]
        public async Task GetAllAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var result = await _repository.GetAllAsync();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(5);
            DisposeConnection();
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var entities = await _repository.GetAllAsync();
            var result = await _repository.GetByIdAsync(entities.FirstOrDefault()?.Id);

            result.ShouldNotBeNull();
            result.ShouldBeEquivalentTo(entities.FirstOrDefault());
            DisposeConnection();
        }
        [Fact]
        public async Task GetAllAsyncWithPagination()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var result = await _repository.GetAllAsync(2, 2);
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            DisposeConnection();
        }

        [Fact]
        public async Task AddAsync()
        {
            CreateConnection();

            var entity = new TestMongoEntity
            {
                Name = "Test1"
            };

            var result = await _repository.AddAsync(entity);
            Assert.Equal(1, result);
            DisposeConnection();
        }

        [Fact]
        public async Task AddRangeAsync()
        {
            CreateConnection();

            var entities = new List<TestMongoEntity>()
            {
                new TestMongoEntity
                {
                    Name = "Test1"
                },
                new TestMongoEntity
                {
                    Name = "Test2"
                }
            };

            var resultCount = await _repository.AddRangeAsync(entities);
            resultCount.ShouldBe(2);
            var result = await _repository.GetAllAsync();
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            DisposeConnection();
        }
        [Fact]
        public async Task DeleteAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var entities = await _repository.GetAllAsync();
            var resultCount = await _repository.DeleteAsync(entities.FirstOrDefault()!);
            resultCount.ShouldBe(1);
            var result = await _repository.GetAllAsync();

            result.Count.ShouldBeLessThan(entities.Count);
            result.ShouldNotContain(entities.FirstOrDefault());
            DisposeConnection();
        }

        [Fact]
        public async Task DeleteManyAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var entities = await _repository.GetAllAsync();
            var resultCount = await _repository.DeleteManyAsync(x => x.Indicator > 3);
            // resultCount.ShouldBe(1); TODO: sayı kaç olmalı?
            var result = await _repository.GetAllAsync();

            result.Count.ShouldBeLessThan(entities.Count);
            result.ShouldNotContain(x => x.Indicator > 3);
            DisposeConnection();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var entities = await _repository.GetAllAsync();

            TestMongoEntity subject = entities.FirstOrDefault()!;
            subject.Name = "Updated";
            var resultCount = await _repository.UpdateAsync(subject);
            resultCount.ShouldBe(1);
            var updatedEntity = await _repository.GetByIdAsync(subject.Id);
            var result = updatedEntity.Equals(subject);
            result.ShouldBeTrue();
            DisposeConnection();
        }

        [Fact]
        public async Task AnyAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var result = await _repository.AnyAsync(x => x.Name.Contains("Test"));
            result.ShouldBeTrue();
            DisposeConnection();
        }

        [Fact]
        public async Task UpdateManyAsync()
        {
            CreateConnection();
            await SeedMongo(_repository);
            var updateDefinition = Builders<TestMongoEntity>.Update.Set(x => x.Name, "Updated");
            var resultCount = await _repository.UpdateManyAsync(x => x.Indicator > 4, updateDefinition);
            // resultCount.ShouldBe(1); TODO: sayı kaç olmalı?
            var entities = await _repository.GetAsync(x => x.Indicator > 4);

            var result = entities.All(x => x.Name == "Updated");
            result.ShouldBeTrue();
            DisposeConnection();
        }
    }
}
