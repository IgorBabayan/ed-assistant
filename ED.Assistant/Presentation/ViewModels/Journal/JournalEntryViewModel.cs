using System.Text;
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

	private string? _formattedJson;

	public JournalEntryViewModel(JournalLogEntry entry)
	{
		Sequence = entry.Sequence;
		RawLine = entry.RawLine;
		EventName = ReadEventName(entry.RawLine);
	}

	public long Sequence { get; }

	public string RawLine { get; }

	public string EventName { get; }

	/// <summary>Formatted only when the row is expanded, and only once.</summary>
	public string? FormattedJson => IsExpanded
		? _formattedJson ??= Format(RawLine)
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

	// Cheap forward-only scan: reads just the top-level "event" value and skips
	// nested objects/arrays instead of building a whole DOM for every row.
	private static string ReadEventName(string rawLine)
	{
		try
		{
			var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(rawLine));

			while (reader.Read())
			{
				if (reader.TokenType != JsonTokenType.PropertyName || reader.CurrentDepth != 1)
					continue;

				if (reader.ValueTextEquals("event"u8))
				{
					reader.Read();
					return reader.GetString() ?? "Unknown";
				}

				reader.Skip();
			}
		}
		catch (JsonException)
		{
		}
		catch (InvalidOperationException)
		{
		}

		return "Unknown";
	}
}