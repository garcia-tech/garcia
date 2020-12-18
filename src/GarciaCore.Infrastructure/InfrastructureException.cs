using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message) : base(message)
        {
        }
    }
}
