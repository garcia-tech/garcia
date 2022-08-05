using System;

namespace Garcia.Application.Contracts.Identity
{
    public interface ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey UserId { get; set; }
        public LoggedInUser LoggedInUser { get; set; }
    }
}