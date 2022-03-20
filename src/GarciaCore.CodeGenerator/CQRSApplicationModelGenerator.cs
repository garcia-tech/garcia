using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationModelGenerator : Generator<CQRSApplicationModelTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "Model";
        public override GeneratorType GeneratorType => GeneratorType.Model;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
    }
}