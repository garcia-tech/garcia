namespace GarciaCore.CodeGenerator
{

    //public interface IEntityGenerator<T> : IGenerator<T> where T : BaseTemplate
    //{
    //}

    public class EntityGenerator : Generator<EntityTemplate>
    {
        public override string DefaultBaseClass => "Entity";
        protected override string FileNamePostfix => "";
    }
}