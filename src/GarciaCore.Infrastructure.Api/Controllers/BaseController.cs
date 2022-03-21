using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarciaCore.Application;
using GarciaCore.Application.Services;
using GarciaCore.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GarciaCore.Infrastructure.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController<TService, TEntity, TDto, TKey> : ControllerBase
        where TService : IBaseService<TEntity, TDto, TKey>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
        where TDto : class
    {
        private readonly TService _service;

        public BaseController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<BaseResponse<IEnumerable<TDto>>>> GetAll()
        {
            var response = await _service.GetAllAsync();
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<BaseResponse<TDto>>> GetById(TKey id)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPost]
        public virtual async Task<ActionResult<BaseResponse<long>>> Create([FromBody] TDto requestBody)
        {
            var response = await _service.AddAsync(_service.Mapper.Map<TEntity>(requestBody));
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<BaseResponse<long>>> Update(TKey id, [FromBody] TDto requestBody)
        {
            var response = await _service.UpdateAsync(id, requestBody);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<BaseResponse<long>>> Delete(TKey id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }
    }
}
