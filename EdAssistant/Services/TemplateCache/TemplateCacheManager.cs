namespace EdAssistant.Services.TemplateCache;

class TemplateCacheManager(IMemoryCache memoryCache) : ITemplateCacheManager
{
    private readonly ConcurrentDictionary<string, IDataTemplate> _templateCache = new();
    private readonly ConcurrentDictionary<string, object> _iconCache = new();
    private readonly object _lock = new();
    
    private readonly MemoryCacheEntryOptions _templateCacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(30),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
        Priority = CacheItemPriority.High,
        Size = 1
    };
    private readonly MemoryCacheEntryOptions _iconCacheOptions = new()
    {
        SlidingExpiration = TimeSpan.FromHours(1),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4),
        Priority = CacheItemPriority.Normal,
        Size = 1
    };

    public IDataTemplate GetOrCreateTemplate(string key, Func<IDataTemplate> factory)
    {
        if (_templateCache.TryGetValue(key, out var cachedTemplate))
        {
            memoryCache.Set($"template_access_{key}", DateTime.UtcNow, _templateCacheOptions);
            return cachedTemplate;
        }

        lock (_lock)
        {
            if (_templateCache.TryGetValue(key, out cachedTemplate))
            {
                return cachedTemplate;
            }

            var template = factory();
            _templateCache[key] = template;
            
            memoryCache.Set($"template_{key}", template, _templateCacheOptions);
            memoryCache.Set($"template_access_{key}", DateTime.UtcNow, _templateCacheOptions);
            
            return template;
        }
    }

    public T GetOrCreateIcon<T>(string key, Func<T> factory)
    {
        if (_iconCache.TryGetValue(key, out var cachedIcon) && cachedIcon is T typedIcon)
        {
            memoryCache.Set($"icon_access_{key}", DateTime.UtcNow, _iconCacheOptions);
            return typedIcon;
        }

        var icon = factory();
        _iconCache[key] = icon!;
        
        memoryCache.Set($"icon_{key}", icon, _iconCacheOptions);
        memoryCache.Set($"icon_access_{key}", DateTime.UtcNow, _iconCacheOptions);
        
        return icon;
    }

    public void ClearAll()
    {
        _templateCache.Clear();
        _iconCache.Clear();
        
        if (memoryCache is MemoryCache mc)
        {
            mc.Compact(1.0);
        }
    }

    public void ClearExpired()
    {
        var expiredTemplates = new List<string>();
        var expiredIcons = new List<string>();

        // Check for expired templates
        foreach (var kvp in _templateCache)
        {
            var accessKey = $"template_access_{kvp.Key}";
            if (!memoryCache.TryGetValue(accessKey, out _))
            {
                expiredTemplates.Add(kvp.Key);
            }
        }

        // Check for expired icons
        foreach (var kvp in _iconCache)
        {
            var accessKey = $"icon_access_{kvp.Key}";
            if (!memoryCache.TryGetValue(accessKey, out _))
            {
                expiredIcons.Add(kvp.Key);
            }
        }

        foreach (var key in expiredTemplates)
        {
            _templateCache.TryRemove(key, out _);
        }

        foreach (var key in expiredIcons)
        {
            _iconCache.TryRemove(key, out _);
        }
    }

    public (int Templates, int Icons, long MemoryUsage) GetCacheStats()
    {
        var templateCount = _templateCache.Count;
        var iconCount = _iconCache.Count;
        
        var estimatedMemoryUsage = (templateCount * 1024) + (iconCount * 64);
        return (templateCount, iconCount, estimatedMemoryUsage);
    }

    public void Dispose()
    {
        ClearAll();
        memoryCache.Dispose();
    }
}