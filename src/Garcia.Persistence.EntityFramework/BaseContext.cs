using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Identity;
using Garcia.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Garcia.Persistence.EntityFramework
{
    public class BaseContext : DbContext
    {
        private readonly ILoggedInUserService<long> _loggedInUserService;
        private readonly IMediator _mediator;

        public BaseContext(DbContextOptions options, ILoggedInUserService<long> loggedInUserService) : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        public BaseContext(DbContextOptions options, ILoggedInUserService<long> loggedInUserService, IMediator mediator) : base(options)
        {
            _loggedInUserService = loggedInUserService;
            _mediator = mediator;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<Entity<long>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        entry.Entity.CreatedBy = _loggedInUserService?.UserId != null ?
                            _loggedInUserService!.UserId : default;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastUpdatedOn = DateTimeOffset.UtcNow;
                        entry.Entity.LastUpdatedBy = _loggedInUserService?.UserId != null ?
                            _loggedInUserService!.UserId : default;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedOn = DateTimeOffset.UtcNow;
                        entry.Entity.DeletedBy = _loggedInUserService?.UserId != null ?
                            _loggedInUserService!.UserId : default;
                        break;
                }

                if (_mediator != null && entry.Entity.DomainEvents.Count > 0)
                {
                    _ = entry.Entity.PublishDomainEvents(_mediator, cancellationToken);
                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entities = ResolveEntities();

            foreach (var entity in entities)
            {
                modelBuilder.Entity(entity.GetTypeInfo()).HasKey("Id");
            }
        }

        protected virtual IEnumerable<TypeInfo> ResolveEntities()
        {
            var assemblies = Assembly.GetEntryAssembly()?
                .GetReferencedAssemblies()
                .Where(x => !x.FullName.Contains("System")
                    && !x.FullName.Contains("Microsoft")
                    && !x.FullName.Contains("Garcia"))
                .Select(x => Assembly.Load(x.FullName));
            assemblies?.Append(Assembly.GetEntryAssembly());
            assemblies?.Append(Assembly.GetExecutingAssembly());

            return assemblies?
                .SelectMany(x => x.DefinedTypes)
                    .Where(x => x.BaseType == typeof(Entity<long>) || x.BaseType == typeof(Entity<int>)) ?? new List<TypeInfo>();
        }
    }
}