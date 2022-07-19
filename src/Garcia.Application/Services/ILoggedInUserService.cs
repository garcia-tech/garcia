using System;

namespace Garcia.Application.Services
{
    public interface ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey UserId { get; set; }
    }
}