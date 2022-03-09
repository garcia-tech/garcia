using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarciaCore.Domain
{
    public abstract partial class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }
        public virtual Guid UniqueId { get; set; } = Guid.NewGuid();
        public virtual DateTimeOffset CreatedOn { get; set; }
        public virtual DateTimeOffset LastUpdatedOn { get; set; }
        public virtual bool Active { get; set; }
        public virtual int? CreatedBy { get; set; }
        public virtual int? LastUpdatedBy { get; set; }
        [JsonIgnore]
        public virtual int? DeletedBy { get; set; }
        public virtual DateTimeOffset DeletedOn { get; set; }
        [JsonIgnore]
        public virtual bool Deleted { get; set; }
        private List<INotification> _domainEvents = new List<INotification>();
        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public Entity()
        {
            CreatedOn = LastUpdatedOn = DateTime.Now;
            _domainEvents = new List<INotification>();
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public virtual void SetIsActive(bool isActive)
        {
            if (Active != isActive)
            {
                Active = isActive;
                AddIsActiveChangedDomainEvent(isActive);
            }
        }

        protected virtual void AddIsActiveChangedDomainEvent(bool isActive)
        {
            AddDomainEvent(new IsActiveChangedEvent<TKey>(this.Id, this, isActive));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!(obj is Entity<TKey>))
            {
                return false;
            }
            else
            {
                return this.Id.Equals(((Entity<TKey>)obj).Id);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}