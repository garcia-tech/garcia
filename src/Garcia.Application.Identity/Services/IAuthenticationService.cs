using Garcia.Application.Contracts.Identity;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Models.Response;
using Garcia.Domain;
using Garcia.Domain.Identity;

namespace Garcia.Application.Identity.Services
{
    public interface IAuthenticationService<TUser, TUserDto, TKey>
        where TUser : IUserEntity<TKey>
        where TUserDto : IUser
    {
        Task<BaseResponse<TUserDto>> ValidateUser(Credentials request);
        Task<BaseResponse<LoginResponse<TUserDto>>> Login(Credentials request, string ip);
    }
}
