namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationDeleteCommandGenerator : CQRSApplicationCommandGenerator<CQRSApplicationDeleteCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
        protected override string FileNamePrefix => "Delete";
    }
}