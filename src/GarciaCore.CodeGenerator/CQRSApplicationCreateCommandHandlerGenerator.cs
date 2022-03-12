namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCreateCommandHandlerGenerator : CommandHandlerGenerator<CQRSApplicationCreateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Create";
    }
}