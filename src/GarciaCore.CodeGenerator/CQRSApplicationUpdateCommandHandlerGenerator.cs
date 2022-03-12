namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationUpdateCommandHandlerGenerator : CommandHandlerGenerator<CQRSApplicationUpdateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Update";
    }
}