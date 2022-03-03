using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GarciaCore.Application.Contracts.Infrastructure;
using StackExchange.Redis;

namespace GarciaCore.Application.Redis.Contracts.Persistence
{
    public interface IRedisCache : IAsyncCache
    {
        /// <summary>
        /// Returns if key exits.
        /// </summary>
        /// <param name="key">Name of the key.</param>
        /// <returns><see langword="true" /> if key exists. <see langword="false"/> if key not exits.</returns>
        Task<bool> ExistsAsync(string key);
        /// <summary>
        /// Returns all keys matching pattern.
        /// </summary>
        /// <param name="key">Name of the key.</param>
        /// <returns><see cref="IEnumerable{string}">IEnumerable&lt;string&gt;</see></returns>
        Task<IEnumerable<string>> GetMatchingKeysAsync(string key);
        /// <summary>
        /// Removes all keys matching pattern.
        /// </summary>
        /// <param name="key">Name of the key.</param>
        /// <returns></returns>
        Task RemoveMatchingKeysAsync(string key);
        /// <summary>
        /// Sets all fields of the value to the hash key, 
        /// overwrites the new values ​​if a value exists in the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Name of the key.</param>
        /// <param name="model">The model to be setted.</param>
        /// <returns></returns>
        Task HashSetAsync<T>(string key, T model);
        /// <summary>
        /// Returns the value of specified field in the hash stored at key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="key">Name of the key.</param>
        /// <param name="field">Desired field.</param>
        /// <returns>The field value <typeparamref name="TField"/>, or <see langword="null"/> when specified field or hash key does not exits</returns>
        /// <exception cref="KeyValueMatchException"/>
        Task<TField> HashGetAsync<T, TField>(string key, Expression<Func<T, TField>> field) where T : class;
        /// <summary>
        /// Returns complete value of hash key.
        /// <para>
        ///     <typeparamref name="T"/>  must have parameterless constructor
        /// </para>
        /// <para>
        ///     Fields without public setter definition will not be able to fill from hash key.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Type param must have parameterless constructor</typeparam>
        /// <param name="key">Name of the key.</param>
        /// <returns>Complete value of hash key <c><typeparamref name="T"/></c>  if hash key exists; otherwise <see langword="null"/></returns>
        /// <exception cref="KeyValueMatchException"></exception>
        Task<T> HashGetAllAsync<T>(string key) where T : class;
        /// <summary>
        /// Execute an arbitrary command against the server; this is primarily intended for
        /// executing modules, but may also be used to provide access to new features that
        /// lack a direct API.
        /// </summary>
        /// <param name="cmd">Command to be executed</param>
        /// <param name="args">Arguments of the command</param>
        /// <returns>A dynamic representation of the command's result.</returns>
        Task<RedisResult> ExecuteAsync(string cmd, params object[] args);
    }
}
