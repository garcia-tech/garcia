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
        MappingProfile = 256,
        ApplicationServiceRegistration = 512,
        Program = 1024,
        EntityFrameworkDbContext = 2048,
        WebApiAuthentication = 4096,
        WebApiAppSettings = 8192,
        SolutionJson = 16384,
    }
}