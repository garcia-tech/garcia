using System;
using System.Threading.Tasks;
using GarciaCore.Domain;

namespace GarciaCore.Application.Contracts.Infrastructure
{
    public interface IServiceBus : IDisposable
    {
        /// <summary>
        /// Creates an instance for the <typeparamref name="T"/> and publishes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to sent.</param>
        /// <returns></returns>
        Task PublishAsync<T>(T message) where T : IMessage;
        /// <summary>
        /// When a message model created in <see cref="IMessage"/> type is published, 
        /// a queue is created for the relevant message. This queue is listened and the incoming message is processed using the <paramref name="receiveHandler"/> method.
        /// </summary>
        /// <typeparam name="T">The message to receive.</typeparam>
        /// <param name="receiveHandler">An action which handles incoming message.</param>
        /// <param name="rejectHandler">An action which handles case of an error.</param>
        /// <returns></returns>
        Task SubscribeAsync<T>(Func<T, Task> receiveHandler, Func<Exception, string, Task> rejectHandler) where T : IMessage;
    }
}
