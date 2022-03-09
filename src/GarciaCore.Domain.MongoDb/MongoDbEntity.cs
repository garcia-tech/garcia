using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GarciaCore.Domain.MongoDb
{
    [BsonIgnoreExtraElements]
    public abstract class MongoDbEntity : Entity<string>
    {
        public MongoDbEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public override string Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public override DateTimeOffset CreatedOn { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public override DateTimeOffset LastUpdatedOn { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public override DateTimeOffset DeletedOn { get; set; }

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
