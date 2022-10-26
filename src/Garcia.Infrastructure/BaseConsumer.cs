using System;
using System.Threading;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Domain;
using Microsoft.Extensions.Hosting;

namespace Garcia.Infrastructure
{
    /// <summary>
    /// Subscribes to the <typeparamref name="T"/> channel via service bus.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseConsumer<T> : BackgroundService where T : IMessage
    {
        private readonly IServiceBus _bus;

        protected BaseConsumer(IServiceBus bus)
        {
            _bus = bus;
        }

        protected abstract Task Consume(T message);
        protected abstract Task HandleException(Exception e, string message);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            await _bus.SubscribeAsync<T>(Consume, HandleException);
        }
    }
}
