using System;
using System.Threading.Tasks;
using Garcia.Application;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Models.Response;
using Garcia.Application.Identity.Services;
using Garcia.Domain.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Garcia.Infrastructure.Api.Controllers
{
    [Route("api/[controller]")]
    public class BaseAuthController<TService, TUser, TUserDto, TKey> : ControllerBase
        where TService : IAuthenticationService<TUser, TUserDto, TKey>
        where TUser : User<TKey>
        where TKey : IEquatable<TKey>
        where TUserDto : IUser
    {
        private readonly TService _service;

        public BaseAuthController(TService service)
        {
            _service = service;
        }

        [HttpPost("validate")]
        public virtual async Task<ActionResult<BaseResponse<TUserDto>>> ValidateUser([FromBody] Credentials request)
        {
            if (!ModelState.IsValid)
            {
                var error = new ApiError();
                error.SetStatusCode(System.Net.HttpStatusCode.BadRequest);
                return StatusCode(400, error);
            }

            var response = await _service.ValidateUser(request);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPost("login")]
        public virtual async Task<ActionResult<BaseResponse<LoginResponse<TUserDto>>>> Login([FromBody] Credentials request)
        {
            if(!ModelState.IsValid)
            {
                var error = new ApiError();
                error.SetStatusCode(System.Net.HttpStatusCode.BadRequest);
                return StatusCode(400, error);
            }

            var response = await _service.Login(request, GetIpAddress());
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        protected virtual string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }

            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
