using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.Redis
{
    public interface IRedisService : IAsyncPubSubService
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
    }
}
