using System;
using System.Collections.Generic;
using System.Linq;
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
        protected internal Guid Uid { get; set; }

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
            
            foreach (var generator in Generators.Where(x => x.Generator.IsItemLevel && !x.Generator.IsApplicationGenerator()))
            {
                var generationResult = await generator.Generate(item);
                generationResults.Add(generationResult);
            }

            if (item.AddApplication)
            {
                foreach (var generator in Generators.Where(x => x.Generator.IsItemLevel && x.Generator.IsApplicationGenerator()))
                {
                    var generationResult = await generator.Generate(item);
                    generationResults.Add(generationResult);
                }
            }

            return generationResults;
        }

        public virtual async Task<List<GenerationResult>> Generate()
        {
            var generationResults = new List<GenerationResult>();

            foreach (var generator in Generators.Where(x => !x.Generator.IsItemLevel))
            {
                var generationResult = await generator.Generate(null);
                generationResults.Add(generationResult);
            }

            return generationResults;
        }

        public void AddGenerator(string name, string folder, string @namespace, string baseClass, IGenerator generator)
        {
            var projectGenerator = new ProjectGenerator(this, name, $"{Folder}\\{folder}".Trim('\\'), $"{Namespace}.{@namespace}".Trim('.'), baseClass, generator);
            this.Generators.Add(projectGenerator);
            GeneratorRepository.AddGenerator(projectGenerator.Generator);
        }

        public void AddGenerator(string name, string @namespace, string baseClass, IGenerator generator)
        {
            var projectGenerator = new ProjectGenerator(this, name, Folder, $"{Namespace}.{@namespace}".Trim('.'), baseClass, generator);
            this.Generators.Add(projectGenerator);
            GeneratorRepository.AddGenerator(projectGenerator.Generator);
        }

        public void AddGenerator(string name, string folder, IGenerator generator)
        {
            var projectGenerator = new ProjectGenerator(this, name, $"{Folder}\\{folder}".Trim('\\'), $"{Namespace}.{folder.Replace("\\", ".")}".Trim('.'), generator.DefaultBaseClass, generator);
            this.Generators.Add(projectGenerator);
            GeneratorRepository.AddGenerator(projectGenerator.Generator);
        }
    }
}