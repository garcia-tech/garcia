﻿using System;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IAsyncCache : IDisposable
    {
        /// <summary>
        /// Gets the value of related <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Desired object.</typeparam>
        /// <param name="key">Name of the key.</param>
        /// <returns> <typeparamref name="T"/> if key exists and not null; otherwise <see langword="null"/></returns>
        /// <exception cref="KeyValueMatchException"></exception>
        Task<T> GetAsync<T>(string key);
        /// <summary>
        /// Sets value to the related <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Name of the key.</param>
        /// <param name="model">Object to be setted.</param>
        /// <param name="persist">Is setted data would be persist</param>
        /// <param name="expirationInMinutes">Expiration time. If not setted, will be used default expiration time.</param>
        /// <returns>Returns setted <typeparamref name="T"/> object</returns>
        Task<T> SetAsync<T>(string key, T model, bool persist = false, int? expirationInMinutes = null);
        /// <summary>
        /// Removes the specifed key. A key is ignored if does not exists
        /// </summary>
        /// <param name="key">Name of the key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
