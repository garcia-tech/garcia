using System;

namespace Garcia.Application.Services
{
    public class LoggedInUserService<TKey> : ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey> 
    {
        public TKey UserId { get; set; }
        public TKey ConvertToId(string value)
        {
            return (TKey)Convert.ChangeType(value, typeof(TKey));
        }
    }
}
