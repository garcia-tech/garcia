using System;

namespace GarciaCore.Domain
{
    public interface IEntity<out TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
        bool Active { get; set; }
        bool Deleted { get; set; }
        int? CreatedBy { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        int? LastUpdatedBy { get; set; }
        DateTimeOffset LastUpdatedOn { get; set; }
        int? DeletedBy { get; set; }
        DateTimeOffset DeletedOn { get; set; }
    }
}