﻿using System;
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
        where TUser : IUserEntity<TKey>
        where TKey : IEquatable<TKey>
        where TUserDto : IUser
    {
        protected TService Service { get; }

        public BaseAuthController(TService service)
        {
            Service = service;
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

            var response = await Service.ValidateUser(request);
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPost("login")]
        public virtual async Task<ActionResult<BaseResponse<LoginResponse<TUserDto>>>> Login([FromBody] Credentials request)
        {
            if (!ModelState.IsValid)
            {
                var error = new ApiError();
                error.SetStatusCode(System.Net.HttpStatusCode.BadRequest);
                return StatusCode(400, error);
            }

            var response = await Service.Login(request, GetIpAddress());
            return StatusCode(
                response.StatusCode,
                response.Success ? response.Result : response.Error);
        }

        [HttpPost("signup")]
        public virtual async Task<ActionResult<BaseResponse<LoginResponse<TUserDto>>>> Signup([FromBody] Credentials request)
        {
            if (!ModelState.IsValid)
            {
                var error = new ApiError();
                error.SetStatusCode(System.Net.HttpStatusCode.BadRequest);
                return StatusCode(400, error);
            }

            var response = await Service.Signup(request, GetIpAddress());
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

    [Route("api/[controller]")]
    public class BaseAuthController<TUser, TUserDto> : BaseAuthController<IAuthenticationService<TUser, TUserDto, long>, TUser, TUserDto, long>
        where TUser : IUserEntity<long>
        where TUserDto : IUser

    {
        public BaseAuthController(IAuthenticationService<TUser, TUserDto, long> service) : base(service)
        {
        }
    }
}
