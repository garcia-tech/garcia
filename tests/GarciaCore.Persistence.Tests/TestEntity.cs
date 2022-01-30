using GarciaCore.Domain;

namespace GarciaCore.Persistence.Tests;

public class TestEntity : Entity
{
    public TestEntity(int key, string name)
    {
        Key = key;
        Name = name;
    }

    public TestEntity()
    {
    }

    public int Key { get; set; }
    public string Name { get; set; }
}