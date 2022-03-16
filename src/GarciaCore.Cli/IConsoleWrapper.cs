using System;

namespace GarciaCore.Cli;

public interface IConsoleWrapper
{
    void WriteLine(object text);
}

class SystemConsoleWrapper : IConsoleWrapper
{
    public void WriteLine(object text)
    {
        Console.WriteLine(text);
    }
}