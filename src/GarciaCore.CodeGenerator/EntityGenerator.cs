using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class EntityGenerator : Generator<EntityTemplate>
    {
        public override string DefaultBaseClass => "Entity";
        protected override string FileNamePostfix => "";
        public override GeneratorType GeneratorType => GeneratorType.Domain;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
    }
}