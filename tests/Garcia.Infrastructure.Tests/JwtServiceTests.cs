using System.Linq;
using System.Text;
using Garcia.Domain.Identity;
using Garcia.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;
using Xunit;

namespace Garcia.Infrastructure.Tests
{
    public class JwtServiceTests
    {
        private readonly JwtService _service;

        public JwtServiceTests()
        {
            var mockOptions = new Mock<IOptions<JwtIssuerOptions>>();
            SymmetricSecurityKey signingKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("8b95c3c301a54b39b7b9b4c612bc6844"));

            mockOptions.Setup(x => x.Value)
                .Returns(new JwtIssuerOptions
                {
                    Audience = "garcia",
                    Issuer = "garcia",
                    SecretKey = "garcia123321",
                    ValidFor = new System.TimeSpan(0, 15, 0),
                    RefreshTokenOptions = new RefreshTokenOptions
                    {
                        ValidFor = new System.TimeSpan(0, 7200, 0)
                    },

                    SigningCredentials = new SigningCredentials(signingKey, "HS256")
                });

            _service = new JwtService(mockOptions.Object);
        }

        [Fact]
        public void GenerateRefreshToken_Should_Generate_Valid_Token()
        {
            var staticIp = "88.243.149.20";
            var refreshToken = _service.GenerateRefreshToken<RefreshToken>(staticIp);
            var totalCharCount = refreshToken.Token.Length;

            var groupCount = refreshToken.Token.GroupBy(x => x)
                .Select(x => x.ToList())
                .Count();

            var compare = (totalCharCount - groupCount) > 2;
            compare.ShouldBeTrue();
        }
    }
}
