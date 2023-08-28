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
using System.Linq.Expressions;

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

        public IQueryable Set(Type T)
        {
            MethodInfo method = typeof(DbContext).GetMethods().Where(x => x.Name == "Set").FirstOrDefault(x => x.IsGenericMethod);
            method = method?.MakeGenericMethod(T);
            return method?.Invoke(this, null) as IQueryable;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entities = ResolveEntities();

            foreach (var entity in entities)
            {
                modelBuilder.Entity(entity.GetTypeInfo()).HasKey("Id");
            }

            ApplySoftDeleteQueryFilter(modelBuilder);
        }

        protected virtual IEnumerable<TypeInfo> ResolveEntities()
        {
            var assemblies = Assembly.GetEntryAssembly()?
                .GetReferencedAssemblies()
                .Where(x => !x.FullName.Contains("System")
                    && !x.FullName.Contains("Microsoft")
                    && !x.FullName.Contains("Garcia"))
                .Select(x => Assembly.Load(x.FullName)) ?? new List<Assembly>();
            assemblies = assemblies?.Append(Assembly.GetEntryAssembly());

            return assemblies?
                .SelectMany(x => x.DefinedTypes)
                    .Where(x => x.BaseType == typeof(Entity<long>) || x.BaseType == typeof(Entity<int>)) ?? new List<TypeInfo>();
        }

        protected virtual void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var isActiveProperty = entityType.FindProperty("Deleted");

                if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
                {
                    var entityBuilder = modelBuilder.Entity(entityType.ClrType);
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var methodInfo = typeof(EF).GetMethod(nameof(EF.Property))!.MakeGenericMethod(typeof(bool))!;
                    var efPropertyCall = Expression.Call(null, methodInfo, parameter, Expression.Constant("Deleted"));
                    var body = Expression.MakeBinary(ExpressionType.Equal, efPropertyCall, Expression.Constant(false));
                    var expression = Expression.Lambda(body, parameter);
                    entityBuilder.HasQueryFilter(expression);
                }
            }
        }
    }
}