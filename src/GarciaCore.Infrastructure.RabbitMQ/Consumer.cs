using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.RabbitMQ.Contracts.Infrastructure;
using GarciaCore.Domain;

namespace GarciaCore.Infrastructure.RabbitMQ
{
    public abstract class Consumer<T> : BaseConsumer<T> where T : IMessage
    {
        private readonly IRabbitMqService _rabbit;

        protected Consumer(RabbitMqConnectionFactory factory)
        {
            _rabbit = new RabbitMqService(factory);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            await _rabbit.SubscribeAsync<T>(Consume, HandleException);
        }
    }
}
