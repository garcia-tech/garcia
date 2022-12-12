using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Garcia.Domain
{
    public abstract partial class Entity<TKey> : EntityBase<TKey> where TKey : struct, IEquatable<TKey>
    {
        [JsonIgnore]
        public virtual TKey? CreatedBy { get; set; }
        [JsonIgnore]
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