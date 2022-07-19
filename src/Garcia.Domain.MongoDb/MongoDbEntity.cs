using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;


namespace Garcia.Domain.MongoDb
{
    [BsonIgnoreExtraElements]
    public abstract class MongoDbEntity : EntityBase<string>
    {
        public virtual string CreatedBy { get; set; }
        public virtual string LastUpdatedBy { get; set; }
        [JsonIgnore]
        public virtual string DeletedBy { get; set; }

        public MongoDbEntity()
        {
            CreatedOn = LastUpdatedOn = DateTime.Now;
            domainItems = new List<INotification>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is MongoDbEntity && Id.Equals(((MongoDbEntity)obj).Id);
        }

        public override int GetHashCode() => base.GetHashCode();

    }
}
