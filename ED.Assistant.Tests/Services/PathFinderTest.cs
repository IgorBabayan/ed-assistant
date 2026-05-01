using ED.Assistant.Data.Services.Path;

namespace ED.Assistant.Tests.Services;

[TestClass]
public sealed class PathFinderTest
{
    [TestMethod]
    public void WindowsPathToLog_Should_Return_EliteDangerous_Log_Folder()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
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
        StringAssert.Contains(actual, Path.Combine("Saved Games", "Frontier Developments", "Elite Dangerous"));
    }

    [TestMethod]
    public void LinuxPathToLog_Should_Throw_NotImplementedException()
    {
        var resolver = new LinuxPathResolver();
        Assert.Throws<NotImplementedException>(() => resolver.GetLogsPath());
        Assert.Throws<NotImplementedException>(() => resolver.GetConfigPath());
    }

    [TestMethod]
    public void MacOSPathToLog_Should_Throw_NotImplementedException()
    {
        var resolver = new MacPathResolver();
        Assert.Throws<NotImplementedException>(() => resolver.GetLogsPath());
        Assert.Throws<NotImplementedException>(() => resolver.GetConfigPath());
    }

	[TestMethod]
    public void GetConfigPath_Should_Return_EliteDangerous_Config_File()
    {
		var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		var expected = Path.Combine(localAppData, "ED Assistant", "config.json");
		var resolver = new WindowsPathResolver();
		var actual = resolver.GetConfigPath();

		Assert.AreEqual(expected, actual);
	}
}