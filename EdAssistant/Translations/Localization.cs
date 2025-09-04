namespace EdAssistant.Translations;

public sealed partial class Localization : ObservableObject
{
    private static readonly Lazy<Localization> _lazy =
        new(() => new Localization(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static Localization Instance => _lazy.Value;

    private ImmutableDictionary<string, string> _dict =
        ImmutableDictionary<string, string>.Empty.WithComparers(StringComparer.OrdinalIgnoreCase);

    [ObservableProperty]
    private string _currentLanguage = "en";

    public string this[string key] =>
        _dict.TryGetValue(key, out var v) ? v : $"⟦{key}⟧";

    private Localization() { }

    /// <summary>
    /// Load the JSON for the given language and notify the UI.
    /// Safe to call from any thread (no UI-thread requirement here).
    /// </summary>
    public void UseLanguage(string langCode)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(langCode);

        var loaded = LoadJson(langCode) ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var next = ImmutableDictionary.CreateRange(StringComparer.OrdinalIgnoreCase, loaded);

        Interlocked.Exchange(ref _dict, next);

        CurrentLanguage = langCode;
        OnPropertyChanged("Item[]");
    }

    private static Dictionary<string, string>? LoadJson(string langCode)
    {
        var uri = new Uri($"avares://EdAssistant/Translations/i18n/{langCode}.json");
        try
        {
            using var stream = AssetLoader.Open(uri);
            using var streamReader = new StreamReader(stream);
            var json = streamReader.ReadToEnd();

            var raw = JsonSerializer.Deserialize<JsonElement>(json);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            FlattenInto(raw, dict, string.Empty);
            return dict;
        }
        catch
        {
            return null;
        }
    }

    private static void FlattenInto(JsonElement src, Dictionary<string, string> dst, string prefix)
    {
        switch (src.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in src.EnumerateObject())
                {
                    var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                    FlattenInto(prop.Value, dst, key);
                }
                break;

            case JsonValueKind.Array:
                var i = 0;
                foreach (var item in src.EnumerateArray())
                {
                    FlattenInto(item, dst, $"{prefix}[{i++}]");
                }
                break;

            case JsonValueKind.String:
                dst[prefix] = src.GetString() ?? string.Empty;
                break;

            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                dst[prefix] = src.ToString();
                break;

            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                dst[prefix] = string.Empty;
                break;
        }
    }
}