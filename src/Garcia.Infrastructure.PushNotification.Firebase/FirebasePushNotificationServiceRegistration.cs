using Garcia.Application.Contracts.PushNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.PushNotification.Firebase
{
    public static class FirebasePushNotificationServiceRegistration
    {
        public static IServiceCollection AddFirebasePushNotificationService(this IServiceCollection services, FirebasePushNotificationSettings settings)
        {
            services.Configure<FirebasePushNotificationSettings>(options =>
            {
                options.AccessToken = settings.AccessToken;
                options.JsonString = settings.JsonString;
                options.JsonFilePath = settings.JsonFilePath;
            });

            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            return services;
        }

        public static IServiceCollection AddFirebasePushNotificationService(this IServiceCollection services, Action<FirebasePushNotificationSettings> options)
        {
            services.Configure(options);
            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            return services;
        }

        public static IServiceCollection AddFirebasePushNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebasePushNotificationSettings>(options =>
            {
                options.AccessToken = configuration[$"{nameof(FirebasePushNotificationSettings)}:{nameof(options.AccessToken)}"];
                options.JsonString = configuration[$"{nameof(FirebasePushNotificationSettings)}:{nameof(options.JsonString)}"];
                options.JsonFilePath = configuration[$"{nameof(FirebasePushNotificationSettings)}:{nameof(options.JsonFilePath)}"];
            });

            services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
            return services;
        }
    }
}
