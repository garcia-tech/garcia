using System;

namespace Garcia.Domain
{
    public interface IId<out TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
    }
}