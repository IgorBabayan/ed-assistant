namespace EdAssistant.Models.Route;

public class StarSystem
{
    [JsonPropertyName("StarSystem")]
    public string Name { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarPos")]
    public double[] Position { get; set; }

    [JsonPropertyName("StarClass")]
    public string StarClass { get; set; }

    [JsonIgnore]
    public double X => Position?[0] ?? 0;

    [JsonIgnore]
    public double Y => Position?[1] ?? 0;

    [JsonIgnore]
    public double Z => Position?[2] ?? 0;

    /// <summary>
    /// Calculates the distance to another star system
    /// </summary>
    /// <param name="other">The other star system</param>
    /// <returns>Distance in light years</returns>
    public double DistanceTo(StarSystem other)
    {
        if (other?.Position == null || Position == null) return 0;

        double dx = X - other.X;
        double dy = Y - other.Y;
        double dz = Z - other.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}