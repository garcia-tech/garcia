using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garcia.Infrastructure.Api.Middlewares.Security
{
    public class CacheControlHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting((state) =>
            {
                context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                context.Response.Headers.Append("Pragma", "no-cache");
                context.Response.Headers.Append("Expires", "0");
                return Task.FromResult(0);
            }, null);
            await next(context);
        }
    }
}
