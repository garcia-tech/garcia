using System;
using MediatR;

namespace GarciaCore.Domain
{
    public class IsActiveChangedEvent<TKey> : INotification where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public Entity<TKey> Entity { get; set; }
        public bool IsActive { get; set; }

        public IsActiveChangedEvent(TKey id, Entity<TKey> entity, bool isActive)
        {
            Id = id;
            Entity = entity;
            IsActive = isActive;
        }
    }
}