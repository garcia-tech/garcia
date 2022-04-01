using System;
using System.Threading.Tasks;
using GarciaCore.Domain;
using Microsoft.Extensions.Hosting;

namespace GarciaCore.Infrastructure
{
    public abstract class BaseConsumer<T> : BackgroundService where T : IMessage
    {
        protected abstract Task Consume(T message);
        protected abstract Task HandleException(Exception e, string message);
    }
}
