using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Application.RabbitMQ.Contracts.Infrastructure
{
    public interface IRabbitMqService
    {
        /// <summary>
        /// Sends the <paramref name="message"/> to the specified <paramref name="channelName"/>.
        /// </summary>
        /// <param name="channelName">The name of the channel.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="exchange">The name of the exchange. Takes the default if not specified</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="noWait"><see cref="https://www.rabbitmq.com/dotnet-api-guide.html#nowait-methods"/></param>
        void Publish(string channelName, string message, string exchange = default, bool persistent = true, bool noWait = false);
        /// <summary>
        /// Handles incoming messages by listening to the specified channel.
        /// Use the <paramref name="receiveHandler"/> method to determine how the incoming message should be handled.
        /// If the message is processed successfully, ack is sent to the server.
        /// Use the <paramref name="rejectHandler"/> method to determine what happens in case of an error.
        /// If the message is not processed successfully, reject is sent to the server.
        /// </summary>
        /// <param name="channelName">The name of the channel.</param>
        /// <param name="receiveHandler">An action which handles incoming message.</param>
        /// <param name="rejectHandler">An action which handles case of an error.</param>
        /// <param name="requeueOnReject">Requeue the message when case of an error.</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="prefetchSize"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="global"></param>
        void Subscribe(string channelName, Action<ReadOnlyMemory<byte>> receiveHandler, Action<Exception, ReadOnlyMemory<byte>> rejectHandler,
            bool requeueOnReject = false, bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false);

    }
}
