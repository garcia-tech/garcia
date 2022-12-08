using Garcia.Application.Contracts.Infrastructure;

namespace Garcia.Application.RabbitMQ.Contracts.Infrastructure
{
    public interface IRabbitMqService : IServiceBus
    {
        /// <summary>
        /// Sends the <paramref name="message"/> to the specified <paramref name="queueName"/>.
        /// </summary>
        /// <param name="queueName">The name of the channel.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="exchange">The name of the exchange. Takes the default if not specified</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="noWait"><see cref="https://www.rabbitmq.com/dotnet-api-guide.html#nowait-methods"/></param>
        void Publish(string queueName, string message, string exchange = default, bool persistent = true);
        /// <summary>
        /// Sends the <paramref name="message"/> to the specified <paramref name="queueName"/> asynchronously.
        /// </summary>
        /// <param name="queueName">The name of the channel.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="exchange">The name of the exchange. Takes the default if not specified</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="noWait"><see cref="https://www.rabbitmq.com/dotnet-api-guide.html#nowait-methods"/></param>
        Task PublishAsync(string queueName, string message, string exchange = "", bool persistent = true);
        /// <summary>
        /// Handles incoming messages by listening to the specified channel.
        /// Use the <paramref name="receiveHandler"/> method to determine how the incoming message should be handled.
        /// If the message is processed successfully, ack is sent to the server.
        /// Use the <paramref name="rejectHandler"/> method to determine what happens in case of an error.
        /// If the message is not processed successfully, reject is sent to the server.
        /// </summary>
        /// <param name="queueName">The name of the channel.</param>
        /// <param name="receiveHandler">An action which handles incoming message.</param>
        /// <param name="rejectHandler">An action which handles case of an error.</param>
        /// <param name="requeueOnReject">Requeue the message when case of an error.</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="prefetchSize"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="global"></param>
        void Subscribe(string queueName, Action<ReadOnlyMemory<byte>> receiveHandler, Action<Exception, ReadOnlyMemory<byte>> rejectHandler,
            bool requeueOnReject = false, bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false);
        /// <summary>
        /// Handles incoming messages by listening to the specified channel asynchronously.
        /// Use the <paramref name="receiveHandler"/> method to determine how the incoming message should be handled.
        /// If the message is processed successfully, ack is sent to the server.
        /// Use the <paramref name="rejectHandler"/> method to determine what happens in case of an error.
        /// If the message is not processed successfully, reject is sent to the server.
        /// </summary>
        /// <param name="queueName">The name of the channel.</param>
        /// <param name="receiveHandler">An action which handles incoming message.</param>
        /// <param name="rejectHandler">An action which handles case of an error.</param>
        /// <param name="requeueOnReject">Requeue the message when case of an error.</param>
        /// <param name="persistent">Queue not deleted when server restarts.</param>
        /// <param name="prefetchSize"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="global"></param>
        Task SubscribeAsync(string queueName, Func<ReadOnlyMemory<byte>, Task> receiveHandler, Func<Exception, ReadOnlyMemory<byte>, Task> rejectHandler,
            bool requeueOnReject = false, bool persistent = true, uint prefetchSize = 0, ushort prefetchCount = 1, bool global = false);
        /// <summary>
        /// Deletes specified queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="ifUnsued"></param>
        /// <param name="ifEmpty"></param>
        /// <returns></returns>
        Task DeleteQueue(string queueName, bool ifUnsued, bool ifEmpty);
        /// <summary>
        /// Deletes specified exchange
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="ifUnsued"></param>
        /// <param name="ifEmpty"></param>
        /// <returns></returns>
        Task DeleteExchange(string queueName, bool ifUnsued);


    }
}
