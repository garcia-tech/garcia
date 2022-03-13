namespace GarciaCore.CodeGenerator
{
    //public interface IRepositoryGenerator<T> where T : BaseTemplate
    //{
    //    Task<string> Generate(Item item);
    //}

    public class RepositoryGenerator : Generator<RepositoryTemplate>
    {
        public override string DefaultBaseClass => "Repository";
        protected override string FileNamePostfix => "Repository";
    }
}