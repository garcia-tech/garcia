namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationDeleteCommandGenerator : CommandGenerator<CQRSApplicationDeleteCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
        protected override string FileNamePrefix => "Delete";
    }
}