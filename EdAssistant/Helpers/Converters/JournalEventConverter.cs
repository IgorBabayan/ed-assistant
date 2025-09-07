using ShipLockerEvent = EdAssistant.Models.Journal.ShipLockerEvent;

namespace EdAssistant.Helpers.Converters;

public sealed class JournalEventConverter : JsonConverter<IJournalEvent>
{
    public override IJournalEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        if (!doc.RootElement.TryGetProperty("event", out var evProp)) return null;

        var ev = evProp.GetString() ?? string.Empty;
        var type = ev.ToLowerInvariant() switch
        {
            "fileheader" => typeof(FileheaderEvent),
            "commander" => typeof(CommanderEvent),
            "materials" => typeof(MaterialsEvent),
            "rank" => typeof(RankEvent),
            "progress" => typeof(ProgressEvent),
            "reputation" => typeof(ReputationEvent),
            "engineerprogress" => typeof(EngineerProgressEvent),
            "loadgame" => typeof(LoadGameEvent),
            "carrierlocation" => typeof(CarrierLocationEvent),
            "statistics" => typeof(StatisticsEvent),
            "receivetext" => typeof(ReceiveTextEvent),
            "location" => typeof(LocationEvent),
            "powerplay" => typeof(PowerplayEvent),
            "music" => typeof(MusicEvent),
            "shiplocker" => typeof(ShipLockerEvent),
            "missions" => typeof(MissionsEvent),
            "loadout" => typeof(LoadoutEvent),
            _ => null
        };

        var json = doc.RootElement.GetRawText();
        if (type is null)
        {
            // Fallback: preserve timestamp and allow later handling
            return JsonSerializer.Deserialize<UnknownEvent>(json, options);
        }
        return (IJournalEvent?)JsonSerializer.Deserialize(json, type, options);
    }

    public override void Write(Utf8JsonWriter writer, IJournalEvent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}