using Garcia.Application.Contracts.Identity;
using Garcia.Domain.Identity;

namespace Garcia.Application.Identity.Models.Response
{
    public class LoginResponse<TUserDto>
        where TUserDto : IUser
    {
        public TUserDto User { get; set; }
        public TokenInfo TokenInfo { get; set; }

    }
}
