using System;
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

        private static readonly object _lock = new();

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
        public void GetByIdAsync()
        {
        }

        [Fact]
        public void GetAllAsync()
        {
        }

        [Fact]
        public void AddAsync()
        {
        }

        [Fact]
        public void UpdateAsync()
        {
        }

        [Fact]
        public void DeleteAsync()
        {
        }

        [Fact]
        public void GetAllAsync2()
        {
        }

        [Fact]
        public void GetByKeyAsync()
        {
        }

        [Fact]
        public void GetAsync()
        {
        }

        [Fact]
        public void GetAsync2()
        {
        }
    }
}