using System;

namespace GarciaCore.CodeGenerator
{
    public class CodeGeneratorException : Exception
    {
        public CodeGeneratorException(string message) : base(message)
        {
        }

        public CodeGeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}