using Garcia.Application.Contracts.Identity;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Models.Response;
using Garcia.Domain;

namespace Garcia.Application.Identity.Services
{
    public interface IAuthenticationService<TUser, TUserDto, TKey>
        where TKey : IEquatable<TKey>
        where TUser : Entity<TKey>
        where TUserDto : IUser
    {
        Task<BaseResponse<TUserDto>> ValidateUser(Credentials request);
        Task<BaseResponse<LoginResponse<TUserDto>>> Login(Credentials request, string ip);
    }
}
