namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCommandHandlerGenerator<T> : Generator<T>
      where T : BaseTemplate
    {
        public override string DefaultBaseClass => string.Empty;

        protected override string FileNamePostfix => "CommandHandler";
    }
}