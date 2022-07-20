using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Garcia.Domain
{
    public abstract partial class Entity<TKey> : EntityBase<TKey> where TKey : struct, IEquatable<TKey>
    {
        public virtual TKey? CreatedBy { get; set; }
        public virtual TKey? LastUpdatedBy { get; set; }
        [JsonIgnore]
        public virtual TKey? DeletedBy { get; set; }

        public Entity()
        {
            CreatedOn = LastUpdatedOn = DateTime.Now;
            domainItems = new List<INotification>();
        }
    }
}