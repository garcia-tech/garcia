namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationCommandGenerator<T> : Generator<T>
        where T : BaseTemplate
    {
        public override string DefaultBaseClass => "IRequest<int>";
        protected override string FileNamePostfix => "Command";
        public override GeneratorType GeneratorType => GeneratorType.Command;
    }
}