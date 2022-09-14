using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Api.Middlewares.Security
{
    public static class SecurityHandlerRegistrations
    {
        public static IServiceCollection AddResponseHeadersHandler(this IServiceCollection services)
        {
            return services.AddTransient<ResponseHeadersHandler>();
        }

        public static IServiceCollection AddCacheControlHandler(this IServiceCollection services)
        {
            return services.AddTransient<CacheControlHandler>();
        }

        public static IApplicationBuilder UseSecurityHandlers(this IApplicationBuilder builder)
        {
            return
                builder
                .UseMiddleware<ResponseHeadersHandler>()
                .UseCookiePolicy(new CookiePolicyOptions
                {
                    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                    Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always
                });
        }

        public static IApplicationBuilder UseCacheControlHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheControlHandler>();
        }
    }
}
