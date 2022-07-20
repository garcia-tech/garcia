using System;

namespace Garcia.Domain
{
    public interface IId<TKey>
    {
        public TKey Id { get; }
    }
}