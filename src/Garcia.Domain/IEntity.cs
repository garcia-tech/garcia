using System;

namespace Garcia.Domain
{
    public interface IEntity<TKey> : IId<TKey> where TKey : IEquatable<TKey>
    {
        bool Active { get; set; }
        bool Deleted { get; set; }
        TKey? CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        TKey? LastUpdatedBy { get; set; }
        DateTimeOffset LastUpdatedOn { get; set; }
        TKey? DeletedBy { get; set; }
        DateTimeOffset DeletedOn { get; set; }
    }
}