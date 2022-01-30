using System;
using ToolBox.Bridge;
using ToolBox.Files;
using ToolBox.Notification;
using ToolBox.Platform;

namespace GarciaCore.Infrastructure;

public class ShellHelper : IShellHelper
{
    protected ShellConfigurator _shell { get; set; }
    protected DiskConfigurator _disk { get; set; }
    protected PathsConfigurator _path { get; set; }
    protected INotificationSystem _notificationSystem { get; set; }
    protected IBridgeSystem _bridgeSystem { get; set; }

    public ShellHelper()
    {
        _disk = new DiskConfigurator(FileSystem.Default);
        switch (OS.GetCurrent())
        {
            case "win":
                _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                break;
            case "mac":
                _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                break;
        }

        _notificationSystem = NotificationSystem.Default;
        switch (OS.GetCurrent())
        {
            case "win":
                _bridgeSystem = BridgeSystem.Bat;
                break;
            case "gnu":
                _bridgeSystem = BridgeSystem.Bash;
                break;
            case "mac":
                _bridgeSystem = BridgeSystem.Bash;
                break;
        }
        _shell = new ShellConfigurator(_bridgeSystem, _notificationSystem);
    }

    public Response RunInternalCommand(string command)
    {
        try
        {
            Response result = _shell.Term(command, Output.Internal);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public Response RunExternalCommand(string command)
    {
        try
        {
            Response result = _shell.Term(command, Output.External);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}