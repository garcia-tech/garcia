using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Garcia.Infrastructure.Identity
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public DateTime NotBefore => DateTime.UtcNow;
        public DateTime IssuedAt => DateTime.UtcNow;
        public TimeSpan ValidFor { get; set; }
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials SigningCredentials { get; set; }
        public string SecretKey { get; set; }
        public RefreshTokenOptions? RefreshTokenOptions { get; set; } = default;
    }
}
