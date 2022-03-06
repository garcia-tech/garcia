﻿using GarciaCore.CodeGenerator;
using GarciaCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextCopy;
using ToolBox.Bridge;

namespace MigrationNameGenerator
{
    class Program
    {
        private static ShellHelper _shellHelper = new ShellHelper();
        private static ISolutionService _solutionService = new SolutionService();

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("\tmigrate");
                Console.WriteLine("\tmigrateandupdatedatabase");
                Console.WriteLine("\tgenerate");
                return;
            }

            switch (args[0])
            {
                case "migrate":
                    var migrationName1 = CreateAndCopyMigrationName(false);
                    Response result1 = _shellHelper.RunInternalCommand(migrationName1);
                    Console.WriteLine(result1);
                    break;
                case "migrateandupdatedatabase":
                    var migrationName2 = CreateAndCopyMigrationName(true);
                    Response result2 = _shellHelper.RunInternalCommand(migrationName2);
                    Console.WriteLine(result2);
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

        static string CreateMigrationName()
        {
            var migrationName = "Migrations_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
            return migrationName;
        }

        static string CreateAndCopyMigrationName(bool updateaDatabase)
        {
            var migrationName = CreateMigrationName();
            var text = "add-migration " + migrationName;

            if (updateaDatabase)
            {
                text += ";update-database";
            }

            Console.WriteLine(text);
            Clipboard clipboard = new();
            clipboard.SetText(text);
            return text;
        }

        //static string Generate(Item item)
        //{
        //    var generator = new EntityGenerator();
        //    var text = generator.Generate(item);
        //    var generator2 = new RepositoryGenerator();
        //    return text + "\n\n" + generator2.Generate(item);
        //}
    }
}