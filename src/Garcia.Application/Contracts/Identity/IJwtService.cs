using System.Collections.Generic;
using System.Threading.Tasks;
using Garcia.Domain.Identity;

namespace Garcia.Application.Contracts.Identity
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates a json web token using the options in the JwtIssuerOptions
        /// </summary>
        /// <param name="userName">Requesting username.</param>
        /// <param name="id">Requesting user id.</param>
        /// <param name="roles">Requesting user roles.</param>
        /// <returns><see cref="TokenInfo"/></returns>
        Task<TokenInfo> GenerateJwt(string userName, string id, List<string> roles);
        /// <summary>
        /// Generates a <typeparamref name="T"/> type refresh token. 
        /// Generated refresh token is not an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ip">Created by ip.</param>
        /// <returns></returns>
        T GenerateRefreshToken<T>(string ip) where T : RefreshToken, new();
        /// <summary>
        /// Revokes related refresh token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">Refresh token to be revoked.</param>
        /// <param name="ip">Revoked by ip.</param>
        /// <returns></returns>
        T RevokeRefreshToken<T>(T token, string ip) where T : RefreshToken;
    }
}
