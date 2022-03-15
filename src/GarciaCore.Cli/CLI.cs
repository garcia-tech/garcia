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
                var migrationName1 = CreateAndCopyMigrationName(false);
                Response result1 = _shellHelper.RunInternalCommand(migrationName1);
                _consoleWrapper.WriteLine(result1);
                break;
            case "migrateandupdatedatabase":
                var migrationName2 = CreateAndCopyMigrationName(true);
                Response result2 = _shellHelper.RunInternalCommand(migrationName2);
                _consoleWrapper.WriteLine(result2);
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
            default:
                break;
        }
    }

    static void CreateMigration()
    {
        var migrationName = "Migrations_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
        Console.WriteLine(migrationName);
        Clipboard clipboard = new();
        clipboard.SetText("add-migration " + migrationName + ";update-database");
    }

    string CreateAndCopyMigrationName(bool updateaDatabase)
    {
        var migrationName = CreateMigrationName();
        var text = "add-migration " + migrationName;

        if (updateaDatabase)
        {
            text += ";update-database";
        }

        _consoleWrapper.WriteLine(text);
        _clipboard.SetText(text);
        return text;
    }

    internal virtual string CreateMigrationName()
    {
        var migrationName = "Migrations_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
        return migrationName;
    }
}