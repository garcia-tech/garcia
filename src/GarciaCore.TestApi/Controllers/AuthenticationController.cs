using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GarciaCore.Infrastructure;
using GarciaCore.Domain;
using GarciaCore.Persistence;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using GarciaCore.Application.Contracts.Persistence;
using GarciaCore.Persistence.EntityFramework;
using GarciaCore.Application;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using GarciaCore.Application.Contracts.Identity;
using GarciaCore.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GarciaCore.TestApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService, IJwtService jwtService, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("api/Authentication/Token")]
        public async Task<IActionResult> PostToken([FromBody] CredentialsModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await _authenticationService.ValidateUser(credentials.UserName, credentials.Password);

            if (identity == null)
            {
                return BadRequest("invalid_grant");
            }

            var jwt = await _jwtService.GenerateJwt(identity.UserName, identity.Id, identity.Roles);
            return new OkObjectResult(jwt);
        }

        [HttpGet("api/Authentication/Claims")]
        public async Task<IEnumerable<ClaimsModel>> GetClaims()
        {
            return User.Claims.Select(x => new ClaimsModel(x.Type, x.Value));
        }
    }

    public class ClaimsModel
    {
        public ClaimsModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value{ get; set; }
    }

    public class LoggedInUserService : ILoggedInUserService
    {
        public int UserId { get; set; }
    }

    public class AuthenticationService : IAuthenticationService
    {
        public async Task<IUser> ValidateUser(string userName, string password)
        {
            return new UserModel()
            {
                UserName = userName,
                Id = "1",
                Roles = new List<string>()
                {
                    "User"
                }
            };
        }
    }

    public class UserModel : IUser
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class CredentialsModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

