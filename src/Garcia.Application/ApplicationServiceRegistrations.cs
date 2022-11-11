using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Services;
using Garcia.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Garcia.Application
{
    public static class ApplicationServiceRegistrations
    {
        public static IServiceCollection AddBaseService<TRepository, TEntity, TDto, TKey>(this IServiceCollection serivces)
            where TRepository : IAsyncRepository<TEntity, TKey>
            where TKey : struct, IEquatable<TKey>
            where TEntity : Entity<TKey>
            where TDto : class
        {
            return serivces.AddScoped<IBaseService<TEntity, TDto, TKey>,
                BaseService<TRepository, TEntity, TDto, TKey>>();
        }
    }
}
