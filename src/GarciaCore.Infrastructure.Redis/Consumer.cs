using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GarciaCore.Domain;

namespace GarciaCore.Infrastructure.Redis
{
    public abstract class Consumer<T> : BaseConsumer<T> where T : IMessage
    {
        private readonly RedisService _service;

        public Consumer(RedisConnectionFactory connectionFactory)
        {
            _service = new RedisService(connectionFactory);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            await _service.SubscribeAsync<T>(Consume, HandleException);
        }
    }
}
