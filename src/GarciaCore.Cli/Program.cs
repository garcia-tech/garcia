using GarciaCore.Infrastructure;
using TextCopy;
using ToolBox.Bridge;
using System;

namespace MigrationNameGenerator
{
    class Program
    {
        private static ShellHelper _shellHelper = new ShellHelper();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: gr [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("\tmigrate");
                Console.WriteLine("\tmigrateandupdatedatabase");
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
    }
}
