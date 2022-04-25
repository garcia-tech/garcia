using System;

namespace Garcia.Exceptions.Redis
{
    /// <summary>
    /// The exception that is thrown when type parameter doest not match value of key
    /// </summary>
    public class KeyValueMatchException : Exception
    {
        public KeyValueMatchException()
        {
        }

        public KeyValueMatchException(string message)
            : base($"The type parameter (${message}) does not match value of key")
        {
        }

        public KeyValueMatchException(string message, Exception innerException)
            : base($"The type parameter (${message}) does not match value of key", innerException)
        {
        }
    }
}
