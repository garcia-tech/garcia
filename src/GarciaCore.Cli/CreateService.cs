using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GarciaCore.CodeGenerator;
using GarciaCore.Infrastructure;
using ToolBox.Bridge;

namespace MigrationNameGenerator;

public class CreateService
{
    private ShellHelper _shellHelper = new ShellHelper();
    private string dotnetNewPrefix = "dotnet new ";
    private string newSolutionPrefix = "sln --name ";
    private string newClassLibPrefix = "classlib -o src/";
    private string newConsolePrefix = "console -o src/";
    private string newApiPrefix = "webapi -o src/";
    private string newUnitTestPrefix = "nunit -o src/";


    public async Task<bool> CreateMicroService(string templatePath)
    {
        var _solutionService = new SolutionService();
        var templateJson = File.ReadAllText(templatePath);
        var solution = await _solutionService.CreateSolutionAsync(templateJson);
        var solutionName = solution.Solution.Name;

        var items = await GetItemsFromTemplate(null);
        var genrationResult = await solution.Solution.Generate(items);

        try
        {
            CreateSolutionFile(solutionName);

            if (solution.Solution.Projects != null)
            {
                foreach (var project in solution.Solution.Projects)
                {
                    CreateProject(solutionName, project.Name, project.ProjectType);
                }

                foreach (var project in solution.Solution.Projects)
                {
                    foreach (var dependency in project.ProjectDependencies)
                    {
                        AddProjectReference(project.Name, dependency.Name);
                    }
                }

                foreach (var result in genrationResult)
                {
                    Directory.CreateDirectory(result.Folder);
                    await File.WriteAllTextAsync($"{result.Folder}\\{result.File}", result.Code);
                }


                // CreateProject(solutionName, project.Name, project.ProjectType);
            }

            // CreateProject(solutionName, "Domain", ProjectTypeEnum.ClassLib);
            // CreateProject(solutionName, "Infrastructure", ProjectTypeEnum.ClassLib);
            // CreateProject(solutionName, "Application", ProjectTypeEnum.ClassLib);
            // CreateProject(solutionName, "Api", ProjectTypeEnum.WebApi);
            // CreateProject(solutionName, "Test", ProjectTypeEnum.NUnit);
        }
        catch (System.Exception ex)
        {
            return false;
        }

        return true;
    }

    private Response CreateSolutionFile(string projectName)
    {
        var commandText = dotnetNewPrefix + newSolutionPrefix + projectName;
        return RunCommand(commandText);
    }

    private Response CreateProject(string solutionName, string projectName,
        ProjectType projectType)
    {
        var commandText = dotnetNewPrefix + GetProjectTypePrefix(projectType) + projectName;
        var result = RunCommand(commandText);

        var slnCommand = $"dotnet sln {solutionName}.sln add --solution-folder {projectName}.csproj";
        RunCommand(slnCommand);

        return result;
    }

    private string GetCsProj(string projectName) => $"{projectName}/{projectName}.csproj";

    private Response AddProjectReference(string source, string reference)
    {
        var command = $"dotnet add {GetCsProj(source)} reference {GetCsProj(reference)}";
        var result = RunCommand(command);
        return result;
    }

    private string GetProjectTypePrefix(ProjectType projectType)
    {
        var prefix = string.Empty;

        switch (projectType)
        {
            case ProjectType.ClassLibrary:
                prefix = newClassLibPrefix;
                break;
            case ProjectType.WebApi:
                prefix = newApiPrefix;
                break;
            case ProjectType.Test:
                prefix = newUnitTestPrefix;
                break;
            case ProjectType.Console:
                prefix = newConsolePrefix;
                break;
        }

        return prefix;
    }

    private async Task<List<Item>> GetItemsFromTemplate(string path)
    {
        // TODO : Mock test items. 
        var items = new List<Item>()
        {
            new Item()
            {
                Name = "Content",
                IdType = IdType.Int,
                Properties = new List<ItemProperty>()
                {
                    new ItemProperty()
                    {
                        Name = "Date", Type = ItemPropertyType.DateTime, MappingType = ItemPropertyMappingType.Property
                    },
                    new ItemProperty()
                    {
                        Name = "Items", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.List,
                        InnerType = new Item() {Name = "ContentItem"}
                    },
                }
            },
            new Item()
            {
                Name = "ContentItem",
                IdType = IdType.Int,
                Properties = new List<ItemProperty>()
                {
                    new ItemProperty()
                    {
                        Name = "Date", Type = ItemPropertyType.DateTime, MappingType = ItemPropertyMappingType.Property
                    },
                    new ItemProperty()
                    {
                        Name = "Files", Type = ItemPropertyType.Class, MappingType = ItemPropertyMappingType.List,
                        InnerType = new Item() {Name = "File"}
                    },
                }
            },
            new Item()
            {
                Name = "File",
                IdType = IdType.Int,
                Properties = new List<ItemProperty>()
                {
                    new ItemProperty()
                        {Name = "Url", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property},
                    new ItemProperty()
                    {
                        Name = "Description", Type = ItemPropertyType.String,
                        MappingType = ItemPropertyMappingType.Property
                    },
                }
            }
        };

        return items;
    }

    private Response RunCommand(string commandText)
    {
        var response = _shellHelper.RunExternalCommand(commandText);
        return response;
    }
}