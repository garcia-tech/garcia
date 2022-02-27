using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public abstract class Generator<T> : Generator where T : BaseTemplate
    {
        public override async Task<string> Generate(Item item, string @namespace, string baseClass)
        {
            return await Generate<T>(item, @namespace, baseClass);
        }
    }
}