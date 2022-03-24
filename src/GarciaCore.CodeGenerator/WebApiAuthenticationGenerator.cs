using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class WebApiAuthenticationGenerator : Generator<WebApiAuthenticationTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "AuthenticationController";
        public override GeneratorType GeneratorType => GeneratorType.WebApiAuthentication;
        public override bool IsItemLevel => false;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
        public override string Name => "AuthenticationController";
    }
}