using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Garcia.Domain;

namespace Garcia.Application.Services
{
    /// <summary>
    /// Service base included basic crud methods.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TDto">Entity's data transfer object.</typeparam>
    /// <typeparam name="TKey">Entity id type.</typeparam>
    public interface IBaseService<TEntity, TDto, TKey>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Finds the <typeparamref name="TEntity"/> by id and gets it as a <typeparamref name="TDto"/>.
        /// </summary>
        /// <param name="id"></param>
        Task<BaseResponse<TDto>> GetByIdAsync(TKey id);
        /// <summary>
        /// Gets all records of <typeparamref name="TEntity"/> and returns it as a <typeparamref name="TDto"/>
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync();
        /// <summary>
        /// Inserts a new record of <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BaseResponse<TKey>> AddAsync(TEntity entity);
        /// <summary>
        /// Finds the <typeparamref name="TEntity"/> by id and updates it.
        /// The <paramref name="updateRequest"/> is mapped to the relevant fields of the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<TKey>> UpdateAsync(TKey id, object updateRequest);
        /// <summary>
        /// Deletes the <typeparamref name="TEntity"/> by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<TKey>> DeleteAsync(TKey id);
        IMapper Mapper { get; }
    }

    public interface IBaseService<TEntity, TDto> : IBaseService<TEntity, TDto, long>
        where TEntity : IEntity<long> 
    { }
}
