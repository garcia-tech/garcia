using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
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

        public void AddDomainEvent(INotification eventItem)
        {
            domainItems.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            domainItems?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            domainItems?.Clear();
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
