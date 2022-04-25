using System;

namespace Garcia.Domain
{
    public abstract class ChildEntity : ValueObject
    {
        protected ChildEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.UtcNow;
        }

        public string Id { get; }
        public DateTimeOffset CreatedOn { get; }
    }
}
