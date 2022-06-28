using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Garcia.Application.Contracts.Email;

namespace Garcia.Infrastructure.Email
{
    public static class EmailServiceRegistrations
    {
        public static IServiceCollection AddEmailConfigurations(this IServiceCollection services, Action<EmailConfigurations> options) => services.Configure(options);
        public static IServiceCollection AddEmailServices(this IServiceCollection services) => services.AddScoped<IEmailService, EmailService>();

        public static IServiceCollection AddEmailConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<EmailConfigurations>(options =>
            {
                options.SmtpConfigurations = JsonConvert.DeserializeObject<SmtpConfigurations>(configuration.GetSection("EmailConfigurations").GetValue<string>("SmtpConfigurations"));
                options.Credentials = JsonConvert.DeserializeObject<EmailCredentials>(configuration.GetSection("EmailConfigurations").GetValue<string>("Credentials"));
            });
        }

    }
}
