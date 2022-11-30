using Garcia.Application.Identity.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
public class AuthenticationController : BaseAuthController<User, UserDto>
{
    public AuthenticationController(IAuthenticationService<User, UserDto, long> service) : base(service)
    {
    }
}

