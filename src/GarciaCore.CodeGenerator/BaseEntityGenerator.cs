using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class BaseEntityGenerator : Generator<BaseEntityTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "BaseEntity";
        public override GeneratorType GeneratorType => GeneratorType.Domain;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
        public override bool IsItemLevel => false;
    }
}