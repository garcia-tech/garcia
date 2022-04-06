using GarciaCore.Application.RabbitMQ.Contracts.Infrastructure;
using GarciaCore.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using GarciaCore.Domain;

namespace GarciaCore.Infrastructure.RabbitMQ
{
    public static class RabbitMqServiceRegistration
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(o =>
            {
                o.Host = configuration[$"{nameof(RabbitMqSettings)}:{o.GetHostKeyValue()}"];
                o.Port = configuration[$"{nameof(RabbitMqSettings)}:{o.GetPortKeyValue()}"];
                o.UserName = configuration[$"{nameof(RabbitMqSettings)}:{o.GetUserNameKeyValue()}"];
                o.Password = configuration[$"{nameof(RabbitMqSettings)}:{o.GetPasswordKeyValue()}"];
            });

            services.AddSingleton<RabbitMqConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, RabbitMqSettings settings)
        {
            services.Configure<RabbitMqSettings>(o =>
            {
                o.Host = settings.Host;
                o.Port = settings.Port;
                o.UserName = settings.UserName;
                o.Password = settings.Password;
            });

            services.AddSingleton<RabbitMqConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRabbitMQ<T>(this IServiceCollection services, IOptions<T> options) where T : RabbitMqSettings
        {
            services.Configure<RabbitMqSettings>(o =>
            {
                o.Host = options.Value.Host;
                o.Port = options.Value.Port;
                o.UserName =  options.Value.UserName;
                o.Password =  options.Value.Password;
            });

            services.AddSingleton<RabbitMqConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddSingleton<IServiceBus, RabbitMqService>();
            return services;
        }

        public static IServiceCollection AddRabbitConsumer<TConsumer, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TConsumer : BaseConsumer<TMessage>
        {
            services.AddHostedService<TConsumer>();
            return services;
        }
    }
}
