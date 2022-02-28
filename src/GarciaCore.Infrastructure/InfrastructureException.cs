using System;

namespace GarciaCore.Infrastructure
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
}