using System.Text;
using System.Text.Json;

namespace ED.Assistant.Domain.Events;

public static class JournalLine
{
    // The game writes compact JSON: { "timestamp":"...", "event":"Scan", ... }
    private const string EventMarker = "\"event\":\"";

    /// <summary>
    /// Returns the top-level "event" value of a journal line, or null if there is none.
    /// Uses a plain ordinal search first (no allocations besides the result) and falls back
    /// to a forward-only JSON scan for lines that are formatted differently.
    /// </summary>
    public static string? ReadEventName(string? line)
    {
        if (string.IsNullOrEmpty(line))
            return null;

        var span = line.AsSpan();
        var start = span.IndexOf(EventMarker, StringComparison.Ordinal);

        if (start >= 0)
        {
            start += EventMarker.Length;

            var length = span[start..].IndexOf('"');
            if (length > 0)
                return line.Substring(start, length);
        }

        return ReadEventNameSlow(line);
    }

    // Forward-only scan: reads just the top-level "event" value and skips
    // nested objects/arrays instead of building a whole DOM.
    private static string? ReadEventNameSlow(string line)
    {
        try
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(line));

            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.CurrentDepth != 1)
                    continue;

                if (reader.ValueTextEquals("event"u8))
                {
                    reader.Read();
                    return reader.GetString();
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

        return null;
    }
}