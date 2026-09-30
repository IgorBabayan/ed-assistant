using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ED.Assistant.Presentation.Collections;

/// <summary>
/// ObservableCollection that can swap its whole content with one Reset notification
/// instead of one Remove per old item and one Add per new item.
/// Must be changed on the UI thread, like any bound collection.
/// </summary>
public class BulkObservableCollection<T> : ObservableCollection<T>
{
    private static readonly PropertyChangedEventArgs CountChanged = new(nameof(Count));
    private static readonly PropertyChangedEventArgs IndexerChanged = new("Item[]");
    private static readonly NotifyCollectionChangedEventArgs ResetArgs = new(NotifyCollectionChangedAction.Reset);

    public BulkObservableCollection() { }

    public BulkObservableCollection(IEnumerable<T> items) : base(items) { }

    public void ReplaceAll(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        CheckReentrancy();

        // Materialize before clearing: callers may pass this collection or a query over it.
        var replacement = items.ToArray();
        Items.Clear();
        foreach (var item in replacement)
            Items.Add(item);

        OnPropertyChanged(CountChanged);
        OnPropertyChanged(IndexerChanged);
        OnCollectionChanged(ResetArgs);
    }
}