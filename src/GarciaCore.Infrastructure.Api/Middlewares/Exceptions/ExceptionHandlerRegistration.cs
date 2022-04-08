using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using GarciaCore.Application;

namespace GarciaCore.Infrastructure.Api.Middlewares.Exceptions
{
    public static class ExceptionHandlerRegistration
    {
        public static IServiceCollection AddExceptionHandlingOptions(this IServiceCollection services, Action<ExceptionHandlingOptions> options)
        {
            return services.Configure(options);
        }
        public static IApplicationBuilder UseGarciaExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler<ApiError>>();
        }

        public static IApplicationBuilder UseGarciaExceptionHandler<THandler>(this IApplicationBuilder builder)
            where THandler : ExceptionHandler<ApiError>
        {
            return builder.UseMiddleware<THandler>();
        }

        public static IApplicationBuilder UseGarciaExceptionHandler<THandler, TErrorModel>(this IApplicationBuilder builder)
            where TErrorModel : ApiError, new()
            where THandler : ExceptionHandler<TErrorModel>
        {
            return builder.UseMiddleware<THandler>();
        }
    }
}
