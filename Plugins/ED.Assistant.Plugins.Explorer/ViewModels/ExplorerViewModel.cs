using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ED.Assistant.Plugins.Explorer.Data;
using ED.Assistant.Plugins.Explorer.Services;
using Microsoft.EntityFrameworkCore;

namespace ED.Assistant.Plugins.Explorer.ViewModels;

public sealed partial class ExplorerViewModel : ObservableObject, IPluginPageViewModel, IDisposable
{
    private readonly ExplorerDbFactory _dbFactory;
    private readonly ExplorerSyncService _sync;
    private readonly SemaphoreSlim _refreshGate = new(1, 1);

    // Only the newest bodies are loaded up front; older ones are fetched as the list scrolls
    private const int PageSize = 100;

    // Paging state, only touched while holding _refreshGate.
    // Keyset cursor = last loaded row in (DateScanned desc, Id desc) order: unlike Skip(),
    // it doesn't shift or duplicate rows when new scans arrive between pages.
    private (DateTime DateScanned, int Id)? _cursor;
    private int _loadedCount;
    private volatile bool _hasMore;

    public ObservableCollection<ExplorerItemViewModel> Items { get; } = [];

    [ObservableProperty]
    public partial int UnsoldCount { get; set; }

    [ObservableProperty]
    public partial string UnsoldValue { get; set; } = "—";

    [ObservableProperty]
    public partial int FirstDiscoveryCount { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasImportMessage))]
    public partial string? ImportMessage { get; set; }

    [ObservableProperty]
    public partial bool IsImportError { get; set; }

    [ObservableProperty]
    public partial bool IsImporting { get; set; }

    [ObservableProperty]
    public partial int ImportProgress { get; set; }

    [ObservableProperty]
    public partial int ImportTotal { get; set; } = 1;

    public bool HasImportMessage => ImportMessage is not null;

    public bool HasItems => Items.Count > 0;

    public ExplorerViewModel(ExplorerDbFactory dbFactory, ExplorerSyncService sync)
    {
        _dbFactory = dbFactory;
        _sync = sync;

        _sync.Progress += OnProgress;
        _sync.Synced += OnSynced;

        _ = RefreshWhenReadyAsync();
    }

    public void Dispose()
    {
        _sync.Progress -= OnProgress;
        _sync.Synced -= OnSynced;
        _refreshGate.Dispose();
    }

    // The data lives in the DB; a journal change is only a hint to re-read it
    public Task OnJournalChangedAsync(PluginJournalSnapshot snapshot, CancellationToken ct) =>
        _sync.Ready.IsCompletedSuccessfully ? RefreshAsync(ct) : Task.CompletedTask;

    [RelayCommand]
    private void DismissImportMessage() => ImportMessage = null;

    private async Task RefreshWhenReadyAsync()
    {
        try
        {
            await _sync.Ready;
            await RefreshAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            // A setup failure is reported through Synced
            Trace.WriteLine($"Explorer initial refresh failed: {ex.Message}");
        }
    }

    private void OnProgress(object? sender, ExplorerSyncProgress progress) =>
        Dispatcher.UIThread.Post(() =>
        {
            IsImporting = true;
            IsImportError = false;
            ImportTotal = Math.Max(1, progress.FilesTotal);
            ImportProgress = progress.FilesRead;
            ImportMessage = $"Reading journals for exploration data… {progress.FilesRead}/{progress.FilesTotal}";
        });

    private void OnSynced(object? sender, ExplorerSyncResult result)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (!result.IsSuccess)
            {
                IsImporting = false;
                IsImportError = true;
                ImportMessage = $"Journal scan failed: {result.Error}";
                return;
            }

            if (!result.IsInitialImport)
                return;

            IsImporting = false;
            IsImportError = false;
            ImportMessage = result.BodiesAdded > 0
                ? $"Imported {result.BodiesAdded} scanned bodies from {result.FilesRead} journals in {result.Folder}"
                : $"No exploration data found in {result.Folder}";
        });

        if (result.HasChanges)
            _ = RefreshSafeAsync();
    }

    private async Task RefreshSafeAsync()
    {
        try
        {
            await RefreshAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Explorer refresh failed: {ex}");
        }
    }

    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        await _refreshGate.WaitAsync(cancellationToken);
        try
        {
            // Re-read as many rows as are already shown, so a new scan doesn't snap the list back to page 1
            var take = Math.Max(PageSize, _loadedCount);

            // Microsoft.Data.Sqlite executes "async" queries synchronously: keep the DB work off the UI thread
            var (count, total, firstDiscoveries, rows) = await Task.Run(async () =>
            {
                await using var db = _dbFactory.Create();
                var active = db.Bodies.AsNoTracking().Where(x => x.IsActive);

                // Summary cards cover every unsold body, so aggregate in SQL instead of loading all rows
                var count = await active.CountAsync(cancellationToken);
                var total = await active.SumAsync(x => x.Value, cancellationToken);
                var firstDiscoveries = await active.CountAsync(x => !x.WasDiscovered, cancellationToken);
                var rows = await NextPage(active, null, take + 1).ToListAsync(cancellationToken);

                return (count, total, firstDiscoveries, rows);
            }, cancellationToken);

            var hasMore = TrimPage(rows, take);
            var items = rows.Select(ExplorerItemViewModel.From).ToArray();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                UnsoldCount = count;
                UnsoldValue = total > 0 ? Formatting.Compact(total) : "—";
                FirstDiscoveryCount = firstDiscoveries;

                OnPropertyChanged(nameof(HasItems));
            });

            _loadedCount = items.Length;
            _cursor = rows.Count > 0 ? (rows[^1].DateScanned, rows[^1].Id) : null;
            _hasMore = hasMore;
        }
        finally
        {
            _refreshGate.Release();
        }
    }

    /// <summary>Appends the next page of older bodies. Called by the view when scrolled near the bottom.</summary>
    [RelayCommand]
    private async Task LoadMoreAsync()
    {
        if (!_hasMore)
            return;

        await _refreshGate.WaitAsync();
        try
        {
            // A refresh may have run while we waited
            if (!_hasMore || _cursor is not { } cursor)
                return;

            var rows = await Task.Run(async () =>
            {
                await using var db = _dbFactory.Create();
                var active = db.Bodies.AsNoTracking().Where(x => x.IsActive);
                return await NextPage(active, cursor, PageSize + 1).ToListAsync();
            });

            var hasMore = TrimPage(rows, PageSize);
            var items = rows.Select(ExplorerItemViewModel.From).ToArray();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var item in items)
                    Items.Add(item);
            });

            _loadedCount += items.Length;
            if (rows.Count > 0)
                _cursor = (rows[^1].DateScanned, rows[^1].Id);
            _hasMore = hasMore;
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Explorer load more failed: {ex}");
        }
        finally
        {
            _refreshGate.Release();
        }
    }

    // Newest first; Id breaks ties between bodies scanned in the same second
    private static IQueryable<ExplorerBody> NextPage(IQueryable<ExplorerBody> active,
        (DateTime DateScanned, int Id)? after, int take)
    {
        if (after is { } c)
            active = active.Where(x => x.DateScanned < c.DateScanned
                                       || (x.DateScanned == c.DateScanned && x.Id < c.Id));

        return active
            .OrderByDescending(x => x.DateScanned)
            .ThenByDescending(x => x.Id)
            .Take(take);
    }

    // Pages are queried with one extra row to know whether more exist without a COUNT
    private static bool TrimPage(List<ExplorerBody> rows, int pageSize)
    {
        if (rows.Count <= pageSize)
            return false;

        rows.RemoveRange(pageSize, rows.Count - pageSize);
        return true;
    }
}
