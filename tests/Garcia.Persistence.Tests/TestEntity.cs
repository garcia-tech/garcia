using System.Collections.Generic;
using Garcia.Domain;

namespace Garcia.Persistence.Tests
{
    public class TestEntity : Entity<long>
    {
        public TestEntity(int key, string name)
        {
            Key = key;
            Name = name;
        }

        public TestEntity()
        {
        }

        public int Key { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public ICollection<TestChildEntity> TestChildEntities { get; set; } = new List<TestChildEntity>();

        public void AddChild(TestChildEntity data)
        {
            TestChildEntities.Add(data);
        }
    }

    public class TestChildEntity : Entity<long>
    {
        public string Name { get; set; }
        public TestEntity TestEntity { get; set; }
        public long TestEntityId { get; set; }
    }
}