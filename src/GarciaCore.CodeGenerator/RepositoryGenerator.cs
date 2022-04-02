using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class RepositoryGenerator : Generator<RepositoryTemplate>
    {
        public override string DefaultBaseClass => "EntityFrameworkRepository";
        protected override string FileNamePostfix => "Repository";
        public override GeneratorType GeneratorType => GeneratorType.Repository;
        public override List<string> GarciaCoreDependencies => new List<string>() { "GarciaCore.Domain" };
    }
}