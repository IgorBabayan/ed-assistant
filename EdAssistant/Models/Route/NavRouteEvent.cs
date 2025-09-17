namespace EdAssistant.Models.Route;

public class NavRouteEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    
    [JsonPropertyName("event")]
    public required string Event { get; set; }

    [JsonPropertyName("Route")]
    public List<StarSystem> Route { get; set; } = [];
}