using System;
using GarciaCore.CodeGenerator;
using GarciaCore.Infrastructure;
using TextCopy;
using ToolBox.Bridge;

namespace GarciaCore.Cli;

public class CLI
{
    private readonly IShellHelper _shellHelper;
    private readonly ISolutionService _solutionService;
    private readonly string[] _args;
    private readonly IConsoleWrapper _consoleWrapper;
    private readonly IClipboard _clipboard;

    public CLI(IShellHelper shellHelper, 
        ISolutionService solutionService,
        IConsoleWrapper consoleWrapper,
        IClipboard clipboard,
        string[] args )
    {
        _shellHelper = shellHelper;
        _solutionService = solutionService;
        _consoleWrapper = consoleWrapper;
        _clipboard = clipboard;
        _args = args;
    }
        
    public void Run()
    {
        if (_args.Length == 0)
        {
            _consoleWrapper.WriteLine("Options:");
            _consoleWrapper.WriteLine("\tmigrate");
            _consoleWrapper.WriteLine("\tmigrateandupdatedatabase");
            _consoleWrapper.WriteLine("\tgenerate");
            return;
        }

        switch (_args[0])
        {
            case "migrate":
            case "migrateandupdatedatabase":
                RunMigration(_args[0]);
                break;
            case "generate":
                var item = new Item()
                {
                    Name = "Test",
                    Properties = new System.Collections.Generic.List<ItemProperty>()
                    {
                        new ItemProperty(){Name = "Test property", Type = ItemPropertyType.String, MappingType = ItemPropertyMappingType.Property },
                        new ItemProperty(){Name = "Test property list", Type = ItemPropertyType.Integer, MappingType = ItemPropertyMappingType.List }
                    }
                };
                //var text = Generate(item);
                //Console.WriteLine(text);
                break;
        }
    }

    private void RunMigration(string option)
    {
        var updateDatabase = option == "migrateandupdatedatabase";
        var migrationName = CreateMigrationName();
        var command = CreateCommand(migrationName, updateDatabase);
        CopyCommand(command);
        RunCommand(command);
    }

    private void CopyCommand(string command)
    {
        _consoleWrapper.WriteLine(command);
        _clipboard.SetText(command);
    }

    private void RunCommand(string command)
    {
        Response response = _shellHelper.RunInternalCommand(command);
        _consoleWrapper.WriteLine(response);
    }

    string CreateCommand(string migrationName, bool updateDatabase)
    {
        var command = "add-migration " + migrationName;

        if (updateDatabase)
        {
            command += ";update-database";
        }
        
        return command;
    }

    internal virtual string CreateMigrationName() 
        => "Migrations_" + Guid.NewGuid().ToString().Replace("-", "")
                                                    .Substring(0, 5);
}