using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garcia.Application;
using Garcia.Application.Services;
using Garcia.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Garcia.Infrastructure.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController<TService, TEntity, TDto, TKey> : ControllerBase
        where TService : IBaseService<TEntity, TDto, TKey>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
        where TDto : class
    {
        protected TService Service { get; }

        public BaseController(TService services)
        {
            Service = services;
        }

        [HttpGet]
        public virtual async Task<ActionResult<BaseResponse<IEnumerable<TDto>>>> GetAll()
        {
            var response = await Service.GetAllAsync();
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<BaseResponse<TDto>>> GetById(TKey id)
        {
            var response = await Service.GetByIdAsync(id);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPost]
        public virtual async Task<ActionResult<BaseResponse<long>>> Create([FromBody] TDto requestBody)
        {
            var response = await Service.AddAsync(Service.Mapper.Map<TEntity>(requestBody));
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<BaseResponse<long>>> Update(TKey id, [FromBody] TDto requestBody)
        {
            var response = await Service.UpdateAsync(id, requestBody);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<BaseResponse<long>>> Delete(TKey id)
        {
            var response = await Service.DeleteAsync(id);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }
    }
}
