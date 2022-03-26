using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class SolutionJsonGenerator : Generator<SolutionJsonTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "SolutionTemplate";
        public override GeneratorType GeneratorType => GeneratorType.SolutionJson;
        public override bool IsItemLevel => false;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
    }
}