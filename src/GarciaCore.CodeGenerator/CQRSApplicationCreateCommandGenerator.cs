namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCreateCommandGenerator : CQRSApplicationCommandGenerator<CQRSApplicationCreateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";

        protected override string FileNamePrefix => "Create";
    }
}