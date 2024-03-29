﻿using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Services;
using Garcia.Application.Tests.Services.Models;
using Garcia.Infrastructure;
using Garcia.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;
using Xunit;
using static Garcia.Application.Tests.Helpers;

namespace Garcia.Application.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAsyncRepository<TestUser, long>> _mockRepository;
        private readonly AuthenticationService<IAsyncRepository<TestUser, long>, TestUser, TestUserDto, long> _service;
        private readonly Mock<IOptions<JwtIssuerOptions>> _mockOptions = new Mock<IOptions<JwtIssuerOptions>>();

        public AuthServiceTests()
        {
            var options = new JwtIssuerOptions
            {
                Issuer = "garcia",
                Audience = "garcia",
                SecretKey = "8b95c3c301a54b39b7b9b4c612bc6844",
                ValidFor = new System.TimeSpan(0, 5, 0),
                RefreshTokenOptions = new RefreshTokenOptions
                {
                    ValidFor = new System.TimeSpan(0, 15, 0)
                }
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey));
            options.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            _mockOptions.Setup(x => x.Value).Returns(options);

            _mockRepository = InitializeRepositoryInstance("seckinpullu", "CY9rzUYh03PK3k6DJie09g==");

            _service = new(_mockRepository.Object, new Encryption(), new JwtService(_mockOptions.Object));
        }

        [Theory(DisplayName = "Login should be successful when entered username and password are correct")]
        [InlineData("seckinpullu", "test")]
        public async Task Login_Should_Success(string username, string password)
        {
            var request = new Credentials
            {
                Username = username,
                Password = password
            };

            var result = await _service.Login(request, "127.0.0.1");
            result.StatusCode.ShouldBe(200);
            result.Result.TokenInfo.AuthToken.ShouldNotBeNull();
            result.Result.TokenInfo.ExpiresIn.ShouldBeGreaterThan(0);
            result.Result.User.Id.ShouldNotBeNull();
        }

        [Theory(DisplayName = "Login should not be successful when entered username and password are invalid")]
        [InlineData("seckin", "test")]
        public async Task Login_Should_Failed(string username, string password)
        {
            var request = new Credentials
            {
                Username = username,
                Password = password
            };

            var result = await _service.Login(request, "127.0.0.1");
            result.StatusCode.ShouldBe(401);
        }

        [Theory(DisplayName = "ValidateUser should success when entered username and password are correct")]
        [InlineData("seckinpullu", "test")]
        public async Task ValidateUser_Should_Success(string username, string password)
        {
            var request = new Credentials
            {
                Username = username,
                Password = password
            };

            var result = await _service.ValidateUser(request);
            result.StatusCode.ShouldBe(200);
            result.Result.Id = "1";
        }

        [Theory(DisplayName = "Signup should success when custom signup request successfully entered")]
        [ClassData(typeof(SignupServiceTestData))]
        public async Task Singup_Should_Success(SignupServiceTestData request)
        {
            var result = await _service.Signup(request, "127.0.0.1");
            result.Success.ShouldBeTrue();
            result.Result.User.Email.ShouldBe(request.Email);
            result.Result.User.Username.ShouldBe(request.Username);
        }

        [Theory(DisplayName = "Signup should not be success when provided username has already been taken")]
        [ClassData(typeof(SignupServiceTestData))]
        public async Task Singup_Should_Not_Success(SignupServiceTestData request)
        {
            request.Username = "seckinpullu";
            var result = await _service.Signup(request, "127.0.0.1");
            result.Success.ShouldBeFalse();
            result.Status.ShouldBe(System.Net.HttpStatusCode.Conflict);
        }
    }
}
