using Garcia.Application.Contracts.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.Email.SendGrid
{
    public static class SendGridEmailServiceRegistrations
    {
        public static IServiceCollection AddSendGridEmailService(this IServiceCollection services, SendGridEmailSettings settings)
        {
            services.Configure<SendGridEmailSettings>(options =>
            {
                options.ApiKey = settings.ApiKey;
                options.Bcc = settings.Bcc;
                options.SenderEmailAddress = settings.SenderEmailAddress;
                options.TemplateId = settings.TemplateId;
            });

            services.AddScoped<ISendGridEmailService, SendGridEmailService>();
            return services;
        }

        public static IServiceCollection AddSendGridEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SendGridEmailSettings>(options =>
            {
                options.ApiKey = configuration[$"{nameof(SendGridEmailSettings)}:{nameof(options.ApiKey)}"];
                options.Bcc = configuration[$"{nameof(SendGridEmailSettings)}:{nameof(options.Bcc)}"];
                options.SenderEmailAddress = configuration[$"{nameof(SendGridEmailSettings)}:{nameof(options.SenderEmailAddress)}"];
                options.TemplateId = configuration[$"{nameof(SendGridEmailSettings)}:{nameof(options.TemplateId)}"];
            });

            services.AddScoped<ISendGridEmailService, SendGridEmailService>();
            return services;
        }
    }
}
