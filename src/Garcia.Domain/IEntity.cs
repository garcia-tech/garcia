using System;

namespace Garcia.Domain
{
    public interface IEntity<out TKey> : IId<TKey> where TKey : IEquatable<TKey>
    {
        bool Active { get; set; }
        bool Deleted { get; set; }
        string? CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        string? LastUpdatedBy { get; set; }
        DateTimeOffset LastUpdatedOn { get; set; }
        string? DeletedBy { get; set; }
        DateTimeOffset DeletedOn { get; set; }
    }
}