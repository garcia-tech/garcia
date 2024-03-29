﻿using Garcia.Application.Contracts.Marketing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Marketing.MailChimp
{
    public static class MailChimpMarketingServiceRegistration
    {
        public static IServiceCollection AddMailChimpMarketingService(this IServiceCollection services, MailChimpMarketingSettings settings)
        {
            services.Configure<MailChimpMarketingSettings>(options =>
            {
                options.ApiKey = settings.ApiKey;
                options.AudienceListId = settings.AudienceListId;
            });

            services.AddScoped<IMarketingService, MailChimpMarketingService>();
            return services;
        }

        public static IServiceCollection AddMailChimpMarketingService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailChimpMarketingSettings>(options =>
            {
                options.ApiKey = configuration[$"{nameof(MailChimpMarketingSettings)}:{nameof(options.ApiKey)}"];
                options.AudienceListId = configuration[$"{nameof(MailChimpMarketingSettings)}:{nameof(options.AudienceListId)}"];
            });

            services.AddScoped<IMarketingService, MailChimpMarketingService>();
            return services;
        }
    }
}
