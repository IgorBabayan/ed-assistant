using ED.Assistant.Data.Models.Events;
using System.Text.Json;

namespace ED.Assistant.Data.Converters;

public class ParentConverter : JsonConverter<Parent>
{
	public override Parent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var doc = JsonDocument.ParseValue(ref reader);
		var obj = doc.RootElement;

		if (obj.EnumerateObject().FirstOrDefault() is var prop && prop.Name != null)
		{
			return new Parent
			{
				Type = prop.Name,
				BodyId = prop.Value.GetInt32()
			};
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
