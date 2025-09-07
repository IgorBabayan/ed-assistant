namespace EdAssistant.Models.Journal;

public sealed class JournalLine
{
    [JsonConverter(typeof(JournalEventConverter))]
    public IJournalEvent? Event { get; set; }
}