using System;

namespace Garcia.Application.Contracts.Localization
{
    public class LocalizationException : Exception
    {
        public LocalizationException()
        {
        }

        public LocalizationException(string message) : base(message)
        {
        }
    }
}
