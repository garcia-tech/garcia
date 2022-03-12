namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCreateCommandGenerator : CommandGenerator<CQRSApplicationCreateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";

        protected override string FileNamePrefix => "Create";
    }

    public class CommandGenerator<T> : Generator<T>
        where T : BaseTemplate
    {
        public override string DefaultBaseClass => "IRequest<int>";

        protected override string FileNamePostfix => "Command";
    }

    public class CommandHandlerGenerator<T> : Generator<T>
      where T : BaseTemplate
    {
        public override string DefaultBaseClass => string.Empty;

        protected override string FileNamePostfix => "CommandHandler";
    }
}