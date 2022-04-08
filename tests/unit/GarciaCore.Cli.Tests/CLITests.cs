using System;
using System.Text;
using GarciaCore.Cli;
using GarciaCore.CodeGenerator;
using GarciaCore.Infrastructure;
using Moq;
using TextCopy;
using ToolBox.Bridge;
using Xunit;

namespace Garcia.Cli.Tests;

public class ProgramTests
{
    private FakeConsoleWrapper _fakeConsoleWrapper;
    private Mock<IShellHelper> _shellHelperMock = new();
    private Mock<IClipboard> _clipboardMock = new();

    public ProgramTests()
    {
        _shellHelperMock.Setup(x => x.RunInternalCommand(It.IsAny<string>()))
            .Returns(new Response());
        _fakeConsoleWrapper = new FakeConsoleWrapper();
    }
    
    [Fact(DisplayName = "Show the options if nothing provided")]
    public void Test1()
    {
        string[] args = { };
        var cli = new CLI(_shellHelperMock.Object, null, _fakeConsoleWrapper, _clipboardMock.Object, args);
        
        cli.Run();

        Assert.Equal($"Options:{Environment.NewLine}\tmigrate{Environment.NewLine}\tmigrateandupdatedatabase{Environment.NewLine}\tgenerate{Environment.NewLine}", _fakeConsoleWrapper.ConsoleMessage);
    }
    
    [Theory(DisplayName = "When the option is migrate, run internal internal add migration command with migration name")]
    [InlineData("migrate", "add-migration Fake CLI")]
    [InlineData("migrateandupdatedatabase", "add-migration Fake CLI;update-database")]
    public void Test2(string arg, string expectedCommand)
    {
        string[] args = { arg };
        var cli = new FakeCLI(_shellHelperMock.Object, null, _fakeConsoleWrapper, _clipboardMock.Object, args);
        
        cli.Run();

        _shellHelperMock.Verify(x => x.RunInternalCommand(expectedCommand), Times.Once);
    }
    
    [Theory(DisplayName = "When the option is migrate, copy add migration command with migration name")]
    [InlineData("migrate", "add-migration Fake CLI")]
    [InlineData("migrateandupdatedatabase", "add-migration Fake CLI;update-database")]
    public void Test3(string arg, string expectedCommand)
    {
        string[] args = { arg };
        var cli = new FakeCLI(_shellHelperMock.Object, null, _fakeConsoleWrapper, _clipboardMock.Object, args);
        
        cli.Run();

        _clipboardMock.Verify(x => x.SetText(expectedCommand), Times.Once);
    }
    
    [Theory(DisplayName = "When the option is migrate, write add migration command with migration name and internal command result")]
    [InlineData("migrate", "add-migration Fake CLI")]
    [InlineData("migrateandupdatedatabase", "add-migration Fake CLI;update-database")]
    public void Test4(string arg, string expectedCommand)
    {
        string[] args = { arg };
        var cli = new FakeCLI(_shellHelperMock.Object, null, _fakeConsoleWrapper, _clipboardMock.Object, args);
        
        cli.Run();

        Assert.Equal($"{expectedCommand}{Environment.NewLine}ToolBox.Bridge.Response{Environment.NewLine}", _fakeConsoleWrapper.ConsoleMessage);
    }

    public void CreateMSTest(string[] args, string expectedCommand)
    {
        
    }
}

public class FakeConsoleWrapper : IConsoleWrapper
{
    private StringBuilder StringBuilder { get; set; }
    public string ConsoleMessage => StringBuilder.ToString();

    public FakeConsoleWrapper()
    {
        StringBuilder = new StringBuilder();
    }
    
    public void WriteLine(object text)
    {
        StringBuilder.AppendLine(text.ToString());
    }
}

public class FakeCLI : CLI
{
    public FakeCLI(IShellHelper shellHelper, ISolutionService solutionService, IConsoleWrapper consoleWrapper, IClipboard clipboard, string[] args) : base(shellHelper, solutionService, consoleWrapper, clipboard, args)
    {
    }
    
    internal override string CreateMigrationName()
    {
        return "Fake CLI";
    }
}