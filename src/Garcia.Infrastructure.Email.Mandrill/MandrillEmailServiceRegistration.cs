using Garcia.Application.Contracts.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Email.Mandrill
{
    public static class MandrillEmailServiceRegistration
    {
        public static IServiceCollection AddMandrillEmailService(this IServiceCollection services, MandrillEmailSettings settings)
        {
            services.Configure<MandrillEmailSettings>(options =>
            {
                options.ApiKey = settings.ApiKey;
                options.Bcc = settings.Bcc;
                options.SenderEmailAddress = settings.SenderEmailAddress;
            });

            services.AddScoped<IMandrillEmailService, MandrillEmailService>();
            return services;
        }

        public static IServiceCollection AddMandrillEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MandrillEmailSettings>(options =>
            {
                options.ApiKey = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.ApiKey)}"];
                options.Bcc = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.Bcc)}"];
                options.SenderEmailAddress = configuration[$"{nameof(MandrillEmailSettings)}:{nameof(options.SenderEmailAddress)}"];
            });

            services.AddScoped<IMandrillEmailService, MandrillEmailService>();
            return services;
        }
    }
}
