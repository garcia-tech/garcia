namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationUpdateCommandGenerator : Generator<CQRSApplicationUpdateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }
}