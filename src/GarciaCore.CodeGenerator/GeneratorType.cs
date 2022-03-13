using System;

namespace GarciaCore.CodeGenerator
{
    [Flags]
    public enum GeneratorType
    {
        Domain = 1,
        Repository = 2,
        Api = 4,
        Command = 8,
        CommandHandler = 16,
        Query = 32,
        Service = 64,
        Model = 128,
    }
}