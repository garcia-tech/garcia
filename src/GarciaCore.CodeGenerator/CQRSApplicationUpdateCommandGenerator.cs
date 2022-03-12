namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationUpdateCommandGenerator : CommandGenerator<CQRSApplicationUpdateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
        protected override string FileNamePrefix => "Update";
    }
}