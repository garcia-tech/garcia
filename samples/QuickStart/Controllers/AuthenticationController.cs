using Garcia.Application.Identity.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
public class AuthenticationController : BaseAuthController<IAuthenticationService<User, UserDto, long>, User, UserDto, long>
{
    public AuthenticationController(IAuthenticationService<User, UserDto, long> service) : base(service)
    {
    }
}

