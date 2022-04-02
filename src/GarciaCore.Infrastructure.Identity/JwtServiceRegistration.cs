using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace GarciaCore.Infrastructure.Identity
{
    public static class JwtServiceRegistration
    {
        public static IServiceCollection AddJwtOptions(this IServiceCollection services, JwtIssuerOptions jwtOptions)
        {
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey));
            
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions.Issuer;
                options.Audience = jwtOptions.Audience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = jwtOptions.ValidFor;
                options.RefreshTokenOptions = jwtOptions.RefreshTokenOptions;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtOptions.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Role, Constants.Strings.JwtClaimIdentifiers.Role));
            });

            return services;
        }

        public static IServiceCollection AddJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration[$"{nameof(JwtIssuerOptions)}:{nameof(JwtIssuerOptions.SecretKey)}"]));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(options.Issuer)}"];
                options.Audience = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(options.Audience)}"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                options.ValidFor = configuration.GetSection(nameof(JwtIssuerOptions)).GetValue<TimeSpan>(nameof(options.ValidFor));
                options.RefreshTokenOptions = configuration.GetSection(nameof(JwtIssuerOptions)).GetValue<RefreshTokenOptions>(nameof(options.RefreshTokenOptions));
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(JwtIssuerOptions.Issuer)}"],
                ValidateAudience = true,
                ValidAudience = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(JwtIssuerOptions.Audience)}"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(JwtIssuerOptions.Issuer)}"];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Role, Constants.Strings.JwtClaimIdentifiers.Role));
            });

            return services;
        }
    }
}
