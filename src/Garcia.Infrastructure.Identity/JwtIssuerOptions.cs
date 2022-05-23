using Microsoft.IdentityModel.Tokens;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Garcia.Infrastructure.Identity
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        [JsonIgnore]
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        [JsonIgnore]
        public DateTime NotBefore => DateTime.UtcNow;
        [JsonIgnore]
        public DateTime IssuedAt => DateTime.UtcNow;
        public TimeSpan ValidFor { get; set; }
        [JsonIgnore]
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        [JsonIgnore]
        public SigningCredentials SigningCredentials { get; set; }
        [JsonIgnore]
        public string SecretKey { get; set; }
        public RefreshTokenOptions? RefreshTokenOptions { get; set; } = default;
    }
}
