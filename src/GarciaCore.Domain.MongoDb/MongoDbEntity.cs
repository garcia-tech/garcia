using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace GarciaCore.Domain.MongoDb
{
    [BsonIgnoreExtraElements]
    public abstract class MongoDbEntity : Entity<string>
    {
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
