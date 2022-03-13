namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationDeleteCommandHandlerGenerator : CQRSApplicationCommandHandlerGenerator<CQRSApplicationDeleteCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Delete";
    }
}