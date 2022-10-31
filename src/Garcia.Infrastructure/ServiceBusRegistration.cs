using Garcia.Application.Contracts.Infrastructure;
using Garcia.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure
{
    public static class ServiceBusRegistration
    {
        public static IServiceCollection AddConsumer<TConsumer, TMessage>(this IServiceCollection services)
            where TMessage : IMessage
            where TConsumer : BaseConsumer<TMessage>
        {
            return services.AddHostedService<TConsumer>();
        }

        public static IServiceCollection AddServiceBus<TImplementer>(this IServiceCollection services)
            where TImplementer : class, IServiceBus
        {
            return services.AddSingleton<IServiceBus, TImplementer>();
        }
    }
}
