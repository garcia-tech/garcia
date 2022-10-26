using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Garcia.Domain
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public TKey Id { get; set; }
        public virtual Guid UniqueId { get; set; } = Guid.NewGuid();
        public virtual bool Active { get; set; }
        public virtual bool Deleted { get; set; }
        [BsonRepresentation(BsonType.String)]
        public virtual DateTimeOffset CreatedOn { get; set; }
        [BsonRepresentation(BsonType.String)]
        public virtual DateTimeOffset LastUpdatedOn { get; set; }
        [BsonRepresentation(BsonType.String)]
        public virtual DateTimeOffset DeletedOn { get; set; }
        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        public bool CachingEnabled { get; set; } = false;
        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        public int CacheExpirationInMinutes { get; set; } = 1;
        protected List<INotification> domainItems;
        [JsonIgnore]
        [BsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => domainItems?.AsReadOnly();
        /// <summary>
        /// Adds domain event. If <see cref="IMediator"/> is registered, 
        /// domain events will be published automatically via <see cref="EntityBase{TKey}.PublishDomainEvents(IMediator, CancellationToken)"/>
        /// when saving to the database.
        /// </summary>
        /// <param name="eventItem"></param>
        public void AddDomainEvent(INotification eventItem)
        {
            domainItems.Add(eventItem);
        }
        /// <summary>
        /// Removes domain event.
        /// </summary>
        /// <param name="eventItem"></param>
        public void RemoveDomainEvent(INotification eventItem)
        {
            domainItems?.Remove(eventItem);
        }
        /// <summary>
        /// Clears all domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            domainItems?.Clear();
        }
        /// <summary>
        /// Sets entity's <see cref="EntityBase{TKey}.Active"/> property.
        /// If the entity's <see cref="EntityBase{TKey}.Active"/> property has changed,
        /// <see cref="IsActiveChangedEvent{TKey}"/> is added to the entity's <see cref="EntityBase{TKey}.DomainEvents"/>.
        /// </summary>
        /// <param name="isActive"></param>
        public virtual void SetIsActive(bool isActive)
        {
            if (Active != isActive)
            {
                Active = isActive;
                AddIsActiveChangedDomainEvent(isActive);
            }
        }

        /// <summary>
        /// Adds <see cref="IsActiveChangedEvent{TKey}"/> to the <see cref="EntityBase{TKey}.DomainEvents"/>.
        /// </summary>
        /// <param name="isActive"></param>

        protected virtual void AddIsActiveChangedDomainEvent(bool isActive)
        {
            AddDomainEvent(new IsActiveChangedEvent<TKey>(Id, this, isActive));
        }

        /// <summary>
        /// Publishes all events in <see cref="EntityBase{TKey}.DomainEvents"/> via <see cref="IMediator"/>.
        /// If <see cref="IMediator"/> is registered, the repository does it automatically. 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task PublishDomainEvents(IMediator mediator, CancellationToken cancellationToken)
        {
            if (!DomainEvents.Any()) return;

            await Parallel.ForEachAsync(DomainEvents, async (eventItem, cancellationToken) =>
            {
                await mediator.Publish(eventItem, cancellationToken);
            });
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!(obj is EntityBase<TKey>))
            {
                return false;
            }
            else
            {
                return Id!.Equals(((EntityBase<TKey>)obj).Id);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public TKey ConvertToId(string value)
        {
            return (TKey)Convert.ChangeType(value, typeof(TKey));
        }

    }
}
