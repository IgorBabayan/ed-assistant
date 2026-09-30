using System.Text.Json;

namespace ED.Assistant.Presentation.Converters;

public class ParentConverter : JsonConverter<Parent>
{
	public override Parent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var doc = JsonDocument.ParseValue(ref reader);
		var obj = doc.RootElement;

		// Expected shape: { "Planet": 3 }. FirstOrDefault() on an empty object returned
		// default(JsonProperty), whose Name throws, and "Name != null" was always true.
		if (obj.ValueKind == JsonValueKind.Object)
		{
			foreach (var prop in obj.EnumerateObject())
			{
				if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.TryGetInt32(out var bodyId))
				{
					return new Parent
					{
						Type = prop.Name,
						BodyId = bodyId
					};
				}

				break;
			}
		}

		throw new JsonException("Invalid Parent format");
	}

	public override void Write(Utf8JsonWriter writer, Parent value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber(value.Type, value.BodyId);
		writer.WriteEndObject();
	}
}
