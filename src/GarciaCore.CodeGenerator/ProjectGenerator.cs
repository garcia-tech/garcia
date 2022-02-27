using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public class ProjectGenerator
    {
        public ProjectGenerator(Project project, string name, string folder, string @namespace, string baseClass, IGenerator generator)
        {
            Project = project;
            Name = name;
            Folder = folder;
            Namespace = @namespace;
            Generator = generator;
            BaseClass = baseClass;
        }

        public ProjectGenerator(Project project, string name, IGenerator generator) : this(project, name, name, name.Replace(" ", ""), generator.DefaultBaseClass, generator)
        {
        }

        public Project Project { get; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Namespace { get; }
        public string BaseClass { get; }
        public IGenerator Generator { get; set; }

        public virtual async Task<GenerationResult> Generate(Item item, string @namespace)
        {
            if (Generator == null)
            {
                throw new CodeGeneratorException("Generator cannot be null");
            }

            var code = await Generator.Generate(item, @namespace, BaseClass);
            var generationResult = new GenerationResult(Folder, Generator, code);
            return generationResult;
        }
    }
}