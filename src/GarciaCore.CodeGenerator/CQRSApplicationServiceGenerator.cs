namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationServiceGenerator : Generator<CQRSApplicationServiceTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "Service";
        public override GeneratorType GeneratorType => GeneratorType.Service;
    }
}