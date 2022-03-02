using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarciaCore.Domain;
using GarciaCore.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GarciaCore.Persistence.Tests
{
    public class EntityFrameworkTestFixture
    {
        private const string ConnectionString =
            @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

        private EntityFrameworkRepository<TestEntity> _repository;

        public EntityFrameworkTestFixture()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.AddRange(
                    new TestEntity(1, "One"),
                    new TestEntity(2, "Two"));
                context.SaveChanges();
                _repository = new EntityFrameworkRepository<TestEntity>(CreateContext());
            }
        }

        public DbContext CreateContext()
            => new EntityFrameworkTestsContext(
                new DbContextOptionsBuilder<EntityFrameworkTestsContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        [Fact]
        public async void GetByIdAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            Assert.NotNull(item);
            item = await _repository.GetByIdAsync(3);
            Assert.Null(item);
        }

        [Fact]
        public async void GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            Assert.NotNull(items);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async void AddAsync()
        {
            var item = new TestEntity(3, "Three");
            var result = await _repository.AddAsync(item);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void UpdateAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            item.Name = $"One-{DateTime.Now}";
            await _repository.UpdateAsync(item);
            var existingItem = await _repository.GetByIdAsync(1);
            Assert.Equal(item.Name, existingItem.Name);
        }

        [Fact]
        public async void DeleteAsync()
        {
            var item = await _repository.GetByIdAsync(1);
            await _repository.DeleteAsync(item);
            item = await _repository.GetByIdAsync(1);
            Assert.Null(item);

            item = await _repository.GetByIdAsync(2);
            item.IsDeleted = true;
            await _repository.SaveAsync(item);
            item = await _repository.GetByIdAsync(2);
            Assert.Null(item);
        }

        [Fact]
        public async void GetAllAsync2()
        {
            var items = await _repository.GetAllAsync(1, 1);
            Assert.NotNull(items);
            Assert.Equal(1, items.Count);
            items = await _repository.GetAllAsync(1, 2);
            Assert.NotNull(items);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async void GetAsync()
        {
            var items = await _repository.GetAsync(x => x.Id == 1);
            Assert.NotNull(items);
            Assert.Equal(1, items.Count);
            items = await _repository.GetAsync(x => x.Id == 3);
            Assert.NotNull(items);
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public async void AddRangeAsync()
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
        public async void DeleteManyAsync()
        {
            await _repository.DeleteManyAsync(x => x.Key > 4);
            var entities = await _repository.GetAllAsync();
            Assert.DoesNotContain(entities, x => x.Key > 4);
        }
    }
}