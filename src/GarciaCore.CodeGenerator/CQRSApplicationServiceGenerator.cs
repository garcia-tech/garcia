using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationServiceGenerator : Generator<CQRSApplicationServiceTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "Service";
        public override GeneratorType GeneratorType => GeneratorType.Service;
        public override List<string> GarciaCoreDependencies => new List<string>() { "GarciaCore.Application" };
    }
}