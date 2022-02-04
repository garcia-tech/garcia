using MediatR;

namespace GarciaCore.Domain
{
    public class IsActiveChangedEvent : INotification
    {
        public long Id { get; set; }
        public Entity Entity { get; set; }
        public bool IsActive { get; set; }

        public IsActiveChangedEvent(long id, Entity entity, bool isActive)
        {
            Id = id;
            Entity = entity;
            IsActive = isActive;
        }
    }
}