namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationDeleteCommandGenerator : Generator<CQRSApplicationDeleteCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }
}