using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;

namespace Garcia.Application.Services
{
    /// <summary>
    /// Service base included basic crud methods.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TDto">Entity's data transfer object.</typeparam>
    /// <typeparam name="TKey">Entity id type.</typeparam>
    /// <typeparam name="TRepository">The repository of entity. It might be entityframework, mongodb or cassandra repository. Doesn't matter. It must be registered to service collection.</typeparam>
    public class BaseService<TRepository, TEntity, TDto, TKey> : IBaseService<TEntity, TDto, TKey>
        where TRepository : IAsyncRepository<TEntity, TKey>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
    {
        private readonly TRepository _repository;
        /// <summary>
        /// An AutoMapper's mapper instance. Provides mapping <typeparamref name="TEntity"/> to <typeparamref name="TDto"/> and reverse.
        /// </summary>
        public virtual IMapper Mapper { get; }

        public BaseService(TRepository repository)
        {
            _repository = repository;
            Mapper = InitializeMapper()
                .CreateMapper();
        }

        public virtual async Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var result = Mapper.Map<IEnumerable<TDto>>(entities);
            return new BaseResponse<IEnumerable<TDto>>(result);
        }

        public virtual async Task<BaseResponse<TDto>> GetByIdAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);
            var result = Mapper.Map<TDto>(entity);
            return new BaseResponse<TDto>(result);

        }

        public virtual async Task<BaseResponse<TKey>> AddAsync(TEntity entity)
        {
            var result = await _repository.AddAsync(entity);

            if (result == 0) return new BaseResponse<TKey>(default, System.Net.HttpStatusCode.UnprocessableEntity,
                new ApiError("Process Failed", "The entity cannot be inserted"));

            return new BaseResponse<TKey>(entity.Id, System.Net.HttpStatusCode.Created);
        }

        public virtual async Task<BaseResponse<TKey>> UpdateAsync(TKey id, object updateRequest)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new BaseResponse<TKey>(default,
                    System.Net.HttpStatusCode.NotFound,
                    new ApiError("Entity Not Found",
                    "The id entered does not match any entity"));
            }

            var result = await _repository.UpdateAsync(Helpers.BasicMap(entity, updateRequest));

            if (result == 0) return new BaseResponse<TKey>(default, System.Net.HttpStatusCode.UnprocessableEntity,
                new ApiError("Process Failed", "The entity cannot be updated"));

            return new BaseResponse<TKey>(entity.Id, System.Net.HttpStatusCode.OK);
        }

        public virtual async Task<BaseResponse<TKey>> DeleteAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new BaseResponse<TKey>(default,
                    System.Net.HttpStatusCode.NotFound,
                    new ApiError("Entity Not Found",
                    "The id entered does not match any entity"));
            }

            var result = await _repository.DeleteAsync(entity);

            if (result == 0) return new BaseResponse<TKey>(default, System.Net.HttpStatusCode.UnprocessableEntity,
                new ApiError("Process Failed", "The entity cannot be deleted"));

            return new BaseResponse<TKey>(entity.Id, System.Net.HttpStatusCode.OK);
        }

        protected virtual MapperConfiguration InitializeMapper() =>
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntity, TDto>()
                   .ReverseMap();
            });

    }

    public class BaseService<TRepository, TEntity, TDto> : BaseService<TRepository, TEntity, TDto, long>, IBaseService<TEntity, TDto>
        where TRepository : IAsyncRepository<TEntity, long>
        where TEntity : IEntity<long>
    {
        public BaseService(TRepository repository) : base(repository)
        {
        }
    }

    public class BaseService<TEntity, TDto> : BaseService<IAsyncRepository<TEntity>, TEntity, TDto>
        where TEntity : Entity<long>
    {
        public BaseService(IAsyncRepository<TEntity> repository) : base(repository)
        {
        }
    }
}
