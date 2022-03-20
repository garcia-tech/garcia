using System.Collections.Generic;

namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiControllerGenerator : Generator<CQRSWebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
        protected override string FileNamePostfix => "Controller";
        public override GeneratorType GeneratorType => GeneratorType.Api;
        public override List<string> GarciaCoreDependencies => new List<string>() { "GarciaCore.Application" };
    }
}