using ED.Assistant.Domain.DTO;

namespace ED.Assistant.Domain.Events;

public class SellOrganicDataEvent : BaseJournalEvent
{
    internal const string EventName = "SellOrganicData";

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("BioData")]
    public IEnumerable<BioDataItem>? BioData { get; set; }
}