using System;
using System.Linq;
using Garcia.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Application
{
    public static class ApplicationServiceRegistrations
    {
        public static IServiceCollection AddLoggedInUserService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ILoggedInUserService>(provider =>
            {
                return new LoggedInUserService
                {
                    UserId = Convert.ToInt32(provider.GetService<IHttpContextAccessor>()?
                    .HttpContext?
                    .User
                    .Claims
                    .FirstOrDefault(x => x.Type == "id")?.Value)
                };
            });

            return services;
        }
    }
}
