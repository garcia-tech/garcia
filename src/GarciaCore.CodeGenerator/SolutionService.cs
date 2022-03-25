﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarciaCore.CodeGenerator
{
    public class SolutionService : ISolutionService
    {
        public async Task<Solution> CreateSampleSolutionAsync()
        {
            var solution = new Solution("TestSolution", "c:\\files\\garciacoretest", new List<string>(), "SQLServer");

            var infrastructure = new Project("TestSolution.Infrastructure", ProjectType.ClassLibrary);
            infrastructure.AddGenerator("Repository", "Repository", new RepositoryGenerator());
            solution.Projects.Add(infrastructure);

            var domain = new Project("TestSolution.Domain", ProjectType.ClassLibrary);
            domain.AddGenerator("Entity", "Entity", new EntityGenerator());
            domain.ProjectDependencies.Add(infrastructure);
            solution.Projects.Add(domain);

            var api = new Project("TestSolution.Api", ProjectType.WebApi);
            api.AddGenerator("Controller", "Controllers", "ApiController", new CQRSWebApiControllerGenerator());
            api.ProjectDependencies.Add(infrastructure);
            api.ProjectDependencies.Add(domain);
            solution.Projects.Add(api);

            var application = new Project("TestSolution.Application", ProjectType.ClassLibrary);
            application.AddGenerator("Queries", "Queries", new CQRSApplicationQueryGenerator());
            application.AddGenerator("CreateCommand", "Commands", new CQRSApplicationCreateCommandGenerator());
            application.AddGenerator("UpdateCommand", "Commands", new CQRSApplicationUpdateCommandGenerator());
            application.AddGenerator("DeleteCommand", "Commands", new CQRSApplicationDeleteCommandGenerator());
            application.AddGenerator("CreateCommandHandler", "Commands", new CQRSApplicationCreateCommandHandlerGenerator());
            application.AddGenerator("UpdateCommandHandler", "Commands", new CQRSApplicationUpdateCommandHandlerGenerator());
            application.AddGenerator("DeleteCommandHandler", "Commands", new CQRSApplicationDeleteCommandHandlerGenerator());
            solution.Projects.Add(application);

            return solution;
        }

        public async Task<SolutionModel> CreateSampleSolution2Async()
        {
            var model = new SolutionModel()
            {
                Name = "TestSolution",
                Folder = "c:\\files\\garciacoretest",
                Projects = new List<ProjectModel>()
                {
                    new ProjectModel()
                    {
                        Name = "TestSolution.Application",
                        // ProjectType = ProjectType.ClassLibrary.ToString(),
                        Generators = new List<ProjectGeneratorModel>
                        {
                            new ProjectGeneratorModel() { Name = "Commands", GeneratorName = GeneratorNames.CQRSApplicationCreateCommandGenerator } ,
                            new ProjectGeneratorModel() { Name = "Commands", GeneratorName = GeneratorNames.CQRSApplicationUpdateCommandGenerator }
                        }
                    },
                    new ProjectModel()
                    {
                        Name = "TestSolution.Api",
                        ProjectType = ProjectType.WebApi.ToString(),
                        Generators = new List<ProjectGeneratorModel>
                        {
                            new ProjectGeneratorModel() { Name = "Controllers", GeneratorName = GeneratorNames.CQRSWebApiControllerGenerator }
                        },
                        ProjectDependencies = new List<string>() { "TestSolution.Application" }
                    }
                }
            };

            return model;
        }

        public async Task<List<Item>> CreateItemsAsync(string itemsJson)
        {
            var itemsModel = JsonSerializer.Deserialize<ItemsModel>(itemsJson);
            var messages = new List<string>();

            if (itemsModel == null)
                throw new CodeGeneratorException("Cannot convert json to ItemsModel, please check sample json.");

            var items = new List<Item>();

            foreach (var itemModel in itemsModel.Items)
            {
                if (string.IsNullOrEmpty(itemModel.IdType))
                    itemModel.IdType = IdType.Int.ToString();

                Enum.TryParse(itemModel.IdType, out IdType idType);
                var item = new Item(itemModel.Name, itemModel.IsEnum, idType, itemModel.AddApplication);

                foreach (var propertyModel in itemModel.Properties)
                {
                    if (string.IsNullOrEmpty(propertyModel.Type))
                        propertyModel.Type = ItemPropertyType.String.ToString();

                    if (string.IsNullOrEmpty(propertyModel.MappingType))
                        propertyModel.MappingType = ItemPropertyMappingType.Property.ToString();

                    Enum.TryParse(propertyModel.Type, out ItemPropertyType itemPropertyType);
                    Enum.TryParse(propertyModel.MappingType, out ItemPropertyMappingType itemPropertyMappingType);
                    Item innerItem = null;

                    if (!string.IsNullOrEmpty(propertyModel.InnerType))
                    {
                        innerItem = new Item(propertyModel.InnerType, false, item.IdType, false);
                    }

                    var itemProperty = new ItemProperty(propertyModel.Name, itemPropertyType, itemPropertyMappingType, innerItem);
                    item.Properties.Add(itemProperty);
                }

                items.Add(item);
            }

            return items;
        }

        public async Task<SolutionGenerationResult> CreateSolutionAsync(string solutionJson)
        {
            //var solution = JsonConvert.DeserializeObject<Solution>(solutionJson);
            var solutionModel = JsonSerializer.Deserialize<SolutionModel>(solutionJson);
            var messages = new List<string>();

            if (solutionModel == null)
                throw new CodeGeneratorException("Cannot convert json to SolutionModel, please check sample json.");

            var solution = new Solution(solutionModel.Name, solutionModel.Folder, solutionModel.Integrations, solutionModel.DefaultDatabaseServer);
            var allProjects = new List<Project>();

            if (solutionModel.Projects != null)
            {
                foreach (var projectModel in solutionModel.Projects)
                {
                    if (!Enum.TryParse(projectModel.ProjectType, out ProjectType projectType))
                    {
                        messages.Add($"String \"{projectModel.ProjectType}\" could not be converted to ProjectType for project {projectModel.Name}. ProjectType.ClassLibrary will be used.");
                    }

                    if (string.IsNullOrEmpty(projectModel.Folder))
                    {
                        projectModel.Folder = projectModel.Name;
                    }

                    var project = new Project(projectModel.Name, $"{solutionModel.Folder}\\{projectModel.Folder}".TrimEnd('\\'), projectModel.Namespace, projectType);
                    project.Uid = projectModel.Uid;
                    solution.Projects.Add(project);
                    allProjects.Add(project);

                    if (projectModel.Generators != null)
                    {
                        foreach (var generatorModel in projectModel.Generators)
                        {
                            var type = Type.GetType($"GarciaCore.CodeGenerator.{generatorModel.GeneratorName}");

                            if (type == null)
                            {
                                messages.Add($"Generator with name {generatorModel.GeneratorName} could not be found in assembly GarciaCore.CodeGenerator.");
                            }
                            else
                            {
                                var generator = Activator.CreateInstance(type) as IGenerator;
                                var baseClass = !string.IsNullOrEmpty(generatorModel.BaseClass) ? generatorModel.BaseClass : generator.DefaultBaseClass;
                                var @namespace = !string.IsNullOrEmpty(projectModel.Namespace) ? projectModel.Namespace : projectModel.Name;
                                @namespace = $"{@namespace}.{generatorModel.Name}".TrimEnd('.');
                                project.AddGenerator(generatorModel.Name, generatorModel.Name.Replace('.', '\\'), @namespace, baseClass, generator);
                            }
                        }
                    }
                }

                Dictionary<Guid, List<string>> namespaces = new Dictionary<Guid, List<string>>();

                foreach (var projectModel in solutionModel.Projects)
                {
                    if (projectModel.ProjectDependencies != null)
                    {
                        var projectNamespaces = new List<string>();

                        foreach (var projectDependency in projectModel.ProjectDependencies)
                        {
                            var dependentProject = allProjects.FirstOrDefault(x => x.Name == projectDependency);

                            if (dependentProject == null)
                            {
                                messages.Add($"Project with name {projectDependency} could not be found in projects.");
                            }
                            else
                            {
                                var project = allProjects.FirstOrDefault(x => x.Uid == projectModel.Uid);
                                if (project == null)
                                    messages.Add($"Project with Uid {projectModel.Uid} could not be found in projects.");
                                else
                                    project.ProjectDependencies.Add(dependentProject);

                                foreach (var dependentProjectGenerator in dependentProject.Generators)
                                {
                                    if (!projectNamespaces.Contains(dependentProjectGenerator.Namespace))
                                        projectNamespaces.Add(dependentProjectGenerator.Namespace);
                                }
                            }
                        }

                        namespaces.Add(projectModel.Uid, projectNamespaces);
                    }
                    else
                    {
                        namespaces.Add(projectModel.Uid, new List<string>());
                    }
                }

                foreach (var projectModel in solutionModel.Projects)
                {
                    var project = allProjects.FirstOrDefault(x => x.Uid == projectModel.Uid);
                    var projectNamespaces = new List<string>();

                    foreach (var projectGenerator in project.Generators)
                    {
                        if (!projectNamespaces.Contains(projectGenerator.Namespace))
                            projectNamespaces.Add(projectGenerator.Namespace);
                    }

                    foreach (var @namespace in projectNamespaces)
                    {
                        if (!namespaces[project.Uid].Contains(@namespace))
                        {
                            namespaces[project.Uid].Add(@namespace);
                        }
                    }
                }

                foreach (var project in allProjects)
                {
                    var usings = namespaces.FirstOrDefault(x => x.Key == project.Uid).Value;

                    if (usings.Count != 0)
                    {
                        foreach (var generator in project.Generators)
                        {
                            generator.Generator.Usings = usings;
                        }
                    }
                }
            }

            return new SolutionGenerationResult(solution, messages);
        }

        public async Task<string> GetSampleJsonAsync()
        {
            return "";
            //var solution = await CreateSampleSolutionAsync();
            //return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<string> GetSolutionJsonAsync(Solution solution)
        {
            //return "";
            return JsonSerializer.Serialize(solution, new JsonSerializerOptions() { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve });
            //return JsonConvert.SerializeObject(solution, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}