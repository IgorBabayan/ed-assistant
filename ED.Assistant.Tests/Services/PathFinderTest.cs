using ED.Assistant.Data;
using ED.Assistant.Data.Services.Path;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Tests.Services;

[TestClass]
public sealed class PathFinderTest
{
    [TestInitialize]
    public void Setup()
    {
        
    }

    [TestMethod]
    public void WindowsPathToLog_Should_Return_EliteDangerous_Log_Folder()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var expected = Path.Combine(localAppData, "Saved Games", "Frontier Developments", "Elite Dangerous");
        var resolver = new WindowsPathResolver();
        var actual = resolver.GetLogsPath();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetPathToLogs_Should_Return_Path_On_Windows()
    {
        var resolver = new WindowsPathResolver();
        var actual = resolver.GetLogsPath();

        Assert.IsFalse(string.IsNullOrWhiteSpace(actual));
        StringAssert.Contains(actual, "Elite Dangerous");
    }

    [TestMethod]
    public void LinuxPathToLog_Should_Throw_NotImplementedException()
    {
        var resolver = new LinuxPathResolver();
        Assert.Throws<NotImplementedException>(() => resolver.GetLogsPath());
    }

    [TestMethod]
    public void MacOSPathToLog_Should_Throw_NotImplementedException()
    {
        var resolver = new MacPathResolver();
        Assert.Throws<NotImplementedException>(() => resolver.GetLogsPath());
    }
}