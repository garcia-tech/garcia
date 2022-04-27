using Garcia.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Identity
{
    public interface IJwtService
    {
        Task<TokenInfo> GenerateJwt(string userName, string id, List<string> roles);
        T GenerateRefreshToken<T>(string ip) where T : RefreshToken, new();
        T RevokeRefreshToken<T>(T token, string ip) where T : RefreshToken;
    }
}
