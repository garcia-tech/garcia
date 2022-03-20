using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class ApplicationServiceRegistrationGenerator : Generator<ApplicationServiceRegistrationTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "ApplicationServiceRegistration";
        public override GeneratorType GeneratorType => GeneratorType.ApplicationServiceRegistration;
        public override bool IsItemLevel => false;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
    }
}