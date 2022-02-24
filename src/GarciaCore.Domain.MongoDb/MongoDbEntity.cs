using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GarciaCore.Domain.MongoDb
{
    public abstract class MongoDbEntity : IEntity<string>
    {
        public MongoDbEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public string Id { get; private set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset DeletedOn { get; set; }
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
