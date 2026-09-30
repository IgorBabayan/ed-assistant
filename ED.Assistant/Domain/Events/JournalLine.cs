using System.Text;
using System.Text.Json;

namespace ED.Assistant.Domain.Events;

public static class JournalLine
{
    /// <summary>Reads the top-level event property, including escaped JSON strings.</summary>
    public static string? ReadEventName(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

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