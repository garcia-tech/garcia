using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCommandHandlerGenerator<T> : Generator<T>
      where T : BaseTemplate
    {
        public override string DefaultBaseClass => string.Empty;
        protected override string FileNamePostfix => "CommandHandler";
        public override GeneratorType GeneratorType => GeneratorType.CommandHandler;
        public override List<string> GarciaCoreDependencies => new List<string>() { "GarciaCore.Application" };
    }
}