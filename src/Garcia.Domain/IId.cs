using System;

namespace Garcia.Domain
{
    public interface IId<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
    }
}