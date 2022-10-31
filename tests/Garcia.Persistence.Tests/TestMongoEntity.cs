using Garcia.Domain.MongoDb;

namespace Garcia.Persistence.Tests
{
    public class TestMongoEntity : MongoDbEntity
    {
        public TestMongoEntity()
        {
            CacheExpirationInMinutes = 2;
            CachingEnabled = true;
        }

        public string Name { get; set; }
        public int Indicator { get; set; }
    }
}
