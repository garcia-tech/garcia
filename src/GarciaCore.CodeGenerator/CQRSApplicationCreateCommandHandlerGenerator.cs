namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCreateCommandHandlerGenerator : CQRSApplicationCommandHandlerGenerator<CQRSApplicationCreateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Create";
    }
}