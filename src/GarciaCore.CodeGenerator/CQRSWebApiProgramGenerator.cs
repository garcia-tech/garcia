namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiProgramGenerator : Generator<CQRSWebApiProgramTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "Program";
        public override GeneratorType GeneratorType => GeneratorType.Program;
        public override bool IsItemLevel => false;
    }
}