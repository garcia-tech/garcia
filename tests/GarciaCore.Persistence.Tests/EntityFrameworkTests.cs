using System;
using System.Threading;
using System.Threading.Tasks;
using GarciaCore.Domain;
using GarciaCore.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GarciaCore.Persistence.Tests;

public class EntityFrameworkTestFixture
{
    private const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

    private EntityFrameworkRepository<TestEntity> _repository );

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public EntityFrameworkTestFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.AddRange(
                        new TestEntity(1, "One"),
                        new TestEntity(2, "Two"));
                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public DbContext CreateContext()
        => new EntityFrameworkTestsContext(
            new DbContextOptionsBuilder<EntityFrameworkTestsContext>()
                .UseSqlServer(ConnectionString)
                .Options);
}