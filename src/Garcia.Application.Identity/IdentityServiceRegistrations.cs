using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Identity.Services;
using Garcia.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Application.Identity
{
    public static class IdentityServiceRegistrations
    {
        public static IServiceCollection AddAuthenticationService<TRepository, TUser, TUserDto, TKey>(this IServiceCollection serivces)
            where TRepository : IAsyncRepository<TUser, TKey>
            where TKey : IEquatable<TKey>
            where TUser : class, IUserEntity<TKey>
            where TUserDto : class, IUser
        {
            return serivces.AddScoped<IAuthenticationService<TUser, TUserDto, TKey>, 
                AuthenticationService<TRepository, TUser, TUserDto, TKey>>();
        }
    }
}
