using Garcia.Application.Contracts.Identity;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Models.Response;
using Garcia.Domain.Identity;

namespace Garcia.Application.Identity.Services
{
    public interface IAuthenticationService<TUser, TUserDto, TKey>
        where TUser : IUserEntity<TKey>
        where TUserDto : IUser
    {
        /// <summary>
        /// Validates user via provided credentials.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>If user exists returns <typeparamref name="TUserDto"/>; otherwise <see cref="System.Net.HttpStatusCode.NotFound"/>.</returns>
        Task<BaseResponse<TUserDto>> ValidateUser(Credentials request);
        /// <summary>
        /// Logs in and returns <see cref="LoginResponse{TUserDto}"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ip"></param>
        /// <returns>Returns <see cref="LoginResponse{TUserDto}"/> if the credentials matches; otherwise <see cref="System.Net.HttpStatusCode.Unauthorized"/>.</returns>
        Task<BaseResponse<LoginResponse<TUserDto>>> Login(Credentials request, string ip);
        /// <summary>
        /// Creates new a <typeparamref name="TUser"/>.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="ip"></param>
        /// <returns>Returns <see cref="LoginResponse{TUserDto}"/> if provided <paramref name="request"/>'s Username is unique; otherwise <see cref="System.Net.HttpStatusCode.Conflict"/>.</returns>
        Task<BaseResponse<LoginResponse<TUserDto>>> Signup<TRequest>(TRequest request, string ip) where TRequest : Credentials;
    }
}
