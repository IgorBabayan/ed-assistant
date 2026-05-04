namespace ED.Assistant.Data.Models.Events;

public class ScanOrganicEvent : BaseJournalEvent
{
	internal const string EventName = "ScanOrganic";

	[JsonPropertyName("ScanType")]
	public string ScanType { get; set; } = string.Empty;

	[JsonPropertyName("Genus_Localised")]
	public string Genus { get; set; } = string.Empty;

	[JsonPropertyName("Species_Localised")]
	public string Species { get; set; } = string.Empty;

	[JsonPropertyName("Variant_Localised")]
	public string Variant { get; set; } = string.Empty;

	[JsonPropertyName("WasLogged")]
	public bool WasLogged { get; set; }

	[JsonPropertyName("Body")]
	public int BodyId { get; set; }

	[JsonPropertyName("SystemAddress")]
	public long SystemAddress { get; set; }
}
