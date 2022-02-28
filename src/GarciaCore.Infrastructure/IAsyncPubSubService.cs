using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure
{
    public interface IAsyncPubSubService : IDisposable
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
    }
}
