namespace EdAssistant.Models.Journal;

public sealed class LoadoutEvent : JournalEventBase
{
    public sealed class EngineeringModifier
    {
        public string? Label { get; set; }
        public double? Value { get; set; }
        public double? OriginalValue { get; set; }
        public int? LessIsGood { get; set; }
    }

    public sealed class Engineering
    {
        public string? Engineer { get; set; }
        public int? EngineerID { get; set; }
        public long? BlueprintID { get; set; }
        public string? BlueprintName { get; set; }
        public int? Level { get; set; }
        public double? Quality { get; set; }
        public string? ExperimentalEffect { get; set; }
        [JsonPropertyName("ExperimentalEffect_Localised")] public string? ExperimentalEffectLocalised { get; set; }
        public List<EngineeringModifier>? Modifiers { get; set; }
    }

    public sealed class Module
    {
        public string? Slot { get; set; }
        public string? Item { get; set; }
        public bool? On { get; set; }
        public int? Priority { get; set; }
        public double? Health { get; set; }
        public long? Value { get; set; }

        // Ammo fields (only present for a few modules)
        public int? AmmoInClip { get; set; }
        public int? AmmoInHopper { get; set; }

        public Engineering? Engineering { get; set; }

        [JsonExtensionData] public Dictionary<string, JsonElement>? Extra { get; set; }
    }

    public sealed class FuelCapacityBlock
    {
        public double Main { get; set; }
        public double Reserve { get; set; }
    }

    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? Ship { get; set; }
    public int ShipID { get; set; }
    public string? ShipName { get; set; }
    public string? ShipIdent { get; set; }
    public long? ModulesValue { get; set; }
    public double? HullHealth { get; set; }
    public double? UnladenMass { get; set; }
    public int? CargoCapacity { get; set; }
    public double? MaxJumpRange { get; set; }
    public FuelCapacityBlock? FuelCapacity { get; set; }
    public long? Rebuy { get; set; }
    public List<Module>? Modules { get; set; }

    [JsonExtensionData] public Dictionary<string, JsonElement>? Extra { get; set; }

    public override JournalEventType EventType => JournalEventType.Loadout;
}