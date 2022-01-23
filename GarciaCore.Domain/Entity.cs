using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace GarciaCore.Domain;

public abstract partial class Entity
{
    public virtual long Id { get; set; }
    public virtual Guid UniqueId { get; set; } = Guid.NewGuid();
    public virtual DateTime CreatedOn { get; private set; }
    public virtual DateTime LastUpdatedOn { get; set; }
    public virtual bool IsActive { get; set; }
    [JsonIgnore]
    public virtual bool IsDeleted { get; set; }
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
        if (IsActive != isActive)
        {
            IsActive = isActive;
            AddIsActiveChangedDomainEvent(isActive);
        }
    }

    protected virtual void AddIsActiveChangedDomainEvent(bool isActive)
    {
        AddDomainEvent(new IsActiveChangedEvent(this.Id, this, isActive));
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        else if (!(obj is Entity))
        {
            return false;
        }
        else
        {
            return this.Id.Equals(((Entity)obj).Id);
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}