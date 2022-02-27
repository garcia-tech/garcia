using GarciaCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarciaCore.CodeGenerator
{
    public abstract class Generator : IGenerator
    {
        protected ItemPropertyType[] notNullablePropertyTypes = new ItemPropertyType[] { ItemPropertyType.String, ItemPropertyType.Class, ItemPropertyType.Unknown };

        public virtual List<IGenerator> Dependencies { get; set; } = new List<IGenerator>();

        public abstract string DefaultBaseClass { get; }

        public virtual async Task<string> Generate<T>(Item item, string @namespace, string baseClass) where T : BaseTemplate
        {
            var template = CreateItem<T>();
            template.Item = item;
            template.BaseClass = baseClass;
            template.Includes = baseClass;
            template.Namespace = @namespace;
            var text = template.TransformText();
            return text;
        }

        protected virtual T CreateItem<T>() where T : BaseTemplate
        {
            var item = Activator.CreateInstance<T>();
            return item;
        }

        protected virtual string GetInnerTypeClassName(string innerTypeName)
        {
            return innerTypeName;
        }

        public virtual string GetInnerTypeName(ItemProperty property, bool useCollections = true)
        {
            string typeName = "";

            switch (property.Type)
            {
                case ItemPropertyType.Integer:
                    typeName = "int";
                    break;
                case ItemPropertyType.Double:
                    typeName = "double";
                    break;
                case ItemPropertyType.Float:
                    typeName = "float";
                    break;
                case ItemPropertyType.Decimal:
                    typeName = "decimal";
                    break;
                case ItemPropertyType.DateTime:
                    typeName = "DateTime";
                    break;
                case ItemPropertyType.TimeSpan:
                    typeName = "TimeSpan";
                    break;
                case ItemPropertyType.String:
                    typeName = "string";
                    break;
                case ItemPropertyType.Char:
                    typeName = "char";
                    break;
                case ItemPropertyType.Boolean:
                    typeName = "bool";
                    break;
                case ItemPropertyType.Class:
                case ItemPropertyType.Enum:
                    string name = property.InnerType != null ? property.InnerType.Name : property.Name;

                    if (property.Type != ItemPropertyType.Enum)
                    {
                        name = this.GetInnerTypeClassName(name);
                    }

                    //if (useCollections)
                    //{
                    //    switch (property.MappingType)
                    //    {
                    //        case ItemPropertyMappingType.Property:
                    //            typeName = name;
                    //            break;
                    //        case ItemPropertyMappingType.List:
                    //            typeName = "List<" + name + ">";
                    //            break;
                    //        case ItemPropertyMappingType.Array:
                    //            typeName = name + "[]";
                    //            break;
                    //    }
                    //}
                    //else
                    {
                        typeName = name;
                    }
                    break;
            }

            if (property.IsNullable && !this.notNullablePropertyTypes.Contains(property.Type))
            {
                typeName = typeName + "?";
            }

            if (useCollections)
            {
                switch (property.MappingType)
                {
                    case ItemPropertyMappingType.Property:
                        break;
                    case ItemPropertyMappingType.List:
                        typeName = "List<" + typeName + ">";
                        break;
                    case ItemPropertyMappingType.Array:
                        typeName = typeName + "[]";
                        break;
                }
            }

            return typeName;
        }

        public virtual string GetIdTypeName(Item item)
        {
            switch (item.IdType)
            {
                case IdType.Int:
                    return "int";
                case IdType.Long:
                    return "long";
                case IdType.Guid:
                    return "Guid";
                case IdType.ObjectId:
                    return "string";
                default:
                    return string.Empty;
            }
        }

        public virtual string GetIdTypeAttributes(Item item)
        {
            switch (item.IdType)
            {
                case IdType.ObjectId:
                    return "string";
                default:
                    return string.Empty;
            }
        }

        public abstract Task<string> Generate(Item item, string @namespace, string baseClass);
    }

    public abstract class Generator<T> : Generator where T : BaseTemplate
    {
        public override async Task<string> Generate(Item item, string @namespace, string baseClass)
        {
            return await Generate<T>(item, @namespace, baseClass);
        }
    }

    public interface IGenerator
    {
        string DefaultBaseClass { get; }
        Task<string> Generate(Item item, string @namespace, string baseClass);
        List<IGenerator> Dependencies { get; set; }
    }

    public interface IGenerator<T> : IGenerator where T : BaseTemplate
    {
    }

    //public interface IEntityGenerator<T> : IGenerator<T> where T : BaseTemplate
    //{
    //}

    public class EntityGenerator : Generator<EntityTemplate>
    {
        public override string DefaultBaseClass => "Entity";
    }

    //public interface IRepositoryGenerator<T> where T : BaseTemplate
    //{
    //    Task<string> Generate(Item item);
    //}

    public class RepositoryGenerator : Generator<RepositoryTemplate>
    {
        public override string DefaultBaseClass => "Repository";
    }

    public class WebApiControllerGenerator : Generator<WebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
    }

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

    public class CodeGeneratorException : Exception
    {
        public CodeGeneratorException(string message) : base(message)
        {
        }

        public CodeGeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class Solution
    {
        public Solution(string name, string folder)
        {
            Name = name;
            Folder = folder;
        }

        public string Name { get; set; }
        public string Folder { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();

        public virtual async Task<List<GenerationResult>> Generate(Item item)
        {
            var generationResults = new List<GenerationResult>();

            foreach (var project in Projects)
            {
                var generationResult = await project.Generate(item);
                generationResults.AddRange(generationResult);
            }

            return generationResults;
        }
    }

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

    public class GenerationResult
    {
        public GenerationResult(string folder, IGenerator generator, string code)
        {
            Folder = folder;
            Generator = generator;
            Code = code;
        }

        public string Folder { get; set; }
        public IGenerator Generator { get; set; }
        public string Code { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

    //public class SolutionTemplate
    //{
    //    public List<ProjectTemplate> Projects { get; set; } = new List<ProjectTemplate>();
    //}

    //public class ProjectTemplate
    //{
    //    public List<IGenerator> Generators { get; set; } = new List<IGenerator>();
    //    public List<ProjectTemplate> ProjectDependencies { get; set; } = new List<ProjectTemplate>();
    //}

    public interface ISolutionService
    {
        Solution CreateSolution(string solutionJson);
        string GetSolutionJson(Solution solution);
        string GetSampleJson();
        Solution CreateSampleSolution();
    }

    public class SolutionService : ISolutionService
    {
        public Solution CreateSampleSolution()
        {
            var solution = new Solution("TestSolution", "c:\\files\\garciacoretest");

            var infrastructure = new Project("Infrastructure");
            infrastructure.AddGenerator("Entity", new EntityGenerator());
            solution.Projects.Add(infrastructure);

            var domain = new Project("Domain");
            domain.AddGenerator("Entity", new EntityGenerator());
            domain.AddGenerator("Repository", new RepositoryGenerator());
            domain.ProjectDependencies.Add(infrastructure);
            solution.Projects.Add(domain);

            var api = new Project("Api");
            api.AddGenerator("Controller", "Controllers", "ApiController", new WebApiControllerGenerator());
            api.ProjectDependencies.Add(infrastructure);
            api.ProjectDependencies.Add(domain);
            solution.Projects.Add(api);

            return solution;
        }

        public Solution CreateSolution(string solutionJson)
        {
            var solution = JsonConvert.DeserializeObject<Solution>(solutionJson);

            if (solution == null)
            {
                throw new CodeGeneratorException("Cannot convert json to solution, please check sample json.");
            }

            return solution;
        }

        public string GetSampleJson()
        {
            var solution = CreateSampleSolution();
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public string GetSolutionJson(Solution solution)
        {
            return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}