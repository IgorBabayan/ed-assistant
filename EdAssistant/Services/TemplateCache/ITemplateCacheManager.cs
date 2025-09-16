namespace EdAssistant.Services.TemplateCache;

public interface ITemplateCacheManager : IDisposable
{
    IDataTemplate GetOrCreateTemplate(string key, Func<IDataTemplate> factory);
    T GetOrCreateIcon<T>(string key, Func<T> factory);
    void ClearAll();
    void ClearExpired();
    (int Templates, int Icons, long MemoryUsage) GetCacheStats();
}