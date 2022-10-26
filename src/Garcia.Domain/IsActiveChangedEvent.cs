using MediatR;

namespace Garcia.Domain
{
    public class IsActiveChangedEvent<TKey> : INotification
    {
        public TKey Id { get; set; }
        public EntityBase<TKey> Entity { get; set; }
        public bool IsActive { get; set; }

        public IsActiveChangedEvent(TKey id, EntityBase<TKey> entity, bool isActive)
        {
            Id = id;
            Entity = entity;
            IsActive = isActive;
        }
    }
}