using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using Garcia.Application.Contracts.Identity;
using Garcia.Domain.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Garcia.Infrastructure.Identity
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;
        protected SymmetricSecurityKey _signingKey;

        public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id),
            };

            claims.AddRange(identity.Claims);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id, List<string> roles)
        {
            var claims = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id)
            });

            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(Constants.Strings.JwtClaimIdentifiers.Role, role.Trim()));
            }

            return claims;
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        public async Task<TokenInfo> GenerateJwt(ClaimsIdentity identity, string userName)
        {
            var response = new TokenInfo
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                AuthToken = await GenerateEncodedToken(userName, identity),
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return response;
        }

        public async Task<TokenInfo> GenerateJwt(string userName, string userId, List<string> roles)
        {
            var claims = GenerateClaimsIdentity(userName, userId, roles);
            return await GenerateJwt(claims, userName);
        }

        public T GenerateRefreshToken<T>(string ip) where T : RefreshToken, new()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new()
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpirationDate = _jwtOptions.RefreshTokenOptions?.Expiration ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(7200)),
                CreatedByIp = ip
            };
        }

        public T RevokeRefreshToken<T>(T token, string ip) where T : RefreshToken
        {
            if (token.Revoked)
            {
                throw new ArgumentException("Refresh token is already revoked");
            }

            token.Revoked = true;
            token.RevokedByIp = ip;
            token.RevokedDate = DateTime.UtcNow;
            return token;
        }
    }
}
