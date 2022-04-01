using System.Threading.Tasks;
using GarciaCore.Domain;

namespace GarciaCore.Application.Contracts.Infrastructure
{
    public interface IServiceBus
    {
        /// <summary>
        /// Creates an instance for the <typeparamref name="T"/> and publishes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to sent.</param>
        /// <returns></returns>
        Task PublishAsync<T>(T message) where T : IMessage;
    }
}
