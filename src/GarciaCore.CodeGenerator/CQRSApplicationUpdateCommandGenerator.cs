namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationUpdateCommandGenerator : CQRSApplicationCommandGenerator<CQRSApplicationUpdateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
        protected override string FileNamePrefix => "Update";
    }
}