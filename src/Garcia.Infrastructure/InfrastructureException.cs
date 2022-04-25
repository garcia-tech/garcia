using System;

namespace Garcia.Infrastructure
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message) : base(message)
        {
        }
    }

    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message) : base(message)
        {
        }

        public DomainNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DomainExistingItemException : Exception
    {
        public DomainExistingItemException(string message) : base(message)
        {
        }

        public DomainExistingItemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message)
        {
        }

        public DomainValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}