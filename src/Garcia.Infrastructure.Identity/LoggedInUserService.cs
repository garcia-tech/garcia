using System.IdentityModel.Tokens.Jwt;
using Garcia.Application;
using Garcia.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;

namespace Garcia.Infrastructure.Identity
{
    public class LoggedInUserService<TKey> : ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey UserId { get; set; }
        public LoggedInUser LoggedInUser { get; set; }

        public LoggedInUserService(IHttpContextAccessor contextAccessor)
        {
            UserId = ConvertToId(contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == Constants.Strings.JwtClaimIdentifiers.Id)?.Value ?? "0");

            LoggedInUser = new LoggedInUser
            {
                Id = contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == Constants.Strings.JwtClaimIdentifiers.Id)?.Value ?? default,
                Username = contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? default,
                Roles = contextAccessor.HttpContext?.User.Claims.Where(x => x.Type == Constants.Strings.JwtClaimIdentifiers.Role).Select(x => x.Value).ToList() ?? default,
                IpAddress = GetIpAddress(contextAccessor.HttpContext)
            };
        }

        private TKey ConvertToId(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            return (TKey)Convert.ChangeType(value, typeof(TKey));
        }

        private string GetIpAddress(HttpContext context)
        {
            if(context == null)
                return string.Empty;

            return context.Request.Headers.ContainsKey("X-Forwarded-For") ?
                context.Request.Headers["X-Forwarded-For"] :
            context.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}
