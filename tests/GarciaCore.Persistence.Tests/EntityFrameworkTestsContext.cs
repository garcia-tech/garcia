using GarciaCore.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace GarciaCore.Persistence.Tests;

public class EntityFrameworkTestsContext : BaseContext
{
    public EntityFrameworkTestsContext(DbContextOptions<EntityFrameworkTestsContext> options) : base(options, null)
    {
    }
    
    public DbSet<TestEntity> TestEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityFrameworkTestsContext).Assembly);
        modelBuilder.Entity<TestEntity>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}