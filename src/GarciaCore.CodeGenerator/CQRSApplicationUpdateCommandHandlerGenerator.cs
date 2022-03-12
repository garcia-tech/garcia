namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationUpdateCommandHandlerGenerator : CQRSApplicationCommandHandlerGenerator<CQRSApplicationUpdateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Update";
    }
}