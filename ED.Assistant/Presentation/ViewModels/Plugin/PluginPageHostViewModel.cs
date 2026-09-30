using System.Diagnostics;
using ED.Assistant.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Presentation.ViewModels.Plugin;

public sealed class PluginPageHostViewModel : LoadableViewModel
{
    public PluginPage Page { get; }
    public IPluginPageViewModel Inner { get; }
    public Control View { get; }

    public PluginPageHostViewModel(PluginPage page, IPluginPageViewModel inner,
        IJournalLoaderService loader, IJournalStateStore store, IMemoryCache cache)
        : base(loader, store, cache)
    {
        Page = page;
        Inner = inner;
        View = page.CreateView();
        View.DataContext = inner;
    }

    protected override Task UpdateFromStateAsync(JournalState state, CancellationToken ct)
        => Inner.OnJournalChangedAsync(new PluginJournalSnapshot(
            state.Commander?.Name, state.LoadGame?.ShipFullTitle,
            state.FileName, state.LastEvent?.Event), ct);
}

public interface IPluginRegistry { IReadOnlyList<PluginPageHostViewModel> Pages { get; } }

internal sealed class PluginRegistry(IEnumerable<LoadedPlugin> plugins, IServiceProvider sp) : IPluginRegistry
{
    private IReadOnlyList<PluginPageHostViewModel>? _pages;

    public IReadOnlyList<PluginPageHostViewModel> Pages => _pages ??= plugins
        .SelectMany(p => SafeGetPages(p.Plugin))
        .Select(page => ActivatorUtilities.CreateInstance<PluginPageHostViewModel>(
            sp, page, (IPluginPageViewModel)sp.GetRequiredService(page.ViewModelType)))
        .ToList();

    private static IEnumerable<PluginPage> SafeGetPages(IPlugin plugin)
    {
        try { return plugin.GetPages().ToList(); }
        catch (Exception ex) { Trace.WriteLine($"{plugin.Id}: {ex}"); return []; }
    }
}