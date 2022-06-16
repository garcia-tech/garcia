using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.Infrastructure;

namespace Garcia.Infrastructure.Identity
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

                configureOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string authorizationStringEquivalent = context.Request.Headers["Authorization"];

                        var authorizationHeader = authorizationStringEquivalent?.Split(' ');

                        var bearerToken = authorizationHeader?.Length > 1 ? authorizationHeader[1] : null;

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(bearerToken) &&
                            (path.StartsWithSegments("/hubs")))
                        {
                            context.Token = bearerToken;
                        }

                        return Task.CompletedTask;
                    }
                };
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
                options.ValidFor = new TimeSpan(0, configuration.GetSection(nameof(JwtIssuerOptions)).GetValue<int>(nameof(options.ValidFor)), 0);

                options.RefreshTokenOptions = new RefreshTokenOptions
                {
                    ValidFor = new TimeSpan(0, configuration.GetSection(nameof(JwtIssuerOptions) + ":" + nameof(options.RefreshTokenOptions)).GetValue<int>(nameof(options.RefreshTokenOptions.ValidFor)), 0)
                };

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

                configureOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string authorizationStringEquivalent = context.Request.Headers["Authorization"];

                        var authorizationHeader = authorizationStringEquivalent?.Split(' ');

                        var bearerToken = authorizationHeader?.Length > 1 ? authorizationHeader[1] : null;

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(bearerToken) &&
                            (path.StartsWithSegments("/hubs")))
                        {
                            context.Token = bearerToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Role, Constants.Strings.JwtClaimIdentifiers.Role));
            });

            return services;
        }

        public static IServiceCollection AddJwtOptions(this IServiceCollection services, string authenticationScheme, IConfiguration configuration)
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
            }).AddJwtBearer(authenticationScheme, configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration[$"{nameof(JwtIssuerOptions)}:{nameof(JwtIssuerOptions.Issuer)}"];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string authorizationStringEquivalent = context.Request.Headers["Authorization"];

                        var authorizationHeader = authorizationStringEquivalent?.Split(' ');

                        var bearerToken = authorizationHeader?.Length > 1 ? authorizationHeader[1] : null;

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(bearerToken) &&
                            (path.StartsWithSegments("/hubs")))
                        {
                            context.Token = bearerToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Role, Constants.Strings.JwtClaimIdentifiers.Role));
            });

            return services;
        }

        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {
            return services.AddScoped<IJwtService, JwtService>();
        }

        public static IServiceCollection AddEncryption(this IServiceCollection services)
        {
            return services.AddScoped<IEncryption, Encryption>();
        }
    }
}
