using System;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Services;
using Garcia.Application.Services;
using Garcia.Domain;
using Garcia.Domain.Identity;
using Garcia.Infrastructure.Api.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Garcia.Infrastructure.Api.Extentions
{
    public static class EndpointRouteBuilderExtentions
    {
        public static IEndpointRouteBuilder CreateCrudApi<TEntity, TDto, TKey>(this IEndpointRouteBuilder endpointRoute, bool authorizationRequired = false)
            where TEntity : Entity<TKey>
            where TDto : class, new()
            where TKey : struct, IEquatable<TKey>
        {

            var mapper = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntity, TDto>()
                    .ReverseMap();
            }).CreateMapper();

            var endpointRouteName = typeof(TEntity).Name;

            endpointRoute.MapGet(endpointRouteName + "/{id}", async (TKey id, IBaseService<TEntity, TDto, TKey> service) =>
            {
                var result = await service.GetByIdAsync(id);

                if (result.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result.Result);
            }).Authorization(authorizationRequired);

            endpointRoute.MapGet(endpointRouteName, async (IBaseService<TEntity, TDto, TKey> service) =>
            {
                var result = await service.GetAllAsync();
                return Results.Ok(result.Result);
            }).Authorization(authorizationRequired);

            endpointRoute.MapPost(endpointRouteName, async (TDto request, IBaseService<TEntity, TDto, TKey> service) =>
            {
                var data = mapper.Map<TEntity>(request);
                await service.AddAsync(data);
                return Results.Created(endpointRouteName + $"/{data.Id}", mapper.Map<TDto>(data));
            }).Authorization(authorizationRequired);

            endpointRoute.MapPut(endpointRouteName + "/{id}", async (TKey id, TDto request, IBaseService<TEntity, TDto, TKey> service) =>
            {

                var result = await service.UpdateAsync(id, request);

                if (result.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.NoContent();
            }).Authorization(authorizationRequired);

            endpointRoute.MapDelete(endpointRouteName + "/{id}", async (TKey id, IBaseService<TEntity, TDto, TKey> service) =>
            {
                var result = await service.DeleteAsync(id);

                if (result.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.NoContent();
            }).Authorization(authorizationRequired);

            return endpointRoute;
        }

        public static IEndpointRouteBuilder CreateCrudApi<TEntity, TDto>(this IEndpointRouteBuilder endpointRoute, bool authorizationRequired = false)
            where TEntity : Entity<long>
            where TDto : class, new()
        {
            return endpointRoute.CreateCrudApi<TEntity, TDto, long>(authorizationRequired);
        }

        public static IEndpointRouteBuilder CreateAuthApi<TUser, TUserDto, TKey>(this IEndpointRouteBuilder endpointRoute)
            where TUser : IUserEntity<TKey>
            where TKey : IEquatable<TKey>
            where TUserDto : IUser
        {

            endpointRoute.MapPost("authentication/validate", async (Credentials request, IAuthenticationService<TUser, TUserDto, TKey> service) =>
            {
                var result = await service.ValidateUser(request);

                if (result.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result.Result);
            }).AllowAnonymous();

            endpointRoute.MapPost("authentication/signup", async (Credentials request, HttpContext context, IAuthenticationService<TUser, TUserDto, TKey> service) =>
            {
                var ipAddress = (string)context.Request.Headers["X-Forwarded-For"] ?? context.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                var result = await service.Signup(request, ipAddress);

                if (result.Status == System.Net.HttpStatusCode.Conflict)
                {
                    return Results.Conflict(result.Error);
                }

                return Results.Ok(result.Result);
            }).AllowAnonymous();

            endpointRoute.MapPost("authentication/login", async (Credentials request, HttpContext context, IAuthenticationService<TUser, TUserDto, TKey> service) =>
            {
                var ipAddress = (string)context.Request.Headers["X-Forwarded-For"] ?? context.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                var result = await service.Login(request, ipAddress);

                if (result.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(result.Result);
            }).AllowAnonymous();

            return endpointRoute;
        }

        public static IEndpointRouteBuilder CreateAuthApi<TUser, TUserDto>(this IEndpointRouteBuilder endpointRoute)
            where TUser : IUserEntity<long>
            where TUserDto : IUser
        {
            return endpointRoute.CreateAuthApi<TUser, TUserDto>();
        }

        private static RouteHandlerBuilder Authorization(this RouteHandlerBuilder routeHandlerBuilder, bool authorizationRequired)
        {
            return authorizationRequired ? routeHandlerBuilder.RequireAuthorization(new AuthorizeData { AuthenticationSchemes = "Bearer" }) : routeHandlerBuilder;
        }
    }
}
