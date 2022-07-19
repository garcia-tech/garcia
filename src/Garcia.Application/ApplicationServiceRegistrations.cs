using System;
using System.Linq;
using Garcia.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Application
{
    public static class ApplicationServiceRegistrations
    {
        public static IServiceCollection AddLoggedInUserService<TKey>(this IServiceCollection services)
            where TKey : IEquatable<TKey>
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ILoggedInUserService<TKey>>(provider =>
            {
                var loggedInUser = new LoggedInUserService<TKey>();
                loggedInUser.UserId = loggedInUser.ConvertToId(
                    provider.GetService<IHttpContextAccessor>()?
                    .HttpContext?
                    .User
                    .Claims
                    .FirstOrDefault(x => x.Type == "id")?.Value);
                return loggedInUser;
            });

            return services;
        }
    }
}
