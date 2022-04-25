using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Garcia.Application;
using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;

namespace Garcia.Application.Services
{
    public interface IBaseService<TEntity, TDto, TKey>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
    {
        Task<BaseResponse<TDto>> GetByIdAsync(TKey id);
        Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync();
        Task<BaseResponse<long>> AddAsync(TEntity entity);
        Task<BaseResponse<long>> UpdateAsync(TKey id, object updateRequest);
        Task<BaseResponse<long>> DeleteAsync(TKey id);
        IMapper Mapper { get; }
    }
}
