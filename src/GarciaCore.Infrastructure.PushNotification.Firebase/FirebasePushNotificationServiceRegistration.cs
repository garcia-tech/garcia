using GarciaCore.Application.Contracts.Email;
using GarciaCore.Application.Contracts.Marketing;
using GarciaCore.Application.Contracts.PushNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Infrastructure.PushNotification.Firebase
{
    public static class FirebasePushNotificationServiceRegistration
    {
        public static IServiceCollection AddFirebasePushNotificationService(this IServiceCollection services, FirebasePushNotificationSettings settings)
        {
            services.Configure<FirebasePushNotificationSettings>(options =>
            {
                options.AccessToken = settings.AccessToken;
            });

            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            return services;
        }

        public static IServiceCollection AddFirebasePushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebasePushNotificationSettings>(options =>
            {
                options.AccessToken = configuration[$"{nameof(FirebasePushNotificationSettings)}:{nameof(options.AccessToken)}"];
            });

            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            return services;
        }
    }
}
