using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.RabbitMQ.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GaricaCore.Infrastructure.RabbitMQ
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

            return services.AddRabbitMqServices();
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

            return services.AddRabbitMqServices();
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

            return services.AddRabbitMqServices();
        }

        private static IServiceCollection AddRabbitMqServices(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqConnetionFactory>();
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }
}
