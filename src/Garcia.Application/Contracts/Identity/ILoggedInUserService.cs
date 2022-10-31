using System;

namespace Garcia.Application.Contracts.Identity
{
    /// <summary>
    /// Accesses the session user. Gets session user's informations.
    /// </summary>
    /// <typeparam name="TKey">The type of user id in the system.</typeparam>
    public interface ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey UserId { get; set; }
        public LoggedInUser LoggedInUser { get; set; }
    }
}