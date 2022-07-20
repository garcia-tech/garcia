using System;

namespace Garcia.Domain
{
    public interface IEntity<TKey> : IId<TKey>
    {
        bool Active { get; set; }
        bool Deleted { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset LastUpdatedOn { get; set; }
        DateTimeOffset DeletedOn { get; set; }
    }
}