using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garcia.Infrastructure.Api.Middlewares.Security
{
    public class ResponseHeadersHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            await next(context);
        }
    }
}
