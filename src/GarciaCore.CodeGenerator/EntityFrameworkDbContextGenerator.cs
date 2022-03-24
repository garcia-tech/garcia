using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class EntityFrameworkDbContextGenerator : Generator<EntityFrameworkDbContextTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => $"{GeneratorRepository.Solution.Name}DbContext";
        public override GeneratorType GeneratorType => GeneratorType.EntityFrameworkDbContext;
        public override bool IsItemLevel => false;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
    }
}