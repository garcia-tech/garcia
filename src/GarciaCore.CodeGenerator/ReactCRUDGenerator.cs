using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class ReactCRUDGenerator : Generator<ReactCRUDTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "";
        public override GeneratorType GeneratorType => GeneratorType.ReactCRUD;
        public override bool IsItemLevel => true;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
        public override string FileExtension => "js";
        public override bool IsMultiple => true;
    }
}