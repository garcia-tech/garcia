using System;
using System.Threading;
using System.Threading.Tasks;
using GarciaCore.Application.Contracts.Infrastructure;
using GarciaCore.Domain;
using Microsoft.Extensions.Hosting;

namespace GarciaCore.Infrastructure
{
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
