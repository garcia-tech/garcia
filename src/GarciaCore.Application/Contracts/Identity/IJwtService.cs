using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.Application.Contracts.Identity
{
    public interface IJwtService
    {
        Task<string> GenerateJwt(string userName, string id, List<string> roles);
    }
}
