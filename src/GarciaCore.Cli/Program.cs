using System.Threading.Tasks;
using GarciaCore.CodeGenerator;
using GarciaCore.Infrastructure;
using TextCopy;

namespace GarciaCore.Cli
{
    class Program
    {
        private static ShellHelper _shellHelper = new ShellHelper();
        private static ISolutionService _solutionService = new SolutionService();
        private static IClipboard _clipboard = new Clipboard();

        static void Main(string[] args)
        {
            var cli = new CLI(_shellHelper, _solutionService, new SystemConsoleWrapper(), _clipboard, args);
            cli.Run();
        }
    }
}