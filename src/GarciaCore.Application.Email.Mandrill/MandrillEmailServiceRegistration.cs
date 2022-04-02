using GarciaCore.Application.Contracts.Email;
using GarciaCore.Application.Contracts.Marketing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Application.Email.Mandrill
{
    public static class MandrillEmailServiceRegistration
    {
        public static IServiceCollection RegisterMandrillEmailService(this IServiceCollection services, MandrillEmailSettings settings)
        {
            services.Configure<MandrillEmailSettings>(options =>
            {
                options.ApiKey = settings.ApiKey;
                options.Bcc = settings.Bcc;
                options.SenderEmailAddress = settings.SenderEmailAddress;
            });

            services.AddScoped<IEmailService, MandrillEmailService>();
            return services;
        }

        public static IServiceCollection RegisterMandrillEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MandrillEmailSettings>(options =>
            {
                options.ApiKey = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.ApiKey)}"];
                options.Bcc = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.Bcc)}"];
                options.SenderEmailAddress = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.SenderEmailAddress)}"];
            });

            services.AddScoped<IEmailService, MandrillEmailService>();
            return services;
        }
    }
}
