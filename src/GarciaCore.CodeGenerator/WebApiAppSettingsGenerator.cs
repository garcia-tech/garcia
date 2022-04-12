using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class WebApiAppSettingsGenerator : Generator<WebApiAppSettingsTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "appsettings";
        public override GeneratorType GeneratorType => GeneratorType.WebApiAppSettings;
        public override bool IsItemLevel => false;
        public override List<string> GarciaCoreDependencies => new List<string>() { };
        public override string Name => "appsettings";
        public override string FileExtension => "json";
    }
}