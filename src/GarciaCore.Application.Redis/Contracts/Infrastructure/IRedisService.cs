using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.Contracts.Infrastructure;
using GarciaCore.Domain;

namespace GarciaCore.Application.Redis.Contracts.Infrastructure
{
    public interface IRedisService : IAsyncPubSubService, IServiceBus
    {
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
