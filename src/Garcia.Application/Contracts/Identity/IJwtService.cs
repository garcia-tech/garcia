using Garcia.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Identity
{
    public interface IJwtService
    {
        Task<string> GenerateJwt(string userName, string id, List<string> roles);
        T GenerateRefreshToken<T>(string id, string userId) where T : RefreshToken, new();
        T RevokeRefreshToken<T>(T token, string ip) where T : RefreshToken;
    }
}
