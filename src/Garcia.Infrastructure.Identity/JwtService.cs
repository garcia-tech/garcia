using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Garcia.Domain.Identity;
using System.Security.Cryptography;
using Garcia.Application.Contracts.Identity;

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

            claims.AddRange(identity.Claims.Where(x => x.Type == Constants.Strings.JwtClaimIdentifiers.Role));

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

        public async Task<string> GenerateJwt(ClaimsIdentity identity, string userName)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await GenerateEncodedToken(userName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return JsonSerializer.Serialize(response);
        }

        public async Task<string> GenerateJwt(string userName, string userId, List<string> roles)
        {
            var claims = GenerateClaimsIdentity(userName, userId, roles);
            return await GenerateJwt(claims, userName);
        }

        public T GenerateRefreshToken<T>(string ip, string userId) where T : RefreshToken, new()
        {
            var randomBytes = new byte[64];
            var refreshToken = Convert.ToBase64String(randomBytes);

            return new()
            {
                Token = refreshToken,
                ExpirationDate = _jwtOptions.RefreshTokenOptions.Expiration,
                UserId = userId,
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
