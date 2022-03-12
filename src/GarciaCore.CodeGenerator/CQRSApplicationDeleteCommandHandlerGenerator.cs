namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationDeleteCommandHandlerGenerator : CommandHandlerGenerator<CQRSApplicationDeleteCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePrefix => "Delete";
    }
}