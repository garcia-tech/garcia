using System;

namespace Garcia.Application.Services
{
    public class LoggedInUserService<TKey> : ILoggedInUserService<TKey>
        where TKey : IEquatable<TKey> 
    {
        public TKey UserId { get; set; }
        public TKey ConvertToId(string value)
        {
            if (string.IsNullOrEmpty(value)) return default;

            return (TKey)Convert.ChangeType(value, typeof(TKey));
        }
    }
}
