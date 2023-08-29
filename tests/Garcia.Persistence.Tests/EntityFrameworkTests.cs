using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using Garcia.Infrastructure;
using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Persistence.Tests
{
    public class EntityFrameworkTestFixture
    {
        private const string ConnectionString =
            @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

        private readonly IAsyncRepository<TestEntity> _repository;

        public EntityFrameworkTestFixture()
        {
            using (var context = CreateContext())
            {
                var child = new TestChildEntity()
                {
                    Name = "test"
                };
                var parent = new TestEntity(1, "One");
                parent.AddChild(child);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.AddRange(
                    parent,
                    new TestEntity(2, "Two"));
                context.Add(child);
                context.SaveChanges();
                var mockOptions = new Mock<IOptions<CacheSettings>>();
                mockOptions.Setup(x => x.Value).Returns(new CacheSettings { CacheExpirationInMinutes = 2 });
                _repository = new EntityFrameworkRepository<TestEntity>(CreateContext(), new GarciaMemoryCache(new MemoryCache(new MemoryCacheOptions()), mockOptions.Object));
            }
        }

        public BaseContext CreateContext()
            => new EntityFrameworkTestsContext(
                new DbContextOptionsBuilder<EntityFrameworkTestsContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        [Fact]
        public async Task GetByIdAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            Assert.NotNull(item);
            item = await _repository.GetByIdAsync(3);
            Assert.Null(item);
        }

        [Fact]
        public async Task GetByIdWithNavigations()
        {

            var item = await _repository.GetByIdWithNavigationsAsync(1);
            Assert.NotNull(item);
            Assert.NotEqual(0, item.TestChildEntities.Count);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            Assert.NotNull(items);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task AddAsync()
        {
            var item = new TestEntity(3, "Three");
            var result = await _repository.AddAsync(item);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            item.Name = $"One-{DateTime.Now}";
            await _repository.UpdateAsync(item);
            var existingItem = await _repository.GetByIdAsync(1);
            Assert.Equal(item.Name, existingItem.Name);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            await _repository.DeleteAsync(item);
            item = await _repository.GetByIdAsync(1);
            Assert.Null(item);

            item = await _repository.GetByIdAsync(2);
            item.Deleted = true;
            await _repository.UpdateAsync(item);
            item = await _repository.GetByIdAsync(2);
            Assert.Null(item);
        }

        [Fact]
        public async Task GetAllAsync2()
        {
            var items = await _repository.GetAllAsync(1, 1);
            Assert.NotNull(items);
            Assert.Equal(1, items.Count);
            items = await _repository.GetAllAsync(1, 2);
            Assert.NotNull(items);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetAsync()
        {
            var items = await _repository.GetAsync(x => x.Id == 1);
            Assert.NotNull(items);
            Assert.Equal(1, items.Count);
            items = await _repository.GetAsync(x => x.Id == 3);
            Assert.NotNull(items);
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public async Task GetAsync_Should_Not_Get_Deleted_Items()
        {
            var item = new TestEntity(6, "Six");
            await _repository.AddAsync(item);
            await _repository.DeleteAsync(item);
            var items = await _repository.GetAsync(x => x.Id == 6);
            Assert.NotNull(items);
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public async Task GetByFilterAsync_Should_Not_Get_Deleted_Items()
        {
            var item = new TestEntity(7, "Seven");
            await _repository.AddAsync(item);
            await _repository.DeleteAsync(item);
            var items = await _repository.GetByFilterAsync(x => x.Id == 7);
            Assert.Null(items);
        }

        [Fact]
        public async Task AddRangeAsync()
        {
            var items = new List<TestEntity>()
            {
                new TestEntity(4 , "Test4"),
                new TestEntity(5 , "Test5")

            };

            await _repository.AddRangeAsync(items);
            var entities = await _repository.GetAllAsync();
            Assert.NotNull(entities);
            Assert.NotEqual(0, entities.Count);
            Assert.Contains(entities, x => x.Key == 4 || x.Key == 5);
        }

        [Fact]
        public async Task DeleteManyAsync()
        {
            await _repository.DeleteManyAsync(x => x.Key > 1);
            var entities = await _repository.GetAllAsync();
            entities.Any(x => x.Key > 1).ShouldBeFalse();
        }

        [Fact]
        public async Task CountAsync()
        {
            var count = await _repository.CountAsync(x => x.Id == 1);
            count.ShouldBe(1);
        }

        [Fact]
        public async Task Soft_Delete_Should_Success()
        {
            var item = await _repository.GetByIdAsync(1);
            await _repository.DeleteAsync(item);
            var list = await _repository.GetAllAsync();
            list.Any(x => x.Id == 1).ShouldBeFalse();
            item = await _repository.GetByIdAsync(1);
            item.ShouldBeNull();
            list = await _repository.GetAsync(x => x.Id == 1);
            list.Any(x => x.Id == 1).ShouldBeFalse();
            item = await _repository.GetByFilterAsync(x => x.Id == 1);
            item.ShouldBeNull();
            item = await _repository.GetByFilterWithNavigationsAsync(x => x.Id == 1);
            item.ShouldBeNull();
            item = await _repository.GetByIdWithNavigationsAsync(1);
            item.ShouldBeNull();
        }

        [Theory(DisplayName = "GetByFilterWithNavigationsAsync method do not sets cache key with variable name instead of their value")]
        [InlineData(1)]
        public async Task GetByFilterWithNavigationsAsync_CacheKey_Must_Be_Unique(int id)
        {
            var testItem1 = await _repository.GetByFilterWithNavigationsAsync(x => x.Id == id);
            id = 2;
            var testItem2 = await _repository.GetByFilterWithNavigationsAsync(x => x.Id == id);

            testItem1.ShouldNotBe(testItem2);
        }
    }
}