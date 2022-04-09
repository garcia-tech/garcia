using System.Collections.Generic;
using System.Linq;
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

        public ProjectGenerator()
        {
        }

        public Project Project { get; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Namespace { get; }
        public string BaseClass { get; }
        public IGenerator Generator { get; set; }

        public virtual async Task<List<GenerationResult>> Generate(Item item)
        {
            if (Generator == null)
            {
                throw new CodeGeneratorException("Generator cannot be null");
            }

            var results = new List<GenerationResult>();
            var code = await Generator.Generate(item, Namespace, BaseClass);
            var file = await Generator.GetFileName(item);

            if (Generator.IsMultiple)
            {
                var codes = code.Split("##");

                if (codes.Length > 0)
                {
                    var fileName = string.Empty;

                    for (int i = 1; i < codes.Length; i++)
                    {
                        if (i % 2 == 1)
                        {
                            fileName = codes[i];
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                var foldersAndFile = fileName.Split('\\');
                                var folder = fileName.Replace($"\\{foldersAndFile.Last()}", "");
                                var result2 = new GenerationResult($"{Folder}\\{folder}", Generator, $"{codes[0]}{codes[i]}", file.Replace($".{Generator.FileExtension}", $"{foldersAndFile.Last()}.{Generator.FileExtension}"));
                                results.Add(result2);
                            }

                            fileName = string.Empty;
                        }
                    }
                }
            }
            else
            {
                var generationResult = new GenerationResult(Folder, Generator, code, file);
                results.Add(generationResult);
            }

            return results;
        }
    }
}