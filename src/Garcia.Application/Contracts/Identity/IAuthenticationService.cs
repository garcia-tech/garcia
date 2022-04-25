using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<IUser> ValidateUser(string userName, string password);
    }
}
