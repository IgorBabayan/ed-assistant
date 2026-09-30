using System.Text.Encodings.Web;
using System.Text.Json;

namespace ED.Assistant.Presentation.ViewModels.Journal;

public sealed partial class JournalEntryViewModel : ObservableObject
{
    private static readonly JsonSerializerOptions IndentedOptions = new()
    {
        WriteIndented = true,
        // keep localised names ("Sagittarius A*", Cyrillic, etc.) readable instead of \uXXXX
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public JournalEntryViewModel(JournalLogEntry entry)
    {
        Sequence = entry.Sequence;
        RawLine = entry.RawLine;
        EventName = JournalLine.ReadEventName(entry.RawLine) ?? "Unknown";
    }

    public long Sequence { get; }

    public string RawLine { get; }

    public string EventName { get; }

    /// <summary>Formatted only when the row is expanded, and only once.</summary>
    public string? FormattedJson => IsExpanded
        ? field ??= Format(RawLine)
        : null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormattedJson))]
    public partial bool IsExpanded { get; set; }

    [RelayCommand]
    private void Toggle() => IsExpanded = !IsExpanded;

    private static string Format(string rawLine)
    {
        try
        {
            using var document = JsonDocument.Parse(rawLine);
            return JsonSerializer.Serialize(document.RootElement, IndentedOptions);
        }
        catch (JsonException)
        {
            return rawLine;
        }
    }
}