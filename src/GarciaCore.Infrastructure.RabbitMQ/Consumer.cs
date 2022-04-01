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

        protected async Task StartConsuming(Func<T, Task> consume, Func<Exception, string, Task> handleException)
        {
            await _rabbit.SubscribeAsync(consume, handleException);
        }
    }
}
