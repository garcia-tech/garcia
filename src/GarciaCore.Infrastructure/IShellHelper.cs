using ToolBox.Bridge;

namespace GarciaCore.Infrastructure
{
    public interface IShellHelper
    {
        Response RunExternalCommand(string command);
        Response RunInternalCommand(string command);
    }
}
