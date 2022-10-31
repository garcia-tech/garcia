using System.Collections.Generic;
using System.Threading.Tasks;
using Garcia.Persistence.MongoDb;

namespace Garcia.Persistence.Tests.Utils
{
    public static class Helpers
    {
        public static async Task SeedMongo(MongoDbRepository<TestMongoEntity> repository)
        {
            var entities = new List<TestMongoEntity>()
            {
                new TestMongoEntity
                {
                    Name = "Test1",
                    Indicator = 1
                },
                new TestMongoEntity
                {
                    Name = "Test2",
                    Indicator = 2
                },
                new TestMongoEntity
                {
                    Name = "Test3",
                    Indicator = 3
                },
                new TestMongoEntity
                {
                    Name = "Test4",
                    Indicator = 4
                },
                new TestMongoEntity
                {
                    Name = "Test5",
                    Indicator = 5
                }
            };

            await repository.AddRangeAsync(entities);
        }
    }
}
