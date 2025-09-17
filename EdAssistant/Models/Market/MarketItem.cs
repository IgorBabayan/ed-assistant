namespace EdAssistant.Models.Market;

public class MarketItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    [JsonPropertyName("Name_Localised")]
    public required string NameLocalised { get; set; }

    [JsonPropertyName("Category")]
    public required string Category { get; set; }

    [JsonPropertyName("Category_Localised")]
    public required string CategoryLocalised { get; set; }

    [JsonPropertyName("BuyPrice")]
    public int BuyPrice { get; set; }

    [JsonPropertyName("SellPrice")]
    public int SellPrice { get; set; }

    [JsonPropertyName("MeanPrice")]
    public int MeanPrice { get; set; }

    [JsonPropertyName("StockBracket")]
    public int StockBracket { get; set; }

    [JsonPropertyName("DemandBracket")]
    public int DemandBracket { get; set; }

    [JsonPropertyName("Stock")]
    public int Stock { get; set; }

    [JsonPropertyName("Demand")]
    public int Demand { get; set; }

    [JsonPropertyName("Consumer")]
    public bool Consumer { get; set; }

    [JsonPropertyName("Producer")]
    public bool Producer { get; set; }

    [JsonPropertyName("Rare")]
    public bool Rare { get; set; }
}