using System;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace Garcia.Domain.Tests;

public class EntityTests
{
    private FakeEntity _sut;

    public EntityTests()
    {
        _sut = new FakeEntity();
    }

    [Fact(DisplayName = "Set initial fields in constructor")]
    public void Test1()
    {
        Assert.NotNull(_sut.DomainEvents);
        Assert.NotEqual(default, _sut.CreatedOn);
        Assert.NotEqual(default, _sut.LastUpdatedOn);
    }

    [Fact(DisplayName = "Add domain events")]
    public void Test2()
    {
        var isActiveChangedEvent = new IsActiveChangedEvent<int>(1, new FakeEntity(), false);

        _sut.AddDomainEvent(isActiveChangedEvent);

        var addedEvent = (IsActiveChangedEvent<int>) _sut.DomainEvents.First();

        Assert.Equal(1, _sut.DomainEvents.Count);
        Assert.Equal(1, addedEvent.Id);
        Assert.False(addedEvent.IsActive);
    }
    
    [Fact(DisplayName = "Remove domain events")]
    public void Test3()
    {
        var isActiveChangedEvent = new IsActiveChangedEvent<int>(1, new FakeEntity(), false);
        var isActiveChangedEvent2 = new IsActiveChangedEvent<int>(2, new FakeEntity(), false);
        _sut.AddDomainEvent(isActiveChangedEvent);
        _sut.AddDomainEvent(isActiveChangedEvent2);

        _sut.RemoveDomainEvent(isActiveChangedEvent);

        var notDeletedDomainEvent = (IsActiveChangedEvent<int>) _sut.DomainEvents.First();
        Assert.Equal(1, _sut.DomainEvents.Count);
        Assert.Equal(2, notDeletedDomainEvent.Id);
    }
    
    [Fact(DisplayName = "Clear domain events")]
    public void Test4()
    {
        var isActiveChangedEvent = new IsActiveChangedEvent<int>(1, new FakeEntity(), false);
        var isActiveChangedEvent2 = new IsActiveChangedEvent<int>(2, new FakeEntity(), false);
        _sut.AddDomainEvent(isActiveChangedEvent);
        _sut.AddDomainEvent(isActiveChangedEvent2);

        _sut.ClearDomainEvents();

        Assert.Equal(0, _sut.DomainEvents.Count);
    }
}

public class FakeEntity : Entity<int>
{
}