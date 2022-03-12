namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCreateCommandGenerator : Generator<CQRSApplicationCreateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }
}