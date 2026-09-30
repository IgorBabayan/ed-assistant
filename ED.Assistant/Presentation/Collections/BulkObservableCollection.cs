using System.Collections.Specialized;
using System.ComponentModel;

namespace ED.Assistant.Presentation.Collections;

/// <summary>
/// Immutable notification args shared by every <see cref="BulkObservableCollection{T}"/>,
/// kept outside the generic type so there is one instance instead of one per closed type.
/// </summary>
internal static class BulkObservableCollectionEventArgs
{
    internal static readonly PropertyChangedEventArgs CountChanged = new("Count");
    internal static readonly PropertyChangedEventArgs IndexerChanged = new("Item[]");
    internal static readonly NotifyCollectionChangedEventArgs ResetArgs = new(NotifyCollectionChangedAction.Reset);
}

/// <summary>
/// ObservableCollection that can swap its whole content with one Reset notification
/// instead of one Remove per old item and one Add per new item.
/// Must be changed on the UI thread, like any bound collection.
/// </summary>
public class BulkObservableCollection<T> : ObservableCollection<T>
{
    public void ReplaceAll(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        CheckReentrancy();

        // Materialize before clearing: callers may pass this collection or a query over it.
        var replacement = items.ToArray();
        Items.Clear();
        foreach (var item in replacement)
            Items.Add(item);

        OnPropertyChanged(BulkObservableCollectionEventArgs.CountChanged);
        OnPropertyChanged(BulkObservableCollectionEventArgs.IndexerChanged);
        OnCollectionChanged(BulkObservableCollectionEventArgs.ResetArgs);
    }
}