namespace GarciaCore.CodeGenerator
{
    public class CQRSApplicationQueryGenerator : Generator<CQRSApplicationQueryTemplate>
    {
        public override string DefaultBaseClass => "";
        protected override string FileNamePostfix => "Query";
        public override GeneratorType GeneratorType => GeneratorType.Query;
    }
}