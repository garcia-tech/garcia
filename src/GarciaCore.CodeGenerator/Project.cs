using System.Collections.Generic;
using System.Threading.Tasks;
using GarciaCore.Application;

namespace GarciaCore.CodeGenerator
{
    public class Project
    {
        public Project(string name, string folder, string @namespace)
        {
            Name = name;
            Folder = folder;
            Namespace = @namespace;
        }

        public Project(string name, string folder) : this(name, folder, folder.Replace(" ", ""))
        {
        }

        public Project(string name) : this(name, name, name.Replace(" ", ""))
        {
        }

        public string Name { get; set; }
        public string Folder { get; set; }
        public string Namespace { get; set; }
        public List<ProjectGenerator> Generators { get; set; } = new List<ProjectGenerator>();
        public List<Project> ProjectDependencies { get; set; } = new List<Project>();

        public virtual ValidationResults Validate()
        {
            ValidationResults validationResults = new ValidationResults();

            foreach (var item in Generators)
            {
            }

            foreach (var item in ProjectDependencies)
            {
            }

            return validationResults;
        }

        public virtual async Task<List<GenerationResult>> Generate(Item item)
        {
            var generationResults = new List<GenerationResult>();

            foreach (var generator in Generators)
            {
                var generationResult = await generator.Generate(item, Namespace);
                generationResults.Add(generationResult);
            }

            return generationResults;
        }

        public void AddGenerator(string name, string @namespace, string baseClass, IGenerator generator)
        {
            this.Generators.Add(new ProjectGenerator(this, name, Folder, @namespace, baseClass, generator));
        }

        public void AddGenerator(string name, IGenerator generator)
        {
            this.Generators.Add(new ProjectGenerator(this, name, generator));
        }
    }
}