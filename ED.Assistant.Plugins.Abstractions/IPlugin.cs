using Avalonia.Controls;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Plugins;

public interface IPlugin
{
    string Id { get; }
    string Name { get; }
    Version Version { get; }

    void ConfigureServices(IServiceCollection services, IPluginContext context);

    IEnumerable<PluginPage> GetPages();
}

public sealed record PluginPage(
    string Title,
    MaterialIconKind Icon,
    Type ViewModelType,          // must implement IPluginPageViewModel, registered in ConfigureServices
    Func<Control> CreateView);

public interface IPluginPageViewModel
{
    /// <summary>Called on a background thread. Marshal UI changes to Dispatcher.UIThread.</summary>
    Task OnJournalChangedAsync(PluginJournalSnapshot snapshot, CancellationToken ct);
    void OnNavigatedTo() { }
    void OnNavigatedFrom() { }
}

public interface IPluginContext
{
    string PluginDirectory { get; }
    string DataDirectory { get; }   // writable, per-plugin

    /// <summary>
    /// The host's SQLite database. Plugins may add their own tables to it, but must keep
    /// their own migrations history table so they never touch the host's migrations.
    /// </summary>
    string DatabasePath { get; }
}

public sealed record PluginJournalSnapshot(
    string? Commander, string? Ship, string? FileName, string? LastEventName);
