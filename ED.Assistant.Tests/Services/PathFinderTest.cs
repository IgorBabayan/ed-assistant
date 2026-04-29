using ED.Assistant.Data;
using ED.Assistant.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Tests.Services;

[TestClass]
public sealed class PathFinderTest
{
    private IPathFinder _pathFinder = default!;

    [TestInitialize]
    public void Setup()
    {
        // Create a service collection like the application does and register data services.
        var services = new ServiceCollection();
        // Register services from the data project (this registers IPathFinder -> PathFinder)
        services.RegisterServices();

        // ED.Assistant.Data.PathFinder is internal; construct it directly in tests and
        // register a factory so the DI container can resolve it here.
        services.AddSingleton<IPathFinder>(_ => new PathFinder());

        var provider = services.BuildServiceProvider();
        _pathFinder = provider.GetRequiredService<IPathFinder>();
    }

    [TestMethod]
    public void GetPathToLogs_Should_Return_Path()
    {
        var path = _pathFinder.GetPathToLogs();
        Assert.IsNotNull(path);
        
    }

    /*[TestMethod]
    public void GetPathToLogs_Should_Return_Windows_Path()
    {

    }*/
}
