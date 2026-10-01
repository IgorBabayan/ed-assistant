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
            List<ExplorerBody> rows;
            await using (var db = _dbFactory.Create())
            {
                rows = await db.Bodies.AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.DateScanned)
                    .ToListAsync(cancellationToken);
            }

            var items = rows.Select(ExplorerItemViewModel.From).ToArray();
            var total = rows.Sum(x => x.Value);
            var firstDiscoveries = rows.Count(x => !x.WasDiscovered);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

                UnsoldCount = items.Length;
                UnsoldValue = total > 0 ? Formatting.Compact(total) : "—";
                FirstDiscoveryCount = firstDiscoveries;

                OnPropertyChanged(nameof(HasItems));
            });
        }
        finally
        {
            _refreshGate.Release();
        }
    }
}
