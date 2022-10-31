using System;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Infrastructure;

namespace Garcia.Application.Redis.Contracts.Infrastructure
{
    public interface IRedisService : IServiceBus
    {
        /// <summary>
        /// Publishes a message to the related channel
        /// </summary>
        /// <param name="channel">Name of the related channel.</param>
        /// <param name="message">A message to be published</param>
        /// <returns></returns>
        Task PublishAsync(string channel, string message);
        /// <summary>
        /// Subscribe to perform some operation when a change to the preferred/active node
        /// is broadcast.
        /// </summary>
        /// <param name="channel">Name of the related channel.</param>
        /// <param name="action">An action that processes the received message</param>
        /// <returns></returns>
        Task SubscribeAsync(string channel, Action<string> action);
        /// <summary>
        /// Executes the given action with distributed lock object asynchronously 
        /// </summary>
        /// <param name="action">An action requested to run using distributed lock object</param>
        /// <param name="expiryInMilliSeconds">Lock expiration duration</param>
        /// <returns></returns>
        Task ExecuteWithLockAsync(Action action, int expiryInMilliSeconds);
        /// <summary>
        /// Takes a lock object if it is not already taken
        /// </summary>
        /// <param name="key">Lock object key</param>
        /// <param name="expiryInMilliSeconds">Lock expiration duration</param>
        /// <returns><see langword="true"/> if it is successfuly taken; otherwise <see langword="false"/></returns>
        Task<bool> AcquireLockAsync(string key, int expiryInMilliSeconds);
    }
}
