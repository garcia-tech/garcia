using ToolBox.Bridge;

namespace Garcia.Infrastructure
{
    public interface IShellHelper
    {
        Response RunExternalCommand(string command);
        Response RunInternalCommand(string command);
    }
}