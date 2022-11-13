using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Identity.Services;
using Garcia.Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Identity
{
    public static class IdentityServiceRegistrations
    {
        public static IServiceCollection AddAuthenticationService<TRepository, TUser, TUserDto, TKey>(this IServiceCollection services, IConfiguration configuration)
            where TRepository : IAsyncRepository<TUser, TKey>
            where TKey : IEquatable<TKey>
            where TUser : class, IUserEntity<TKey>
            where TUserDto : class, IUser
        {
            return services
                .AddEncryption()
                .AddJwtService()
                .AddJwtOptions(configuration)
                .AddScoped<IAuthenticationService<TUser, TUserDto, TKey>,
                    AuthenticationService<TRepository, TUser, TUserDto, TKey>>();
        }

        public static IServiceCollection AddAuthenticationService<TRepository, TUser, TUserDto, TKey>(this IServiceCollection services, JwtIssuerOptions jwtOptions)
            where TRepository : IAsyncRepository<TUser, TKey>
            where TKey : IEquatable<TKey>
            where TUser : class, IUserEntity<TKey>
            where TUserDto : class, IUser
        {
            return services
                .AddEncryption()
                .AddJwtService()
                .AddJwtOptions(jwtOptions)
                .AddScoped<IAuthenticationService<TUser, TUserDto, TKey>,
                    AuthenticationService<TRepository, TUser, TUserDto, TKey>>();
        }
    }
}
