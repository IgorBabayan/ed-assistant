namespace ED.Assistant.Data.Models.Events;

public class FSDJumpEvent : BaseJournalEvent
{
	internal const string EventName = "FSDJump";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }

	[JsonPropertyName("Taxi")]
	public bool Taxi { get; set; }

	[JsonPropertyName("Multicrew")]
	public bool Multicrew { get; set; }

	[JsonPropertyName("StarSystem")]
	public string StarSystem { get; set; } = string.Empty;

	[JsonPropertyName("SystemAddress")]
	public long SystemAddress { get; set; }

	[JsonPropertyName("StarPos")]
	public IEnumerable<float>? StarPos { get; set; } = default;

	[JsonPropertyName("SystemAllegiance")]
	public string Allegiance { get; set; } = string.Empty;

	[JsonPropertyName("SystemEconomy_Localised")]
	public string Economy { get; set; } = string.Empty;

	[JsonPropertyName("SystemSecondEconomy_Localised")]
	public string SecondEconomy { get; set; } = string.Empty;

	[JsonPropertyName("SystemSecurity_Localised")]
	public string Security { get; set; } = string.Empty;

	[JsonPropertyName("Population")]
	public ulong Population { get; set; }

	[JsonPropertyName("Body")]
	public string Body { get; set; } = string.Empty;

	[JsonPropertyName("BodyType")]
	public string BodyType { get; set; } = string.Empty;

	[JsonPropertyName("JumpDist")]
	public double JumpDist { get; set; }

	[JsonPropertyName("FuelUsed")]
	public double FuelUsed { get; set; }

	[JsonPropertyName("FuelLevel")]
	public double FuelLevel { get; set; }

	[JsonPropertyName("SystemGovernment_Localised")]
	public string Government { get; set; } = string.Empty;
}
