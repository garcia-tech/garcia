using System.Collections.Generic;
using System.Threading.Tasks;
using GarciaCore.Application;

namespace GarciaCore.CodeGenerator
{
    public class Project
    {
        public Project(string name, string folder, string @namespace, ProjectType projectType)
        {
            Name = name;
            Folder = folder;
            Namespace = @namespace;
            ProjectType = projectType;
        }

        public Project(string name, string folder, ProjectType projectType) : this(name, folder, folder.Replace(" ", ""), projectType)
        {
        }

        public Project(string name, ProjectType projectType) : this(name, name, name.Replace(" ", ""), projectType)
        {
        }

        public Project()
        {
        }

        public string Name { get; set; }
        public string Folder { get; set; }
        public string Namespace { get; set; }
        public List<ProjectGenerator> Generators { get; set; } = new List<ProjectGenerator>();
        public List<Project> ProjectDependencies { get; set; } = new List<Project>();
        public ProjectType ProjectType { get; set; }

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
                var generationResult = await generator.Generate(item);
                generationResults.Add(generationResult);
            }

            return generationResults;
        }

        public void AddGenerator(string name, string folder, string @namespace, string baseClass, IGenerator generator)
        {
            this.Generators.Add(new ProjectGenerator(this, name, $"{Folder}\\{folder}".Trim('\\'), $"{Namespace}.{@namespace}".Trim('.'), baseClass, generator));
        }

        public void AddGenerator(string name, string @namespace, string baseClass, IGenerator generator)
        {
            this.Generators.Add(new ProjectGenerator(this, name, Folder, $"{Namespace}.{@namespace}".Trim('.'), baseClass, generator));
        }

        public void AddGenerator(string name, string folder, IGenerator generator)
        {
            this.Generators.Add(new ProjectGenerator(this, name, $"{Folder}\\{folder}".Trim('\\'), $"{Namespace}.{folder.Replace("\\", ".")}".Trim('.'), generator.DefaultBaseClass, generator));
        }
    }

    public enum ProjectType
    {
        ClassLibrary = 0,
        WebApi,
        Console
    }
}